using EButtonInputState = PHATASS.InputSystem.EButtonInputState;

namespace PHATASS.InteractableSystem
{
	//interactable interface element, like a button
	public interface IInteractor
	{
		//process input. returns true if an interactable is in range
		bool Input (EButtonInputState state);

		//Check if hovering over a valid interactable - does NOT propagate hovering calls
		bool IsHovering ();

		//propagate hovering call to IInteractable under the interactor
		//returns true if an IInteractable was found
		bool PropagateHover ();
	}
}