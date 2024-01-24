using UnityEngine;

using CubismParameter = Live2D.Cubism.Core.CubismParameter;

using IEmotion = PHATASS.EmotionSystem.IEmotion;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

using static PHATASS.Utils.Extensions.FloatExtensions;
using static PHATASS.Utils.Extensions.IntExtensions;
using static PHATASS.Utils.Extensions.CubismParameterExtensions;
namespace PHATASS.L2DTools
{
//[TO-DO]: Port this to something that uses the existing SawtoothGeneratorMagnitudeFromValue implementation for IValue
	public class L2DParameterOscillationFromEmotionIntensityBehaviour :
		MonoBehaviour//, IFloatValue
	{
	//Serialized fields
		[SerializeField]
		private CubismParameter cubismParameter;

		[SerializeField]
		[SerializedTypeRestriction(typeof(IEmotion))]
		private UnityEngine.Object _sourceEmotion;
		private IEmotion sourceEmotion { get { return this._sourceEmotion as IEmotion; }}

		[SerializeField]
		[Tooltip("Oscillation central point in normalized units - will swing outward in each direction at a width")]
		private float centralPoint = 0.5f;

		[SerializeField]
		[Tooltip("Curve defining width of oscillation (in normalized units) by normalized intensity value for emotion controller")]
		private AnimationCurve oscillationWidthByEmotionIntensity;

		[SerializeField]
		[Tooltip("Curve defining speed of oscillation (in normalized units per second) by normalized intensity value for emotion controller")]
		private AnimationCurve oscillationSpeedByEmotionIntensity;

		[SerializeField]
		[Tooltip("Multiplies speed curve result by this value")]
		private float baseSpeed = 1f;

		[SerializeField]
		[Tooltip("1 or -1. Current direction of swing. Set this to change initial direction - random if 0")]
		private int direction = 0;
	//ENDOF Serialized

	//MonoBehaviour lifecycle
		private void Start ()
		{
			this.direction = this.direction.ESign();
			if (this.direction == 0) { this.direction = PHATASS.Utils.RandomUtils.RandomSign.Int(); }
		}

		private void LateUpdate ()
		{
			this.CheckDirectionChange();
			this.UpdateSwing();
		}

	//ENDOF MonoBehaviour

	//private
		private float emotionIntensity { get { return this.sourceEmotion.normalizedBuildUp; }}
		private float normalizedValue
		{
			get { return this.cubismParameter.EGetNormalizedValue(); }
			set { this.cubismParameter.ESetNormalizedValue(value); }
		}

		private float currentWidth { get { return this.oscillationWidthByEmotionIntensity.Evaluate(this.emotionIntensity)/2; }}
		private float currentSpeed { get { return this.baseSpeed * this.oscillationSpeedByEmotionIntensity.Evaluate(this.emotionIntensity); }}

		private float swingUpperLimit { get { return this.centralPoint + this.currentWidth; }}
		private float swingLowerLimit { get { return this.centralPoint - this.currentWidth; }}

		private void CheckDirectionChange ()
		{
			//Debug.Log("upper limit: " + this.swingUpperLimit + " lower limit: " + this.swingLowerLimit);
			if (this.normalizedValue >= this.swingUpperLimit) { this.direction = -1; }
			else if (this.normalizedValue <= this.swingLowerLimit) { this.direction = 1; }
		}

		private void UpdateSwing ()
		{
			float newValue = this.normalizedValue + (this.currentSpeed * Time.deltaTime * this.direction);
			//Debug.Log("swing old value: " + this.normalizedValue + " new value: " + newValue);
			this.normalizedValue = newValue;
			//Debug.Log("Set to: " + this.normalizedValue);
		}
	//ENDOF private	
	}
}