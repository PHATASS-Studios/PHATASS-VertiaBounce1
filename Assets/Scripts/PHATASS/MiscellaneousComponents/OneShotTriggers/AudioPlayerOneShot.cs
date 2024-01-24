using UnityEngine;

namespace PHATASS.Miscellaneous
{
	public class AudioPlayerOneShot : PlayerOneShotBase<AudioSource>
	{
		protected override void Play (AudioSource audioSource)
		{
			audioSource.Play();
		}
	}
}