using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;

namespace cQualizer.OpenGL {

	class Program {

		static void Main(string[] args) {

			var gameSettings = new GameWindowSettings();
			var nativeSettings = new NativeWindowSettings();

			gameSettings.IsMultiThreaded = false;
			gameSettings.RenderFrequency = 0;

			nativeSettings.IsFullscreen = false;
			nativeSettings.StartFocused = true;
			nativeSettings.NumberOfSamples = 0;
			nativeSettings.Flags = ContextFlags.Debug;
			nativeSettings.WindowBorder = WindowBorder.Resizable;
			nativeSettings.Title = "cQualizer";

			var window = new ApplicationWindow(gameSettings, nativeSettings);

			window.Run();
		}
	}
}