using UnityEngine;

using IAction = PHATASS.ActionSystem.IAction;
using IInteractor = PHATASS.InteractableSystem.IInteractor;

using SerializableAnimatorVariableIdentifier = PHATASS.Utils.Types.SerializableAnimatorVariableIdentifier;
using IAnimatorVariableIdentifier = PHATASS.Utils.Types.IAnimatorVariableIdentifier;

using IActionGrab = PHATASS.ActionSystem.IActionGrab;
using IActionSlap = PHATASS.ActionSystem.IActionSlap;

namespace PHATASS.ToolSystem.Tools
{
	public class HandToolAnimatorControllerBehaviour : BaseToolAnimatorControllerBehaviour
	{
	//serialized fields
	   //general action animations
		[Tooltip("Bool to set TRUE while using an IActionGrab action")]
		[SerializeField]
		private SerializableAnimatorVariableIdentifier toolActionGrabBoolName = "StateGrab";
		private IAnimatorVariableIdentifier toolActionGrabBoolID { get { return this.toolActionGrabBoolName; }}

		[Tooltip("Bool to set TRUE while using an IActionSlap action")]
		[SerializeField]
		private SerializableAnimatorVariableIdentifier toolActionSlapTriggerName = "StateSlap";
		private IAnimatorVariableIdentifier toolActionSlapTriggerID { get { return this.toolActionSlapTriggerName; }}
	//ENDOF serialized fields

	//private fields
	//ENDOF fields

	//private properties
	//ENDOF properties

	//method overrides
		protected override void AdditionalAnimationUpdate()
		{ this.AnimateFromAction(); }
	//ENDOF method overrides

	//private methods
		private void AnimateFromAction ()
		{
			if (this.toolAction == null)
			{
				this.ResetActionAnimations();
				return;
			}

			if ((this.toolAction as IActionGrab) != null && this.toolAction.ongoing)
			{ this.animator.SetBool(this.toolActionGrabBoolID.variableID, true); }
			else
			{ this.animator.SetBool(this.toolActionGrabBoolID.variableID, false); }

			if ((this.toolAction as IActionSlap) != null && this.toolAction.ongoing)
			{ this.animator.SetTrigger(this.toolActionSlapTriggerID.variableID); }
		}

		private void ResetActionAnimations ()
		{
			this.animator.SetBool(this.toolActionGrabBoolID.variableID, false);
			//this.animator.SetBool(this.toolActionSlapBoolID.variableID, false);
		}
	//ENDOF methods
	}
}