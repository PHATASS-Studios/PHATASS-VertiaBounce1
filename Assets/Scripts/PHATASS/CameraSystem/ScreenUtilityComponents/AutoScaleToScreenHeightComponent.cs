using UnityEngine;

using ControllerCache = PHATASS.ControllerSystem.ControllerCache;

namespace PHATASS.CameraSystem.ScreenUtilityComponents
{
	[DefaultExecutionOrder(-2000)]
	public class AutoScaleToScreenHeightComponent : MonoBehaviour
	{
	//serialized fields
		//[TO-DO] this should not require original sizes to be manually set
		[SerializeField]
		protected Vector3 baseScale = Vector3.one;
	//ENDOF serialized fields

	//MonoBehaviour lifecycle
		private void Start () 
		{
			this.UpdateScale();
		}

		private void LateUpdate ()
		{
			this.UpdateScale();
		}
	//ENDOF MonoBehaviour lifecycle

	//private properties
		protected float scaleFactor { get { return ControllerCache.viewportController.viewportScaleFactor; }}
	//ENDOF private properties

	//private methods
		protected virtual void UpdateScale()
		{
			//Debug.Log(this.name + " scale factor: " + this.scaleFactor);
			this.transform.localScale = this.baseScale * this.scaleFactor;
		}
	//ENDOF private methods
	}
}