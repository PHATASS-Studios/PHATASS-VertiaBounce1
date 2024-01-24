using UnityEngine;

namespace PHATASS.CameraSystem
{
	public interface IViewportController : PHATASS.ControllerSystem.IController
	{
		Rect rect {get;}		//current size of the viewport
		float size {get;}		//current height value of the viewport
		Vector2 position {get;}	//world-space position of the camera

		float viewportScaleFactor {get;} //size modifier of the viewport. this returns a modifier for the scale of elements and effects that should scale with viewport size

		//moves and resizes camera viewport
		void ChangeViewport (Vector2? position = null, float? size = null);

		//transforms a screen point into a world position
		//if worldSpace is false, the returned Vector3 ignores camera transform position
		Vector2 ScreenSpaceToWorldSpace (Vector2 mousePosition, bool worldSpace = true);

		//transforms a world position into a screen point in pixels
		Vector2 WorldSpaceToScreenSpace (Vector3 worldPosition);

		//Prevents position from going outside of this camera's boundaries
		Vector2 ClampPositionToViewport (Vector2 position);
		Vector3 ClampPositionToViewport (Vector3 position);
	}
}