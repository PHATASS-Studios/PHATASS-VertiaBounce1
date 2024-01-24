using UnityEngine;

using static PHATASS.Utils.Extensions.RectExtensions;

namespace PHATASS.CameraSystem
{
	public class RectCameraControllerSmooth : RectCameraControllerBase
	{
	//serialized fields
		[SerializeField]
		private float positionLerpRate = 0.05f;
		[SerializeField]
		private float sizeLerpRate = 0.05f;
	//ENDOF serialized fields

	//private properties
		private Rect _targetRect;
		protected Rect targetRect
		{
			get { return _targetRect; }
			set { _targetRect = this.ValidateCameraRect(value); }
		}

		private Rect baseRect
		{
			get { return base.rect; }
			set { base.rect = value; }
		}
	//ENDOF private fields

	//base class overrides
		protected override Rect rect
		{
			get { return baseRect; }
			set { targetRect = value; }
		}
	//ENDOF base class overrides

	//MonoBehaviour lifecycle implementation
		public void Start ()
		{
			targetRect = baseRect;
		}

		public void Update ()
		{
			this.UpdateRect();
		}
	//ENDOF MonoBehaviour lifecycle implementation

	//private methods
		private void UpdateRect ()
		{
			baseRect = baseRect.ELerpRect(
				to: targetRect,
				positionLerpRate: positionLerpRate,
				sizeLerpRate: sizeLerpRate
			);
		}
	//ENDOF private methods
	}
}
