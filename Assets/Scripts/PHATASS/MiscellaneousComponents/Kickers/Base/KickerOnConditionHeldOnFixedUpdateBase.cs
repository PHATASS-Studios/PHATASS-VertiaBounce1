using UnityEngine;

namespace PHATASS.Miscellaneous.Kickers
{
	public abstract class KickerOnConditionHeldOnFixedUpdateBase : KickerOnConditionHeldBase
	{
	//MonoBehaviour Lifecycle
		public virtual void FixedUpdate()
		{
			UpdateCondition(Time.fixedDeltaTime);
		}
	//ENDOF MonoBehaviour Lifecycle
	}
}