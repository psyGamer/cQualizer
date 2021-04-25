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
			// Generate Vertecies

			int sampleSize = 10;
			float radius = 0.5f;

			vertecies = new float[sampleSize * 4 * 2 + 2];
			indicies = new uint[sampleSize * 4 * 3];

			vertecies[sampleSize * 4 * 2 + 0] = 0.0f;
			vertecies[sampleSize * 4 * 2 + 1] = 0.0f;

			for (float a = 0 ; a < 360 ; a += 90.0f / sampleSize) {
				radius = (float) new Random().NextDouble();
				float radiens = (MathF.PI / 180) * a;
				float x = MathF.Cos(radiens) * radius;
				float y = MathF.Sin(radiens) * radius;
				uint i = (uint) (a / (90 / sampleSize));

				vertecies[i * 2 + 0] = x;
				vertecies[i * 2 + 1] = y;

				indicies[i * 3 + 0] = 0;
				indicies[i * 3 + 1] = i + 1;
				indicies[i * 3 + 2] = (uint) ((i + 2) % (sampleSize * 4));
			}



			vertexArray = GL.GenVertexArray();
			vbo = new VertexBuffer(vertecies);
			ibo = new IndexBuffer(indicies);
			shader = ShaderLoader.LoadShader("Color");
		}

		protected override void OnResize(ResizeEventArgs e) {
			base.OnResize(e);

			GL.Viewport(0, 0, Size.X, Size.Y);
		}

		private static float[] vertecies;

		private uint[] indicies;

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
			shader.SetUniformInt("uRadius", (int) (MathF.Cos(i) * 100));
			shader.SetUniformInt("uSampleSize", vertecies.Length);
			shader.Disable();

			//GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
			GL.DrawElements(PrimitiveType.Triangles, indicies.Length, DrawElementsType.UnsignedInt, 0);

			SwapBuffers();
		}

		protected override void OnUpdateFrame(FrameEventArgs args) {
			base.OnUpdateFrame(args);

			i += 0.01f;
		}
	}
}