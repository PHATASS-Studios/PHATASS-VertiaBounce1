namespace PHATASS.EmotionSystem
{
// Interface representing an emotional/sensory stimulus
	public interface IStimulus
	{
		// intensity of the stimulus
		float intensity { get; }

		// depth of the stimulus. A receiver partially ignores its own desensitization for higher depth stimuli
		float depth { get; }

	}

	public static class IStimulusExtensions
	{
		public static IStimulus EScale (this IStimulus a, IStimulus b)
		{ return new Stimulus(intensity: a.intensity * b.intensity, depth: a.depth * b.depth); }
	}
}