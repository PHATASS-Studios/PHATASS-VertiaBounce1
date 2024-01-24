using UnityEngine;
using UnityEvent = UnityEngine.Events.UnityEvent;

using SerializableAnimatorVariableIdentifier = PHATASS.Utils.Types.SerializableAnimatorVariableIdentifier;

using EButtonInputState = PHATASS.InputSystem.EButtonInputState; //EButtonInputState

using TPriorizableInteractable = PHATASS.Utils.Types.Priorizables.IPriorizable<PHATASS.InteractableSystem.IInteractable>;

using static PHATASS.Utils.Extensions.AnimatorListExtensions;

namespace PHATASS.InteractableSystem
{
	public abstract class InteractableBase : MonoBehaviour, IInteractable
	{
	//serialized fields and properties
		[Tooltip("callback stack to execute upon activation")]
		[SerializeField]
		private UnityEvent activationCallbacks = null;

		[Tooltip("List of Animators used by this interactable. Will set the corresponding animator variable changes on every one of them at once.")]
		[SerializeField]
		protected Animator[] animators;

		[Tooltip("Animator bool to set to true/false when interactable is being hovered")]
		[SerializeField]
		private SerializableAnimatorVariableIdentifier hoveredAnimatorBool = "Hovered";

		[Tooltip("These callbacks will be triggered each Update this is being hovered")]
		[SerializeField]
		private UnityEvent hoveredCallbacks;
	//serialized fields and properties

	//IInteractable implementation
		//Used to transmit button down, held, and up inputs to this interactable
		//Called by a corresponding interactor
		void IInteractable.Interact (EButtonInputState state) { this.Interact(state); }
		protected abstract void Interact (EButtonInputState state);

		void IInteractable.Hovered () { this.hovered = true; }
	//ENDOF IInteractable implementation

	//IPriorizable implementation
		[Tooltip("Interactables with highest priority (within range) will always be considered first. Ties are generally resolved by distance.")]
		[SerializeField]
		private int _priority = 0;
		int TPriorizableInteractable.priority { get { return this.priority; }}
		protected int priority { get { return this._priority; }}
	//ENDOF IPriorizable

	//IComparable<IPriorizable<IInteractable>>
		int System.IComparable<IInteractable>.CompareTo(IInteractable other)
		{ return this.CompareTo(other); }
		private int CompareTo (IInteractable other)
		{ return other.priority - this.priority; }
	//ENDOF IComparable<IPriorizable<IInteractable>>

	//private fields and properties
		//wether this interactable is being highlighted by an active interactor
		private bool _hovered = false;
		protected virtual bool hovered
		{
			get { return this._hovered; }
			set
			{
				if (value) { this.hoveredSetFrameNumber = Time.frameCount; }
				if (value != hovered)
				{
					this._hovered = value;
					this.HoveredStateChanged(value);
				}
			}
		}
		private int hoveredSetFrameNumber;
	//ENDOF private fields and properties

	//MonoBehaviour lifecycle
		//protected virtual void Awake () {}
		private void Update ()
		{
			if (this.hovered)
			{ this.hoveredCallbacks?.Invoke(); }
		}

		//IF hovered state wasn't set during this frame, reset it
		private void LateUpdate ()
		{ if (this.hoveredSetFrameNumber != Time.frameCount) { this.hovered = false; }}
	//ENDOF MonoBehaviour lifecycle

	//private methods
		protected virtual void HoveredStateChanged (bool state)
		{
			this.animators.ESetBool(this.hoveredAnimatorBool, state);
		}
		
		protected virtual void InteractableTriggered ()
		{
			this.TriggerActivationCallbacks();
		}
		protected void TriggerActivationCallbacks () { this.activationCallbacks?.Invoke(); }
	//ENDOF private methods
	}
}