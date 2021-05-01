using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Mathematics;

namespace cQualizer.OpenGL {

	class Program {

		static void Main(string[] args) {

			var gameSettings = new GameWindowSettings();
			var nativeSettings = new NativeWindowSettings();

			gameSettings.IsMultiThreaded = false;
			gameSettings.RenderFrequency = 60;
			gameSettings.UpdateFrequency = 60;

			nativeSettings.IsFullscreen = false;
			nativeSettings.StartFocused = true;
			nativeSettings.NumberOfSamples = 0;
			nativeSettings.Flags = ContextFlags.Debug;
			nativeSettings.WindowBorder = WindowBorder.Resizable;
			nativeSettings.Title = "cQualizer";
			nativeSettings.Size = new Vector2i(400, 400);

			var window = new ApplicationWindow(gameSettings, nativeSettings);

			window.Run();
		}
	}
}