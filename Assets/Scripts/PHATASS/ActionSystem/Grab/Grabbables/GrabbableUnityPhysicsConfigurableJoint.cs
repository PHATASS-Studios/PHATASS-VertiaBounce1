using UnityEngine;

using TransformPositionCenteredSpringJointAxisPair_Physics1D_MonoBehaviour = PHATASS.L2DTools.Physics1DComponents.TransformPositionCenteredSpringJointAxisPair_Physics1D_MonoBehaviour;

using IPhysicsBody1D = PHATASS.Utils.Physics.Physics1D.IPhysicsBody1D;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

using static PHATASS.Utils.Extensions.GameObjectExtensions;
using static PHATASS.Utils.Extensions.JointConfigurationExtensions;

namespace PHATASS.ActionSystem
{
// Grabbable monobehaviour that links grabber to a PhysicsBody1D with a fixed joint
//	fixed joint's position
	public class GrabbableUnityPhysicsConfigurableJoint :
		MonoBehaviour,
		IGrabbable
	{
	//serialized fields
		[Tooltip("Sample joint configuration. Must be any kind of UnityEngine.ConfigurableJoint. It is recommended that this refers to a prefab's component.")]
		[SerializeField]
		private ConfigurableJoint sampleJoint;

		[Tooltip("Root ArticulationBody that the joint will connect to. This object will be assigned to the joint's ConnectedArticulationBody property. It is recommended to use EITHER this OR ConnectedRigidBody, not both.")]
		[SerializeField]
		private ArticulationBody connectedArticulationBody;

		[Tooltip("Root RigidBody that the joint will connect to. This object will be assigned to the joint's ConnectedBody property. It is recommended to use EITHER this OR ConnectedRigidBody, not both.")]
		[SerializeField]
		private Rigidbody connectedRigidBody;
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
			ConfigurableJoint joint = originGameObject.ECreateComponent<ConfigurableJoint>();

			joint.connectedArticulationBody = this.connectedArticulationBody;
			joint.connectedBody = this.connectedRigidBody;

			joint.EApplySettings(this.sampleJoint);

			return joint;
		}
	//ENDOF private



	//Editor gizmos
/*
#if UNITY_EDITOR
		private void OnDrawGizmosSelected ()
		{
			
		}
#endif
//*/
	//ENDOF Editor gizmos
	}
}