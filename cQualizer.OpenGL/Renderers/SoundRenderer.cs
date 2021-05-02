using cQualizer.OpenGL.Components.Graphics;
using cQualizer.OpenGL.Utils;

using Vector2 = OpenTK.Mathematics.Vector2;
using OpenTK.Graphics.OpenGL4;

using System;
using System.Linq;
using System.Numerics;

namespace cQualizer.OpenGL.Renderers {

	public class SoundRenderer : IRenderer {

		public override Shader Shader { get; protected set; }

		private VertexArray vertexArray;
		private VertexBuffer vertexBuffer;
		private IndexBuffer indexBuffer;

		private SoundSignal signal;

		private uint faces;

		private float[] vertecies;
		private uint [] indicies;

		public SoundRenderer(uint faces) {
			this.faces = faces;

			float theeta = 2 * MathF.PI / faces;

			vertecies = new float[faces * 3 + 3];
			indicies  = new uint [faces * 3];

			signal = new SoundSignal();
			signal.Start();

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

			Shader = ShaderUtil.LoadShader("Sound");
		}

		private float off = 0;
		private float[] fftData = new float[2048];

		private static float constrain(float n, float high, float low) {
			return MathF.Max(MathF.Min(n, low), high);
		}

		private static float map(float startA, float endA, float startB, float endB, float n) {
			float newval = (n - startA) / (endA - startA) * (endB - startB) + startB;

			if (startB < endB) {
				return constrain(newval, startB, endB);
			} else {
				return constrain(newval, endB, startB);
			}
		}

		private static Complex[] downsample(Complex[] numbers, int sampleSize) {
			var newNumbers = new Complex[sampleSize];

			for (int i = 0 ; i < sampleSize ; i++) {
				double realSum = 0;
				double imaginarySum = 0;

				for (int j = i * (numbers.Length / sampleSize) ; j < (i + 1) * (numbers.Length / sampleSize) ; j++) {
					realSum += numbers[j].Real;
					imaginarySum += numbers[j].Imaginary;
				}

				newNumbers[i] = new Complex(realSum / numbers.Length * sampleSize, imaginarySum / numbers.Length * sampleSize);
			}

			return newNumbers;
		}

		private void UpdateRadius() {
			var fftData = signal.FFT.Where((_, i) => i <= signal.FFT.Length / 2).ToArray();

			if (fftData.Length <= 0) {
				Console.WriteLine("Unable to get data");
				return;
			}

			for (int i = 0 ; i < fftData.Length ; i++) {
				fftData[i] = new Complex(
					Math.Asin(2 * i / (float) fftData.Length - 1) / (0.5 * Math.PI),
					Math.Sin(0.5 * Math.PI * fftData[i].Imaginary)
				);
			}

			fftData = downsample(fftData, (int) faces);

			for (uint i = 0 ; i <= Math.Min(fftData.Length, faces) ; i++) {
				vertecies[i * 3 + 2] = (float) Math.Abs(fftData[i % fftData.Length].Imaginary * 3.0) + 0.1f;

								//float positionOnCircumference = map(1, faces / 3, 0.0f, 1.0f, (i - 2) / 3);//(i - 2) / 3 / (float) (faces + 1);
				//int index = (int) map(0.0f, 1.0f, 0.0f, fftData.Length - 1, positionOnCircumference);

				////vertecies[i] = fftData[(int) MathF.Min(index, fftData.Length - 1)] * 10000;
				//Console.WriteLine(vertecies[i]);

				//vertecies[i] = MathF.Abs(MathF.Cos(positionOnCircumference + off)) * 0.6f + 0.2f;//positionOnCircumference / 2.0f; MathF.Abs(vertecies[i - 1]);//(positionOnCircumference * 1000) % 2 == 0 ? 0.3f : 0.6f; //
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

			vertexArray.Enable();
			vertexBuffer.SetData(vertecies, -1, BufferUsageHint.DynamicDraw);
			vertexArray.Disable();

			//Console.WriteLine("Update Array");
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
