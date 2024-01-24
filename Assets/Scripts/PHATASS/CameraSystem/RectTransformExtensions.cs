using UnityEngine;
using Axis = UnityEngine.RectTransform.Axis;

using static PHATASS.Utils.Extensions.RectExtensions;

namespace PHATASS.CameraSystem
{
	public static class RectTransformExtensions
	{
		//returns this rectTransform's rect with its worldspace position applied
		public static Rect EMGetWorldRect (this RectTransform rectTransform)
		{
			return rectTransform.rect.EMoveRect(movement: rectTransform.position);
		}

		//alters rectTransform's dimensions and position according to given rect
		public static void EMSetRect (this RectTransform rectTransform, Rect rect)
		{
			//set position
			rectTransform.position = rectTransform.EMGetPivotedPosition(rect);

			//set width
			rectTransform.SetSizeWithCurrentAnchors(
				axis: Axis.Horizontal,
				size: rect.width
			);

			//set height
			rectTransform.SetSizeWithCurrentAnchors(
				axis: Axis.Vertical,
				size: rect.height
			);
		}

		//returns rect position applying this rectTransform's pivot
		public static Vector2 EMGetPivotedPosition (
			this RectTransform rectTransform,
			Rect? _rect = null
		) {
			//store a casted copy of received rect if any
			//or a copy of rectTransform's rect if none
			Rect rect = (_rect != null)
				? (Rect) _rect
				: rectTransform.rect;

			return rect.position + (rect.size * rectTransform.pivot);
		}

		/*
		//returns rect with its position offset by this rectTransform's pivot
		public static Rect EMGetPivotedRect (
			this RectTransform rectTransform,
			Rect? _rect = null
		) {
			//store a casted copy of received rect if any
			//or a copy of rectTransform's rect if none
			Rect rect = (_rect != null)
				? (Rect) _rect
				: rectTransform.rect;

			rect.position = rectTransform.EMGetPivotedPosition(rect);
			return rect;
		}
		*/
	}
}