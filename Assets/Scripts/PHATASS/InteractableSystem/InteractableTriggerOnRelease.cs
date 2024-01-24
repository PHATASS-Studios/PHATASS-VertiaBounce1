using UnityEngine;

using UnityEvent = UnityEngine.Events.UnityEvent;

using EButtonInputState = PHATASS.InputSystem.EButtonInputState; //EButtonInputState
using SerializableAnimatorVariableIdentifier = PHATASS.Utils.Types.SerializableAnimatorVariableIdentifier;

using static PHATASS.Utils.Extensions.AnimatorListExtensions;

namespace PHATASS.InteractableSystem
{
// Interactable designed to be used as a simple button.
//
//	base.activationCallbacks will be triggered when FULL PRESS is finished on this interactable:
//		this means, click (start) the interactable, DON'T leave the button, and release (end) the click
//
//	pressStartedCallbacks will be triggered when any interaction is STARTED
//
	public class InteractableTriggerOnRelease : InteractableBase
	{
	//serialized fields and properties
		[Tooltip("Animator bool set to TRUE when interactable is being pressed, set to FALSE when interactable is released")]
		[SerializeField]
		private SerializableAnimatorVariableIdentifier pressedAnimatorBool = "Pressed";

		[Tooltip("These callbacks will be triggered when STARTING a button press")]
		[SerializeField]
		private UnityEvent pressStartedCallbacks;
	//serialized fields and properties

	//private fields and properties
		//wether this interactable is being pressed down by an active interactor
		//will trigger on input release if input started over this item
		private bool _pressed = false;
		protected virtual bool pressed
		{
			get { return this._pressed; }
			set
			{
				if (value != this._pressed)
				{
					this._pressed = value;
					this.animators.ESetBool(this.pressedAnimatorBool, pressed);
				}
			}
		}
		//protected override bool highlighted
	//ENDOF private fields and properties

	//overrides implementation
		//IInteractable.Interact() override
		protected override void Interact (EButtonInputState state)
		{
			//if initiating a click over this button enter pressed state
			if (state == EButtonInputState.Started)
			{
				this.pressed = true;
				this.TriggerPressed();
			}
			//if ending a click over this button, register the trigger
			else if (state == EButtonInputState.Ended)
			{
				if (this.pressed) 
				{
					this.InteractableTriggered();
					this.pressed = false;
				}
			}
		}

		//when highlight state changes to false, un-set pressed state
		protected override void HoveredStateChanged (bool state)
		{
			if (!state)	{ this.pressed = false; }
			base.HoveredStateChanged(state);
		}
	//overrides implementation

	//private methods
		private void TriggerPressed ()
		{ this.pressStartedCallbacks?.Invoke(); }
	//ENDOF private methods
	}
}