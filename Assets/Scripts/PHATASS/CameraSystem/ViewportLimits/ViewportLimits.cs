using UnityEngine;

using static PHATASS.Utils.Extensions.RectExtensions;
using static PHATASS.Utils.Extensions.Vector2Extensions;

#if UNITY_EDITOR
	using static PHATASS.Utils.Extensions.RectEditorExtensions;
#endif 

namespace PHATASS.CameraSystem
{
// Limits for a 2D viewport. Clamps a viewport rect so it always falls within the acceptable scene
//	This implementation tries to fully cover a desiredBounds rect while never exceeding a maximumBounds
//	It the maximum boundaries for the scene by:
	// 1. Create the smallest rect respecting camera aspect ratio that fits outside desired bounds
	// 2. Trim/crop rect size by maximum bounds
	// 3. Center rect around desired
	// 4. Move rect to fit maximum
	public class ViewportLimits :
		MonoBehaviour,
		IViewportLimits
	{
	//Serialized fields
		[Tooltip("Camera giving the reference for the viewport's aspect ratio. If null/not set automatically picks Camera.main")]
		[SerializeField]
		private Camera _referenceCamera;
		private Camera referenceCamera
		{
			set { this._referenceCamera = value; }
			get
			{
				if (this._referenceCamera == null) { return Camera.main; }
				return this._referenceCamera;
			}
		}

		[Tooltip("Desired bounds of the scene. Generated limits will always contain all of this rect.")]
		[SerializeField]
		private Rect desiredBoundsLocalSpace;
		private Rect desiredBoundsWorldSpace
		{ get { return this.desiredBoundsLocalSpace.EMoveRect(this.transform.position); }}

		[Tooltip("Maximum bounds of the scene. Generated limits will never exceed this rect.")]
		[SerializeField]
		private Rect maximumBoundsLocalSpace;
		private Rect maximumBoundsWorldSpace
		{ get { return this.maximumBoundsLocalSpace.EMoveRect(this.transform.position); }}
	//ENDOF Serialized fields

	//IViewportLimits
		// Returns a Rect clamped within this viewport's maximum bounds
		// Clamp is done first by moving within bounds, then by ratio-respecting scaling
		Rect PHATASS.Utils.Types.Constraints.IConstraint<Rect>.Clamp (Rect value)
		{ return this.Clamp(value); }

		// Returns true if given rect fully contained within this viewport's maximum bounds.
		//	Value's borders can contain the limit's borders but not exceed them.
		bool PHATASS.Utils.Types.Constraints.IConstraint<Rect>.Contains (Rect value)
		{ return this.Contains(value); }

		// Returns a Rect representing the calculated viewport's maximum limits.
		Rect PHATASS.Utils.Types.Values.IValue<Rect>.value
		{ get { return this.viewportLimits; }}

		//camera giving the reference for the viewport's aspect ratio
		//if null/not set automatically picks Camera.main
		Camera IViewportLimits.referenceCamera
		{ set { this.referenceCamera = value; }}
	//ENDOF IViewportLimits

	//MonoBehaviour
	//ENDOF MonoBehaviour

	//private implementation of interface methods
		//Clamps a rect to container limits by ensuring it is not larger than necessary and then moving it if any part of it is outside
		private Rect Clamp (Rect value)
		{
			Rect limits = this.viewportLimits;
			return value.EDownscaleIfLarger(limits).EMoveToFitWithinRect(limits);
		}

		// Returns true if given rect fully contained within this viewport's maximum bounds.
		private bool Contains (Rect value)
		{
			Rect limits = this.viewportLimits;

			return (value.xMin >= limits.xMin
			&&	value.xMax <= limits.xMax
			&&	value.yMin >= limits.yMin
			&&	value.yMax <= limits.yMax
			);
		}
	//ENDOF private implementation		

	//Calculation of limits rect
		//[TO-DO]: Cache this value and update it only once per frame
		private Rect viewportLimits
		{ get { return this.CalculateLimits(); }}
		/*
		private Rect viewportLimits;
		private void UpdateLimits ()
		{ this.viewportLimits = this.CalculateLimits(); }
		*/
		private Rect CalculateLimits ()
		{
			if (this.referenceCamera == null)
			{
				Debug.LogError("ViewportLimits component requires either referenceCamera being initialized or a camera tagged MainCamera to exist.");
				return default(Rect);
			}

			Rect rect;
		// 1. Create the smallest rect respecting camera aspect ratio that fits outside desired bounds
			rect = this.referenceCamera.aspect.ERectFromAspectRatio();	//Create a height 1 rect with the camera's aspect ratio
			rect = rect.EScaleToFitOutside(this.desiredBoundsLocalSpace);	//scale initial rect by the largest dimension of the bounds-by-size ratio to fit outside desired rect
		// 2. Trim/crop rect size by maximum bounds
			rect = rect.ETrimRectSizeToRect(this.maximumBoundsWorldSpace);
		// 3. Center rect around desired
			rect = rect.EMakeConcentric(this.desiredBoundsWorldSpace);
		// 4. Move rect to fit maximum
			rect = rect.EMoveToFitWithinRect(this.maximumBoundsWorldSpace);
		// Done
			return rect;
		}
	//ENDOF limits rect

	//Editor gizmos
#if UNITY_EDITOR
		private void OnDrawGizmos ()
		{
			this.maximumBoundsWorldSpace.EDrawGizmo(color: Color.red, label: "View-Max");
			this.desiredBoundsWorldSpace.EDrawGizmo(color: Color.blue, label: "View-Min");
		}

		private void OnDrawGizmosSelected ()
		{
			this.CalculateLimits().EDrawGizmo(color: Color.green);
		}
#endif
	//ENDOF Editor gizmos
	}
}