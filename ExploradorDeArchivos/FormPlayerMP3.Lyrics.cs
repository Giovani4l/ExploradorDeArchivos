using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExploradorDeArchivos
{
    public partial class FormPlayerMP3
    {
// ══════════════════════════════════════════════════════════════════════
        //  LYRICS
        // ══════════════════════════════════════════════════════════════════════

        private async void GetLyricsAsync(string artist, string title)
        {
            _lyricsCts?.Cancel();
            _lyricsCts = new CancellationTokenSource();
            var ct = _lyricsCts.Token;

            txtLyrics.Text = "⏳  Buscando letra...";
            string searchArtist = artist == "Desconocido" ? "" : artist;

            try
            {
                string? lyr =
                    await TryLrcLibAsync(searchArtist, title, ct)       ??
                    await TryLrcLibSearchAsync(searchArtist, title, ct) ??
                    await TryMusixmatchAsync(searchArtist, title, ct)   ??
                    await TryLyricsOvhAsync(searchArtist, title, ct)    ??
                    await TryGeniusScraperAsync(searchArtist, title, ct);

                if (ct.IsCancellationRequested) return;
                txtLyrics.Text = !string.IsNullOrWhiteSpace(lyr) ? StripTimestamps(lyr) : "📭  Letra no encontrada.";
            }
            catch (OperationCanceledException) { }
            catch { if (!ct.IsCancellationRequested) txtLyrics.Text = "❌  Error de red al buscar la letra."; }
        }

        private async Task<string?> TryLrcLibAsync(string artist, string title, CancellationToken ct)
        {
            try
            {
                var url  = $"https://lrclib.net/api/get?artist_name={Uri.EscapeDataString(artist)}&track_name={Uri.EscapeDataString(title)}";
                var resp = await _http.GetAsync(url, ct);
                if (!resp.IsSuccessStatusCode) return null;
                using var doc = JsonDocument.Parse(await resp.Content.ReadAsStringAsync(ct));
                var root = doc.RootElement;
                if (root.TryGetProperty("plainLyrics",  out var pl) && pl.GetString() is string p && p.Length > 20) return p;
                if (root.TryGetProperty("syncedLyrics", out var sl) && sl.GetString() is string sy && sy.Length > 20) return sy;
            }
            catch (OperationCanceledException) { throw; }
            catch { }
            return null;
        }

        private async Task<string?> TryLrcLibSearchAsync(string artist, string title, CancellationToken ct)
        {
            try
            {
                var url  = $"https://lrclib.net/api/search?artist_name={Uri.EscapeDataString(artist)}&track_name={Uri.EscapeDataString(title)}";
                var resp = await _http.GetAsync(url, ct);
                if (!resp.IsSuccessStatusCode) return null;
                using var doc = JsonDocument.Parse(await resp.Content.ReadAsStringAsync(ct));
                if (doc.RootElement.ValueKind != JsonValueKind.Array || doc.RootElement.GetArrayLength() == 0) return null;
                var first = doc.RootElement[0];
                if (first.TryGetProperty("plainLyrics",  out var pl) && pl.GetString() is string p && p.Length > 20) return p;
                if (first.TryGetProperty("syncedLyrics", out var sl) && sl.GetString() is string sy && sy.Length > 20) return sy;
            }
            catch (OperationCanceledException) { throw; }
            catch { }
            return null;
        }

        private async Task<string?> TryMusixmatchAsync(string artist, string title, CancellationToken ct)
        {
            try
            {
                var url  = $"https://api.musixmatch.com/ws/1.1/matcher.lyrics.get"
                         + $"?q_artist={Uri.EscapeDataString(artist)}&q_track={Uri.EscapeDataString(title)}"
                         + "&apikey=1b6a940a6d6e04c28a8cef7b7bea6cf2";
                var resp = await _http.GetAsync(url, ct);
                if (!resp.IsSuccessStatusCode) return null;
                using var doc = JsonDocument.Parse(await resp.Content.ReadAsStringAsync(ct));
                var body = doc.RootElement.GetProperty("message").GetProperty("body");
                if (body.TryGetProperty("lyrics", out var lyrNode) &&
                    lyrNode.TryGetProperty("lyrics_body", out var lyrBody))
                {
                    var text = lyrBody.GetString();
                    if (!string.IsNullOrWhiteSpace(text) && text.Length > 30)
                    {
                        int cut = text.IndexOf("******* This Lyrics", StringComparison.Ordinal);
                        return cut > 0 ? text[..cut].Trim() : text.Trim();
                    }
                }
            }
            catch (OperationCanceledException) { throw; }
            catch { }
            return null;
        }

        private async Task<string?> TryLyricsOvhAsync(string artist, string title, CancellationToken ct)
        {
            try
            {
                var resp = await _http.GetAsync(
                    $"https://api.lyrics.ovh/v1/{Uri.EscapeDataString(artist)}/{Uri.EscapeDataString(title)}", ct);
                if (!resp.IsSuccessStatusCode) return null;
                using var doc = JsonDocument.Parse(await resp.Content.ReadAsStringAsync(ct));
                if (doc.RootElement.TryGetProperty("lyrics", out var lyr)) return lyr.GetString();
            }
            catch (OperationCanceledException) { throw; }
            catch { }
            return null;
        }

        private async Task<string?> TryGeniusScraperAsync(string artist, string title, CancellationToken ct)
        {
            try
            {
                var req = new HttpRequestMessage(HttpMethod.Get,
                    $"https://api.genius.com/search?q={Uri.EscapeDataString($"{artist} {title}")}");
                req.Headers.Add("Authorization", "Bearer SiH4_HC3k3kYkqiNs4ZqZRJIoZm5KKoE2PMm4U0j0RhxJbajPRWh-7pj2PZeNsMi");
                var resp = await _http.SendAsync(req, ct);
                if (!resp.IsSuccessStatusCode) return null;

                using var doc  = JsonDocument.Parse(await resp.Content.ReadAsStringAsync(ct));
                var hits = doc.RootElement.GetProperty("response").GetProperty("hits");
                if (hits.GetArrayLength() == 0) return null;

                var songPath = hits[0].GetProperty("result").GetProperty("path").GetString();
                if (string.IsNullOrEmpty(songPath)) return null;

                var html    = await _http.GetStringAsync($"https://genius.com{songPath}", ct);
                var matches = Regex.Matches(html, @"<div[^>]*data-lyrics-container[^>]*>(.*?)</div>", RegexOptions.Singleline);
                if (matches.Count == 0) return null;

                var raw = string.Join("\n", matches.Select(m => m.Groups[1].Value));
                raw = Regex.Replace(raw, @"<br\s*/?>", "\n", RegexOptions.IgnoreCase);
                raw = Regex.Replace(raw, @"<[^>]+>", "");
                raw = System.Net.WebUtility.HtmlDecode(raw).Trim();
                return raw.Length > 30 ? raw : null;
            }
            catch (OperationCanceledException) { throw; }
            catch { }
            return null;
        }

        private static string StripTimestamps(string s) =>
            string.Join("\n",
                s.Split('\n')
                 .Select(l => Regex.Replace(l, @"^\[\d+:\d+\.\d+\]\s*", "").Trim())
                 .Where(l => l.Length > 0));
        
    }
}

