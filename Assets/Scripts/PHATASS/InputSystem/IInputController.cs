using Vector3 = UnityEngine.Vector3;

namespace PHATASS.InputSystem
{
	public interface IInputController :
		PHATASS.ControllerSystem.IController
	{
		float zoomDelta { get; }			//zoom input delta for the frame
		EButtonInputState quitButton { get; }		//will return true if esc key or other quitting input is received
	}
}