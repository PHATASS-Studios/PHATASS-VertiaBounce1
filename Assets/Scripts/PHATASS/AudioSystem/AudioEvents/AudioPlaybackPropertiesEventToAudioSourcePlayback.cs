using System.Collections.Generic;

using UnityEngine;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

using IAudioPlaybackProperties = PHATASS.AudioSystem.IAudioPlaybackProperties;

namespace PHATASS.Utils.Events
{
	public class AudioPlaybackPropertiesEventToAudioSourcePlayback : AudioPlaybackPropertiesEventReceiverBase
	{
	//Serialized fields
		[SerializeField]
		[Tooltip("AudioSource used to play audio clips as desired")]
		private AudioSource audioSource;
	//ENDOF Serialized fields

	//MonoBehaviour lifecycle
		private void Reset ()
		{
			if (this.audioSource == null) { this.audioSource = this.GetComponent<AudioSource>(); }
		}
	//ENDOF MonoBehaviour

	//overrides
		protected override void Event (IAudioPlaybackProperties param0)
		{
			this.audioSource.pitch = param0.pitch.random;

			//if no looping required, do a one shot call
			if (!param0.loop)
			{ this.audioSource.PlayOneShot(param0.clip, param0.volume.random); }
			else
			{	//other
				this.audioSource.volume = param0.volume.random;
				this.audioSource.loop = param0.loop;
				this.audioSource.clip = param0.clip;
				this.audioSource.Play();
			}
		}
	//ENDOF overrides

	//overridable members
	//ENDOF overridable members
	}
}
