using cQualizer.OpenGL.Components;
using cQualizer.OpenGL.Utils;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using RandomAccessPerlinNoise;

namespace cQualizer.OpenGL.Renderers {
	class SoundRenderer : IRenderer {

		public override Shader Shader { get; protected set; }

		private VertexArray vertexArray;
		private VertexBuffer vertexBuffer;
		private IndexBuffer indexBuffer;

		private uint faces;

		private float[] vertecies;
		private uint [] indicies;

		public SoundRenderer(uint faces) {
			this.faces = faces;

			float theeta = 2 * MathF.PI / faces;

			vertecies = new float[faces * 3 + 3];
			indicies  = new uint [faces * 3];

			float angle = 0;

			for (int i = 0 ; i < faces * 3 ; i += 3) { 
				float posX = MathF.Cos(angle);
				float posY = MathF.Sin(angle);
				float radius = 0.5f;

				vertecies[i + 3] = posX;
				vertecies[i + 4] = posY;
				vertecies[i + 5] = radius;

				indicies[i + 0] = 0;
				indicies[i + 1] = (uint) (i / 3 + 1);
				indicies[i + 2] = (uint) ((i / 3 + 1) % faces + 1);

				angle += theeta;
			}

			vertexBuffer = new VertexBuffer(vertecies, -1, BufferUsageHint.DynamicDraw);
			indexBuffer = new IndexBuffer(indicies);

			vertexArray = new VertexArray(vertexBuffer, indexBuffer);

			GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
			GL.VertexAttribPointer(1, 1, VertexAttribPointerType.Float, false, 3 * sizeof(float), 2 * sizeof(float));
			GL.EnableVertexAttribArray(0);
			GL.EnableVertexAttribArray(1);

			Shader = ShdaerUtil.LoadShader("Sound");

			//gen = new NoiseGenerator(new Random().Next(), 1.0, 1, new int[] { vertecies.Length / 3, 100 }, true, Interpolations.Linear);
		}

		private float off = 0;

		private void UpdateRadius() {
			for (int i = 5 ; i < vertecies.Length ; i += 3) {
				float positionOnCircumference = ((i - 2) / 3) / (float) (faces + 1);

				positionOnCircumference *= 3 * MathF.PI;

				vertecies[i] = MathF.Abs(MathF.Cos(positionOnCircumference + off)) * 0.6f + 0.2f;//positionOnCircumference / 2.0f; MathF.Abs(vertecies[i - 1]);//(positionOnCircumference * 1000) % 2 == 0 ? 0.3f : 0.6f; //
			}

			/*
			for (int i = 5 ; i < vertecies.Length ; i += 3) {
				float idx = (i - 2) / 3.0f / (vertecies.Length / 3.0f);
				//vertecies[i] = MathF.Asin(idx);
				vertecies[i] = (float) gen.GetValue(new double[] { i, off });
				Console.WriteLine($"{vertecies[i]} | {(i - 2) / 3.0f} / {vertecies.Length / 3}");
				//MathF.Sin(i / 30 + off);
				//(float) gen.GetValue(new double[] { i, off });
				//vertecies[i] = new NoiseMap().GetValue(i, Environment.TickCount);
				//vertecies[i] += (float) new Random().NextDouble() * 0.5f + 0.5f;
			}*/

			off += 0.01f;
			off %= MathF.PI;

			vertexArray.Enable();
			vertexBuffer.SetData(vertecies, -1, BufferUsageHint.DynamicDraw);
			vertexArray.Disable();

			Console.WriteLine("Update Array");
		}

		public override void Render(ApplicationWindow window, Vector2 aspectRatio) {
			Shader.Enable();
			vertexArray.Enable();

			GL.DrawElements(BeginMode.Triangles, indicies.Length, DrawElementsType.UnsignedInt, 0);

			vertexArray.Disable();
			Shader.Disable();
		}

		public override void Update(ApplicationWindow window, Vector2 aspectRatio) {
			UpdateRadius();
			//if (new Random().NextDouble() > 0.9) {
			//} else {
			//	Console.WriteLine("Update");
			//}
		}
	}
}
