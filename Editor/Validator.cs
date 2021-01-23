using System;
using System.Linq;
using System.Reflection;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Grigorov.Unity.Controllers.Editor {
	public static class Validator {
		const string LogFormat = "<b>[{0}]</b> Do not use the <b>[{1}]</b> method. Implement the method <b>[{2}]</b> and add class in <b>[UpdateController]</b>";
		
		static Assembly Assembly      => Assembly.Load("Assembly-CSharp");
		static Type     ComponentType => typeof(Component);

		[DidReloadScripts]
		static void OnScriptsReloaded() {
			CheckInterfacesInComponents();
		}

		static void CheckInterfacesInComponents() {
			var components = Assembly.GetTypes()
				.Where(t => t.IsSubclassOf(ComponentType));

			
			foreach ( var component in components ) {
				if ( CheckInterface(component, typeof(IUpdate)) && MethodExists(component, "Update") ) {
					Debug.LogErrorFormat(LogFormat, component.Name, "Update", "IUpdate.OnUpdate");
				}

				if ( CheckInterface(component, typeof(ILateUpdate)) && MethodExists(component, "LateUpdate") ) {
					Debug.LogErrorFormat(LogFormat, component.Name, "LateUpdate", "ILateUpdate.OnLateUpdate");
				}

				if ( CheckInterface(component, typeof(IFixedUpdate)) && MethodExists(component, "FixedUpdate") ) {
					Debug.LogErrorFormat(LogFormat, component.Name, "FixedUpdate", "IFixedUpdate.OnFixedUpdate");
				}
			}
		}

		static bool MethodExists(Type type, string methodName) {
			return type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
				.ToList()
				.Exists(m => m.Name == methodName);
		}

		static bool CheckInterface(Type type, Type interfaceType) {
			return type.GetInterface(interfaceType.ToString()) != null;
		}
	}
}