namespace PHATASS.ActionSystem
{
//Interface representing an element subsceptible to ActionSlap
//Elements capable of receiving an ActionSlap must implement ISlappable
//implements:
//	IAngle2DIntensityEventReceiver, to receive the force/direction data of a slap
	public interface ISlappable :
		IActionReceiver,
		IPushable2D
	{
	}
}