using UnityEngine;

using SerializableAnimatorVariableIdentifier = PHATASS.Utils.Types.SerializableAnimatorVariableIdentifier;
using IAnimatorVariableIdentifier = PHATASS.Utils.Types.IAnimatorVariableIdentifier;

using IToggleable = PHATASS.Utils.Types.Toggleables.IToggleable;
using IAnimatedToggleable = PHATASS.Utils.Types.Toggleables.IAnimatedToggleable;
using DParameterlessDelegate = PHATASS.Utils.Types.Toggleables.DParameterlessDelegate;

namespace PHATASS.SceneSystem.TransitionSystem
{
	public class AnimatorBasedTransitionController :
		PHATASS.ControllerSystem.MonoBehaviourControllerBase <ITransitionController>,
		ITransitionController
	{
	//serialized fields
		[Tooltip("These animators will be told to change whenever transition state change is requested")]
		[SerializeField]
		private Animator[] managedAnimators;

		[Tooltip("This is the animator variable name that will be set to TRUE when opening requested and FALSE when closing requested")]
		[SerializeField]
		private SerializableAnimatorVariableIdentifier transitionIsOpenVariableName = "Open";
		//private IAnimatorVariableIdentifier transitionIsOpenVariableName { get { return this._transitionIsOpenVariableName; }}

		[Tooltip("CHANGE THIS THROUGH THE ANIMATOR to signal transition opening progress. 0 = fully closed; 1 = fully opened")]
		[SerializeField]
		private float transitionProgress = 1f;
	//ENDOF serialized

	//ITransitionController
		//opens and closes the curtains, or returns the currently DESIRED state
		bool IToggleable.state
		{
			get { return this.state; }
			set { this.state = value; }
		}

		//returns the state of the transition between 1 and 0, 0 meaning fully closed 1 meaning fully opened
		float IAnimatedToggleable.analogTransitionProgress
		{ get { return this.transitionProgress; }}

		// Returns true ONLY if any transition between states has finished AND current state is requiredState
		bool IAnimatedToggleable.StrictStateCheck (bool requiredState)
		{ return this.StrictStateCheck(requiredState); }

		// Transitions current state to desiredState, and triggers finishingCallback when transition finished
		//	returns false if transition fails because initial state == desiredState, true otherwise
		//	finishingCallback can be null, in which case it won't get invoked
		bool IAnimatedToggleable.TransitionStateWithCallback (bool desiredState, DParameterlessDelegate finishingCallback)
		{ return this.TransitionStateWithCallback(desiredState, finishingCallback); }

		// Immediately force set given state
		//   Actually doesn't force state change, transition covers should never make sudden changes.
		void IAnimatedToggleable.ForceSetState (bool desiredState)
		{ this.state = desiredState; }
	//ENDOF ITransitionController

	//MonoBehaviour lifecycle
		protected virtual void Update ()
		{
			this.TryTriggerCallbacks();
		}
	//ENDOF MonoBehaviour

	//private
		private bool state
		{
			get
			//[TO-DO]: initial state of the animator and the controller may be desynchronized - initialize from animator value? force set animator value on start?
			{ return this._state; }
			set
			{
				if (value != this._state)
				{
					this._state = value;
					foreach (Animator animator in this.managedAnimators)
					{ animator.SetBool(this.transitionIsOpenVariableName, value); }
				}
			}
		}
		private bool _state = true;

		// Returns true ONLY if any transition between states has finished AND current state is requiredState
		protected virtual bool StrictStateCheck (bool requiredState)
		{
			if (!requiredState) { return this.transitionProgress <= 0f; }
			if (requiredState) { return this.transitionProgress <= 0f; }
			return false;
		}

		private bool TransitionStateWithCallback (bool desiredState, DParameterlessDelegate finishingCallback)
		{
			if (this.state == desiredState) { return false; }

			if (desiredState == true) { this.queuedOnEnableCallback = finishingCallback; }
			else { this.queuedOnDisableCallback = finishingCallback; }

			this.state = desiredState;
			return true;
		}

	//callback management
		private DParameterlessDelegate queuedOnEnableCallback = null;
		private DParameterlessDelegate queuedOnDisableCallback = null;

		//tries to trigger OnEnable/OnDisable callbacks if necessary, then resets them.
		private void TryTriggerCallbacks ()
		{
			bool desiredState = this.state;

			if (this.queuedOnEnableCallback != null)
			{
				if (desiredState == true && this.transitionProgress >= 1f)
				{
					this.queuedOnEnableCallback.Invoke();
					this.queuedOnEnableCallback = null;
				}
			}

			if (this.queuedOnDisableCallback != null)
			{
				if (desiredState == false && this.transitionProgress <= 0f)
				{
					this.queuedOnDisableCallback.Invoke();
					this.queuedOnDisableCallback = null;
				}
			}
		}
	//ENDOF callback management
	//ENDOF private
	}
}