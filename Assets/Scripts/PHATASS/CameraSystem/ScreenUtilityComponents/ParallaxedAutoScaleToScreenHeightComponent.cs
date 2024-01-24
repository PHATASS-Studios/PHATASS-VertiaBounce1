using UnityEngine;

using ControllerCache = PHATASS.ControllerSystem.ControllerCache;

namespace PHATASS.CameraSystem.ScreenUtilityComponents
{
	[DefaultExecutionOrder(-2000)]
	public class ParallaxedAutoScaleToScreenHeightComponent : AutoScaleToScreenHeightComponent
	{
	//serialized fields
		[Tooltip("Scale changes from the base scale will be changed by this factor. Negative values make scaling slower, positive values make scaling faster")]
		[SerializeField]
		private float parallaxingFactor = 0.5f;
	//ENDOF serialized fields

	//MonoBehaviour lifecycle
	//ENDOF MonoBehaviour lifecycle

	//private properties
		private float scaleDelta { get { return this.scaleFactor - 1f; }}
		private float parallaxedScaleFactor { get { return 1f + (this.scaleDelta * this.parallaxingFactor); }}
	//ENDOF private properties

	//private methods
		protected override void UpdateScale()
		{ 	
			//Debug.Log(this.scaleFactor);
			this.transform.localScale = this.baseScale * this.parallaxedScaleFactor;
		}
	//ENDOF private methods
	}
}