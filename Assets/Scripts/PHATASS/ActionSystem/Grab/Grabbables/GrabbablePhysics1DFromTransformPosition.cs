using UnityEngine;

using TransformPositionCenteredSpringJointAxisPair_Physics1D_MonoBehaviour = PHATASS.L2DTools.Physics1DComponents.TransformPositionCenteredSpringJointAxisPair_Physics1D_MonoBehaviour;

using IPhysicsBody1D = PHATASS.Utils.Physics.Physics1D.IPhysicsBody1D;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

using static PHATASS.Utils.Extensions.GameObjectExtensions;


namespace PHATASS.ActionSystem
{
// Grabbable monobehaviour that links grabber to a PhysicsBody1D with a fixed joint
//	fixed joint's position
	public class GrabbablePhysics1DFromTransformPosition :
		MonoBehaviour,
		IGrabbable
	{
	//serialized fields
		[Tooltip("Sample joint configuration. Must be of type TransformPositionCenteredSpringJointAxisPair_Physics1D_MonoBehaviour")]
		[SerializeField]
		private TransformPositionCenteredSpringJointAxisPair_Physics1D_MonoBehaviour sampleJoint;

		[Tooltip("Point transformer given to each created joint. Used to convert transform's position into the joint's reference position.")]
		[SerializeField]
		private PHATASS.Utils.Types.PointTransformers.RectSpaceVector2PointTransformer worldSpacePointTransformer;


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
	//ENDOF serialized

	//IGrabbable
		// Creates and returns a joint on originGameObject. This joint will pull on this grabbable as originGameObject moves.
		// In order to rescind the grab, use UnityEngine.Object.Destroy() to destroy the joint components
		UnityEngine.Component IGrabbable.CreateGrabJoint (GameObject originGameObject)
		{ return this.CreateGrabJoint(originGameObject); }
	//ENDOF IGrabbable

	//MonoBehaviour lifecycle
	//ENDOF MonoBehaviour

	//private members
		// Create a joint from sample with the appropriate references to the point transformer and Physics1D Objects
		private UnityEngine.Component CreateGrabJoint (GameObject originGameObject)
		{
			TransformPositionCenteredSpringJointAxisPair_Physics1D_MonoBehaviour joint = originGameObject.ECreateComponent<TransformPositionCenteredSpringJointAxisPair_Physics1D_MonoBehaviour>();

			joint.ConfigureFromSample(
				sample: this.sampleJoint,
				pointTransformer: this.worldSpacePointTransformer,
				xAxisTargetBody: this.horizontalAxisBody,
				yAxisTargetBody: this.verticalAxisBody
			);

			return joint;
		}
	//ENDOF private



	//Editor gizmos
#if UNITY_EDITOR
		private void OnDrawGizmosSelected ()
		{
			this.worldSpacePointTransformer.DrawPrimarySpaceRectGizmo(Color.black);
		}
#endif
	//ENDOF Editor gizmos
	}
}