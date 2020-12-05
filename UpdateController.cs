using System.Collections.Generic;

namespace Grigorov.Unity.Controllers
{
	[Controller]
	public class UpdateController
	{
		List<IUpdate>      _targetsUpdate      = new List<IUpdate>();
		List<ILateUpdate>  _targetsLateUpdate  = new List<ILateUpdate>();
		List<IFixedUpdate> _targetsFixedUpdate = new List<IFixedUpdate>();
		
		public void Update()
		{
			foreach (var upd in _targetsUpdate)
			{
				upd.OnUpdate();
			}
		}

	   public void LateUpdate()
		{
			foreach (var upd in _targetsLateUpdate)
			{
				upd.OnLateUpdate();
			}
		}

		public void FixedUpdate()
		{
			foreach (var upd in _targetsFixedUpdate)
			{
				upd.OnFixedUpdate();
			}
		}

		public void AddUpdate(IUpdate target)
		{
			if (CheckNull(target)) return;
			if (_targetsUpdate.Exists(t => t == target)) return;
			_targetsUpdate.Add(target);
		}

		public void AddUpdate(ILateUpdate target)
		{
			if (CheckNull(target)) return;
			if (_targetsLateUpdate.Exists(t => t == target)) return;
			_targetsLateUpdate.Add(target);
		}

		public void AddUpdate(IFixedUpdate target)
		{
			if (CheckNull(target)) return;
			if (_targetsFixedUpdate.Exists(t => t == target)) return;
			_targetsFixedUpdate.Add(target);
		}

		public void RemoveUpdate(IUpdate target)
		{
			_targetsUpdate.Remove(target);
		}

		public void RemoveUpdate(ILateUpdate target)
		{
			_targetsLateUpdate.Remove(target);
		}

		public void RemoveUpdate(IFixedUpdate target)
		{
			_targetsFixedUpdate.Remove(target);
		}

		bool CheckNull(object obj)
		{
			return obj == null;
		}
	}
}