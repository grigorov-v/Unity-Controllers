using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grigorov.Controllers
{
	public class ControllerForComponent<T> : IControllerForComponent where T : Component
	{
		protected List<T> _components = new List<T>();

		public void Init()
		{
			Clear();
			AddComponents(Component.FindObjectsOfType<T>());
		}

		public void Deinit()
		{
			Clear();
		}

		protected void AddComponents(params T[] components)
		{
			Array.ForEach(components, component => AddComponent(component));
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

		void Clear()
		{
			_components.Clear();
		}
	}
}