using System.Collections.Generic;

using UnityEngine;
using Particle = UnityEngine.ParticleSystem.Particle;

using IIntEventReceiver = PHATASS.Utils.Events.IIntEventReceiver;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

namespace PHATASS.Utils.Events
{
	public abstract class ParticleEventPropertyToIntEventBase : ParticleEventReceiverBase
	{
	//Serialized fields
		[Tooltip("Events triggered for each received particle event, with an int value generated from some of the particles properties")]
		[SerializeField]
		[SerializedTypeRestriction(typeof(IIntEventReceiver))]
		private List<UnityEngine.Object> _eventReceivers = null;
		private IList<IIntEventReceiver> _eventReceiversAccessor = null;
		private IList<IIntEventReceiver> eventReceivers
		{ get {
			if (this._eventReceiversAccessor == null && this._eventReceivers != null) //create accessor if unavailable
			{ this._eventReceiversAccessor = new PHATASS.Utils.Types.Wrappers.UnityObjectListCastedAccessor<IIntEventReceiver>(this._eventReceivers); }
			return this._eventReceiversAccessor;
		}}
	//ENDOF Serialized fields

	//overrides
		protected override void Event (Particle param0)
		{
			int value = this.ParticleToValue(param0);
			foreach (IIntEventReceiver eventReceiver in this.eventReceivers)
			{ eventReceiver.Event(value); }
		}
	//ENDOF overrides

	//overridable members
		protected abstract int ParticleToValue (Particle particle);
	//ENDOF overridable members
	}
}
