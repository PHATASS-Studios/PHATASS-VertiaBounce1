using System.Collections.Generic;

using UnityEngine;
using Particle = UnityEngine.ParticleSystem.Particle;

using IAudioPlaybackProperties = PHATASS.AudioSystem.IAudioPlaybackProperties;

using IAudioPlaybackPropertiesEventReceiver = PHATASS.Utils.Events.IAudioPlaybackPropertiesEventReceiver;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

namespace PHATASS.Utils.Events
{
	public abstract class ParticleEventPropertyToAudioPlaybackPropertiesEventBase : ParticleEventReceiverBase
	{
	//Serialized fields
		[Tooltip("Events triggered for each received particle event, with an int value generated from some of the particles properties")]
		[SerializeField]
		[SerializedTypeRestriction(typeof(IAudioPlaybackPropertiesEventReceiver))]
		private List<UnityEngine.Object> _eventReceivers = null;
		private IList<IAudioPlaybackPropertiesEventReceiver> _eventReceiversAccessor = null;
		private IList<IAudioPlaybackPropertiesEventReceiver> eventReceivers
		{ get {
			if (this._eventReceiversAccessor == null && this._eventReceivers != null) //create accessor if unavailable
			{ this._eventReceiversAccessor = new PHATASS.Utils.Types.Wrappers.UnityObjectListCastedAccessor<IAudioPlaybackPropertiesEventReceiver>(this._eventReceivers); }
			return this._eventReceiversAccessor;
		}}
	//ENDOF Serialized fields

	//overrides
		protected override void Event (Particle param0)
		{
			IAudioPlaybackProperties value = this.ParticleToValue(param0);
			foreach (IAudioPlaybackPropertiesEventReceiver eventReceiver in this.eventReceivers)
			{ eventReceiver.Event(value); }
		}
	//ENDOF overrides

	//overridable members
		protected abstract IAudioPlaybackProperties ParticleToValue (Particle particle);
	//ENDOF overridable members
	}
}
