using UnityEngine;

using static PHATASS.Utils.Extensions.FloatExtensions;
using static PHATASS.Utils.Extensions.DoubleExtensions;
using static PHATASS.L2DTools.Extensions.CubismParameterExtensions;

namespace PHATASS.EmotionSystem
{
// MonoBehaviour class that propagates a stimulus to a stimulable when an oscillation peak is detected in a CubismParameter
// Used to create stimulus from the direction change/peak part of oscillations, like: stroking, rubbing, etc
	public class StimulusFromL2DParamOscillationPeaks : StimulusFromL2DParamBehaviourBase
	{
	//Serialized fields
		//Stimulus scaling by oscillation width
		[Header("Stimulus scaling by oscillation width")]
		[Tooltip("Minimum normalized oscillation width to consider - oscillations under this will be ignored")]
		[SerializeField]
		private float minimumOscillationWidth = 0.1f;
		[Tooltip("Maximum normalized oscillation width to consider - oscillations over this will be clamped")]
		[SerializeField]
		private float maximumOscillationWidth = 1f;

		[Tooltip("Stimulus scaling by oscillation width - T=0: scale for minimum width, T=1: scale for maximum width")]
		[SerializeField]
		private StimulusCurve stimulusScalingByOscillationWidth = 1f;
		//ENDOF Stimulus scaling by oscillation width


		//Stimulus scaling by oscillation average speed
		[Header("Stimulus scaling by oscillation speed")]
		[Tooltip("Minimum oscillation speed to consider, in normalized width/second - oscillations under this will be ignored")]
		[SerializeField]
		private float minimumOscillationSpeed = 0.0f;
		[Tooltip("Maximum oscillation speed to consider, in normalized width/second - oscillations over this will be clamped")]
		[SerializeField]
		private float maximumOscillationSpeed = 5.0f;

		[Tooltip("Stimulus scaling by oscillation speed - T=0: scale for minimum width, T=1: scale for maximum width")]
		[SerializeField]
		private StimulusCurve stimulusScalingByOscillationSpeed = 1f;
		//ENDOF Stimulus scaling by oscillation average speed
	//ENDOF Serialized

	//MonoBehaviour lifecycle
		private void Start ()
		{
			this.previousValue = this.currentValue;
			this.previousApex = new OscillationApex (position: this.currentValue, time: 0d);
		}

		private void Update ()
		{
			//Debug.Log("current value: " + this.currentValue);
			//if current value is anywhere between last apex and last frame's position, this means we have just changed movement direction and thus detected a new apex
			if (this.FindApex())

			{ this.DetectedApex(new OscillationApex(position: this.previousValue, time: Time.timeAsDouble)); }


			this.previousValue = this.currentValue;
		}


/*
		private void Update ()
		{
			float changeRate = this.GetChangeRate();

			if (changeRate < this.minimumSpeed) { return; }
			if (changeRate > maximumSpeed) { changeRate = maximumSpeed; }

			float effectRate = (changeRate - this.minimumSpeed) / (this.maximumSpeed - this.minimumSpeed);

			this.PropagateStimulus(new Stimulus(
				intensity: Mathf.Lerp(intensityForMinimum, intensityForMaximum, effectRate),
				depth: Mathf.Lerp(depthForMinimum, depthForMaximum, effectRate)
			));

		}
		*/
	//ENDOF MonoBehaviour

	//private members
		private float currentValue { get { return this.normalizedValue; }}
		private float previousValue;

		/*
		private float currentDirection
		{
			get { return (this.currentValue - this.previousValue).ESign();}
		}
		//*/

		private OscillationApex previousApex;
		private float oscillationDirection = 0;

		private bool FindApex ()
		{
			 return (this.currentValue.EIsBetween(this.previousApex.position, this.previousValue))
			 	||	this.previousValue == this.currentValue;
		}

		private void DetectedApex (OscillationApex currentApex)
		{
			this.ProcessApex(currentApex);
			this.previousApex = currentApex;
		}
		private void ProcessApex (OscillationApex currentApex)
		{
			OscillationApex oscillation = currentApex.Distance(this.previousApex);

			float width = oscillation.position;
			float speed = oscillation.speed;

			//validate width and height
			if (width < this.minimumOscillationWidth || speed < this.minimumOscillationSpeed)
			{ return; }

			if (width > this.maximumOscillationWidth) { width = this.maximumOscillationWidth; }
			if (speed > this.maximumOscillationSpeed) { speed = this.maximumOscillationSpeed; }

			this.PropagateStimulus(
				this.ValueScaledStimulus()
				.EScale(this.stimulusScalingByOscillationWidth.Evaluate(width))
				.EScale(this.stimulusScalingByOscillationSpeed.Evaluate(speed))
			);

		}
	//ENDOF private

	//private sub-classes
		//structure representing one of the two apexes that define a single oscillation
		//contains the position of the apex and its time of occurrence
		private readonly struct OscillationApex
		{
			private readonly float _position;
			public readonly float position { get { return this._position; }}
			private readonly double _time;
			public readonly double time { get { return this._time; }}

			public readonly float speed { get { return (float) ((double)this.position / this.time).EAbs(); }}

			public OscillationApex (float position, double time)
			{
				_position = position;
				_time = time;
			}

			//Creates a new object with the difference between this and other
			public OscillationApex Distance (OscillationApex other)
			{ return new OscillationApex(position: (this.position - other.position).EAbs(), time: (this.time - other.time).EAbs()); }
		}
	//ENDOF private sub-classes
	}
}
