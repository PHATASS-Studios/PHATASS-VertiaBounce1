
using ISceneController = PHATASS.SceneSystem.ISceneController;
using ITransitionController = PHATASS.SceneSystem.TransitionSystem.ITransitionController;
using IViewportController = PHATASS.CameraSystem.IViewportController;
using IInputController = PHATASS.InputSystem.IInputController;
using IToolManager = PHATASS.ToolSystem.Managers.IToolManager;
using IMusicController = PHATASS.AudioSystem.IMusicController;
using ISettingsProvider = PHATASS.SettingSystem.ISettingsProvider;

namespace PHATASS.ControllerSystem
{
	public static class ControllerCache
	{
	//private methods
		//will return false if controller needs to be refreshed
		private static bool ControllerIsValid (IController controller)
		{
			return (controller != null && controller.isValid);
		}

		//if controller is not up to date return a fresh reference
		private static TController ValidateController <TController>
			(ref TController controller)
			where TController : IController
		{
			if (!ControllerIsValid(controller))
			{ controller = ControllerProvider.GetController<TController>(); }
			return controller;
		}
	//ENDOF private methods

	//scene controller
		private static ISceneController _sceneController;
		public static ISceneController sceneController
		{
			get
			{
				return ValidateController<ISceneController>(ref _sceneController);
			}
		}
	//ENDOF

	//transition controller
		private static ITransitionController _transitionController;
		public static ITransitionController transitionController
		{
			get
			{
				return ValidateController<ITransitionController>(ref _transitionController);
			}
		}
	//ENDOF

	//viewport controller
		private static IViewportController _viewportController;
		public static IViewportController viewportController
		{
			get	
			{
				return ValidateController<IViewportController>(ref _viewportController);
			}
		}
	//ENDOF

	//input controller
		private static IInputController _inputController;
		public static IInputController inputController
		{
			get
			{
				return ValidateController<IInputController>(ref _inputController);
			}
		}
	//ENDOF

	//toolManager
		private static IToolManager _toolManager;
		public static IToolManager toolManager
		{
			get
			{
				return ValidateController<IToolManager>(ref _toolManager);
			}
		}
	//ENDOF

	//music controller
		private static IMusicController _musicController;
		public static IMusicController musicController
		{
			get
			{
				return ValidateController<IMusicController>(ref _musicController);
			}
		}
	//ENDOF

	//action settings provider controller
		private static ISettingsProvider _settingsProvider;
		public static ISettingsProvider settingsProvider
		{
			get
			{
				return ValidateController<ISettingsProvider>(ref _settingsProvider);
			}
		}
	//ENDOF
	}
}
