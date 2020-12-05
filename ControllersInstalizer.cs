using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Grigorov.Unity.Controllers
{
	public static class ControllersInstalizer
	{
		static Dictionary<Type, bool> _initedStatuses = new Dictionary<Type, bool>();

		public static void Init(KeyValuePair<Type, object> controller)
		{
			Init(controller.Key, controller.Value);
		}

		public static void Init(Type type, object controller)
		{
			if (IsInitedController(type))
			{
				return;
			}
			(controller as IInit)?.OnInit();
			_initedStatuses[type] = true;
		}

		public static void Reset(KeyValuePair<Type, object> controller)
		{
			(controller.Value as IReset)?.OnReset();
			_initedStatuses[controller.Key] = false;
		}

		public static Dictionary<Type, object> CreateAllControllers()
		{
			var types = Assembly.GetExecutingAssembly()
				.GetTypes()
				.Where(t => t.GetCustomAttributes(typeof(ControllerAttribute), true).Length > 0);

			var result = new Dictionary<Type, object>();
			foreach (var type in types)
			{
				result[type] = Activator.CreateInstance(type);
			}
			return result;
		}

		static bool IsInitedController(Type typeController)
		{
			return _initedStatuses.ContainsKey(typeController) ? _initedStatuses[typeController] : false;
		}
	}
}