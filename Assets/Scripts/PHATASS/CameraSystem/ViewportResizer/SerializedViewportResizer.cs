using UnityEngine;

using static PHATASS.Utils.Extensions.RectExtensions;

using Padding = PHATASS.Utils.Extensions.RectExtensions.Padding;

namespace PHATASS.CameraSystem
{
// IViewportResizer implementation based on exposed serialized variables
//	Can be used with an animator to alter exposed serialized variables
	public class SerializedViewportResizer :
		MonoBehaviour,
		IViewportResizer
	{
	//serialized fields
		[Tooltip("return value for this IViewportResizer.state, dictating active/inactive state.")]
		[SerializeField]
		private bool _state = true;
		private bool state { get { return this._state; } set { this._state = value; }}

		[Tooltip("Inner padding for the upper (+Y) border of the viewport. Negative values create an outer padding.")]
		[SerializeField]
		private Padding upperPadding;

		[Tooltip("Inner padding for the lower (-Y) border of the viewport. Negative values create an outer padding.")]
		[SerializeField]
		private Padding lowerPadding;

		[Tooltip("Inner padding for the left (-X) border of the viewport. Negative values create an outer padding.")]
		[SerializeField]
		private Padding leftPadding;

		[Tooltip("Inner padding for the right (+X) border of the viewport. Negative values create an outer padding.")]
		[SerializeField]
		private Padding rightPadding;
	//ENDOF serialized

	//IViewportResizer
		bool PHATASS.Utils.Types.Toggleables.IToggleable.state
		{ get { return this.state; } set { this.state = value; }}

		Rect IRectResizer.Resize (Rect inputRect)
		{ return this.Resize(inputRect); } 

		Rect IRectResizer.InverseResize (Rect inputRect)
		{ return this.InverseResize(inputRect); } 
	//ENDOF IViewportResizer

	//private
		private Rect Resize (Rect inputRect)
		{
			if (this.state == false)
			{ return inputRect; }
			
			return inputRect.EInnerPad(
				upperPadding: upperPadding,
				lowerPadding: lowerPadding,
				leftPadding: leftPadding,
				rightPadding: rightPadding
			);
		}

		private Rect InverseResize (Rect inputRect)
		{
			if (this.state == false)
			{ return inputRect; }
			
			return inputRect.EInverseInnerPad(
				upperPadding: upperPadding,
				lowerPadding: lowerPadding,
				leftPadding: leftPadding,
				rightPadding: rightPadding
			);
		}

	//ENDOF private
	}
}