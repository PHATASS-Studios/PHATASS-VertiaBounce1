using Vector2 = UnityEngine.Vector2;

namespace PHATASS.InputSystem
{
	public interface IInputMovementDelta
	{
		Vector2 rawDelta { get; }			//input movement delta for last frame
		Vector2 screenSpaceDelta { get; }	//delta, transformed into screen space

		Vector2 scaledDelta { get; }		//delta, scaled by sensitivity, screen size, and other factors
	}
}		