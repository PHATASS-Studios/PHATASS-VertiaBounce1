using ITool = PHATASS.ToolSystem.Tools.ITool; //ITool
using EButtonInputState = PHATASS.InputSystem.EButtonInputState;

namespace PHATASS.ActionSystem
{
	public interface IAction 
	{
		//returns true if this action is currently doing something, like maintaining a grab or slapping
		bool ongoing {get;}	//[CONSIDER] maybe this needs to be typed as EContinuousState with started, held, ended and none states?
		//wether action is to automatically repeat
		bool auto {get;}
		//initialize the action with a reference to the parent tool
		//will return true if action is valid and functional
		bool Initialize (ITool parentTool);
		//receive state of corresponding input medium
		void Input (EButtonInputState state);
		//action update. Must be called once per frame.
		void ActionUpdate ();
		//try to set in automatic state. Returns true on success
		bool Automate ();
		//stop automation
		void DeAutomate ();
		//clears and finishes the action
		void Clear ();	//[TO-DO] remove Clear method, replace by making IAction inherit IDisposable
		//returns true if this action is valid for this hand (targets in range 'n such)
		bool IsValid ();
	}
}