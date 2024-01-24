using UnityEngine;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

using SerializableAnimatorVariableIdentifier = PHATASS.Utils.Types.SerializableAnimatorVariableIdentifier;

using IToggleable = PHATASS.Utils.Types.Toggleables.IToggleable;

using static PHATASS.Utils.Extensions.AnimatorListExtensions;

namespace PHATASS.InteractableSystem
{
	//button that stays locked on after activation
	public class InteractableOnOffSwitch :
		InteractableTriggerOnRelease
	{
	//serialized fields
		[SerializeField]
		[Tooltip("Toggleable element this interactable switches on/off")]
		[SerializedTypeRestriction(typeof (IToggleable))]
		private UnityEngine.Object _linkedToggleable;
		protected IToggleable linkedToggleable { get { return (this._linkedToggleable as IToggleable); }}

		[Tooltip("callback stack to execute upon switching to inactive state")]
		[SerializeField]
		private UnityEngine.Events.UnityEvent deactivationCallbacks = null;

		[Tooltip("Animator bool set to TRUE when switch is ON, set to FALSE when switch is OFF")]
		[SerializeField]
		private SerializableAnimatorVariableIdentifier toggledOnAnimatorBool = "ToggledOn";

		[Tooltip("Animator trigger set when this switch is MANUALLY interacted - not triggered on state changes coming from the backing field")]
		[SerializeField]
		private SerializableAnimatorVariableIdentifier manualInteractionAnimatorTrigger = "ManualInteraction";
	//ENDOF serialized fields

	//MonoBehaviour lifecycle
		protected void Awake ()
		{
			//base.Awake();
			this.InitializeButtonState();
		}

		protected void Update ()
		{
			this.UpdateFromLinkedState();
		}
	//ENDOF MonoBehaviour lifecycle

	//private properties
		private bool linkedState
		{
			get { return (this.linkedToggleable != null) ? this.linkedToggleable.state : false; }
			set
			{
				if (this.linkedToggleable != null) { this.linkedToggleable.state = value; }
				this.animators.ESetBool(this.toggledOnAnimatorBool, this.linkedState);
			}
		}

		private bool toggleState = false;
	//ENDOF private properties

	//private methods
		private void UpdateFromLinkedState ()
		{
			if (this.toggleState != this.linkedState)
			{
				this.SetState(this.linkedState);
			}
		}

		//toggles linked toggleable alternatingly on/off
		private void SwitchState ()
		{
			this.SetState(!this.linkedState);
		}

		private void SetState (bool state)
		{
			this.linkedState = state;

			//trigger callbacks only on a state change
			if (this.linkedState != this.toggleState)
			{
				if (this.linkedState == true) { this.TriggerActivationCallbacks(); }
				else { this.TriggerDeactivationCallbacks(); }

				this.toggleState = this.linkedState;
			}
		}

		private void TriggerDeactivationCallbacks () { this.deactivationCallbacks?.Invoke(); }

		//change animator state without performing any trigger effects
		private void InitializeButtonState ()
		{
			this.toggleState = this.linkedState;
			this.linkedState = this.toggleState;
		}
	//ENDOF private methods

	//method overrides
		protected override void InteractableTriggered ()
		{
			if (this.linkedToggleable == null) { return; }
			// set the animator trigger that tells the switch it has to trigger as if it was pressed to distinguish it from state changes originating from the linked toggleable changing state
			this.animators.ESetTrigger(this.manualInteractionAnimatorTrigger);
			this.SwitchState();
		}
	//ENDOF method overrides
	}
}