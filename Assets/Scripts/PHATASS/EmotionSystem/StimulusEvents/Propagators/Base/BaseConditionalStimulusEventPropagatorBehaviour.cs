using UnityEngine;
using System.Collections.Generic;

using static PHATASS.Utils.Events.IEventReceiverEnumerableExtensions;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

namespace PHATASS.EmotionSystem
{
// Base Stimulus event conditional repropagator
//	When receiving an Event(param) call, a condition is checked
//	If this condition is validated, repropagates the event to all registered receivers
	public abstract class BaseConditionalStimulusEventPropagatorBehaviour :
		PHATASS.Utils.Events.BaseConditionalSimpleEventPropagatorBehaviour<IStimulus>,
		IStimulable
	{
	//overrides
		// Propagation condition checking method. Whenever an event is received, re-propagation will only be performed if this returns true.
		//protected abstract bool CheckCondition (TParam0 param0);

		// Serialized list of receivers
		protected override List<UnityEngine.Object> receiversBacking { get { return this._receiversBacking; }}
			[Tooltip("List of Event receivers to which received events will be propagated if provided stimulus meets given conditions.")]
			[SerializeField]
			[SerializedTypeRestriction(typeof(PHATASS.Utils.Events.ISimpleEventReceiver<IStimulus>))]
			private List<UnityEngine.Object> _receiversBacking;
	//ENDOF overrides

	//private
	//ENDOF private
	}
}