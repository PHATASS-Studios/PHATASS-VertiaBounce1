using UnityEngine;

using IFloatValue = PHATASS.Utils.Types.Values.IFloatValue;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

namespace PHATASS.Miscellaneous.TransformEffects
{
	public class ArticulationBodyZDriveTargetFromIFloatValueBehaviour : MonoBehaviour
	{
	//serialized fields
		[Tooltip("ArticulationBody objects whose Z Drive target will be set to the given value")]
		[SerializeField]
		private ArticulationBody[] subjectArticulationBodies;

		[Tooltip("Input IFloatValue. This angle will be set on the objects Z Rotation")]
		[SerializeField]
		[SerializedTypeRestriction(typeof(IFloatValue))]
		private UnityEngine.Object? _sourceValue = null;
		private IFloatValue sourceValue
		{ get {
			if (this._sourceValue == null) { return null; }
			else { return this._sourceValue as IFloatValue; }
		}}

		[Tooltip("Offset added to the input value before applying")]
		[SerializeField]
		private float offset;
	//ENDOF serialized

	//MonoBehaviour lifecycle
		private void Update ()
		{
			this.ApplyUpdate();
		}
	//ENDOF MonoBehaviour lifecycle

	//private methods
		private void ApplyUpdate ()
		{
			if (this.sourceValue == null)
			{
				Debug.LogError(this.name + " IFloatValue source not set");
				return;
			}

			float desiredValue = this.sourceValue.value + this.offset;

			foreach (ArticulationBody body in this.subjectArticulationBodies)
			{
				body.SetDriveTarget(ArticulationDriveAxis.Z, desiredValue);
			}
		}
	//ENDOF private methods
	}
}