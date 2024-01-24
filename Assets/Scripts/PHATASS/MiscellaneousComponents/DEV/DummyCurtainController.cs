using UnityEngine;

using ITransitionController = PHATASS.SceneSystem.TransitionSystem.ITransitionController;

using IToggleable = PHATASS.Utils.Types.Toggleables.IToggleable;
using IAnimatedToggleable = PHATASS.Utils.Types.Toggleables.IAnimatedToggleable;

namespace DEV
{
	public class DummyCurtainController :
		PHATASS.ControllerSystem.MonoBehaviourControllerBase <ITransitionController>,
		ITransitionController
	{

	//ITransitionController implementation
		//opens and closes the curtains, or returns the currently DESIRED state
		bool IToggleable.state
		{
			get { return this.isOpen; }
			set { this.isOpen = value; }
		}

		float IAnimatedToggleable.analogTransitionProgress { get { return 1f; }}

		bool IAnimatedToggleable.StrictStateCheck (bool requiredState)
		{ return this.isOpen == requiredState; }

		bool IAnimatedToggleable.TransitionStateWithCallback (bool desiredState, PHATASS.Utils.Types.Toggleables.DParameterlessDelegate finishingCallback)
		{
			this.isOpen = desiredState;
			finishingCallback?.Invoke();
			return true;
		}

		void IAnimatedToggleable.ForceSetState (bool desiredState)
		{
			this.isOpen = desiredState;
		}

	//ENDOF ITransitionController implementation

	//Private fields
		private bool isOpen = false;
	//ENDOF Private fields
	}
}