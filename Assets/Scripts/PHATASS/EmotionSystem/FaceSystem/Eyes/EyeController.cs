using UnityEngine;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

using IBoundary2D = PHATASS.Utils.Types.Boundaries.IBoundary2D;

using IAimable = PHATASS.EmotionSystem.Aimables.IAimable;
using IAimTargetTracker = PHATASS.EmotionSystem.Aimables.IAimTargetTracker;

namespace PHATASS.EmotionSystem.FaceSystem
{
	public class EyeController : MonoBehaviour, IAimable
	{
	//serialized fields
		[SerializeField]
		[Tooltip("Boundary limiting pupil position")]
		[SerializedTypeRestriction(typeof(IBoundary2D))]
		private UnityEngine.Object? _eyeBoundary;
		private IBoundary2D eyeBoundary { get { return this._eyeBoundary as IBoundary2D; }}

		[SerializeField]
		[Tooltip("Transform of the pupil. This transform will be moved within the boundary to create eye movement")]
		private Transform pupilTransform;

		[SerializeField]
		[Tooltip("Default resting position of the pupil while no tracker or tracker inactive. (0,0) represents this transform's position.")]
		private Vector2 pupilRestingPosition;

		[SerializeField]
		[Tooltip("Lerp rate of pupil movement")]
		private float lerpRate = 0.05f;
	//ENDOF serialized

	//IAimableController
		//Vector2 IAimable.position { get { return this.transform.position; }}

		IAimTargetTracker IAimable.targetTracker { get { return this.targetTracker; } set { this.targetTracker = value; }}
	//ENDOF IAimableController

	//MonoBehaviour lifecycle
		private void Awake ()
		{
			this.pupilZPosition = new Vector3(x: 0, y: 0, z: this.pupilTransform.position.z);

			if (this.pupilTransform.parent != this.transform) { this.pupilTransform.parent = this.transform; }

			this.pupilRestingPosition = this.pupilTransform.localPosition;
		}

		private void Update ()
		{
			this.UpdateTracking();
		}
	//ENDOF MonoBehaviour

	//private
		//tracker responsible for managing target aiming position
		private IAimTargetTracker _targetTracker = null;
		protected virtual IAimTargetTracker targetTracker
		{
			get { return this._targetTracker; }
			set { this._targetTracker = value; }
		}


		private Vector3 pupilZPosition;	//cache for pupil Z original position

		private Vector2 targetPosition2D
		{ get {
			if (this.targetTracker == null || !this.targetTracker.trackingActive)
			{ return this.defaultPosition2D; }
			return this.targetTracker.targetPosition;
		}}

		private Vector2 defaultPosition2D
		{ get {
			return 	(Vector2) this.transform.position
				+	(Vector2) (this.transform.rotation * (Vector3) this.pupilRestingPosition);
		}}

		private Vector3 targetPosition3D
		{ get {
			return this.pupilZPosition
				+ (Vector3) this.eyeBoundary.Clamp(this.targetPosition2D);
		}}

		private void UpdateTracking ()
		{
			this.pupilTransform.position = Vector3.Lerp(this.pupilTransform.position, this.targetPosition3D, this.lerpRate);
		}
	//ENDOF private
	}
}