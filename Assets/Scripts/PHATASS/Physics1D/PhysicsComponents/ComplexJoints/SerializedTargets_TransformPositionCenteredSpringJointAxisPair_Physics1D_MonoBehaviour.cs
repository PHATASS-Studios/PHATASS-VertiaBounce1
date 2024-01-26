using UnityEngine;

using IPhysicsBody1D = PHATASS.Utils.Physics.Physics1D.IPhysicsBody1D;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

/*
using Physics = PHATASS.Utils.Physics;

using DoubleRange = PHATASS.Utils.Types.Ranges.DoubleRange;
using IDoubleRange = PHATASS.Utils.Types.Ranges.IDoubleRange;

using IVector2Value = PHATASS.Utils.Types.Values.IVector2Value;

using Vector2Axis = PHATASS.Utils.Types.Values.Vector2Axis;

using static PHATASS.Utils.Types.Values.IVector2ValueExtensions;
*/
namespace PHATASS.L2DTools.Physics1DComponents
{
// Pair of Physics1D joints centered around this gameobject's position
// Center value is clamped and converted to a specified range
//
//	This variant automatically connects the joints to respective serialized targets
	public class SerializedTargets_TransformPositionCenteredSpringJointAxisPair_Physics1D_MonoBehaviour :
		TransformPositionCenteredSpringJointAxisPair_Physics1D_MonoBehaviour
	{
	//serialized fields
		[Tooltip("Physics1D Object that will receive incoming forces corresponding to the X (Horizontal) axis. Will be ignored if not set.")]
		[SerializeField]
		[SerializedTypeRestriction(typeof(IPhysicsBody1D))]
		private UnityEngine.Object? _horizontalAxisBody = null;
		private IPhysicsBody1D horizontalAxisBody
		{ get {
			if (this._horizontalAxisBody == null) { return null; }
			else { return this._horizontalAxisBody as IPhysicsBody1D; }
		}}

		[Tooltip("Physics1D Object that will receive incoming forces corresponding to the Y (Vertical) axis. Will be ignored if not set.")]
		[SerializeField]
		[SerializedTypeRestriction(typeof(IPhysicsBody1D))]
		private UnityEngine.Object? _verticalAxisBody = null;
		private IPhysicsBody1D verticalAxisBody
		{ get {
			if (this._verticalAxisBody == null) { return null; }
			else { return this._verticalAxisBody as IPhysicsBody1D; }
		}}
	//ENDOF serialized fields

	//MonoBehaviour lifecycle
		private void Awake ()
		{
			if (this.xAxisJoint != null)
			{ this.xAxisJoint.primarySubject = this.horizontalAxisBody; }

			if (this.yAxisJoint != null)
			{ this.yAxisJoint.primarySubject = this.verticalAxisBody; }
		}
	//ENDOF MonoBehaviour lifecycle
	}
}
