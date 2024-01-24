using UnityEngine;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

using IAction = PHATASS.ActionSystem.IAction;
using IInteractor = PHATASS.InteractableSystem.IInteractor;

using SerializableAnimatorVariableIdentifier = PHATASS.Utils.Types.SerializableAnimatorVariableIdentifier;
using IAnimatorVariableIdentifier = PHATASS.Utils.Types.IAnimatorVariableIdentifier;

using IActionUseInteractor = PHATASS.ActionSystem.IActionUseInteractor;

namespace PHATASS.ToolSystem.Tools
{
	public class BaseToolAnimatorControllerBehaviour : MonoBehaviour
	{
	//serialized fields
		[Tooltip("Tool this controller animates")]
		[SerializeField]
		[SerializedTypeRestrictionAttribute(typeof (ITool))]
		private UnityEngine.Object? _tool;
		protected ITool tool { get { return (this._tool as ITool); }}
		private bool toolIsDestroyed { get { return this._tool == null; }}

		[Tooltip("Animator component handling this tool's animations")]
		[SerializeField]
		protected Animator animator;

	 //animation names
	   //base tool animations
		[Tooltip("Bool to set TRUE when DESTROYED")]
		[SerializeField]
		private SerializableAnimatorVariableIdentifier toolDestroyedBoolName = "Destroyed";
		private IAnimatorVariableIdentifier toolDestroyedBoolID { get { return this.toolDestroyedBoolName; }}

		[Tooltip("Bool to set TRUE while AUTOMATED")]
		[SerializeField]
		private SerializableAnimatorVariableIdentifier toolAutomatedBoolName = "Automated";
		private IAnimatorVariableIdentifier toolAutomatedBoolID { get { return this.toolAutomatedBoolName; }}

		[Tooltip("Bool to set TRUE while FOCUSED")]
		[SerializeField]
		private SerializableAnimatorVariableIdentifier toolFocusedBoolName = "Focused";
		private IAnimatorVariableIdentifier toolFocusedBoolID { get { return this.toolFocusedBoolName; }}

	   //interactor animations
		[Tooltip("Bool to set TRUE while HOVERING an interactable")]
		[SerializeField]
		private SerializableAnimatorVariableIdentifier toolHoveringBoolName = "StateHovering";
		private IAnimatorVariableIdentifier toolHoveringBoolID { get { return this.toolHoveringBoolName; }}

		[Tooltip("Bool to set TRUE while CLICKING an interactable")]
		[SerializeField]
		private SerializableAnimatorVariableIdentifier toolClickingBoolName = "StateClicking";
		private IAnimatorVariableIdentifier toolClickingBoolID { get { return this.toolClickingBoolName; }}
	//ENDOF serialized fields

	//private fields
	//ENDOF fields

	//properties
		private IInteractor toolInteractor { get { return this.tool.interactor; }}

		protected IAction toolAction { get { return this.tool.activeAction; }}
	//ENDOF properties

	//MonoBehaviour
		private void Awake ()
		{
			if (this.tool == null) { Debug.LogWarning(this.name + ".tool not set!"); this.enabled = false; }
			if (this.animator == null) { Debug.LogWarning(this.name + ".animator not set!"); }
		}

		private void Update ()
		{
			if (this.toolIsDestroyed)
			{
				this.AnimateDestroyed();
				return;
			}
			this.AnimateBasicBehaviours();
			this.AnimateFromInteractor();
			this.AdditionalAnimationUpdate();
		}
		protected virtual void AdditionalAnimationUpdate() {}
	//ENDOF MonoBehaviour

	//private methods
		private void AnimateDestroyed ()
		{
			this.animator.SetBool(this.toolDestroyedBoolID.variableID, true);
			this.enabled = false;
		}

		private void AnimateBasicBehaviours ()
		{
			this.animator.SetBool(this.toolFocusedBoolID.variableID, this.tool.focused);
			this.animator.SetBool(this.toolAutomatedBoolID.variableID, this.tool.auto);
		}

		private void AnimateFromInteractor ()
		{
			//Clearing the state in interactor related animations is deemed unnecessary as interactors can't currently be removed in play.
			//if newer tools removing/adding interactors cause ghost animation states, consider resetting interactor state here
			if (this.toolInteractor == null) { return; }

			if (this.toolInteractor.IsHovering())
			{ this.animator.SetBool(this.toolHoveringBoolID.variableID, true); }
			else
			{ this.animator.SetBool(this.toolHoveringBoolID.variableID, false); }

			// move this to AnimateFromAction? or keep it here for interactor animation cohesion?
			if (this.toolAction != null
			&& (this.toolAction as IActionUseInteractor) != null
			&& this.toolAction.ongoing)
			{ this.animator.SetBool(this.toolClickingBoolID.variableID, true); }
			else
			{ this.animator.SetBool(this.toolClickingBoolID.variableID, false); }
		}
	//ENDOF methods
	}
}