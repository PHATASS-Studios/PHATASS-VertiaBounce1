//using static System.Math;

using UnityEngine;

namespace PHATASS.Miscellaneous.ContinuousEffects
{
	public abstract class ContinuousEffectBase :
		MonoBehaviour,
		IContinuousEffect
	{
	//serialized fields
		[SerializeField]
		[Tooltip("Intensity value for this effect")]
		private float _intensityValue;
	//ENDOF serialized fields

	//IFloatEventReceiver
		void PHATASS.Utils.Events.ISimpleEventReceiver<float>.Event (float param0)
		{ this.intensityValue = param0; }

	//ENDOF IFloatEventReceiver

	//protected class properties
		protected virtual float intensityValue
		{
			get { return this._intensityValue; }
			set { this._intensityValue = value; }
		}

		protected virtual float intensityAbsolute
		{
			get { return System.Math.Abs(this.intensityValue); }
			set { this.intensityValue = value * this.intensitySign; }
		}

		protected virtual int intensitySign
		{
			get { return (this.intensityValue >= 0) ? 1 : -1; }
			set
			{
				this.intensityValue = this.intensityAbsolute * ((value >= 0) ? 1 : -1);
			}
		}
	//ENDOF class properties

	//MonoBehaviour lifecycle
	//ENDOF MonoBehaviour lifecycle

	//Abstract method definition
	//ENDOF Abstract method definition
	}
}
