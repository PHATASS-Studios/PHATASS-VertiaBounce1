using UnityEngine;

using IInteractor = PHATASS.InteractableSystem.IInteractor;	//IInteractor
using EButtonInputState = PHATASS.InputSystem.EButtonInputState;

using ISettingsPackageActionUseInteractor = PHATASS.SettingSystem.ISettingsPackageActionUseInteractor;

namespace PHATASS.ActionSystem
{
	public class ActionUseInteractor :
		ActionRadialBase<ISettingsPackageActionUseInteractor>,
		IActionUseInteractor
	{
	//ActionBase override implementation
		protected override bool ongoing 
		{
			get
			{ return this.interacting || base.ongoing; }
		}
			private bool interacting = false;

		//receive state of corresponding input medium
		protected override void Input (EButtonInputState state)
		{
			//propagate input to the interactor
			//if interactor reports failure end the action
			if (!this.tool.interactor.Input(state))
			{
				interacting = false;
				this.Clear();
				return;
			}
			else { interacting = state == EButtonInputState.Started || state == EButtonInputState.Held; }
		}

		//interaction is valid if hovering an interactable
		protected override bool IsValid ()
		{
			if (this.tool.interactor == null)
			{
				Debug.LogError("Tool " + this.tool.gameObject.name + " is missing an interactor - can't perform ActionUseInteractor");
				return false;
			}
			return this.tool.interactor.IsHovering();
		}

		protected override void ActionUpdate () { }

		//Using an interactor is an entirely non-automatable one-shot action, so automation methods just report failure
		protected override bool Automate () { return false; }
		protected override void DeAutomate () {}

		//on clear ensure exiting animation state
		protected override void Clear ()
		{
			base.Clear();
		}
	//ENDOF ActionBase override implementation

	}
}
