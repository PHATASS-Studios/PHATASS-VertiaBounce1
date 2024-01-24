using UnityEngine;

using IToolInputState = PHATASS.ToolSystem.IToolInputState;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

using ControllerCache = PHATASS.ControllerSystem.ControllerCache;

using IAction = PHATASS.ActionSystem.IAction;

using EButtonInputState = PHATASS.InputSystem.EButtonInputState;
using IInteractor = PHATASS.InteractableSystem.IInteractor;

using IActionUseInteractor = PHATASS.ActionSystem.IActionUseInteractor;

using SerializableAnimatorVariableIdentifier = PHATASS.Utils.Types.SerializableAnimatorVariableIdentifier;

namespace PHATASS.ToolSystem.Tools
{
	//[RequireComponent(typeof(Animator))]
	public abstract class ToolBase : MonoBehaviour, ITool
	{
	//serialized fields and properties
		[Tooltip("time a hand stays alive after destruction, to ensure effects play out correctly")]
		[SerializeField]
		protected float destructionTimer = 1.0f;
		//cached reference to animator component
		[SerializeField]
		protected Animator animator;

		[Tooltip("Animator bool is set to TRUE while tool is focused, set to FALSE while tool is not focused")]
		[SerializeField]
		private SerializableAnimatorVariableIdentifier toolFocusedAnimatorBool = "Focused";

		[Tooltip("Animator bool is set to TRUE while tool is automated, set to FALSE while tool is not automated")]
		[SerializeField]
		private SerializableAnimatorVariableIdentifier toolAutomatedAnimatorBool = "Automated";
	//serialized fields and properties

	//Local variables
		//current action
		protected IAction action = null;
	//ENDOF Local variables

	//MonoBehaviour lifecycle Implementation
		protected virtual void Awake ()
		{
			if (this.animator == null) { this.animator = this.GetComponentInChildren<Animator>(); }
			//removed for flulidity. implementors should serialize this. this.interactor = this.GetComponentInChildren<IInteractor>();
		}

		protected virtual void Update ()
		{
			this.ProcessInput();
			this.action?.ActionUpdate();
			this.UpdateInteractor();
		}
	//MonoBehaviour LifeCycle Implementation

	//ITool implementation
		Transform ITool.transform { get { return this.transform; }}
		GameObject ITool.gameObject { get { return this.gameObject; }}

		IInteractor ITool.interactor { get { return this.interactor; }}
			[Tooltip("Interactor component used by UseInteractor actions. REQUIRED if UseInteractor actions are used by this tool.")]
			[SerializeField]
			[SerializedTypeRestriction(typeof(IInteractor))]
			private UnityEngine.Object? _interactor;
			protected IInteractor interactor { get { return this._interactor as IInteractor; }}

		IAction ITool.activeAction { get { return this.action; } }

		Vector3 ITool.position { get { return this.position; } set { this.position = value; }}
			protected Vector3 position
			{
				set 
				{
					if (this.auto) return;
					this.transform.position = ControllerCache.viewportController.ClampPositionToViewport(value);
				}
				get { return this.transform.position; }
			}

		//wether the hand is on focus or not
		bool ITool.focused { get { return this.focused; } set { this.focused = value; }}
			private bool _focused = false;
			protected bool focused
			{
				get { return _focused; }
				set
				{
					if (this._focused == true && value == false)
					{
						//set backing field to false immediately before triggering LostFocus to avoid accidental infinite recursive loop
						this._focused = false;
						this.LostFocus();
					}
					else
					{
						this._focused = value;
					}

					this.animator.SetBool(this.toolFocusedAnimatorBool, value);
				}
			}

		//is this tool in auto mode
		bool ITool.auto { get { return this.auto; }}
			private bool _auto = false;
			protected bool auto
			{
				get { return this._auto; }
				set
				{
					this._auto = value;
					this.animator.SetBool(this.toolAutomatedAnimatorBool, value);
				}
			}

		//wether the hand is in an idle state (not focused nor automating an action)
		bool ITool.idle { get { return this.idle; }}
			protected bool idle
			{
				get { return (!this.focused && !this.auto); }
			}

		//input state used to control this tool
		IToolInputState ITool.input { get { return this.input; } set { this.input = value; }}
			private IToolInputState input { get { return this._input; } set { this._input = value; }}
			private IToolInputState _input;

		//call to force clearing active action
		void ITool.ClearAction () { this.ClearAction(); }
			private void ClearAction ()
			{
				this.action?.Clear();
			}

		//Called by the current action to remove itself
			//[!!] This violates inversion of control - must change
		void ITool.ActionEnded (IAction thisAction) { this.ActionEnded(thisAction); }
			private void ActionEnded (IAction thisAction)
			{
				if (this.action != thisAction)
				{
					//Debug.LogError("ToolBase.ActionEnded() received wrong action id");
					return;
				}

				this.action = null;
			}

		//deletes this tool
			//[TO-DO]: Maybe remove the protected virtual implementation and make this INTERFACE-ONLY access, to prevent destroying the tool without going through the manager
		void ITool.Delete () { this.Delete(); }
			protected virtual void Delete ()
			{
				GameObject.Destroy(this.gameObject, destructionTimer);
				Component.Destroy(this);
			}
	//ENDOF ITool implementation

	//Private functionality
		//read and process inputs if available
		private void ProcessInput ()
		{
			if (this.input == null) { return; }
			this.Move(this.input.scaledDelta);
			this.MainInput(this.input.primaryInputState);
		}

		//move the tool in worldspace
		private void Move (Vector3 delta)
		{
			this.position = this.position + delta;
		}

		//process main input state
		private void MainInput (EButtonInputState state)
		{
			//if automated ignore input save for determining wether to finish or not
			if (this.auto)
			{
				if (state == EButtonInputState.Started)
				{
					this.DeAutomate();
				}
				return;
			}
			
			if (state == EButtonInputState.Held) { this.InputHeld(); }
			else if (state == EButtonInputState.Started) { this.InputStarted(); }
			else if (state == EButtonInputState.Ended) { this.InputEnded(); }

			if (this.action != null)
			{
				this.action.Input(state);
			}
		}

		//Start an action of type T unless its the type currently active
		//the initialize it with a reference to ourselves and return its startup validity check
		protected bool SetAction <T> () where T : class, IAction, new()
		{
			//if the current action is NOT of the same type as the target
			//only then attempt to create a new action
			if ((this.action as T) == null)
			{
				//create the new action
				IAction newAction = new T ();
				//initialize it with a proper reference and check if it's valid
				if (newAction.Initialize((ITool)this))
				{
					this.action?.Clear(); //call Clear on the previous action for cleanup
					this.action = newAction; //store the new action
					return true;	//return true indicating valid action
				}

				return false;	//return false indicating failed action
			}
			//if the current action IS of the target type, re-initialize it and return its validity check
			return this.action.IsValid();
		}

		//called when losing focus
		//when being unfocused the tool tries to automate the ongoing action
		protected virtual void LostFocus ()
		{
			if (!this.auto)
			{
				this.Automate();
			}
		}

		//attempt to set in automated mode. 
		protected void Automate ()
		{
			if (this.action != null && !this.action.auto)
			{
				this.auto = this.action.Automate();
				//Debug.Log("automating tool > " + this.auto);
			}
		}

		//finish auto mode
		protected void DeAutomate ()
		{
			if (this.action != null && this.action.auto)
			{
				this.action.DeAutomate();
			}
			this.auto = false;
		}

		protected void SelfDestruct ()
		{
			ControllerCache.toolManager.DeleteTool(this);
		}

		//Propagate hover using available interactor, but only if we have an interactor, are idle, and existing action is not ongoing.
		private void UpdateInteractor ()
		{
			if (CanUseInteractor()) { this.interactor.PropagateHover(); }

			bool CanUseInteractor ()
			{
				if (this.interactor == null) { return false; }
				if (this.idle) { return false; }

				//if return false if an action NOT IActionUseInteractor is ongoing
				if (this.action != null
				&& (this.action as IActionUseInteractor) == null
				&& this.action.ongoing)
				{ return false; }
				
				return true;
			}
		}
	//ENDOF Private functionality

	//Protected abstract method exposed for implementation
		protected abstract void InputStarted ();
		protected abstract void InputHeld ();
		protected abstract void InputEnded ();
	//ENDOF Protected abstract method exposed for implementation
	}
}