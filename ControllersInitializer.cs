using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Grigorov.Unity.Controllers {
	public static class ControllersInitializer {
		static readonly Dictionary<Type, bool> _initedStatuses = new Dictionary<Type, bool>();

		public static void Init(KeyValuePair<Type, IController> controller) {
			Init(controller.Key, controller.Value);
		}

		public static void Init(Type type, IController controller) {
			if ( !controller.IsActive ) {
				return;
			}
			
			if ( IsInitedController(type) ) {
				return;
			}

			controller.OnInit();
			_initedStatuses[type] = true;
			
			Debug.Log($"Init: {type}");
		}

		public static void Reset(KeyValuePair<Type, IController> controller) {
			if ( !controller.Value.IsActive ) {
				return;
			}
			
			controller.Value.OnReset();
			_initedStatuses[controller.Key] = false;
			CheckFields(controller.Value);
		}

		public static Dictionary<Type, IController> CreateControllers() {
			var componentType = typeof(Component);
			var types = Assembly.GetExecutingAssembly()
				.GetTypes()
				.Where(CheckControllerInterface)
				.Where(t => !t.IsSubclassOf(componentType));

			var result = new Dictionary<Type, IController>();
			foreach ( var type in types ) {
				result[type] = Activator.CreateInstance(type) as IController;
			}

			return result;
		}

		static bool IsInitedController(Type typeController) {
			return _initedStatuses.ContainsKey(typeController) && _initedStatuses[typeController];
		}

		static bool CheckControllerInterface(Type type) {
			var interfaces = type.GetInterfaces();
			var controllerInterface = typeof(IController);
			return interfaces.Any(inter => inter == controllerInterface);
		}

		static void CheckFields(object controller) {
#if UNITY_EDITOR
			var type = controller.GetType();
			var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			foreach ( var field in fields ) {
				if ( field.FieldType.IsValueType ) {
					continue;
				}

				if ( field.GetValue(controller) == null ) {
					continue;
				}
				
				Debug.LogWarning($"<b>[Controller: {type} | Field: {field.Name}]</b> Field value is not null after reset");
			}
#endif
		}
	}
}