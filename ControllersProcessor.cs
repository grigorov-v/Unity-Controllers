using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grigorov.Unity.Controllers {
	public class ControllersProcessor : MonoBehaviour {
		public static ControllersProcessor Instance { get; private set; }

		void Awake() {
			foreach ( var controller in ControllersBox.Controllers ) {
				ControllersInitializer.Init(controller);
			}

			Instance = this;
		}

		void Update() {
			ControllersBox.UpdateController.Update();
		}

		void FixedUpdate() {
			ControllersBox.UpdateController.FixedUpdate();
		}

		void LateUpdate() {
			ControllersBox.UpdateController.LateUpdate();
		}

		void OnDestroy() {
			foreach ( var controller in ControllersBox.Controllers ) {
				ControllersInitializer.Reset(controller);
			}
		}
	}
}