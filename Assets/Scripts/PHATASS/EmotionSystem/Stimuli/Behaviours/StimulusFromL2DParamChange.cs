using UnityEngine;

using static PHATASS.L2DTools.Extensions.CubismParameterExtensions;

namespace PHATASS.EmotionSystem
{
// MonoBehaviour class that propagates a stimulus to a stimulable when given CubismParameter value changes
	// Used to create stimulus from the movement part of oscillations like stroking, rubbing, etc...
	public class StimulusFromL2DParamChange : StimulusFromL2DParamBehaviourBase
	{
	//Serialized fields
		[Header("Stimulus scaling by change speed")]
		[Tooltip("Minimum speed of change that will trigger a stimulus - values below will be ignored")]
		[SerializeField]
		private float minimumSpeed = 0.01f;
		[Tooltip("Maximum speed of change that will be recognized - values above will be clamped")]
		[SerializeField]
		private float maximumSpeed = 10f;

		[Tooltip("Stimulus scaling by speed of value change - T=0: scale for minimum speed, T=1: scale for maximum speed")]
		[SerializeField]
		private StimulusCurve stimulusScalingBySpeed = 1f;
	//ENDOF Serialized

	//MonoBehaviour lifecycle
		private void Start ()
		{ this.previousValue = this.currentValue; }

		private void Update ()
		{
			float changeRate = this.GetChangeRate();

			if (changeRate < this.minimumSpeed) { return; }
			if (changeRate > this.maximumSpeed) { changeRate = this.maximumSpeed; }

			float rateBySpeed = (changeRate - this.minimumSpeed) / (this.maximumSpeed - this.minimumSpeed);
			//float currentDistance = UnityEngine.Mathf.Abs((this.currentValue - this.previousValue));

			this.PropagateStimulus(
				this.ValueScaledStimulus()
				.EScale(this.stimulusScalingBySpeed.Evaluate(rateBySpeed))
			);

			this.previousValue = this.currentValue;
		}
	//ENDOF MonoBehaviour

	//private
		private float currentValue { get { return this.normalizedValue; }}
		private float previousValue;

		private float GetChangeRate ()
		{
			return UnityEngine.Mathf.Abs((this.currentValue - this.previousValue) / Time.deltaTime);
		}
	//ENDOF private
	}
}
