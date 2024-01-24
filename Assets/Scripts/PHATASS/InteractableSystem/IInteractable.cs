using EButtonInputState = PHATASS.InputSystem.EButtonInputState;

namespace PHATASS.InteractableSystem
{
	//interactable interface element, like a button
	public interface IInteractable : PHATASS.Utils.Types.Priorizables.IPriorizable<IInteractable>
	{
		//An interactor that detects this insteractable in range must invoke IInteractable.Hovered() each frame
		//this tells the interactor it is being hovered
		void Hovered ();

		//Used to transmit button down, held, and up inputs to this interactable
		//Called by a corresponding interactor
		void Interact (EButtonInputState state);
	}
}