using UnityEngine;

namespace PHATASS.Miscellaneous.Kickers
{
	public abstract class OnOffFlickerAutoFireBase : OnOffFlickerBase
	{
	//inherited abstract method implementation
		protected override bool CheckCondition ()
		{ return !flickIsUp; }
	//ENDOF inherited abstract method implementation
	}
}