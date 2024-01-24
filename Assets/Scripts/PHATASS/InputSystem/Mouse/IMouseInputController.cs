using IToolInputState = PHATASS.ToolSystem.IToolInputState;

namespace PHATASS.InputSystem
{
	public interface IMouseInputController :
		IInputController,
		IToolInputState
	{
	//have these really become obsolete?
		//returns true if mouse button buttonID has been pressed this frame
		bool GetButtonDown (int buttonID);

		//returns true if mouse button buttonID has been held for at least lenght seconds. If reset = true or omitted, resets held counter until lifted and pressed again
		bool GetButtonHeld (int buttonID, float length, bool reset = true);

		//EButtonInputState GetButtonState (int buttonID);	//returns the current state of the button

	//from IToolInputState
	/*
		Vector2 rawDelta { get; }			//input movement delta for last frame
		Vector2 screenSpaceDelta { get; }	//delta, transformed into screen space
		Vector2 scaledDelta { get; }		//delta, scaled by sensitivity, screen size, and other factors

		EButtonInputState primaryInputState;	// state of primary input (left click, or touch lifecycle state)
	*/
	//ENDOF IToolInputState
	}
}