using UnityEngine;

using RandomFloatRange = PHATASS.Utils.Types.Ranges.RandomFloatRange;
using IFloatRange = PHATASS.Utils.Types.Ranges.IFloatRange;

namespace PHATASS.AudioSystem
{
	//container object with settings for an audiosource playback
	[System.Serializable]
	public class AudioPlaybackProperties : IAudioPlaybackProperties
	{
		[SerializeField]
		private AudioClip _clip = null;				//clip to play back
		AudioClip IAudioPlaybackProperties.clip { get { return this._clip; }}

		[SerializeField]
		private RandomFloatRange _volume = null;		//volume modifier for this clip
		IFloatRange IAudioPlaybackProperties.volume { get { return this._volume; }}

		[SerializeField]
		private bool _loop = false;				//should the clip loop
		bool IAudioPlaybackProperties.loop { get { return this._loop; }}

		[SerializeField]
		private RandomFloatRange _pitch = null;		//pitch
		IFloatRange IAudioPlaybackProperties.pitch { get { return this._pitch; }}

		public AudioPlaybackProperties (
			AudioClip clip,
			RandomFloatRange volume,// = (RandomFloatRange) 1f,
			RandomFloatRange pitch,// = (RandomFloatRange) 1f,
			bool loop = false
		) {
			this._clip = clip;
			this._volume = volume;
			this._loop = loop;
			this._pitch = pitch;

		}
	}
}