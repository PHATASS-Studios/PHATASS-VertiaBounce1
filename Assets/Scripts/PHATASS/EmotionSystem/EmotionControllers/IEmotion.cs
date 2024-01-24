namespace PHATASS.EmotionSystem
{
// Interface representing an emotion controller
	public interface IEmotion : IStimulable
	{
		//returns accumulated stimulus value, both absolute value and a normalized value relative to maximum
		float buildUp { get; }
		float normalizedBuildUp { get; }
	}
}