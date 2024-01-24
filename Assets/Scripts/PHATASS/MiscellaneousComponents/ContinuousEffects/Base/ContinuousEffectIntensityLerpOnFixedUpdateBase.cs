using UnityEngine;

namespace PHATASS.Miscellaneous.ContinuousEffects
{
	public abstract class ContinuousUpdateIntensityLerpOnFixedUpdateBase :
		ContinuousEffectBase
	{
	//serialized fields
		[SerializeField]
		[Tooltip("Lerp rate at which intensity changes. 1 means instantaneous change, 0 means intensity won't update")]
		private float intensityLerpRate = 0.1f;
	//ENDOF serialized fields

	//MonoBehaviour lifecycle
		private void Awake ()
		{
			this.smoothedIntensityValue = this.intensityValue;
		}

		private void FixedUpdate ()
		{
			this.LerpIntensityValue();
			this.UpdateEffect(this.smoothedIntensityValue);
		}
	//ENDOF MonoBehaviour lifecycle

	//protected class properties
		protected float smoothedIntensityValue { get { return this._smoothedIntensityValue; } set { this._smoothedIntensityValue = value; }}
		/*!!Serialized temporarily for debug reasons*/
		[SerializeField]
		private float _smoothedIntensityValue;
	//ENDOF protected class properties

	//private methods
		private void LerpIntensityValue ()
		{
			this.smoothedIntensityValue = Mathf.Lerp(a: smoothedIntensityValue, b: this.intensityValue, t: this.intensityLerpRate);
		}
	//ENDOF private methods

	//Abstract method declaration
		protected abstract void UpdateEffect (float intensity);
	//ENDOF Abstract method declaration
	}
}
