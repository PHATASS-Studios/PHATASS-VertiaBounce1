using UnityEngine;

namespace PHATASS.Miscellaneous.Kickers
{
	public abstract class KickerOnConditionHeldOnUpdateBase : KickerOnConditionHeldBase
	{
	//MonoBehaviour Lifecycle
		public virtual void Update()
		{
			UpdateCondition(Time.deltaTime);
		}
	//ENDOF MonoBehaviour Lifecycle
	}
}