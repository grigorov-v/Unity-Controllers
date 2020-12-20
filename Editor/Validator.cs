using UnityEngine;
using UnityEditor.Callbacks;

using System;
using System.Linq;
using System.Reflection;

namespace Grigorov.Unity.Controllers.Editor
{
	public static class Validator
	{
		static Assembly Assembly => Assembly.Load("Assembly-CSharp");
		static Type ComponentType => typeof(Component);
		static Type AttributeType => typeof(ControllerAttribute);

		[DidReloadScripts]
		static void OnScriptsReloaded()
		{
			CheckAttributeInComponents();
			CheckInterfacesInComponents();
		}

		static void CheckAttributeInComponents()
		{
			var componentsWithAttribute = Assembly.GetTypes()
				.Where(t => t.GetCustomAttributes(AttributeType, true).Length > 0)
				.Where(t => t.IsSubclassOf(ComponentType));

			foreach (var component in componentsWithAttribute)
			{
				Debug.LogErrorFormat("<b>[{0}]</b> Attribute <b>[{1}]</b> doesn't work with unity component", component.Name, AttributeType.Name);
			}
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

			log = "<b>[{0}]</b> Interface <b>[{1}]</b> not compatible with unity component";
			foreach (var component in components)
			{
				if (CheckInterface(component, typeof(IInit)))
				{
					Debug.LogErrorFormat(log, component.Name, typeof(IInit).Name);
				}

				if (CheckInterface(component, typeof(IReset)))
				{
					Debug.LogErrorFormat(log, component.Name, typeof(IReset).Name);
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