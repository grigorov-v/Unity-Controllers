using UnityEngine;
using System.Collections.Generic;

namespace Grigorov.Controllers
{
	public class ControllersProcessor : MonoBehaviour
	{
		public static ControllersProcessor Instance { get; private set; }

		List<IAwake>       _awakes       = new List<IAwake>();
		List<IStart>       _startes      = new List<IStart>();
		List<IUpdate>      _updates      = new List<IUpdate>();
		List<IFixedUpdate> _fixedUpdates = new List<IFixedUpdate>();
		List<IDestroy>     _destroes     = new List<IDestroy>();

		List<IControllerForComponent> _controllersForComponent = new List<IControllerForComponent>();

		List<object> AllControllers => Controller.AllControllers;

		void Awake()
		{
			AllControllers.ForEach(c => RegisterController(c));
			_controllersForComponent.ForEach(cfc => cfc?.Init());
			_awakes.ForEach(a => a?.OnAwake());
			Instance = this;
		}

		void Start()
		{
			_startes.ForEach(s => s?.OnStart());
		}

		void Update()
		{
			_updates.ForEach(u => u?.OnUpdate());
		}

		void FixedUpdate()
		{
			_fixedUpdates.ForEach(u => u?.OnFixedUpdate());
		}

		void OnDestroy()
		{
			_destroes.ForEach(d => d?.OnDestroy());
			_controllersForComponent.ForEach(cfc => cfc?.Deinit());
			ClearAll();
		}

		void RegisterController(object controller)
		{
			TryAddControllerInList(_awakes, controller);
			TryAddControllerInList(_startes, controller);
			TryAddControllerInList(_updates, controller);
			TryAddControllerInList(_fixedUpdates, controller);
			TryAddControllerInList(_destroes, controller);
			TryAddControllerInList(_controllersForComponent, controller);
		}

		void TryAddControllerInList<T>(List<T> list, object obj) where T : class
		{
			list.RemoveAll(elem => elem == null);
			if (obj is T)
			{
				list.Add(obj as T);
			}
		}

		void ClearAll()
		{
			_awakes.Clear();
			_startes.Clear();
			_updates.Clear();
			_fixedUpdates.Clear();
			_destroes.Clear();
			_controllersForComponent.Clear();
		}
	}
}