using UnityEngine;

using Physics = PHATASS.Utils.Physics;

using IPhysicsBody1D = PHATASS.Utils.Physics.Physics1D.IPhysicsBody1D;

using DoubleRange = PHATASS.Utils.Types.Ranges.DoubleRange;
using IDoubleRange = PHATASS.Utils.Types.Ranges.IDoubleRange;

using IVector2Value = PHATASS.Utils.Types.Values.IVector2Value;

using Vector2Axis = PHATASS.Utils.Types.Values.Vector2Axis;

using static PHATASS.Utils.Types.Values.IVector2ValueExtensions;

#if UNITY_EDITOR
	using static PHATASS.Utils.Extensions.RectEditorExtensions;
#endif 

namespace PHATASS.L2DTools.Physics1DComponents
{
// Pair of Physics1D joints centered around this gameobject's position
// Center value is clamped and converted to a specified range
// [TO-DO] Move X/Y springs implementation to a base class?
	public class TransformPositionCenteredSpringJointAxisPair_Physics1D_MonoBehaviour :
		MonoBehaviour//PHATASS.Utils.Physics.Physics1D.BaseFixedSpringJoint1DOnUpdateMonoBehaviour
	{
	//serialized fields
		[Tooltip("World-space transformer. Transforms a point from world space into desired joint space")]
		[SerializeField]
		private PHATASS.Utils.Types.PointTransformers.TransformPositionToTransformedVector2Value _pointSpaceSource;
		private IVector2Value pointSpaceSource { get { return this._pointSpaceSource; }}

		[Tooltip("X Axis spring joint properties.")]
		[SerializeField]
		private Physics.Physics1D.FixedSpringJoint1D _xAxisJoint;
		protected Physics.Physics1D.IFixedJoint1D xAxisJoint { get { return this._xAxisJoint;}}

		[Tooltip("Y Axis spring joint properties.")]
		[SerializeField]
		private Physics.Physics1D.FixedSpringJoint1D _yAxisJoint;
		protected Physics.Physics1D.IFixedJoint1D yAxisJoint { get { return this._yAxisJoint;}}
	//ENDOF serialized fields

	//by-sample configuration methods
		//configures this object by replicating a sample's member values and returns itself
		public TransformPositionCenteredSpringJointAxisPair_Physics1D_MonoBehaviour ConfigureFromSample (
			TransformPositionCenteredSpringJointAxisPair_Physics1D_MonoBehaviour sample,
			PHATASS.Utils.Types.PointTransformers.RectSpaceVector2PointTransformer pointTransformer = null,
			IPhysicsBody1D xAxisTargetBody = null,
			IPhysicsBody1D yAxisTargetBody = null

		) {
			this._pointSpaceSource = new PHATASS.Utils.Types.PointTransformers.TransformPositionToTransformedVector2Value(
				sample: sample._pointSpaceSource,
				transform: this.transform,
				transformer: pointTransformer
			);

			if (xAxisTargetBody == null)
			{ this._xAxisJoint = null; }
			else
			{
				this._xAxisJoint = new Physics.Physics1D.FixedSpringJoint1D(
					sample: sample._xAxisJoint,
					targetBody: xAxisTargetBody,
					referenceCenterValue: this.pointSpaceSource.EVector2ValueToAxisDoubleValue(Vector2Axis.x)
				);
			}

			if (yAxisTargetBody == null)
			{ this._yAxisJoint = null; }
			else
			{
				this._yAxisJoint = new Physics.Physics1D.FixedSpringJoint1D(
					sample: sample._yAxisJoint,
					targetBody: yAxisTargetBody,
					referenceCenterValue: this.pointSpaceSource.EVector2ValueToAxisDoubleValue(Vector2Axis.y)
				);
			}

			return this;
		}
	//ENDOF configuration methods

	//MonoBehaviour lifecycle
		private void Start ()
		{
			this.InitializeJointsCenterValue();
		}

		private void InitializeJointsCenterValue ()
		{
			if (this.xAxisJoint != null)
			{ this.xAxisJoint.centerValue = this.pointSpaceSource.EVector2ValueToAxisDoubleValue(Vector2Axis.x); }

			if (this.yAxisJoint != null)
			{ this.yAxisJoint.centerValue = this.pointSpaceSource.EVector2ValueToAxisDoubleValue(Vector2Axis.y); }
		}

		//MonoBehaviour update calls joint update
		private void Update ()
		{
			if (this.xAxisJoint != null) { this.xAxisJoint.Update(Time.deltaTime); }
			if (this.yAxisJoint != null) { this.yAxisJoint.Update(Time.deltaTime); }
		}
	//ENDOF MonoBehaviour lifecycle

	//Editor gizmos
#if UNITY_EDITOR
		private void OnDrawGizmosSelected ()
		{
			this._pointSpaceSource.DrawWorldSpaceRectGizmo(Color.black);
		}
#endif
	//ENDOF Editor gizmos
	}
}
