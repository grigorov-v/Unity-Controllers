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
			Instance = this;
		}

		void Update()
		{
			foreach (var controller in AllControllers)
			{
				(controller.Value as IUpdate)?.OnUpdate();
			}
		}

		void FixedUpdate()
		{
			foreach (var controller in AllControllers)
			{
				(controller.Value as IFixedUpdate)?.OnFixedUpdate();
			}
		}

		void OnDestroy()
		{
			foreach (var controller in AllControllers)
			{
				(controller.Value as IDeinit)?.OnDeinit();
			}
			ControllersBox.ResetInitedStatuses();
		}
	}
}