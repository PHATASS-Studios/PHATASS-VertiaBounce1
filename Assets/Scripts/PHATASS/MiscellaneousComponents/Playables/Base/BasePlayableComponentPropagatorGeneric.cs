using UnityEngine;
using System.Collections.Generic;

namespace PHATASS.Miscellaneous.Playables
{
// Base class for a MonoBehaviour/Component playable propagator. Handles a list of Component TPlayable objects.
//	Inheriting classes need to provide implementation for the following:
	// abstract void Play (TPlayable playable):
	//		Client class needs to provide the actual "playing" of the playable object.
	//
	public abstract class BasePlayableComponentPropagatorGeneric <TPlayable> :
		BasePlayablePropagatorGeneric <TPlayable>
		where TPlayable : Component
	{
	//overrides
		// TPlayable list access serialization
		protected override IList<TPlayable> playables
		{ get { return this.playableList; }}

		// Client class needs to provide the actual "playing" of the playable object.
		//	Unimplemented
		//protected abstract void Play (TPlayable playable);
	//ENDOF overrides

	//serialized fields
		[Tooltip("List of playables handled. PlayRandom() plays one of these randomly. PlayOne(target) plays the playable at target index. PlayAll() simultaneously triggers every playable. Play() behaviour is defined by defaultPropagationMode")]
		[SerializeField]
		private TPlayable[] playableList;
	//ENDOF serialized fields
	}
}