namespace PHATASS.EmotionSystem
{
//	IStimulable/IStimulusEventReceiver that triggers a set of IPlayables when receiving any stimulus
	public class TriggerPlayableOnStimulusBehaviour :
		PHATASS.Miscellaneous.Playables.SerializedIPlayablePropagator,
		IStimulable
	{
	//serialized fields
	//ENDOF serialized

	//IStimulable
		void PHATASS.Utils.Events.ISimpleEventReceiver<IStimulus>.Event (IStimulus Param0)
		{ this.Play(); }
	//ENDOF IStimulable

	//private members
	//ENDOF private
	}
}