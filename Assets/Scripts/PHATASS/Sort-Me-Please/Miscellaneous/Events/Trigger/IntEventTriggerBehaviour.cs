using System.Collections.Generic;
using UnityEngine;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

namespace PHATASS.Utils.Events
{
	public class IntEventTriggerBehaviour : MonoBehaviour
	{
	//Serialized fields
		[Tooltip("Events trigger for each of these receivers, with an int value as configured")]
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

		[Tooltip("Value passed to triggered events unless another value is passed")]
		[SerializeField]
		private int defaultValue = 0;
	//ENDOF Serialized fields

	//publicly hooks
		public void TriggerIntEvent ()
		{ this.Trigger(this.defaultValue); }

		public void TriggerIntEventEvent (int value)
		{ this.Trigger(value); }
	//ENDOF public hooks

	//private methods
		private void Trigger (int val)
		{
			foreach (IIntEventReceiver eventReceiver in this.eventReceivers)
			{ eventReceiver.Event(val);	}
		}
	//ENDOF private
	}
}
