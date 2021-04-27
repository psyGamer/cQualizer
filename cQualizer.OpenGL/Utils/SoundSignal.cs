using NAudio.Wave;

using System;

using MathNet.Numerics.IntegralTransforms;

namespace cQualizer.OpenGL.Utils {

	public class SoundSignal {

		private WasapiLoopbackCapture capture;

		public double[] Wave { get; private set; }
		public double[] FFT { get; private set; }

		public SoundSignal() {
			capture = new WasapiLoopbackCapture();
			capture.DataAvailable += (sender, args) => Process(args.Buffer);
		}

		~ SoundSignal() {
			capture.StopRecording();
			capture.Dispose();
		}

		public void Process(byte[] data) {
			Wave = new double[data.Length / capture.WaveFormat.Channels];
			FFT  = new double[data.Length / capture.WaveFormat.Channels];

			int waveStep = 0;

			for (int i = 0 ; i < Wave.Length ; i += capture.WaveFormat.Channels) {
				Wave[waveStep] = BitConverter.ToInt16(data, i);
				waveStep++;
			}

			Wave.CopyTo(FFT, 0);
			Fourier.ForwardReal(FFT, 20);
		}

		public void Start() {
			capture.StartRecording();
		}

		public void Stop() {
			capture.StopRecording();
		}



		/*
		private static Complex[] fftBuffer;
		private static float[] lastFftBuffer;

		private static int fftLength;
		private static int fftPos;
		private static bool fftBufferAvailable;
		private static object lockObject;

		public static void Init() {
			capture = new WasapiLoopbackCapture();

			lockObject = new object();

			fftLength = 2048; // 44.1kHz.
			fftBuffer = new Complex[fftLength];
			lastFftBuffer = new float[fftLength];

			capture.DataAvailable += (sender, args) => {
				float[] data = ConvertToFloatArray(args.Buffer, args.BytesRecorded);

				for (int i = 0 ; i < data.Length ; i += capture.WaveFormat.Channels) {
					fftBuffer[fftPos].X = (float) (data[i] * FastFourierTransform.HannWindow(fftPos, fftLength));
					fftBuffer[fftPos].Y = 0;
					fftPos++;

					if (fftPos >= fftLength) {
						fftPos = 0;

						// NAudio FFT implementation.
						FastFourierTransform.FFT(true, (int) MathF.Log(fftLength, 2), fftBuffer);

						// Copy to buffer.
						lock (lockObject) {
							for (int c = 0 ; c < fftLength ; c++) {
								lastFftBuffer[c] = fftBuffer[c].X;
							}

							fftBufferAvailable = true;
						}
					}
				}
			};
		}

		public static void Start() {
			capture.StartRecording();
		}
		public static void Stop() {
			capture.StopRecording();
		}

		public static bool GetFFTData(float[] fftDataBuffer) {
			lock (lockObject) {

				if (fftBufferAvailable) {
					lastFftBuffer.CopyTo(fftDataBuffer, 0);
					fftBufferAvailable = false;

					return true;
				}

				return false;
			}
		}

		public static int GetFFTFrequencyIndex(int frequency) {
			return frequency / (capture.WaveFormat.SampleRate / fftLength / capture.WaveFormat.Channels);
		}

		private static float[] ConvertToFloatArray(byte[] array, int length) {
			int samples = length / 4;
			float[] floatArray = new float[samples];

			for (int i = 0 ; i < samples ; i++) {
				floatArray[i] = BitConverter.ToSingle(array, i * 4);
			}

			return floatArray;
		}*/
	}
}
