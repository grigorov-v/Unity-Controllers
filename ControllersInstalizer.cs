using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using UnityEngine;

namespace Grigorov.Unity.Controllers
{
	public static class ControllersInstalizer
	{
		static Dictionary<Type, bool> _initedStatuses = new Dictionary<Type, bool>();

		public static void Init(KeyValuePair<Type, IController> controller)
		{
			Init(controller.Key, controller.Value);
		}

		public static void Init(Type type, IController controller)
		{
			if (IsInitedController(type))
			{
				return;
			}
			controller.OnInit();
			_initedStatuses[type] = true;
		}

		public static void Reset(KeyValuePair<Type, IController> controller)
		{
			controller.Value.OnReset();
			_initedStatuses[controller.Key] = false;
		}

		public static Dictionary<Type, IController> CreateControllers()
		{
			var componentType = typeof(Component);
			var types = Assembly.GetExecutingAssembly()
				.GetTypes()
				.Where(t => CheckControllerInterface(t))
				.Where(t => !t.IsSubclassOf(componentType));

			var result = new Dictionary<Type, IController>();
			foreach (var type in types)
			{
				result[type] = Activator.CreateInstance(type) as IController;
			}
			
			return result;
		}

		static bool IsInitedController(Type typeController)
		{
			return _initedStatuses.ContainsKey(typeController) ? _initedStatuses[typeController] : false;
		}

		static bool CheckControllerInterface(Type type) {
			var interfaces = type.GetInterfaces();
			var controllerInterface = typeof(IController);
			foreach (var intr in interfaces)
			{
				if (intr == controllerInterface)
				{
					return true;
				}
			}
			return false;
		}
	}
}