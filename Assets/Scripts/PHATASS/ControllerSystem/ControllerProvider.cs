using ServiceContainer = System.ComponentModel.Design.ServiceContainer;

namespace PHATASS.ControllerSystem
{
	public static class ControllerProvider
	{
	//private fields and properties
		private static ServiceContainer serviceContainer;

	//ENDOF private fields and properties


	//public methods
		//return controller for type TController
		public static TController GetController <TController> ()
			where TController : IController
		{
			return (TController) serviceContainer?.GetService(typeof(TController));
		}

		//register a controller instance as TController type
		public static void RegisterController <TController> (IController controller)
			where TController : IController
		{
			InitializeContainer();

			if (GetController<TController>() != null)
			{
				DisposeController<TController>();
			}

			serviceContainer.AddService(typeof(TController), controller);
		}

		//remove the controller of type TController. if a controller parameter is passed, removal will only be performed if controllers coincide
		public static void DisposeController <TController> (IController controller)
			where TController : class, IController
		{
			TController castedController = (TController) controller;
			if (GetController<TController>() == castedController)
			{
				DisposeController<TController>();
			}
		}
		public static void DisposeController <TController> ()
			where TController : IController
		{
			serviceContainer.RemoveService(typeof(TController));
		}
	//ENDOF public methods

	//private methods
		//ensure a container exists
		private static void InitializeContainer ()
		{
			if (serviceContainer == null)
			{ serviceContainer = new ServiceContainer(); }
		}
	//ENDOF private methods
	}
}