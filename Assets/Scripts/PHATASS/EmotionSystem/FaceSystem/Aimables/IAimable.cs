using Vector2 = UnityEngine.Vector2;

namespace PHATASS.EmotionSystem.Aimables
{
	public interface IAimable
	{
		//Vector2 position { get; }

		IAimTargetTracker targetTracker { get; set; }
	}
}