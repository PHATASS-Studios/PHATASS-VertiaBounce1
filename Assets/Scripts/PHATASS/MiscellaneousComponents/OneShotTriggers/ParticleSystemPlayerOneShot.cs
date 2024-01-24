using UnityEngine;

namespace PHATASS.Miscellaneous
{
	public class ParticleSystemPlayerOneShot : PlayerOneShotBase<ParticleSystem>
	{
		[SerializeField]
		private bool forceRestart = true;
		[SerializeField]
		private bool propagateToChildren = true;

		protected override void Play (ParticleSystem particleSystem)
		{
			Debug.LogWarning(gameObject.name + ".Play()");
			if (forceRestart) { particleSystem.Stop(withChildren: propagateToChildren); }
			particleSystem.Play(withChildren: propagateToChildren);
		}
	}
}