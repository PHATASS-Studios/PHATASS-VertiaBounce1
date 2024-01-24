using Vector3 = UnityEngine.Vector3;

namespace PHATASS.EmotionSystem.Aimables
{
	public interface IAimTargetTracker
	{
		//Returns false if target tracking is meant to be inactive. True if active.
		bool trackingActive { get; }

		//Returns the position of the target to aim towards
		// while trackingActive = false, behaviour is undefined/up to the implementor
		Vector3 targetPosition { get; }
	}
}