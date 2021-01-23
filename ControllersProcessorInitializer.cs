using UnityEngine;
using UnityEngine.SceneManagement;

namespace Grigorov.Unity.Controllers {
	public static class ControllersProcessorInitializer {
		const string ControllersProcessorName = "[ControllersProcessor]";
		
		[RuntimeInitializeOnLoadMethod]
		static void OnRuntimeInitializeOnLoad() {
			TryFindOrCreateControllersProcessor();
			SceneManager.sceneLoaded -= OnSceneLoaded;
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		static void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
			TryFindOrCreateControllersProcessor();
		}
		
		static ControllersProcessor TryFindOrCreateControllersProcessor() {
			var processor = Object.FindObjectOfType<ControllersProcessor>();
			return processor ? processor : new GameObject(ControllersProcessorName).AddComponent<ControllersProcessor>();
		}
	}
}