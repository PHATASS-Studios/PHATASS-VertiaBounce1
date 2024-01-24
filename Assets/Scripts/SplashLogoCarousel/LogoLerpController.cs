using UnityEngine;

namespace External.LogoSplash
{		
	public class LogoLerpController : MonoBehaviour, ISplashLogoController
	{
	//ISplashLogoController implementation
		public Vector2 targetPosition { set; private get; }
		public Vector2 targetScale { set; private get; }
		public float targetAlpha { set; private get; }

		public Vector2 forcePosition
		{
			set
			{
				rectTransform.localPosition = value;
				targetPosition = value;
			}
		}

		public Vector2 forceScale
		{
			set
			{
				rectTransform.localScale = value;
				targetScale = value;
			}
		}

		public float forceAlpha
		{
			set
			{
				imageAlpha = value;
				targetAlpha = value;
			}
		}
	//ENDOF ISplashLogoController

	//serialized fields
		[SerializeField]
		private float alphaLerpRate = 0.1f;

		[SerializeField]
		private float positionLerpRate = 0.1f;

		[SerializeField]
		private float scaleLerpRate = 0.1f;
	//ENDOF serialized fields

	//private fields
		private RectTransform rectTransform;
		private UnityEngine.UI.Image image;
	//ENDOF private fields

	//private properties
		private float imageAlpha
		{
			get { return image.color.a; }
			set
			{
				image.color = new Color(
					r: image.color.r,
					g: image.color.g,
					b: image.color.b,
					a: value
				);
			}
		}
	//ENDOF private properties

	//MonoBehavior Lifecycle
		public void Awake ()
		{
			rectTransform = transform as RectTransform;
			image = GetComponent<UnityEngine.UI.Image>();

			targetPosition = rectTransform.localPosition;
			targetScale = rectTransform.localScale;
			targetAlpha = imageAlpha;
		}

		public void Update ()
		{
			UpdatePosition();
			UpdateScale();
			UpdateColor();
		}
	//ENDOF MonoBehavior Lifecycle

	//private methods
		private void UpdatePosition ()
		{
			rectTransform.localPosition = Vector2.Lerp(rectTransform.localPosition, targetPosition, positionLerpRate);
		}

		private void UpdateScale ()
		{
			rectTransform.localScale = Vector2.Lerp(rectTransform.localScale, targetScale, scaleLerpRate);
		}

		private void UpdateColor ()
		{
			imageAlpha = Mathf.Lerp(imageAlpha, targetAlpha, alphaLerpRate);
		}
	//ENDOF private methods
	}
}