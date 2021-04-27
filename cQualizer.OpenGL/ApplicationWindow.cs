using cQualizer.OpenGL.Renderers;
using cQualizer.OpenGL.Utils;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Text;

namespace cQualizer.OpenGL {

	public class ApplicationWindow : GameWindow {

		SoundSignal signal;

		public ApplicationWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
		: base(gameWindowSettings, nativeWindowSettings) { }

		~ ApplicationWindow() {
			signal.Stop();
		}

		protected override void OnLoad() {
			base.OnLoad();

			signal = new SoundSignal();
			signal.Start();

			//RendererRegistry.RegisterRenderer(new CircleRenderer(50, 0.5f));
			//RendererRegistry.RegisterRenderer(new SoundRenderer(2, 0.1f));
			//SoundSignal.Init();
			//SoundSignal.Start();
			//RendererRegistry.RegisterRenderer(new SoundRenderer(100));
		}

		protected override void OnResize(ResizeEventArgs e) {
			base.OnResize(e);

			GL.Viewport(0, 0, Size.X, Size.Y);
		}

		protected override void OnRenderFrame(FrameEventArgs args) {
			base.OnRenderFrame(args);

			GL.Clear(ClearBufferMask.ColorBufferBit);

			RendererRegistry.Render(this, new Vector2(Size.X / MathF.Min(Size.X, Size.Y), Size.Y / MathF.Min(Size.X, Size.Y)));

			SwapBuffers();
		}

		protected override void OnUpdateFrame(FrameEventArgs args) {
			base.OnUpdateFrame(args);

			RendererRegistry.Update(this, new Vector2(Size.X / MathF.Min(Size.X, Size.Y), Size.Y / MathF.Min(Size.X, Size.Y)));

			if (signal.FFT != null) {
				/*foreach (var f in signal.FFT) {
					Console.WriteLine(f);
				}*/

				double s = (signal.FFT[0] + 65536) / (65536 * 2) * 100;
				var b = new StringBuilder();

				for (int i = 0 ; i <= 100 ; i++) {
					if (i <= s)
						b.Append("#");
				}

				Console.WriteLine(b.ToString());
			}
		}

		protected override void OnKeyDown(KeyboardKeyEventArgs e) {
			base.OnKeyDown(e);

			if (e.Key == OpenTK.Windowing.GraphicsLibraryFramework.Keys.R) {
			}

			if (e.Key == OpenTK.Windowing.GraphicsLibraryFramework.Keys.S) {
			}
		}
	}
}