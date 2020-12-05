using System.Collections.Generic;

namespace Grigorov.Unity.Controllers
{
	[Controller]
	public class UpdateController : IUpdate, IFixedUpdate
	{
		List<IUpdate>      _targetsUpdate      = new List<IUpdate>();
		List<IFixedUpdate> _targetsFixedUpdate = new List<IFixedUpdate>();
		
		public void OnUpdate()
		{
			foreach (var upd in _targetsUpdate)
			{
				upd.OnUpdate();
			}
		}

		public void OnFixedUpdate()
		{
			foreach (var upd in _targetsFixedUpdate)
			{
				upd.OnFixedUpdate();
			}
		}

		public void AddUpdate(IUpdate target)
		{
			if (_targetsUpdate.Exists(t => t == target))
			{
				return;
			}
			_targetsUpdate.Add(target);
		}

		public void AddUpdate(IFixedUpdate target)
		{
			if (_targetsFixedUpdate.Exists(t => t == target))
			{
				return;
			}
			_targetsFixedUpdate.Add(target);
		}

		public void RemoveUpdate(IUpdate target)
		{
			_targetsUpdate.Remove(target);
		}

		public void RemoveUpdate(IFixedUpdate target)
		{
			_targetsFixedUpdate.Remove(target);
		}
	}
}