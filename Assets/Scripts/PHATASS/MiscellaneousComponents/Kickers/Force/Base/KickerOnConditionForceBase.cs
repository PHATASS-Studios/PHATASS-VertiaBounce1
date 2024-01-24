using UnityEngine;

using TRandomFloatRange = PHATASS.Utils.Types.Ranges.ILimitedRange<System.Single>;

namespace PHATASS.Miscellaneous.Kickers
{
	public abstract class KickerOnConditionForceBase : KickerOnConditionHeldOnFixedUpdateBase
	{
	//serialized properties 
		[Tooltip("Range of minimum and maximum force for every kick")]
		[SerializeField]
		private PHATASS.Utils.Types.Ranges.RandomFloatRange _randomForce;
		protected TRandomFloatRange randomForce { get { return this._randomForce; }}

		[Tooltip("If true, the same force will be applied to each physics body during a kick. If false, a different random value will be generated for each separate physics body.")]
		[SerializeField]
		protected bool sameValueForAll = true;

		[SerializeField]
		[Tooltip("List of rigidbodies this kicker affects")]
		protected Rigidbody[] rigidbodyList;

		[SerializeField]
		[Tooltip("List of articulation bodies this kicker affects")]
		protected ArticulationBody[] articulationBodyList;
	//ENDOF serialized properties 

	//private fields and properties
	//ENDOF private fields and properties

	//MonoBehaviour Lifecycle
	//ENDOF MonoBehaviour Lifecycle
	}
}