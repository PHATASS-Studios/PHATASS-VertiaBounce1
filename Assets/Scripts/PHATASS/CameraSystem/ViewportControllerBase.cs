using UnityEngine;

using static PHATASS.Utils.Extensions.RectExtensions;

namespace PHATASS.CameraSystem
{
// base viewport class containing support definitions and logic, no implementation details
	public abstract class ViewportControllerBase :
		PHATASS.ControllerSystem.MonoBehaviourControllerBase<IViewportController>,
		IViewportController
	{
	//IViewportController implementation
		//dimensions and position of the viewport
		Rect IViewportController.rect
		{
			get { return this.viewportRect; }
		}

		//current height value of the viewport
		float IViewportController.size
		{
			get { return this.viewportRect.height; }
		}

		//current position
		Vector2 IViewportController.position
		{
			get { return this.viewportRect.center; }
		}

		//current size modifier of the viewport. 1 is base size, 0.5 is the size of the viewport when zooming x2, and so on
		float IViewportController.viewportScaleFactor { get { return this.scaleFactor; }}
		protected abstract float scaleFactor {get;}

		//moves and resizes camera viewport
		//if only one of the parameters is used the other aspect of the viewport is unchanged
		void IViewportController.ChangeViewport (
			Vector2? position,
			float? size
		) {
			this.ChangeViewport(position, size);
		}


		//transforms a screen point into a world position
		//if worldSpace is false, the returned Vector3 ignores camera transform position
		Vector2 IViewportController.ScreenSpaceToWorldSpace (
			Vector2 screenPosition,
			bool worldSpace
		) {
			//normalize position into a 0-1 range
			screenPosition = Vector2.Scale(screenPosition, new Vector2 (1/Screen.width, 1/Screen.height));

			//multiply normalized position by camera size
			Vector2 cameraSize = new Vector2 (viewportRect.width, viewportRect.height);
			screenPosition = Vector2.Scale(screenPosition, cameraSize);

			//finally correct world position if necessary
			if (worldSpace)
			{
				screenPosition = screenPosition + viewportRect.center - (cameraSize/2);
			}

			return screenPosition;
		}

		//transforms a world position into a screen point in pixels
		Vector2 IViewportController.WorldSpaceToScreenSpace (Vector3 worldPosition)
		{ return this.WorldSpaceToScreenSpace(worldPosition); }
			protected abstract Vector2 WorldSpaceToScreenSpace (Vector3 worldPosition);

		//Prevents position from going outside of this camera's boundaries
		Vector2 IViewportController.ClampPositionToViewport (Vector2 position)
		{ return position.EClampWithinRect(viewportRect); }
		Vector3 IViewportController.ClampPositionToViewport (Vector3 position)
		{ return position.EClampWithinRect(viewportRect); }
	//ENDOF IViewportController implementation

	//MonoBehaviour lifecycle
		protected override void Awake ()
		{
			base.Awake();
		}
	//ENDOF MonoBehaviour lifecycle

	//abstract property declaration
		protected abstract Rect viewportRect { get; }
	//ENDOF abstract property declaration

	//abstract method declaration
		protected abstract void ChangeViewport (Vector2? position, float? size);
	//ENDOF abstract method declaration
	}
}
