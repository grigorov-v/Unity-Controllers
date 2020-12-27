using System;
using System.Collections.Generic;

namespace Grigorov.Unity.Controllers
{
	public static class ControllersBox
	{
		static Dictionary<Type, IController> _controllers      = new Dictionary<Type, IController>();
		static UpdateController              _updateController = null;

		public static Dictionary<Type, IController> Controllers
		{
			get
			{
				if (_controllers.Count == 0)
				{
					_controllers = ControllersInstalizer.CreateControllers();
				}
				return _controllers;
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

		public static T Get<T>() where T : class
		{
			var type = typeof(T);
			var controller = Controllers[type];
			ControllersInstalizer.Init(type, controller);
			return controller as T;
		}
	}
}