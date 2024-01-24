using UnityEngine;

namespace PHATASS.EmotionSystem
{
//MAYBE MOVE THIS ELSEWHERE IF IT CAN BE USED
	//interface representing a curve. Only exposes an Evaluate method that returns the curve value at time t
	public interface ICurve<T>
	{
		T Evaluate (float t);
	}

	//Serializable class representing a single
	[System.Serializable]
	public struct StimulusCurve : ICurve<IStimulus>
	{
		[SerializeField][Tooltip("Intensity scaling curve")]
		private AnimationCurve intensityCurve;
		[SerializeField][Tooltip("Depth scaling curve")]
		private AnimationCurve depthCurve;

		public StimulusCurve (AnimationCurve intensity, AnimationCurve depth)
		{
			intensityCurve = intensity;
			depthCurve = depth;
		}

		IStimulus ICurve<IStimulus>.Evaluate (float t) { return this.Evaluate(t); }
		public IStimulus Evaluate (float t)
		{ return new Stimulus(intensity: intensityCurve.Evaluate(t), depth: depthCurve.Evaluate(t)); }

		public static implicit operator StimulusCurve (float val)
		{ return new StimulusCurve(AnimationCurve.Constant(0f, 1f, val), AnimationCurve.Constant(0f, 1f, val)); }
	}
}