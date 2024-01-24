using UnityEngine;
using System.Collections.Generic;

using static PHATASS.Utils.Extensions.IListExtensions;

namespace PHATASS.Miscellaneous.Playables
{
	public abstract class PlayableBase : MonoBehaviour, IPlayable
	{
	//IPlayable implementation
		//default play behaviour
		void IPlayable.Play ()
		{ this.Play(); }
	//ENDOF IPlayable

	//publicly exposed events
		public void DoPlay ()
		{ this.Play(); }
	//ENDOF publicly exposed events

	//abstract members
		protected abstract void Play ();
	//ENDOF abstract members
	}

	public abstract class PlayablePropagatorBase : PlayableBase, IPlayablePropagator
	{
	//IPlayablePropagator implmentation
		//calls Play() on all playables
		void IPlayablePropagator.PlayAll () { this.PlayAll(); }

		//calls Play() on playable with target index. If index < 0, one is chosen randomly
		void IPlayablePropagator.PlayOne (int target) { this.PlayOne(target); }

		//calls Play() on one random playable
		void IPlayablePropagator.PlayRandom () { this.PlayRandom(); }
	//ENDOF IPlayablePropagator

	//publicly exposed events
		public void DoPlayAll () { this.PlayAll(); }
		
		public void DoPlayOne (int target) { this.PlayOne(target); }
		
		public void DoPlayRandom () { this.PlayRandom(); }
	//ENDOF publicly exposed events

	//abstract members
		protected abstract void PlayAll ();
		/*{
			foreach (TPlayable playable in playableList)
			{
				this.Play(playable);
			}
		}*/

		protected abstract void PlayOne (int target);
		/*{
			if ( target < 0)
			{ this.PlayRandom(); }
			else
			{ this.Play(this.playableList[target]); }
		}*/

		protected abstract void PlayRandom ();
		/*{
			this.Play(((IList<TPlayable>) this.playableList).ERandomElement());
		}*/
	//ENDOF abstract members
	}
}