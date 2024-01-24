using UnityEngine;

namespace PHATASS.Miscellaneous.Kickers
{
	public abstract class KickerOnBodySleepBase : KickerOnConditionForceBase
	{
	//abstract method implementation
		//Condition is true if every item is sleeping
		protected override bool CheckCondition ()
		{
			foreach (Rigidbody rigidbody in this.rigidbodyList)
			{
				if (!rigidbody.IsSleeping()) { return false; }
			}

			foreach (ArticulationBody articulationBody in this.articulationBodyList)
			{
				if (!articulationBody.IsSleeping()) { return false; }
			}

			return true;
		}
	//ENDOF abstract method implementation
	}
}