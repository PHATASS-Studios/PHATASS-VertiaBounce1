using UnityEngine;

namespace PHATASS.EmotionSystem
{
// Class representing a single jolt of stimulus
	[System.Serializable]
	public struct Stimulus : IStimulus
	{
	//IStimulus
		[SerializeField]
		private float _intensity;
		float IStimulus.intensity { get { return this._intensity; }}

		[SerializeField]
		private float _depth;
		float IStimulus.depth { get { return this._depth; }}
	//ENDOF IStimulus

	//Constructor
		public Stimulus (float intensity, float depth)
		{
			_intensity = intensity;
			_depth = depth;
		}
	//ENDOF Constructor

	//Operator overrides
		public override string ToString ()
		{
			return System.String.Format("Stimulus intensity: {0:C4} depth: {1:C4}", this._intensity, this._depth);
		}
	//ENDOF operator overrides
	}
}