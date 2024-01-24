using System.Collections.Generic;

using UnityEngine;
using Particle = UnityEngine.ParticleSystem.Particle;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

using TEventReceiver = PHATASS.Utils.Events.IValueChangedEventReceiver<int>;

namespace PHATASS.Utils.Events
{
/*FINALLY UNUSED CODE


//this component increments a counter according to a particle's properties each time the particle event is fired
	public abstract class TriggerModeSkeleton <TValueType> :
		ParticleEventReceiverBase,
		PHATASS.Utils.Types.IValue<TValueType>
		//PHATASS.Utils.Types.IValue<float>
	{
	//Serialized fields
		[Tooltip("Current value sum")]
		[SerializeField]
		private float accumulatedValue;

		[Tooltip(@"Mode of operation for the OnValueChanged event:
			> AsManyTimesAsPossible: Event will be fired EVERY TIME the conditions are met, even multiple times per frame.
			> OnceDuringUpdate: Event will be fired ONLY ONCE during Update, if conditions were met at least once during that frame
			> OnceDuringLateUpdate: Event will be fired ONLY ONCE during LateUpdate, if conditions were met at least once during that frame
			> OnceDuringFixedUpdate: Event will be fired ONLY ONCE during FixedUpdate, if conditions were met at least once during that frame
		")]
		[SerializeField]
		private EventTriggerMode triggerMode = EventTriggerMode.AsManyTimesAsPossible;

		[Tooltip("Events triggered each time value is changed")]
		[SerializeField]
		[SerializedTypeRestriction(typeof(TEventReceiver))]
		private List<UnityEngine.Object> _onValueChanged = null;
		private IList<TEventReceiver> _onValueChangedAccessor = null;
		private IList<TEventReceiver> onValueChanged
		{ get {
			if (this._onValueChangedAccessor == null && this._onValueChanged != null) //create accessor if unavailable
			{ this._onValueChangedAccessor = new PHATASS.Utils.Types.Wrappers.UnityObjectListCastedAccessor<TEventReceiver>(this._onValueChanged); }
			return this._onValueChangedAccessor;
		}}

	//ENDOF Serialized

	//IValue<TValueType>
		TValueType PHATASS.Utils.Types.IValue<TValueType>.value { get { return this.value; }}
		protected abstract TValueType value { get; }
	//ENDOF IValue<TValueType>

	//overrides
		protected override void Event (Particle particle)
		{

		}
	//ENDOF overrides

	//private enums
	//[TO-DO] Take this somewhere safe, maybe?
		//Enumeration defining mode of operation of some event handlers
		private enum EventTriggerMode
		{
			AsManyTimesAsPossible,	//AsManyTimesAsPossible: Event will be fired EVERY TIME the conditions are met, even multiple times per frame.
			OnceDuringUpdate,		//OnceDuringUpdate: Event will be fired ONLY ONCE during Update, if conditions were met at least once during that frame
			OnceDuringLateUpdate,	//OnceDuringLateUpdate: Event will be fired ONLY ONCE during LateUpdate, if conditions were met at least once during that frame
			OnceDuringFixedUpdate	//OnceDuringFixedUpdate: Event will be fired ONLY ONCE during FixedUpdate, if conditions were met at least once during that frame
		}
	//ENDOF private enums
	}
//*/
}