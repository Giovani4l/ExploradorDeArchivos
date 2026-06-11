**ExploradorDeArchivos**
Este proyecto nació de mi frustración personal. Me cansé de tener que abrir cinco programas distintos para tareas cotidianas: uno para explorar archivos, otro para editar un CSV, otro para ver un video y otro para tratar de limpiar datos. Así que, básicamente, construí mi propia "navaja suiza" en .NET 8 con WinForms.
El resultado es el ExploradorDeArchivos v16: una herramienta todo-en-uno que intenta hacer la vida un poco más fácil (al menos la mía).

**¿Qué hay aquí dentro?**
No es solo un explorador. He integrado lo que más uso a diario:
Navegación fluida: Con un sistema de historial tipo navegador para no perderse entre carpetas.
Visor "Todo terreno": Abre tablas, código, imágenes, PDFs y documentos de Word sin tener que instalar software pesado.
Inteligencia de Datos: Un módulo para limpiar archivos tabulares donde la IA analiza las columnas y te sugiere correcciones.
Multimedia a tope: Reproductor de música (con integración de Spotify para los metadatos y portadas), reproductor de video (VLC), cámara en vivo y grabadora de voz.
Edición Visual: Un pequeño editor de imágenes que incluso respeta los metadatos GPS.

**Cómo ponerlo a andar**
Si quieres compilarlo, no es ciencia espacial, pero ten en cuenta un par de cosas:
Requisitos: .NET 8 (Windows) y el WebView2 Runtime instalado (lo usa el visor de PDFs).
Configuración: Al ser un proyecto con binarios nativos (OpenCV y VLC), asegúrate de que al compilar, todos los archivos .dll se muevan correctamente a tu carpeta bin/. Si el reproductor de video se queda en negro, es casi seguro que es porque le falta el acceso a los binarios de VLC.
Compilación: Ábrelo con Visual Studio 2022, pon la configuración en Release x64 y dale a compilar.

**Un par de advertencias técnicas**
Memoria: Uso OpenCvSharp para la cámara. He intentado ser cuidadoso con los recursos, pero recuerda cerrar siempre las ventanas de captura correctamente para evitar fugas de memoria.
IA: La limpieza de datos hace una petición HTTP con un timeout de 60s. Si la API tarda más, la UI te pedirá un poco de paciencia.
Arquitectura: Todo está organizado en clases parciales (partial classes). Si quieres modificar cómo se ve el formulario, ve al .Designer.cs; si quieres cambiar cómo hace la lógica, busca el archivo parcial correspondiente (ej: Form1.Navegacion.cs).
