using System.Collections.Generic;

using UnityEngine;
using Particle = UnityEngine.ParticleSystem.Particle;

using IAudioPlaybackProperties = PHATASS.AudioSystem.IAudioPlaybackProperties;
using AudioPlaybackProperties = PHATASS.AudioSystem.AudioPlaybackProperties;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

using RandomFloatRange = PHATASS.Utils.Types.Ranges.RandomFloatRange;

using static PHATASS.Utils.Extensions.IListExtensions;

namespace PHATASS.Utils.Events
{
	public class ParticleEventStartSizeToAudioPlaybackPropertiesEvent : ParticleEventPropertyToAudioPlaybackPropertiesEventBase 
	{
	//Serialized fields
		[SerializeField]
		[Tooltip("List of potential audioclips to use. One of these will be chosen randomly.")]
		private AudioClip[] audioClips;

		[Header("Pitch")]
		[Tooltip("Curve determining MINIMUM pitch, using particle.startSize as time dimension. Final value will be random between minimum and maximum.")]
		[SerializeField]
		private AnimationCurve sizeToPitchMinimumCurve;
		[Tooltip("Curve determining MAXIMUM pitch, using particle.startSize as time dimension. Final value will be random between minimum and maximum.")]
		[SerializeField]
		private AnimationCurve sizeToPitchMaximumCurve;

		[Header("Volume")]
		[Tooltip("Curve determining MINIMUM volume, using particle.startSize as time dimension. Final value will be random between minimum and maximum.")]
		[SerializeField]
		private AnimationCurve sizeToVolumeMinimumCurve;
		[Tooltip("Curve determining MAXIMUM volume, using particle.startSize as time dimension. Final value will be random between minimum and maximum.")]
		[SerializeField]
		private AnimationCurve sizeToVolumeMaximumCurve;
	//ENDOF Serialized fields

	//overrides
		protected override IAudioPlaybackProperties ParticleToValue (Particle particle)
		{
			float size = particle.startSize;

			return new AudioPlaybackProperties (
				clip: ((IList<AudioClip>) this.audioClips).ERandomElement<AudioClip>(),
				volume: new RandomFloatRange(minimum: this.sizeToVolumeMinimumCurve.Evaluate(size), maximum: this.sizeToVolumeMaximumCurve.Evaluate(size)),
				pitch: new RandomFloatRange(minimum: this.sizeToPitchMinimumCurve.Evaluate(size), maximum: this.sizeToPitchMaximumCurve.Evaluate(size)),
				loop: false
			);
		}
	//ENDOF overrides
	}
}
