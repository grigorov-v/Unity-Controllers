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

		List<object> AllControllers => Controller.AllControllers;

		void Awake()
		{
			AllControllers.ForEach(c => RegisterController(c));
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
			ClearAll();
		}

		void RegisterController(object controller) {
			if ( controller is IAwake ) {
				_awakes.Add(controller as IAwake);
			}
			if ( controller is IStart ) {
				_startes.Add(controller as IStart);
			}
			if ( controller is IUpdate ) {
				_updates.Add(controller as IUpdate);
			}
			if ( controller is IFixedUpdate ) {
				_fixedUpdates.Add(controller as IFixedUpdate);
			}
			if ( controller is IDestroy ) {
				_destroes.Add(controller as IDestroy);
			}
		}

		void ClearAll() {
			_awakes.Clear();
			_startes.Clear();
			_updates.Clear();
			_fixedUpdates.Clear();
			_destroes.Clear();
		}
	}
}