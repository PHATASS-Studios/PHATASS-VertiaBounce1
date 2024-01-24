using System.Collections.Generic;

using OvoidBoundary2DTransform = PHATASS.Utils.Types.Boundaries.OvoidBoundary2DTransform;
using IBoundary2D = PHATASS.Utils.Types.Boundaries.IBoundary2D;

using IAngle2D = PHATASS.Utils.Types.Angles.IAngle2D;

using UnityEngine;

using TConstraintVector2 = PHATASS.Utils.Types.Constraints.IConstraint<UnityEngine.Vector2>;

namespace PHATASS.EmotionSystem.FaceSystem
{
	public class OvoidBoundary2DBehaviour : MonoBehaviour, IBoundary2D
	{
	//serialized fields
		[SerializeField]
		[Tooltip("Properties of the boundary")]
		private OvoidBoundary2DTransform _boundary;
		private IBoundary2D boundary { get { return this._boundary; }}
	//ENDOF serialized fields

	//IBoundary2D
		//returns true if point is in or on the boundaries defined
		bool TConstraintVector2.Contains (Vector2 point) { return this.boundary.Contains(point); }

		//returns the closest point to target that is in or on bounds
		Vector2 TConstraintVector2.Clamp (Vector2 point) { return this.boundary.Clamp(point);  }

		//position of the center of the boundaries
		Vector2 IBoundary2D.center { get { return this.boundary.center; }}

		IAngle2D IBoundary2D.rotation { get { return this.boundary.rotation; }}

		//returns a point between the center (distance 0) and boundaries (distance 1)
		//point is projected in angle direction from bounds center
		Vector2 IBoundary2D.PointAtAngleFromCenter (float normalizedDistance, IAngle2D degrees)
		{ return this.boundary.PointAtAngleFromCenter(normalizedDistance, degrees); }

		//transforms a point into a value representing its distance from the center
		//returns 0 for the center point, 1 for any value exactly on the bounds, and >1 for items outside bounds, in proportion to its distance to the center
		float IBoundary2D.PointToNormalizedDistanceFromCenter (Vector2 point)
		{ return this.boundary.PointToNormalizedDistanceFromCenter(point); }

		//returns the distance from center to the boundaries in target direction
		float IBoundary2D.RadiusAtAngleFromCenter (IAngle2D angle)
		{ return this.boundary.RadiusAtAngleFromCenter(angle); }

		//Iterates over points of the boundary. gives exactly totalPoints points, which are meant to be equidistant around the shape
		IEnumerable<Vector2> IBoundary2D.EnumerateBoundaryPoints (ushort totalPoints)
		{ return this.boundary.EnumerateBoundaryPoints(totalPoints); }
	//ENDOF IBoundary2D

	//editor gizmos
	#if UNITY_EDITOR
		private void OnDrawGizmosSelected ()
		{
			PHATASS.Utils.Types.Boundaries.IBoundary2DGizmos.DoBoundaryGizmoFull(this.boundary, new Vector3(x: 0, y: 0, z: this.transform.position.z));
		}
	#endif
	//ENDOF editor gizmos
	}
}