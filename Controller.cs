using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Grigorov.Controllers
{
	public static class Controller
	{
		static List<object> _allControllers = new List<object>();

		public static List<object> AllControllers
		{
			get
			{
				if (_allControllers.Count == 0)
				{
					_allControllers = CreateAllControllers();
				}
				return _allControllers;
			}
		}

		public static T Get<T>() where T : class
		{
			var controller = AllControllers.Find(c => c is T);
			return (controller != null) ? controller as T : default(T);
		}

		static List<object> CreateAllControllers()
		{
			return Assembly.GetExecutingAssembly().GetTypes()
					.Where(t => t.GetCustomAttributes(typeof(ControllerAttribute), true).Length > 0)
					.Select(type => Activator.CreateInstance(type)).ToList();
		}
	}
}