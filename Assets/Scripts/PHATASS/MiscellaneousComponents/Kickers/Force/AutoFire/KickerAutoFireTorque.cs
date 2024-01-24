using UnityEngine;

using RandomSign = PHATASS.Utils.RandomUtils.RandomSign;

namespace PHATASS.Miscellaneous.Kickers
{
	public class KickerAutoFireTorque : KickerOnConditionForceBase
	{
	//serialized properties
		[Tooltip("Determines the sign of the force applied if >0 or <0. If 0, direction is randomly chosen.")]
		[SerializeField]
		private int direction; //if not zero determines the sign of the force applied. If 0, a direction will be chosen randomly each time
	//ENDOF serialized properties 

	//private fields and properties
	//ENDOF private fields and properties

	//IKicker implementation
		//applies a random torque at a random direction in Z axis as the kick
		public override void Kick ()
		{
			Vector3 torque = Vector3.zero;
			if (this.sameValueForAll) { torque = this.GetRandomTorque(); }

			foreach (Rigidbody rigidbody in this.rigidbodyList)
			{
				if (!this.sameValueForAll) { torque = this.GetRandomTorque(); }

				rigidbody.AddTorque(torque: torque, mode: ForceMode.Impulse);
			}

			foreach (ArticulationBody articulationBody in this.articulationBodyList)
			{
				if (!this.sameValueForAll) { torque = this.GetRandomTorque(); }
				articulationBody.AddTorque(torque: torque);
			}
		}
	//ENDOF IKicker implementation

	//MonoBehaviour Lifecycle
	//ENDOF MonoBehaviour Lifecycle

	//abstract method implementation
		//checkCondition is always true so kick repeats constantly every interval
		protected override bool CheckCondition ()
		{
			return true;
		}
	//ENDOF abstract method implementation

	//private methods
		private Vector3 GetRandomTorque ()
		{
			return Vector3.forward * this.randomForce.random * this.GetDirection();
		}

		private int GetDirection ()
		{
			return (direction > 0)
						? 1			//if direction sign is + use 1
					 : (direction < 0)
						? -1		//if direction sign is - use -1
						: RandomSign.Int();	//if none, get a random sign
		}
	//ENDOF private methods
	}
}