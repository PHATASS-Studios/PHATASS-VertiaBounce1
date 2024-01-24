using UnityEngine;

using RandomFloatRange = PHATASS.Utils.Types.Ranges.RandomFloatRange; //RandomFloatRange
using TFloatRange = PHATASS.Utils.Types.Ranges.ILimitedRange<System.Single>;

namespace PHATASS.Miscellaneous.Kickers
{
	public abstract class KickerOnConditionHeldBase : MonoBehaviour, IKicker
	{
	//serialized properties
		[Tooltip("Time the condition needs to stay true for a kick to trigger. Random between min and max.")]
		[SerializeField]
		private RandomFloatRange _randomDelay = new RandomFloatRange(1, 1);
		private TFloatRange randomDelay { get { return this._randomDelay; }}
	//ENDOF serialized properties 

	//private fields and properties
		private float currentDelay;
		private bool currentCheck;
	 	private bool previousCheck = false;
	//ENDOF private fields and properties

	//IKicker definition
		//executes a momentary effect
		public abstract void Kick ();
	//ENDOF IKicker definition

	//abstract method definition
		protected abstract bool CheckCondition ();
	//ENDOF abstract method definition

	//private methods
	 	//on update check condition state change, timer update
		protected void UpdateCondition (float timeDelta)
		{
			currentCheck = CheckCondition();

			//on condition state change to true, re-initialize timer
			if (currentCheck && !previousCheck)
			{
				currentDelay = randomDelay.random;
			}

			//if condition is true, decrement timer
			if (currentCheck)
			{
				currentDelay -= timeDelta;

				//if timer reaches zero, kick and reset timer
				if (currentDelay <= 0)
				{
					Kick();
					currentDelay = randomDelay.random;
				}
			}
			previousCheck = currentCheck;
		}
	//ENDOF private methods
	}
}