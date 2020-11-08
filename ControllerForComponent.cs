using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Grigorov.Controllers
{
	public class ControllerForComponent<T> : IControllerForComponent where T : Component
	{
		protected List<T> _components = new List<T>();

		public void Init()
		{
			_components = Component.FindObjectsOfType<T>().ToList();
		}

		public void Deinit()
		{
			_components.Clear();
		}

		protected void AddComponent(T component)
		{
			if (_components.Exists(c => c == component))
			{
				return;
			}
			_components.Add(component);
		}

		protected void RemoveComponent(T component)
		{
			_components.Remove(component);
		}

		protected void CallAction(Action<T> action)
		{
			foreach (var component in _components)
			{
				action?.Invoke(component);
			}
		}
	}
}