using UnityEngine;

using CubismParameter = Live2D.Cubism.Core.CubismParameter;

using static PHATASS.Utils.Extensions.CubismParameterExtensions;

namespace PHATASS.EmotionSystem
{
	public abstract class StimulusFromL2DParamBehaviourBase : StimulusBehaviourBase
	{
	//Serialized fields
		[Tooltip("This is the L2D parameter from which value stimulus is inferred")]
		[SerializeField]
		protected CubismParameter cubismParameter;

		[Tooltip("Stimulus scaling by current parameter value, normalized - values from T=0 to T=1 dictate scale of stimulus properties when at that relative point within the parameter's limits")]
		[SerializeField]
		private StimulusCurve stimulusScalingByParameterValue = 1f;
	//ENDOF Serialized fields

	//private
	//ENDOF private

	//inheritable members
		// returns current parameter's normalized value
		protected float normalizedValue { get { return this.cubismParameter.EGetNormalizedValue(); }}

		//creates a stimulus, scaled according to the parameter's position and 
		protected IStimulus ValueScaledStimulus (float intensity, float depth)
		{ return this.ValueScaledStimulus(new Stimulus(intensity: intensity, depth: depth)); }
		protected IStimulus ValueScaledStimulus (IStimulus stimulus = null)
		{
			if (stimulus == null) { stimulus = this.baseStimulus; }
			return stimulus.EScale(this.stimulusScalingByParameterValue.Evaluate(this.normalizedValue));
		}
	//ENDOF inheritable members
	}
}