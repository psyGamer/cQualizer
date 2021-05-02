using System;
using System.Linq;
using System.Numerics;

using CSCore.SoundIn;

using MathNet.Numerics.IntegralTransforms;

namespace cQualizer.OpenGL.Utils {

	public class SoundSignal {

		private WasapiCapture capture;

		public Complex[] Wave { get; private set; }
		public Complex[] FFT { get; private set; }

		public SoundSignal() {
			capture = new WasapiLoopbackCapture();
			capture.Initialize();
			capture.DataAvailable += (sender, args) => Process(args.Data, args.ByteCount);
		}

		~ SoundSignal() {
			if(capture.RecordingState == RecordingState.Recording)
				capture.Stop();
			capture.Dispose();
		}

		public void Process(byte[] data, int bytes) {
			Wave = new Complex[bytes / 4];
			FFT  = new Complex[bytes / 4];
			var samples = new Complex[bytes / 4];

			convertByteArrayToWave(samples, data, bytes);
			samples.CopyTo(Wave, 0);
			Fourier.Forward(samples, FourierOptions.InverseExponent);
			samples.CopyTo(FFT, 0);
		}

		public void Start() {
			capture.Start();
		}

		public void Stop() {
			capture.Stop();
		}

		private static void applyFFT(Complex[] wave, Complex[] fftBuffer) {
			//wave.Select(complex => new Complex(complex.Real * 1000, 0)).ToArray().CopyTo(fftBuffer, 0);
			wave.CopyTo(fftBuffer, 0);
			Fourier.Forward(fftBuffer, FourierOptions.NoScaling);
		}

		private static void convertByteArrayToWave(Complex[] waveBuffer, byte[] byteArray, int length) {
			if (waveBuffer == null || waveBuffer.Length < MathF.Floor(length / 4.0f))
				throw new ArgumentException();

			for (int i = 0 ; i < length / 4.0f ; i ++) {
				waveBuffer[i] = new Complex(BitConverter.ToSingle(byteArray, i * 4), 0);
			}
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
