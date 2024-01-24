using UnityEngine;

using ControllerCache = PHATASS.ControllerSystem.ControllerCache;

namespace PHATASS.CameraSystem.ScreenUtilityComponents
{
	public class ParallaxedTranslationComponent : MonoBehaviour
	{
	//serialized fields
		[Tooltip("All movement of the main viewport will be applied to this gameobject, multiplied by this factor")]
		[SerializeField]
		private float parallaxingFactor = 0.5f;
	//ENDOF serialized

	//MonoBehaviour lifecycle
		private void Awake ()
		{
			this.originalSelfPosition = this.transform.position;
			this.originalViewportPosition = ControllerCache.viewportController.position;
			this.zValueVector = new Vector3 (x: 0f, y: 0f, z: this.transform.position.z);
		}

		private void Update ()
		{
			this.UpdateParallaxedPosition();
		}
	//ENDOF MonoBehaviour

	//private properties
		private Vector2 currentSelfPosition
		{
			get { return this.transform.position; }
			set { this.transform.position = ((Vector3) value) + this.zValueVector; }
		}
		private Vector2 currentViewportPosition { get { return ControllerCache.viewportController.position; }}

		private Vector2 viewportPositionDelta { get { return this.currentViewportPosition - this.originalViewportPosition; }}
		private Vector2 selfPositionDelta { get { return this.currentSelfPosition - this.originalSelfPosition; }}
	//ENDOF private properties

	//private fields
		private Vector2 originalSelfPosition;
		private Vector2 originalViewportPosition;
		private Vector3 zValueVector;
	//ENDOF private fields

	//private methods
		private void UpdateParallaxedPosition ()
		{
			this.currentSelfPosition = this.originalSelfPosition + (this.viewportPositionDelta * this.parallaxingFactor);
		}
	//ENDOF private methods
	}
}