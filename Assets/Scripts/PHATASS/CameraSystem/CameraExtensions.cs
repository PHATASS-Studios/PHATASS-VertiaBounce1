using UnityEngine;

using static PHATASS.Utils.Extensions.RectExtensions;

namespace PHATASS.CameraSystem
{
	public static class CameraExtensions
	{
		//generates a rect from target camera worldspace viewport
		public static Rect EMRectFromOrthographicCamera (this Camera camera)
		{
			float height = camera.orthographicSize * 2;
			float width = height * camera.aspect;
			return camera.transform.position.ERectFromCenterAndSize(
				width: width,
				height: height
			);
		}
	}
}