using UnityEngine;
using System;
using System.Collections.Generic;

namespace Grigorov.Unity.Controllers
{
	public class ControllersProcessor : MonoBehaviour
	{
		public static ControllersProcessor Instance { get; private set; }

		Dictionary<Type, object> Controllers => ControllersBox.Controllers;

		void Awake()
		{
			foreach (var controller in Controllers)
			{
				ControllersInstalizer.Init(controller);
				ControllersBox.UpdateController.AddUpdate(controller.Value as IUpdate);
				ControllersBox.UpdateController.AddUpdate(controller.Value as ILateUpdate);
				ControllersBox.UpdateController.AddUpdate(controller.Value as IFixedUpdate);
			}
			Instance = this;
		}

		void Update()
		{
			ControllersBox.UpdateController.Update();
		}

		void LateUpdate()
		{
			ControllersBox.UpdateController.LateUpdate();
		}

		void FixedUpdate()
		{
			ControllersBox.UpdateController.FixedUpdate();
		}

		void OnDestroy()
		{
			foreach (var controller in Controllers)
			{
				ControllersInstalizer.Reset(controller);
				ControllersBox.UpdateController.RemoveUpdate(controller.Value as IUpdate);
				ControllersBox.UpdateController.RemoveUpdate(controller.Value as ILateUpdate);
				ControllersBox.UpdateController.RemoveUpdate(controller.Value as IFixedUpdate);
			}
		}
	}
}