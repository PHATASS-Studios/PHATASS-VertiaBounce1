using UnityEngine;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

using IRectValue = PHATASS.Utils.Types.Values.IRectValue;

using static PHATASS.CameraSystem.RectTransformExtensions;

// Makes this GameObject's RectTransform replicate size and position of given rect value object
namespace PHATASS.Miscellaneous.UI
{
	[RequireComponent(typeof(RectTransform))]
	[ExecuteInEditMode]
	public class RectTransformRectReplicator : MonoBehaviour
	{
	//serialized fields
		[SerializeField]
		[Tooltip("Source of a rect value. This GameObject's RectTransform will be updated to match this rect. E.G.: Link the IViewportLimits controlling the scene's maximum boundaries to make a scene-scaled interface. Or, link the camera's rect to make this rect follow the camera.")]
		[SerializedTypeRestriction(typeof(IRectValue))]
		private UnityEngine.Object? _rectOrigin;
		private IRectValue rectOrigin { get { return this._rectOrigin as IRectValue; }}
	//ENDOF serialized fields

	//private fields
		private RectTransform rectTransform;
	//ENDOF private fields

	//MonoBehaviour lifecycle
		public void Awake ()
		{
			rectTransform = (RectTransform) transform;
		}

		public void LateUpdate()
		{
			rectTransform.EMSetRect(this.rectOrigin.value);
		}
	//ENDOF MonoBehaviour lifecycle
	}
}
