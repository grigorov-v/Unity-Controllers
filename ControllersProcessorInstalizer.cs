using UnityEngine;
using UnityEngine.SceneManagement;

namespace Grigorov.Unity.Controllers {
	public static class ControllersProcessorInstalizer {
		const string ControllersProcessorName = "[ControllersProcessor]";

		public static ControllersProcessor TryFindOrCreateControllersProcessor() {
			var processor = Object.FindObjectOfType<ControllersProcessor>();
			if ( processor ) {
				return processor;
			}

			return new GameObject(ControllersProcessorName).AddComponent<ControllersProcessor>();
		}

		[RuntimeInitializeOnLoadMethod]
		static void OnRuntimeInitializeOnLoad() {
			TryFindOrCreateControllersProcessor();
			SceneManager.sceneLoaded -= OnSceneLoaded;
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		static void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
			TryFindOrCreateControllersProcessor();
		}
	}
}