using UnityEngine;
using System;
using System.Collections.Generic;

namespace Grigorov.Controllers
{
	public class ControlledBehaviour<T> : MonoBehaviour where T : ControlledBehaviour<T>
	{
		static List<T> _allObjects = new List<T>();

		IUpdate      _update      = null;
		IFixedUpdate _fixedUpdate = null;

		protected virtual void Awake()
		{
			_allObjects.Add(this as T);
			_update      = this as IUpdate;
			_fixedUpdate = this as IFixedUpdate;
		}

		protected virtual void OnDestroy()
		{
			_allObjects.Remove(this as T);
		}

		public static void CallAction(Action<T> action)
		{
			foreach (var obj in _allObjects)
			{
				action?.Invoke(obj);
			}
		}

		public static void CallUpdate()
		{
			foreach (var obj in _allObjects)
			{
				if (obj._update == null)
				{
					Debug.LogErrorFormat("The class {0} does not inherit from IUpdate", typeof(T).ToString());
					continue;
				}
				obj._update.OnUpdate();
			}
		}

		public static void CallFixedUpdate()
		{
			foreach (var cb in _allObjects)
			{
				if (cb._fixedUpdate == null)
				{
					Debug.LogErrorFormat("The class {0} does not inherit from IFixedUpdate", typeof(T).ToString());
					continue;
				}
				cb._fixedUpdate.OnFixedUpdate();
			}
		}
	}
}