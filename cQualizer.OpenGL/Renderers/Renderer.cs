using System;
using System.Collections.Generic;
using System.Text;

namespace cQualizer.OpenGL.Renderers {

	public interface IRenderer {
		void Load();
		void Render();
		void Update();
	}

	public static class RendererRegistry {

		private static List<IRenderer> renderers = new List<IRenderer>();

		public static void RegisterRenderer(IRenderer renderer) {
			renderer.Load();
			renderers.Add(renderer);
		}

		public static void Update() {
			foreach (var renderer in renderers) {
				renderer.Update();
			}
		}

		public static void Render() {
			foreach (var renderer in renderers) {
				renderer.Render();
			}
		}
	}
}
