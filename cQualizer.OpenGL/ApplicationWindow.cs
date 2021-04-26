using cQualizer.OpenGL.Renderers;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;

namespace cQualizer.OpenGL {

	public class ApplicationWindow : GameWindow {

		public ApplicationWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
		: base(gameWindowSettings, nativeWindowSettings) { }

		protected override void OnLoad() {
			base.OnLoad();

			//RendererRegistry.RegisterRenderer(new CircleRenderer(50, 0.5f));
			//RendererRegistry.RegisterRenderer(new SoundRenderer(2, 0.1f));
			RendererRegistry.RegisterRenderer(new SoundRendererAttempt2(300));
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
		}
	}
}