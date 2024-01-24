using UnityEngine;

namespace PHATASS.CameraSystem.ScreenUtilityComponents
{
//MonoBehaviour that alters a slave camera to replicate a master camera's position and dimensions
	public class CameraPositionReplicatorBehaviour : MonoBehaviour
	{
	//serialized fields
		[Tooltip("Reference camera. This camera's position and size is copied over to the slave camera(s) each frame.")]
		[SerializeField]
		private Camera masterCamera;

		[Tooltip("Slave camera. This camera's position and size will be updated to replicate the master's.")]
		[SerializeField]
		private Camera slaveCamera;
	//ENDOF serialized

	//MonoBehaviour
		private void Update ()
		{
			this.ReplicateCameraPosition();
		}
	//ENDOF MonoBehaviour

	//private
		private void ReplicateCameraPosition ()
		{
			this.slaveCamera.transform.position = this.masterCamera.transform.position;
			this.slaveCamera.orthographic = this.masterCamera.orthographic;
			this.slaveCamera.orthographicSize = this.masterCamera.orthographicSize;
			this.slaveCamera.fieldOfView = this.masterCamera.fieldOfView;
		}
	//ENDOF private
	}
}