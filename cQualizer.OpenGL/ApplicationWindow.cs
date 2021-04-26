using cQualizer.OpenGL.Renderers;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace cQualizer.OpenGL {

	public class ApplicationWindow : GameWindow {

		public ApplicationWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
		: base(gameWindowSettings, nativeWindowSettings) { }

		~ ApplicationWindow() {
			
		}

		protected override void OnLoad() {
			base.OnLoad();

			RendererRegistry.RegisterRenderer(new CircleRenderer(50, 0.5f));
		}

		protected override void OnResize(ResizeEventArgs e) {
			base.OnResize(e);

			GL.Viewport(0, 0, Size.X, Size.Y);
		}

		protected override void OnRenderFrame(FrameEventArgs args) {
			base.OnRenderFrame(args);

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			RendererRegistry.Render();

			SwapBuffers();
		}

		protected override void OnUpdateFrame(FrameEventArgs args) {
			base.OnUpdateFrame(args);

			RendererRegistry.Update();
		}
	}
}