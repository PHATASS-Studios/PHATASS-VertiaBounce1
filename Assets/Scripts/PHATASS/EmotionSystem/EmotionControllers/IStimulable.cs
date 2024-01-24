namespace PHATASS.EmotionSystem
{
// Interface representing an stimulable emotion
	// Can receive, accumulate, and handle the decay and desensitization of stimuli
	public interface IStimulable : IStimulusEventReceiver
	{
	// Returns current build up of stimulus of this stimulable
		//float stimulusBuildUp { get; }
	}

}