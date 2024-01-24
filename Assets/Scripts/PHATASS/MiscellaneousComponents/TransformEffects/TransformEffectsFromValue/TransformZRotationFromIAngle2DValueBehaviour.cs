using UnityEngine;

using Angle2D = PHATASS.Utils.Types.Angles.Angle2D;
using IAngle2D = PHATASS.Utils.Types.Angles.IAngle2D;
using static PHATASS.Utils.Types.Angles.IAngle2DFactory;

using IAngle2DValue = PHATASS.Utils.Types.Values.IAngle2DValue;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

namespace PHATASS.Miscellaneous.TransformEffects
{
	public class TransformZRotationFromIAngle2DValueBehaviour : MonoBehaviour
	{
	//serialized fields
		[Tooltip("Transforms whose Z rotation will be set to the given IAngle2D value")]
		[SerializeField]
		private Transform[] subjectTransforms;

		[Tooltip("Input IAngle2DValue. This angle will be set on the objects Z Rotation")]
		[SerializeField]
		[SerializedTypeRestriction(typeof(IAngle2DValue))]
		private UnityEngine.Object? _sourceValue = null;
		private IAngle2DValue sourceValue
		{ get {
			if (this._sourceValue == null) { return null; }
			else { return this._sourceValue as IAngle2DValue; }
		}}

		[Tooltip("If true, sets the transform's local rotation instead of their global rotation")]
		[SerializeField]
		private bool setLocalRotation = false;

		[Tooltip("Angular offset added to the input value before applying")]
		[SerializeField]
		private Angle2D offset;
	//ENDOF serialized

	//MonoBehaviour lifecycle
		private void Update ()
		{
			this.UpdateRotations();
		}
	//ENDOF MonoBehaviour lifecycle

	//private methods
		private void UpdateRotations ()
		{
			if (this.sourceValue == null)
			{
				Debug.LogError(this.name + " IAngle2DValue source not set");
				return;
			}

			IAngle2D desiredZAngle = this.sourceValue.value + (IAngle2D) this.offset;

			foreach (Transform subjectTransform in this.subjectTransforms)
			{
				Vector3 desiredRotationVector = new Vector3(
					x: subjectTransform.localRotation.x,
					y: subjectTransform.localRotation.y,
					z: desiredZAngle.degrees
				);

				if (this.setLocalRotation) { subjectTransform.localEulerAngles = desiredRotationVector; }
				else  { subjectTransform.eulerAngles = desiredRotationVector; }
			}
		}
	//ENDOF private methods
	}
}