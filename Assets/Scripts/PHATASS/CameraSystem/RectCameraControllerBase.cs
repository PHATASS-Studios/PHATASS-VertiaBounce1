using UnityEngine;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

using static PHATASS.Utils.Extensions.RectExtensions;
using static PHATASS.CameraSystem.CameraExtensions; //Camera.EMRectFromOrthographicCamera();

using ControllerProvider = PHATASS.ControllerSystem.ControllerProvider;

using IDoubleValue = PHATASS.Utils.Types.Values.IDoubleValue;

namespace PHATASS.CameraSystem
{
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(Camera))]
	public class RectCameraControllerBase :
		ViewportControllerBase,
		PHATASS.Utils.Types.Values.IRectValue
	{
	//serialized fields
		[SerializeField]
		[Tooltip("Viewport limiter object handling the scene's maximum viewable zone - and clamping the camera to it.")]
		[SerializedTypeRestriction(typeof(IViewportLimits))]
		private UnityEngine.Object? _viewportLimits;
		private IViewportLimits viewportLimits { get { return this._viewportLimits as IViewportLimits; }}

		[SerializeField]
		[Tooltip("Resizer that lets the camera controller know the actual visible area - for use with letterboxing, vignettes, HUDs, etc. When clamping the viewport to viewportLimits, only the area dictated by viewableAreaResizer will be considerd, if it is not null.")]
		[SerializedTypeRestriction(typeof(IViewportResizer))]
		private UnityEngine.Object? _viewableAreaResizer;
		private IViewportResizer viewableAreaResizer
		{ get {
			if (this._viewableAreaResizer == null) { return null; }
			return this._viewableAreaResizer as IViewportResizer;
		}}

		[Tooltip("Reference viewport size for calculating viewport scale factor. This should be the viewport size (CAMERA.Size) for which any elements are intended. Then, IViewportController.viewportScaleFactor will repesent the factor between current viewport size and base viewport size, for proper scaling")]
		[SerializeField]
		private float viewportScalingReferenceSize = 1f;
	//ENDOF serialized fields

	//IRectValue
		Rect PHATASS.Utils.Types.Values.IValue<Rect>.value { get { return this.rect; }}
	//ENDOF IRectValue

	//private fields
		protected Camera cameraComponent; //cached reference to the camera this controller handles
	//ENDOF private fields

	//abstract property implementation
		protected override Rect viewportRect { get { return this.rect; }}

		protected override float scaleFactor
		{ get { return this.rect.height / this.viewportScalingReferenceSize / 2; }}
	//ENDOF abstract property implementation

	//protected class properties
		protected virtual Rect rect
		{
			get { return (this.transform as RectTransform).EMGetWorldRect(); }
			set
			{
				//apply a pre-validated rect to the transform
				(this.transform as RectTransform).EMSetRect(this.ValidateCameraRect(value));
			}
		}
	//protected class properties

	//private properties
		private float screenRatio
		{
			get { return cameraComponent.aspect; }
		}
	//ENDOF private properties

	//inherited method implementation
		//moves and resizes camera viewport
		//if only one of the parameters is used the other aspect of the viewport is unchanged
		protected override void ChangeViewport (Vector2? position, float? size)
		{
			this.rect = this.CreateCameraRect(position: position, height: size);
		}

		protected override Vector2 WorldSpaceToScreenSpace (Vector3 worldPosition)
		{
			return this.cameraComponent.WorldToScreenPoint(worldPosition);
		}
	//ENDOF inherited method implementation

	//MonoBehaviour lifecycle implementation
		protected override void Awake ()
		{
			base.Awake();
			Initialize();
		}

		protected void OnPreCull ()
		{
			ApplyCameraSize();
		}
	//ENDOF MonoBehaviour lifecycle implementation

	//private methods
		//Controller initialization
		private void Initialize ()
		{
			//cache references to Camera components
			cameraComponent = GetComponent<Camera>();

			//initialize limits
			//Deprecated: ViewportLimits object is independently initialized: if (autoConfigureLimits) { viewportLimits = cameraComponent.EMRectFromOrthographicCamera(); }
		}

		//applies the rect height to the camera component right before rendering
		private void ApplyCameraSize ()
		{
			cameraComponent.orthographicSize = rect.height / 2;
		}
	//ENDOF private methods

	//protected class methods
		//Clamps and properly sizes a rect for this camera ratio and limits
		protected Rect ValidateCameraRect (Rect innerRect)
		{
			//clamp rect position within viewport limits
			return this.ClampRectWithinLimits(
				innerRect: this.CreateCameraRect(sampleRect: innerRect), //create a new rect to ensure ensure it fulfills size ratio
				resizer: this.viewableAreaResizer	//pass the resizer so it can be used to more accurately delimit view area
			);
		}

		//creates previewing camera dimensions at target position and height.
		//non included parameters are filled with current camera values
		//Rect width is inferred off of height and screen ratio.
		protected Rect CreateCameraRect (Rect sampleRect)
		{ return CreateCameraRect(position: sampleRect.center, height: sampleRect.height); }
		protected Rect CreateCameraRect (Vector2? position = null, float? height = null)
		{
			//first validate and complete inputs
			Vector2 validPosition = (position != null) 
				?	(Vector2) position
				:	rect.center;
			float validHeight = (height != null) 
				?	(float) height
				:	rect.height;

			//Debug.Log("CreateCameraRect(" + position + ", " + height + ")");
			//Debug.Log(" validPosition: " + validPosition + "\n validHeight: " + validHeight);

////////////////[TO-DO] this is a bit duplicate logic, condense this and CameraExtensions.EMRectFromOrthographicCamera()?
			
			//now create and return a rect with proper dimensions and position
			return validPosition.ERectFromCenterAndSize(
				width: validHeight * screenRatio,
				height: validHeight
			);
		}

		//clamps a rect's height and position to make it fit within viewport limits
		protected Rect ClampRectWithinLimits (Rect innerRect, IViewportResizer resizer = null)
		{
			//if resizer available, perform clamping on a resized version of the viewport then invert the resizing
			if (resizer != null)
			{
				innerRect = resizer.Resize(innerRect);
				innerRect = this.viewportLimits.Clamp(innerRect);
				return resizer.InverseResize(innerRect);
			}
			else
			{ return this.viewportLimits.Clamp(innerRect); }
		}
	//ENDOF inheritable private methods

	//////////////////////////////////////////////////////////////////
	}
}