using IFloatRange = PHATASS.Utils.Types.Ranges.IFloatRange;

namespace PHATASS.AudioSystem
{
	public interface IAudioPlaybackProperties
	{
		//clip to play back
		UnityEngine.AudioClip clip { get; }

		//volume modifier for this clip
		IFloatRange volume { get; }

		//should the clip loop
		bool loop { get; }

		//pitch
		IFloatRange pitch { get; }
	}
}