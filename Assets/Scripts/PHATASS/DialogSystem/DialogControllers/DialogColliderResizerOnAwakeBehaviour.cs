using UnityEngine;

namespace PHATASS.DialogSystem.DialogControllers
{
	public class DialogColliderResizerOnAwakeBehaviour : MonoBehaviour
	{
	//MonoBehaviour lifecycle
		private void Start ()
		{
			ColliderSizeFromRectTransform(
				boxCollider: GetComponent<BoxCollider>(),
				rectTransform: (transform as RectTransform)
			);
		}
	//ENDOF MonoBehaviour lifecycle

	//private method implementation
		private void ColliderSizeFromRectTransform(BoxCollider boxCollider, RectTransform rectTransform)
		{
			float CenterFromPivot (float dimension, float pivot)
			{
				return dimension * (pivot - 0.5f) * -1;
			}

			if (boxCollider == null || rectTransform == null)
			{
				Debug.LogError("DialogColliderResizer no collider or no rectTransform");
				return;
			}

			boxCollider.size = new Vector3(
				x: rectTransform.rect.width,
				y: rectTransform.rect.height,
				z: 1
			);

			boxCollider.center = new Vector3(
				x: CenterFromPivot(rectTransform.rect.width, rectTransform.pivot.x),
				y: CenterFromPivot(rectTransform.rect.height, rectTransform.pivot.y),
				z: 0
			);
		}
	//ENDOF private method implementation
	}
}