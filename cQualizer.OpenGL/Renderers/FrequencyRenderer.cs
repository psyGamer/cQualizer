using cQualizer.OpenGL.Components.Graphics;
using cQualizer.OpenGL.Utils;

using Vector2 = OpenTK.Mathematics.Vector2;

using System.Numerics;
using OpenTK.Graphics.OpenGL4;
using System;
using MathNet.Numerics;
using System.Linq;
using MathNet.Numerics.IntegralTransforms;

namespace cQualizer.OpenGL.Renderers {

	public class FrequencyRenderer : IRenderer {
		public override Shader Shader { get; protected set; }

		private VertexArray vertexArray;
		private VertexBuffer vertexBuffer;
		private IndexBuffer indexBuffer;

		private float[] vertecies = new float[0];
		private uint[] indicies = new uint[0];

		public SoundSignal signal;

		public FrequencyRenderer() {
			signal = new SoundSignal();
			signal.Start();

			Shader = ShaderUtil.LoadShader("Freqency");
		}

		public void UpdateWave(Complex[] Wave, int max, bool useImaginary = false) {
			if (vertecies.Length != Wave.Length * 2) {
				vertecies = new float[Wave.Length * 2];
				indicies = new uint[Wave.Length * 2 - 2];
			}

			for (uint i = 0 ; i < Math.Min(Wave.Length, max) ; i++) {
				//vertecies[i * 2 + 0] = 1 / (Wave.Length - 1.0f) * i * 2 - 1;
				vertecies[i * 2 + 0] = (float) Wave[i].Real;
				vertecies[i * 2 + 1] = MathF.Abs((float) Wave[i].Imaginary * 3) - 0.9f;
				//vertecies[i * 2 + 1] = (float) (2.0 / Wave.Length * Math.Abs(Wave[i].Imaginary)) * 1000.0f - 0.95f;
				//Console.WriteLine(vertecies[i * 2 + 1]);
				//vertecies[i * 2 + 1] = MathF.Abs((float) (useImaginary ? Wave[i].Imaginary : Wave[i].Real) * 2) - 0.9f;

				if (i * 2 >= Wave.Length * 2 - 2)
					break;

				indicies[i * 2 + 0] = i;
				indicies[i * 2 + 1] = i + 1;
			}

			vertexBuffer = new VertexBuffer(vertecies);
			indexBuffer = new IndexBuffer(indicies);
			vertexArray = new VertexArray(vertexBuffer, indexBuffer);

			GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexAttribArray(0);
		}

		public override void Render(ApplicationWindow window, Vector2 aspectRatio) {
			if (!up) return;
			Shader.Enable();
			vertexArray.Enable();

			GL.DrawElements(BeginMode.Lines, indicies.Length, DrawElementsType.UnsignedInt, 0);

			vertexArray.Disable();
			Shader.Disable();
		}

		private bool up = false;

		public override void Update(ApplicationWindow window, Vector2 aspectRatio) {
			//if (signal.FFT == null)
			//return;
			//var a = Generate.Sinusoidal(1000, 2000, 60, 10).Select(sin => new Complex(sin, 0)).ToArray();
			//Fourier.Forward(a);
			//UpdateWave(a.Where((_, i) => i <= a.Length / 2).ToArray(), int.MaxValue);

			//Console.WriteLine(signal.FFT.Length);
			//Console.WriteLine(signal.FFT[0]);
			var wave = signal.FFT.Where((_, i) => i <= signal.FFT.Length / 2).ToArray();
			var waveL = wave.Length;

			for (int i = 0 ; i < wave.Length; i++){
				//double percentPosition = 1 / (wave.Length - 1.0f) * i * 2 - 1;
				//double remapped = Math.Log(waveL, waveL * Math.Abs(Math.Sin((i - 2 * i / 3 * waveL + 2 / 3) / waveL * Math.PI / 2))) - 1.0; 
				double remapped = Math.Asin(2 * i / (float) waveL - 1) / (0.5 * Math.PI);

				wave[i] = new Complex(remapped, Math.Sin(0.5 * Math.PI * wave[i].Imaginary));
			}

			UpdateWave(downsample(wave, 10 * 16), int.MaxValue, false);
			up = true;
		}

		private Complex[] downsample(Complex[] numbers, int sampleSize) {
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
	}
}
