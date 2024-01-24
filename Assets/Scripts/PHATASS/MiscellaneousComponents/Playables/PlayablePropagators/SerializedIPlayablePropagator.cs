using UnityEngine;
using System.Collections.Generic;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

namespace PHATASS.Miscellaneous.Playables
{
//	IPlayable propagator behaviour
	public class SerializedIPlayablePropagator : BasePlayablePropagatorGeneric<IPlayable>
	{
	//overrides
		// TPlayable list access serialization
		protected override IList<IPlayable> playables
		{ get { return this.playableList; }}

		// Client class needs to provide the actual "playing" of the playable object.
		protected override void Play (IPlayable playable)
		{
			playable.Play();
		}
	//ENDOF overrides

	//serialized fields
		[SerializeField]
		[Tooltip("List of playables handled. PlayRandom() plays one of these randomly. PlayOne(target) plays the playable at target index. PlayAll() simultaneously triggers every playable. Play() behaviour is defined by defaultPropagationMode")]
		[SerializedTypeRestriction(typeof(IPlayable))]
		private List<UnityEngine.Object> _playableList = null;			// Private backing field. Don't access this outside the getter property
		private IList<IPlayable> _playableListAccessor = null;	// Wrapper cache. We store here our IList<IPlayable> wrapper for easy access
		private IList<IPlayable> playableList						// Getter property. Access this to get a usable IList<IPlayable>
		{ get {
				if (this._playableListAccessor == null && this._playableList != null) //create accessor if unavailable
				{ this._playableListAccessor = new PHATASS.Utils.Types.Wrappers.UnityObjectListCastedAccessor<IPlayable>(this._playableList); }

				return this._playableListAccessor;
			}
		}
	//ENDOF serialized
	}
}