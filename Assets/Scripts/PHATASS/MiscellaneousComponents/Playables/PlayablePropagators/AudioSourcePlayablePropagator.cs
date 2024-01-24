using UnityEngine;

namespace PHATASS.Miscellaneous.Playables
{
	public class AudioSourcePlayablePropagator : BasePlayableComponentPropagatorGeneric<AudioSource>
	{
		protected override void Play (AudioSource audioSource)
		{
			audioSource.Play();
		}
	}
}