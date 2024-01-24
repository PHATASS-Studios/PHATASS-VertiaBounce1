using System.Collections.Generic;

using UnityEngine;

using RandomFloatRange = PHATASS.Utils.Types.Ranges.RandomFloatRange;

using static PHATASS.Utils.Extensions.IListExtensions;

namespace PHATASS.Miscellaneous.Playables
{
	public class AudioSourceManagedPlayable : PlayableBase
	{
	//serialized fields
		[SerializeField]
		[Tooltip("AudioSource used to play audio clips as desired")]
		private AudioSource audioSource;

		[SerializeField]
		[Tooltip("List of potential audioclips to use when calling play. One of these will be chosen randomly when calling Play()")]
		private AudioClip[] audioClips;


		[SerializeField]
		[Tooltip("If randomizePitch == true, on Play() audioSource pitch will be set to a random value within this range")]
		private bool randomizePitch = true;

		[SerializeField]
		[Tooltip("If randomizePitch == true, on Play() audioSource pitch will be set to a random value within this range")]
		private RandomFloatRange _pitchRandomRange = 1f;
		private PHATASS.Utils.Types.Ranges.ILimitedRange<float> pitchRandomRange { get { return this._pitchRandomRange; }}

		[SerializeField]
		[Tooltip("If randomizeVolume == true, on Play() audioSource ORIGINAL volume (volume during Start()) will be scaled by a random value within this range")]
		private bool randomizeVolume = true;

		[SerializeField]
		[Tooltip("If randomizeVolume == true, on Play() audioSource ORIGINAL volume (volume during Start()) will be scaled by a random value within this range")]
		private RandomFloatRange _volumeScaleRandomRange = 1f;
		private PHATASS.Utils.Types.Ranges.ILimitedRange<float> volumeScaleRandomRange { get { return this._volumeScaleRandomRange; }}
	//ENDOF serialized

	//MonoBehaviour lifecycle
		private void Reset ()
		{
			if (this.audioSource == null)
			{ this.audioSource = this.GetComponent<AudioSource>(); }
		}

		private void Start ()
		{
			this.originalVolume = this.audioSource.volume;
		}
	//ENDOF MonoBehaviour

	//private members
		private float originalVolume = 1f;

		private AudioClip randomAudioClip
		{ get { return ((IList<AudioClip>)this.audioClips).ERandomElement(); }}

		private void PlayRandomClip ()
		{
			this.RandomizeAudioSourceConfiguration();
			this.PlayClip(this.randomAudioClip);
		}

		private void RandomizeAudioSourceConfiguration ()
		{
			if (this.randomizePitch)
			{ this.audioSource.pitch = this.pitchRandomRange.random; }

			if (this.randomizeVolume)
			{ this.audioSource.volume = this.originalVolume * this.volumeScaleRandomRange.random; }
		}

		private void PlayClip (AudioClip clip)
		{
			this.audioSource.PlayOneShot(clip, 1f);
		}
	//ENDOF private members

	//overrides
		protected override void Play ()
		{ this.PlayRandomClip(); }
	//ENDOF overrides
	}
}
