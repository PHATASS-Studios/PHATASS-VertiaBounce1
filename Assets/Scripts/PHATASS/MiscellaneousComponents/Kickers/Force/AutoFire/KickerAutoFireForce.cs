using UnityEngine;

using static PHATASS.Utils.Extensions.Vector3Extensions;

using TFloatRange = PHATASS.Utils.Types.Ranges.ILimitedRange<System.Single>;

namespace PHATASS.Miscellaneous.Kickers
{
	public class KickerAutoFireForce : KickerOnConditionForceBase
	{
	//serialized properties
		[Tooltip("Minimum and maximum Z-Axis world-space angle of the force")]
		[SerializeField]
		private PHATASS.Utils.Types.Ranges.RandomFloatRange _forceAngleRange;
		private TFloatRange forceAngleRange { get { return this._forceAngleRange; }}
	//ENDOF serialized properties 

	//IKicker implementation
		//applies a random force at a random direction as the kick
		public override void Kick ()
		{
			Vector3 force = Vector3.zero;
			if (this.sameValueForAll) { force = this.GetRandomForce(); }

			foreach (Rigidbody rigidbody in this.rigidbodyList)
			{
				if (!this.sameValueForAll) { force = this.GetRandomForce(); }

				rigidbody.AddForce(force: force, mode: ForceMode.Impulse);
			}

			foreach (ArticulationBody articulationBody in this.articulationBodyList)
			{
				if (!this.sameValueForAll) { force = this.GetRandomForce(); }

				articulationBody.AddForce(force: force);
			}
			
		}
	//ENDOF IKicker implementation

	//abstract method implementation
		//checkCondition is always true so kick repeats constantly every interval
		protected override bool CheckCondition ()
		{
			return true;
		}
	//ENDOF abstract method implementation

	//private methods
		private Vector3 GetRandomForce ()
		{
			return this.forceAngleRange.random.EAngleToVector3() * this.randomForce.random;
		}
	//ENDOF private methods
	}
}