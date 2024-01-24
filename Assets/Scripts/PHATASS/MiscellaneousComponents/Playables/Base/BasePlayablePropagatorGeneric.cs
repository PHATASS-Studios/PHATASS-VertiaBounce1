using UnityEngine;
using System.Collections.Generic;

using static PHATASS.Utils.Extensions.IListExtensions;

namespace PHATASS.Miscellaneous.Playables
{
// Base class for a generic playable propagator. Handles a list of TPlayable objects.
//	Manages re-propagation, randomization, and option serialization, but doesn't have a concrete implementation for any type of TPlayable
//	Inheriting classes need to provide implementation for the following:
	// abstract void Play (TPlayable playable):
	//		Client class needs to provide the actual "playing" of the playable object.
	// abstract IList<TPlayable> playables { get; }
	//		Client class needs to provide a proper serialization of the TPlayable list, and provide access to it as an IList<T>
	//
	public abstract class BasePlayablePropagatorGeneric <TPlayable> : PlayablePropagatorBase
		//where TPlayable : Component
	{
	//abstract members
		// Client class needs to provide the actual "playing" of the playable object.
		protected abstract void Play (TPlayable playable);

		// Client class needs to provide a proper serialization of the TPlayable list, and provide access to it as an IList<T>
		protected abstract IList<TPlayable> playables { get; }
	//ENDOF abstract members

	//serialized fields
		[SerializeField]
		[Tooltip("If PlayRandomOnPlay is selected, calling Play() calls PlayRandom(). If PlayAllOnPlay is selected, calling Play() calls PlayAll()")]
		private DefaultPropagationMode defaultPropagationMode = DefaultPropagationMode.PlayRandomOnPlay;
	//ENDOF serialized fields

	//overrides
		//default play behaviour
		protected override void Play ()
		{
			if (this.defaultPropagationMode == DefaultPropagationMode.PlayRandomOnPlay)
			{
				this.PlayRandom();
				return;
			}
			if (this.defaultPropagationMode == DefaultPropagationMode.PlayAllOnPlay)
			{
				this.PlayAll();
				return;
			}
			
			Debug.LogWarning("PlayerOneShotBase.Play(): unrecognized DefaultPropagationMode value");
		}

		//calls Play() on all playables
		protected override void PlayAll ()
		{
			foreach (TPlayable playable in this.playables)
			{
				this.Play(playable);
			}
		}

		//calls Play() on playable with target index. If index < 0, one is chosen randomly
		protected override void PlayOne (int target)
		{
			if ( target < 0)
			{ this.PlayRandom(); }
			else
			{ this.Play(this.playables[target]); }
		}

		//calls Play() on one random playable
		protected override void PlayRandom ()
		{
			this.Play(((IList<TPlayable>) this.playables).ERandomElement());
		}
	//ENDOF overrides

	//private enums
		private enum DefaultPropagationMode
		{
			PlayRandomOnPlay = 0,
			PlayAllOnPlay = 1
		}
	//ENDOF private enums
	}
}