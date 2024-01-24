using UnityEngine;

namespace PHATASS.Miscellaneous
{
	public abstract class PlayerOneShotBase <TPlayable> : MonoBehaviour
		where TPlayable : Component
	{
		[SerializeField]
		public TPlayable[] playableList;

		public void PlayAll ()
		{
			foreach (TPlayable playable in playableList)
			{
				Play(playable);
			}
		}

		public void PlayOne (int target)
		{
			Play(playableList[target]);
		}

		protected abstract void Play (TPlayable playable);
	}
}