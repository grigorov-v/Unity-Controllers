using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Grigorov.Unity.Controllers
{
	public static class ControllersBox
	{
		static Dictionary<Type, object> _allControllers   = new Dictionary<Type, object>();
		static UpdateController         _updateController = null;

		public static Dictionary<Type, object> AllControllers
		{
			get
			{
				if (_allControllers.Count == 0)
				{
					_allControllers = ControllersInstalizer.CreateAllControllers();
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

		public static T Get<T>() where T : class
		{
			var type = typeof(T);
			var controller = AllControllers[type];
			ControllersInstalizer.Init(type, controller);
			return controller as T;
		}
	}
}