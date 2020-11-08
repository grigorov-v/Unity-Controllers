using UnityEngine;
using System.Collections.Generic;

namespace Grigorov.Controllers
{
	public class ControllersProcessor : MonoBehaviour
	{
		public static ControllersProcessor Instance { get; private set; }

		List<object> AllControllers => Controller.AllControllers;

		void Awake()
		{
			foreach (var controller in AllControllers)
			{
				(controller as IControllerForComponent)?.Init();
				(controller as IAwake)?.OnAwake();
			}
			Instance = this;
		}

		void Start()
		{
			foreach (var controller in AllControllers)
			{
				(controller as IStart)?.OnStart();
			}
		}

		void Update()
		{
			foreach (var controller in AllControllers)
			{
				(controller as IUpdate)?.OnUpdate();
			}
		}

		void FixedUpdate()
		{
			foreach (var controller in AllControllers)
			{
				(controller as IFixedUpdate)?.OnFixedUpdate();
			}
		}

		void OnDestroy()
		{
			foreach (var controller in AllControllers)
			{
				(controller as IDestroy)?.OnDestroy();
				(controller as IControllerForComponent)?.Deinit();
			}
		}
	}
}