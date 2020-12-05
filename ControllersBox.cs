using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Grigorov.Unity.Controllers
{
	public static class ControllersBox
	{
		static Dictionary<Type, object> _allControllers   = new Dictionary<Type, object>();
		static Dictionary<Type, bool>   _initedStatuses   = new Dictionary<Type, bool>();
		static UpdateController         _updateController = null;

		public static Dictionary<Type, object> AllControllers
		{
			get
			{
				if (_allControllers.Count == 0)
				{
					_allControllers = CreateAllControllers();
				}
				return _allControllers;
			}
		}

		public static UpdateController UpdateController
		{
			get
			{
				if (_updateController == null)
				{
					_updateController = Get<UpdateController>();
				}
				return _updateController;
			}
		}

		public static T Get<T>(bool tryInit = true) where T : class
		{
			var type = typeof(T);
			var controller = AllControllers[type];
			if (!IsInitedController(type) && tryInit)
			{
				(controller as IInit)?.OnInit();
				ChangeInitedStatus(type);
			}
			return controller as T;
		}

		public static bool IsInitedController(Type typeController)
		{
			return _initedStatuses.ContainsKey(typeController) ? _initedStatuses[typeController] : false;
		}

		public static void ChangeInitedStatus(Type typeController)
		{
			_initedStatuses[typeController] = true;
		}

		public static void ResetInitedStatuses()
		{
			_initedStatuses.Clear();
		}

		static Dictionary<Type, object> CreateAllControllers()
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
	}
}