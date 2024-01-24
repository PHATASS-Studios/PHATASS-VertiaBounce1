using UnityEngine;

using CubismRenderer = Live2D.Cubism.Rendering.CubismRenderer;


//using static PHATASS.Utils.Extensions.CubismRendererExtensions;
using static PHATASS.Utils.Extensions.MeshExtensions;

namespace PHATASS.L2DTools
{
	//This MonoBehaviour makes this transform track a mesh's position
	//It does so by placing the transform position on the mesh's centroid (vertex average position)
	[ExecuteInEditMode]
	public class L2DRendererMeshCentroidPositionTrackerBehaviour : MonoBehaviour
	{
	//serialized fields
		[Tooltip("CubismRenderer object to track")]
		[SerializeField]
		protected CubismRenderer cubismRenderer;
	//ENDOF serialized

	//MonoBehaviour lifecycle
		protected virtual void Update ()
		{
			this.TrackPosition();
		}
	//ENDOF MonoBehaviour

	//inheritable members
		protected Mesh mesh { get { return this.cubismRenderer.Mesh; }}

		protected Vector3 meshLocalCentroid;
	//ENDOF inheritable

	//private members
		private void TrackPosition ()
		{
			this.meshLocalCentroid = this.cubismRenderer.Mesh.EGetCentroid();
			this.transform.position = this.cubismRenderer.transform.TransformPoint(this.meshLocalCentroid);
		}
	//ENDOF private
	}
}