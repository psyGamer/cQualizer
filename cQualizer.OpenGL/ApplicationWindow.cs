using cQualizer.OpenGL.Loaders;
using cQualizer.OpenGL.Utils;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;

namespace cQualizer.OpenGL {

	public class ApplicationWindow : GameWindow {

		public ApplicationWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
		: base(gameWindowSettings, nativeWindowSettings) { }

		protected override void OnLoad() {
			base.OnLoad();

			vertexArray = GL.GenVertexArray();
			vbo = new VertexBuffer(vertecies);
			ibo = new IndexBuffer(indicies);
			shader = ShaderLoader.LoadShader("Color");
		}

		protected override void OnResize(ResizeEventArgs e) {
			base.OnResize(e);

			GL.Viewport(0, 0, Size.X, Size.Y);
		}

		private static readonly float[] vertecies = new float[] {
			-0.5f, -0.5f,
			 0.5f, -0.5f,
			 0.5f,  0.5f,
			-0.5f,  0.5f
		};

		private static readonly uint[] indicies = new uint[] {
			0, 1, 2,
			2, 0, 3
		};

		private int vertexArray;
		private VertexBuffer vbo;
		private IndexBuffer ibo;
		private Shader shader;

		private float i;

		protected override void OnRenderFrame(FrameEventArgs args) {
			base.OnRenderFrame(args);

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.BindVertexArray(vertexArray);
			vbo.Enable();
			/*int vertexBuffer = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, vertecies.Length * sizeof(float), vertecies, BufferUsageHint.StaticDraw);*/
			GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
			GL.EnableVertexAttribArray(0);

			ibo.Enable();
			shader.SetUniformVec4f("uColor", MathF.Sin(i) * 0.5f + 0.5f, 0.0f, 0.0f, 1.0f);

			//GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
			GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);

			SwapBuffers();
		}

		protected override void OnUpdateFrame(FrameEventArgs args) {
			base.OnUpdateFrame(args);

			i += 0.01f;
		}
	}
}