using UnityEngine;

namespace PHATASS.Miscellaneous.Playables
{
	public class GenericPlayableBehaviourPropagator : BasePlayableComponentPropagatorGeneric<PlayableBase>
	{
		protected override void Play (PlayableBase playable)
		{
			((IPlayable) playable).Play();
		}
	}
}