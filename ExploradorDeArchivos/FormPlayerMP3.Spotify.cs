using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExploradorDeArchivos
{
    public partial class FormPlayerMP3
    {
// ══════════════════════════════════════════════════════════════════════
        //  SPOTIFY — Credenciales y Token
        // ══════════════════════════════════════════════════════════════════════

        private void LoadSpotifyCredentials()
        {
            var (id, secret) = FormSpotifyConfig.LoadSaved();
            _spotifyClientId     = id;
            _spotifyClientSecret = secret;

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(secret))
                ShowSpotifyConfig(firstTime: true);
        }

        private void ShowSpotifyConfig(bool firstTime = false)
        {
            using var dlg = new FormSpotifyConfig(_spotifyClientId, _spotifyClientSecret);
            if (firstTime)
                MessageBox.Show(
                    "Para mostrar portada, artista, álbum y duración desde Spotify,\n" +
                    "necesitas ingresar tus credenciales de Spotify Developer.",
                    "Configurar Spotify",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                _spotifyClientId     = dlg.ClientId;
                _spotifyClientSecret = dlg.ClientSecret;
                _spotifyToken        = "";
                _spotifyTokenExpiry  = DateTime.MinValue;
                MessageBox.Show("✓ Credenciales de Spotify guardadas correctamente.",
                    "Listo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async Task<string?> GetSpotifyTokenAsync()
        {
            if (!string.IsNullOrEmpty(_spotifyToken) && DateTime.UtcNow < _spotifyTokenExpiry)
                return _spotifyToken;

            if (string.IsNullOrEmpty(_spotifyClientId) || string.IsNullOrEmpty(_spotifyClientSecret))
                return null;

            try
            {
                var credentials = Convert.ToBase64String(
                    Encoding.UTF8.GetBytes($"{_spotifyClientId}:{_spotifyClientSecret}"));

                var req = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");
                req.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                req.Content = new FormUrlEncodedContent(new[]
                    { new KeyValuePair<string, string>("grant_type", "client_credentials") });

                var resp = await _http.SendAsync(req);
                if (!resp.IsSuccessStatusCode) return null;

                using var doc  = JsonDocument.Parse(await resp.Content.ReadAsStringAsync());
                var token      = doc.RootElement.GetProperty("access_token").GetString();
                var expires    = doc.RootElement.GetProperty("expires_in").GetInt32();
                _spotifyToken       = token ?? "";
                _spotifyTokenExpiry = DateTime.UtcNow.AddSeconds(expires - 30);
                return _spotifyToken;
            }
            catch { return null; }
        }

        private async Task<SpotifyTrackData?> SearchSpotifyAsync(string artist, string title)
        {
            var token = await GetSpotifyTokenAsync();
            if (string.IsNullOrEmpty(token)) return null;

            string CleanQ(string s) => Regex
                .Replace(s ?? "", @"\(.*?\)|\[.*?\]|feat\..*$|ft\..*$", "", RegexOptions.IgnoreCase).Trim();

            var queries = new List<string>();
            string cleanArtist = CleanQ(artist);
            string cleanTitle  = CleanQ(title);

            if (!string.IsNullOrEmpty(cleanArtist))
            {
                queries.Add($"artist:{Uri.EscapeDataString(cleanArtist)} track:{Uri.EscapeDataString(cleanTitle)}");
                queries.Add(Uri.EscapeDataString($"{cleanArtist} {cleanTitle}"));
            }
            queries.Add($"track:{Uri.EscapeDataString(cleanTitle)}");

            try
            {
                string? bestJson = null;
                foreach (var q in queries)
                {
                    var url = $"https://api.spotify.com/v1/search?q={q}&type=track&limit=1&market=MX";
                    var req = new HttpRequestMessage(HttpMethod.Get, url);
                    req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var resp = await _http.SendAsync(req);
                    if (!resp.IsSuccessStatusCode) continue;

                    var json = await resp.Content.ReadAsStringAsync();
                    using var check = JsonDocument.Parse(json);
                    if (check.RootElement.GetProperty("tracks").GetProperty("items").GetArrayLength() > 0)
                    { bestJson = json; break; }
                }

                if (bestJson == null) return null;

                using var doc    = JsonDocument.Parse(bestJson);
                var tracks  = doc.RootElement.GetProperty("tracks").GetProperty("items");
                if (tracks.GetArrayLength() == 0) return null;

                var item    = tracks[0];
                var albumEl = item.GetProperty("album");
                var artists = item.GetProperty("artists");

                string? coverUrl = albumEl.TryGetProperty("images", out var imgs) && imgs.GetArrayLength() > 0
                    ? imgs[0].GetProperty("url").GetString() : null;

                string artistsFull = string.Join(", ",
                    Enumerable.Range(0, artists.GetArrayLength())
                              .Select(i => artists[i].GetProperty("name").GetString() ?? ""));

                string albumName    = albumEl.GetProperty("name").GetString()       ?? "";
                string albumType    = albumEl.GetProperty("album_type").GetString() ?? "";
                int    durationMs   = item.GetProperty("duration_ms").GetInt32();
                int    popularity   = item.TryGetProperty("popularity", out var pop) ? pop.GetInt32() : 0;
                string releaseDate  = albumEl.TryGetProperty("release_date", out var rd) ? rd.GetString() ?? "" : "";
                string spotifyUrl   = item.TryGetProperty("external_urls", out var eu) &&
                                      eu.TryGetProperty("spotify",         out var su)
                                      ? su.GetString() ?? "" : "";
                string trackName    = item.GetProperty("name").GetString() ?? title;

                Image? cover = null;
                if (!string.IsNullOrEmpty(coverUrl))
                {
                    var bytes = await _http.GetByteArrayAsync(coverUrl);
                    cover = Image.FromStream(new MemoryStream(bytes));
                }

                return new SpotifyTrackData(
                    trackName, artistsFull, albumName, albumType,
                    TimeSpan.FromMilliseconds(durationMs),
                    popularity, releaseDate, spotifyUrl, cover);
            }
            catch { return null; }
        }
// ══════════════════════════════════════════════════════════════════════
        //  COVER FALLBACK
        // ══════════════════════════════════════════════════════════════════════

        private async Task<Image?> FetchCoverFallbackAsync(string artist, string title, string album)
        {
            string Clean(string x) => Regex
                .Replace(x ?? "", @"[\(\[].+?[\)\]]|feat\..+$|^\d+[\.\-\s]*", "", RegexOptions.IgnoreCase).Trim();

            var queries = new List<string>();
            if (!string.IsNullOrEmpty(Clean(artist)) && !string.IsNullOrEmpty(Clean(title)))
                queries.Add($"{Clean(artist)} {Clean(title)}");
            if (!string.IsNullOrEmpty(Clean(title)))
                queries.Add(Clean(title));

            foreach (var q in queries)
            {
                var img = await DeezerCoverAsync(q) ?? await ITunesCoverAsync(q);
                if (img != null) return img;
            }
            return null;
        }

        private async Task<Image?> DeezerCoverAsync(string q)
        {
            try
            {
                var resp = await _http.GetAsync($"https://api.deezer.com/search?q={Uri.EscapeDataString(q)}&limit=1");
                if (!resp.IsSuccessStatusCode) return null;
                using var doc = JsonDocument.Parse(await resp.Content.ReadAsStringAsync());
                var data = doc.RootElement.GetProperty("data");
                if (data.GetArrayLength() == 0) return null;
                var url = data[0].GetProperty("album").TryGetProperty("cover_big", out var cb) ? cb.GetString() : null;
                if (string.IsNullOrEmpty(url)) return null;
                return Image.FromStream(new MemoryStream(await _http.GetByteArrayAsync(url)));
            }
            catch { return null; }
        }

        private async Task<Image?> ITunesCoverAsync(string q)
        {
            try
            {
                var resp = await _http.GetAsync($"https://itunes.apple.com/search?term={Uri.EscapeDataString(q)}&media=music&limit=1");
                if (!resp.IsSuccessStatusCode) return null;
                using var doc = JsonDocument.Parse(await resp.Content.ReadAsStringAsync());
                var results = doc.RootElement.GetProperty("results");
                if (results.GetArrayLength() == 0) return null;
                var url = results[0].GetProperty("artworkUrl100").GetString()?.Replace("100x100", "500x500");
                if (string.IsNullOrEmpty(url)) return null;
                return Image.FromStream(new MemoryStream(await _http.GetByteArrayAsync(url)));
            }
            catch { return null; }
        }
    }
}

