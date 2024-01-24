namespace PHATASS.ActionSystem
{
//Interface representing an object that can be pushed in a 2D environment
//implements:
//	IAngle2DIntensityEventReceiver. IAngle2DIntensity object of received event represents the force/direction of a push
	public interface IPushable2D :
		PHATASS.Utils.Events.IAngle2DIntensityEventReceiver
	{
	// Receives a pushing force knowing its origin point rather than its angle.
	//	originPosition represents the point from where the force is applied
	//	pushForce is the force of the push
	//	if fallOffCurve is set, it will be used to evaluate a force multiplying ratio, using distance as the curve's time dimension
		void PushFromPosition (UnityEngine.Vector2 originPosition, float pushForce, UnityEngine.AnimationCurve fallOffCurve = null);
	}
}