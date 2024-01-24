using UnityEngine;

namespace PHATASS.Miscellaneous.Kickers
{
	public class KickerAutoFireRandomFloatEvent : KickerOnConditionRandomFloatEvent
	{
	//serialized properties 
	//ENDOF serialized properties 

	//IKicker implementation
	//ENDOF IKicker implementation

	//abstract method implementation
		//checkCondition is always true so kick repeats constantly every interval
		protected override bool CheckCondition ()
		{
			return true;
		}
	//ENDOF abstract method implementation
	}
}