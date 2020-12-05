using UnityEngine;
using System;
using System.Collections.Generic;

namespace Grigorov.Unity.Controllers
{
	public class ControllersProcessor : MonoBehaviour
	{
		public static ControllersProcessor Instance { get; private set; }

		Dictionary<Type, object> AllControllers => ControllersBox.AllControllers;

		void Awake()
		{
			foreach (var controller in AllControllers)
			{
				var type = controller.Key;
				var controllerObj = controller.Value;
				if (!ControllersBox.IsInitedController(type))
				{
					(controllerObj as IInit)?.OnInit();
					ControllersBox.ChangeInitedStatus(type);
				}
			}

			foreach (var controller in AllControllers)
			{
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
			foreach (var controller in AllControllers)
			{
				(controller.Value as IDeinit)?.OnDeinit();
			}

			ControllersBox.ResetInitedStatuses();

			foreach (var controller in AllControllers)
			{
				ControllersBox.UpdateController.RemoveUpdate(controller.Value as IUpdate);
				ControllersBox.UpdateController.RemoveUpdate(controller.Value as ILateUpdate);
				ControllersBox.UpdateController.RemoveUpdate(controller.Value as IFixedUpdate);
			}
		}
	}
}