using System;
using System.Collections.Generic;

namespace Grigorov.Unity.Controllers {
	public static class ControllersBox {
		static Dictionary<Type, IController> _controllers = new Dictionary<Type, IController>();
		static UpdateController _updateController;

		public static Dictionary<Type, IController> Controllers {
			get {
				if ( _controllers.Count == 0 ) {
					_controllers = ControllersInitializer.CreateControllers();
				}

				return _controllers;
			}
		}

		public static UpdateController UpdateController => _updateController ?? (_updateController = Get<UpdateController>());

		public static T Get<T>() where T : class {
			var type = typeof(T);
			var controller = Controllers[type];
			ControllersInitializer.Init(type, controller);
			return controller as T;
		}
	}
}