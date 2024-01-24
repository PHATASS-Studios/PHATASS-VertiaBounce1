using UnityEngine;

using EButtonInputState = PHATASS.InputSystem.EButtonInputState;
using IInteractor = PHATASS.InteractableSystem.IInteractor;

using IAction = PHATASS.ActionSystem.IAction;

namespace PHATASS.ToolSystem.Tools
{
	public interface ITool
	{
		Transform transform {get;}
		GameObject gameObject {get;}
		IInteractor interactor {get;}
		IAction activeAction {get;}

		Vector3 position {get; set;}	//position of the hand
		bool focused {get; set;}		//wether the hand is on focus or not
		bool auto {get;}				//returns true if the hand is automated
		bool idle {get;}				//returns true if the hand is idling (not focused, nor automating an action)

		IToolInputState input {get; set;}	//input state used to control this tool

		/*This should maybe stay? commented for now
		//move the hand
		void Move (Vector3 delta);
		//*/

		//call to force clearing active action
		void ClearAction ();

		//Called by the current action to remove itself
	//[!!] This violates inversion of control - must change
		void ActionEnded (IAction thisAction);

		//deletes this tool
		void Delete ();
	}
}