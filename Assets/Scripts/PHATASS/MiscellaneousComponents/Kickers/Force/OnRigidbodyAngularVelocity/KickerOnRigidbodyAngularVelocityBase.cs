using UnityEngine;

//Kicker that triggers when angular velocity is above or below

namespace PHATASS.Miscellaneous.Kickers
{
	public abstract class KickerOnRigidbodyAngularVelocityBase : KickerOnConditionHeldOnFixedUpdateBase
	{
	//serialized properties
		[SerializeField]
		public Rigidbody targetRigidbody; //will automatically get this gameobject's rigidbody on awake if none given

		[SerializeField]
		private float cutoffVelocity = 0f;
		[SerializeField]
		private bool triggerBelowCutoff = true;
	//ENDOF serialized properties

	//MonoBehaviour Lifecycle
		public void Awake ()
		{
			if (!targetRigidbody) targetRigidbody = gameObject.GetComponent<Rigidbody>();
		}
	//ENDOF MonoBehaviour Lifecycle

	//abstract method implementation

		//condition evaluates to true if velocity is not at cutoff nor above/below cutoff
		protected override bool CheckCondition ()
		{
			float velocityMagnitude = targetRigidbody.angularVelocity.magnitude;
			return
				(triggerBelowCutoff && velocityMagnitude < cutoffVelocity) ||
				(!triggerBelowCutoff && velocityMagnitude > cutoffVelocity);
		}
	//ENDOF abstract method implementation
	}
}