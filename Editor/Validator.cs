using UnityEngine;
using UnityEditor.Callbacks;

using System;
using System.Linq;
using System.Reflection;

namespace Grigorov.Unity.Controllers.Editor
{
	public static class Validator
	{
		static Assembly Assembly      => Assembly.Load("Assembly-CSharp");
		static Type     ComponentType => typeof(Component);

		[DidReloadScripts]
		static void OnScriptsReloaded()
		{
			CheckInterfacesInComponents();
		}

		static void CheckInterfacesInComponents()
		{
			var components = Assembly.GetTypes()
				.Where(t => t.IsSubclassOf(ComponentType));

			var log = "<b>[{0}]</b> Do not use the <b>[{1}]</b> method. Implement the method <b>[{2}]</b> and add class in <b>[UpdateController]</b>";
			foreach (var component in components)
			{
				if (CheckInterface(component, typeof(IUpdate)) && MethodExists(component, "Update"))
				{
					Debug.LogErrorFormat(log, component.Name, "Update", "IUpdate.OnUpdate");
				}

				if (CheckInterface(component, typeof(ILateUpdate)) && MethodExists(component, "LateUpdate"))
				{
					Debug.LogErrorFormat(log, component.Name, "LateUpdate", "ILateUpdate.OnLateUpdate");
				}

				if (CheckInterface(component, typeof(IFixedUpdate)) && MethodExists(component, "FixedUpdate"))
				{
					Debug.LogErrorFormat(log, component.Name, "FixedUpdate", "IFixedUpdate.OnFixedUpdate");
				}
			}
		}

		static bool MethodExists(Type type, string methodName)
		{
			var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).ToList();
			return methods.Exists(m => m.Name == methodName);
		}

		static bool CheckInterface(Type type, Type interfaceType)
		{
			var interf = type.GetInterface(interfaceType.ToString());
			return (interf != null);
		}
	}
}