using UnityEngine;

using ControllerCache = PHATASS.ControllerSystem.ControllerCache;

namespace PHATASS.CameraSystem
{
	public class ViewportScroller : MonoBehaviour
	{
	//serialized properties
		[Tooltip("Base scrolling rate")]
		[SerializeField]
		private float scrollingRate = 1f;

		[Tooltip("Distance from the screen border at which scroll will start")]
		[SerializeField]
		private float borderScrollLimits = 0.05f;
	//ENDOF serialized properties

	//private fields
		private RectCameraControllerScrollable scrollable;
	//ENDOF private fields

	//private properties
		private Rect cameraRect { get { return ControllerCache.viewportController.rect; }}

		private float noScrollRadius { get { return 0.5f - borderScrollLimits; }}
	//ENDOF private properties

	//MonoBehaviour lifecycle
		public void Awake ()
		{
			scrollable = GetComponent<RectCameraControllerScrollable>();
		}

		public void Update ()
		{
			Vector2 scrollingVector = GetScrollingVector();

			if (scrollingVector.magnitude > 0)
			{
				scrollable.Scroll(scrollingVector * scrollingRate);
			}
		}
	//ENDOF MonoBehaviour lifecycle

	//private methods
		//calculates desired scrolling direction and intensity
		public Vector2 GetScrollingVector ()
		{
			//check if tool is available first
			if (ControllerCache.toolManager == null
			||	ControllerCache.toolManager.activeTool == null)
			{
				//Debug.Log("No tool manager or active tool available");
				return Vector2.zero;
			}

			//gather position we must keep in focus / tool position
			Vector2 focusedPosition = (Vector2) ControllerCache.toolManager.activeTool.position;

			//normalized distance = distance / camera size
			Vector2 normalizedDistance =
				(focusedPosition - cameraRect.center)
				/ cameraRect.size;

			//cut out the inner rectangle by moving towards 0 so centered hands don't move the camera 
			Vector2 marginDistance = new Vector2(
				x: Mathf.MoveTowards(current: normalizedDistance.x, target: 0, maxDelta: noScrollRadius),
				y: Mathf.MoveTowards(current: normalizedDistance.y, target: 0, maxDelta: noScrollRadius)
			);
			/*
			Vector2 marginDistance = new Vector2(
				x: normalizedDistance.x - (Mathf.Sign(normalizedDistance.x) * noScrollRadius),
				y: normalizedDistance.y - (Mathf.Sign(normalizedDistance.y) * noScrollRadius)
			);
			//*/

			Vector2 scrollingMagnitude = marginDistance / borderScrollLimits;


			//return new Vector2(x: Mathf.Clamp(scrollingMagnitude.x, -1, 1), y: Mathf.Clamp(scrollingMagnitude.y, -1, 1));
			return scrollingMagnitude;
		}
	//ENDOF private methods
	}
}
