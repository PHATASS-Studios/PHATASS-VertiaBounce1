using AimTargetTrackerActiveTool = PHATASS.EmotionSystem.Aimables.AimTargetTrackerActiveTool;
using IAimTargetTracker = PHATASS.EmotionSystem.Aimables.IAimTargetTracker;

namespace PHATASS.EmotionSystem.FaceSystem
{
	public class EyeControllerSimpleLookAtTool : EyeController
	{
	//serialized fields
		[UnityEngine.Tooltip("Default tracker will follow active tools. Setting a new IAimable.targetTracker will override this until reset to null.")]
		[UnityEngine.SerializeField]
		private AimTargetTrackerActiveTool defaultTracker;
	//ENDOF serialized

	//overrides
		protected override IAimTargetTracker targetTracker {
			get
			{
				if (base.targetTracker != null)
				{ return base.targetTracker; }
				return this.defaultTracker;
			}
			set { base.targetTracker = value; }
		}
	//ENDOF overrides

	//private
		/*
		private IAimTargetTracker CreateToolTracker ()
		{
			return new AimTargetTrackerActiveTool();
		}*/
	//ENDOF private
	}
}
