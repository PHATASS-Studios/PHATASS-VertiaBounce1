using UnityEngine;

namespace PHATASS.Miscellaneous.ContinuousEffects
{
	public class ContinuousForce :
		ContinuousUpdateIntensityLerpOnFixedUpdateBase
	{
	//serialized fields
		[SerializeField]
		[Tooltip("Base force vector applied PER SECOND. This is scaled by intensity value and multiplied by fixedDeltaTime.")]
		private Vector3 baseForceVector;

		[SerializeField]
		[Tooltip("List of rigidbodies to apply forces to")]
		private Rigidbody[] rigidbodyList;

		[SerializeField]
		[Tooltip("List of articulation bodies to apply forces to")]
		private ArticulationBody[] articulationBodyList;
	//ENDOF serialized fields

	//MonoBehaviour lifecycle
	//ENDOF MonoBehaviour lifecycle

	//protected class properties
	//ENDOF protected class properties

	//class method overrides
		protected override void UpdateEffect (float intensity)
		{
			Vector3 desiredForceVector = this.baseForceVector * intensity * Time.deltaTime;

			foreach (Rigidbody rigidbody in this.rigidbodyList)
			{ rigidbody.AddForce(desiredForceVector, ForceMode.Impulse); }

			foreach (ArticulationBody articulationBody in this.articulationBodyList)
			{ articulationBody.AddForce(desiredForceVector); }
		}
	//ENDOF class method overrides
	}
}
