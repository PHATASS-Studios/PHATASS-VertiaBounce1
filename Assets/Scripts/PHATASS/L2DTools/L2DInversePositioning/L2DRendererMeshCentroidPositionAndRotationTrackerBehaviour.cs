using System.Collections.Generic;

using UnityEngine;

using CubismRenderer = Live2D.Cubism.Rendering.CubismRenderer;

using IAngle2D = PHATASS.Utils.Types.Angles.IAngle2D;
using static PHATASS.Utils.Types.Angles.IAngle2DFactory;
//using Angle2D = PHATASS.Utils.Types.Angles.Angle2D;

using static PHATASS.Utils.Extensions.MeshExtensions;
using static PHATASS.Utils.Extensions.Vector2Extensions;
using Averages = PHATASS.Utils.MathUtils.Averages;

namespace PHATASS.L2DTools
{
	//This MonoBehaviour makes this transform's rotation track a mesh's rotation as well as its position
	//	Rotation is calculated as the average deviation of each vertex from the centroid as compared to the mesh's initial state.
	// NOTE ROTATION ONLY TAKES INTO ACCOUNT X/Y AXES, NEVER Z
	[ExecuteInEditMode]
	public class L2DRendererMeshCentroidPositionAndRotationTrackerBehaviour : L2DRendererMeshCentroidPositionTrackerBehaviour
	{
	//serialized fields
	//ENDOF serialized

	//MonoBehaviour lifecycle
		private void Start ()
		{
			this.InitializeVertexCache();
		}

		protected override void Update ()
		{
			//Debug.LogWarning("FRAME");
			base.Update(); // Important to call base update before tracking rotation so as to initialize base meshCentroid cache
			this.TrackRotation();
		}
	//ENDOF MonoBehaviour

	//private members
		private IAngle2D[] originalAngularPositionCache; //holds the default angle of each vertex with regards to the mesh's centroid
		private IAngle2D[] currentAngularPositionCache;
		private IAngle2D[] angleDifferenceCache;

		private List<Vector3> vertexCache;
		private int vertexCount = 0;

		private Mesh trackedMesh = null;

		//checks wether vertex caches are valid and initializes them as necessary
		private void ValidateVertexCache ()
		{
			if (this.vertexCount != this.mesh.vertexCount
			//||	this.trackedMesh != this.mesh
			//is it necessary to make additional checks?
			) { this.InitializeVertexCache(); }
		}

		// Caches a list of each of the mesh vertices' angle with respect to its centroid
		private void InitializeVertexCache ()
		{
			Debug.LogWarning("Resetting vertex cache");
			this.trackedMesh = this.mesh;

			this.vertexCount = this.mesh.vertexCount;
			this.vertexCache = new List<Vector3>(this.vertexCount);

			Debug.Log("VertexCount: " + this.vertexCount);

			this.originalAngularPositionCache = new IAngle2D[this.vertexCount];
			this.currentAngularPositionCache = new IAngle2D[this.vertexCount];
			this.angleDifferenceCache = new IAngle2D[this.vertexCount];

			this.GetMeshVertexAngularPositionList(this.mesh, this.originalAngularPositionCache);
			//this.trackedMesh = this.cubismRenderer.Mesh;
			//this.originalAngularPositionCache = this.gtfrv(this.cubismRenderer.Mesh, this.originalAngularPositionCache);
			//this.currentAngularPositionCache = null;
			//this.angleDifferenceCache = null;
		}

		private void TrackRotation ()
		{
			this.ValidateVertexCache();

			this.GetMeshVertexAngularPositionList(this.mesh, currentAngularPositionCache);

			for (int i = 0; i < this.vertexCount; i++)
			{
				this.angleDifferenceCache[i] = this.currentAngularPositionCache[i] - this.originalAngularPositionCache[i];
			}

			this.SetAngle(Averages.Angle2DArithmeticAverage(this.angleDifferenceCache));
			
			/*
			IAngle2D averageDeviationAngle = Averages.Angle2DArithmeticAverage(this.angleDifferenceCache);
			this.SetAngle(averageDeviationAngle);
			*/
			//if (this.angleDifferenceCache[0].ShortestDirectionSign(averageDeviationAngle) > 0) { averageDeviationAngle = averageDeviationAngle.Invert(); }

		}

		private void GetMeshVertexAngularPositionList (Mesh mesh, IAngle2D[] outputArray)
		{
			//this.ValidateVertexCache();

			mesh.GetVertices(this.vertexCache);
			
			for (int i = 0; i < this.vertexCount; i++)
			//{ outputList[i] = this.GetVertexAngularPosition(this.vertexCache[i]); }
			{ outputArray[i] = this.GetVertexAngularPosition(this.vertexCache[i]); }
		}

		private IAngle2D GetVertexAngularPosition (Vector3 vertexPosition)
		{
			return ((Vector2) this.meshLocalCentroid).EFromToAngle2D((Vector2) vertexPosition);
			//return ((Vector2) vertexPosition).EFromToAngle2D((Vector2) this.meshLocalCentroid);
		}

		private void SetAngle (float degrees)
		{
			//Debug.Log("Setting angular rotation to: " + degrees);
			this.transform.rotation = Quaternion.Euler(x: 0f, y: 0f, z: degrees);
		}
		private void SetAngle (IAngle2D angle)
		{ this.SetAngle(angle.degrees); }
	  //vertex cache initialization and validation

	//ENDOF private
	}
}