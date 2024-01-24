using System.Collections.Generic;

using UnityEngine;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

using static PHATASS.Utils.Events.IEventReceiverEnumerableExtensions;

using IBoolEventReceiver = PHATASS.Utils.Events.IBoolEventReceiver;

namespace PHATASS.Miscellaneous.Kickers
{
	public class OnOffFlickerBoolEventAuto : OnOffFlickerAutoFireBase
	{
	//serialized fields
		[Tooltip("Target IBoolEventReceiver components whose value will be randomized each kick")]
		[SerializeField]
 		[SerializedTypeRestriction(typeof(IBoolEventReceiver))]
		private List<UnityEngine.Object> _eventReceiverList = null;
		private IList<IBoolEventReceiver> _eventReceiverListAccessor = null;
		private IList<IBoolEventReceiver> eventReceiverList
		{ get {
			//create accessor if unavailable
			if (this._eventReceiverListAccessor == null && this._eventReceiverList != null)
			{ this._eventReceiverListAccessor = new PHATASS.Utils.Types.Wrappers.UnityObjectListCastedAccessor<IBoolEventReceiver>(this._eventReceiverList); }

			return this._eventReceiverListAccessor;
		}}
	//ENDOF serialized fields

	//inherited abstract property implementation
		protected override bool state
		{
			set
			{
				this.eventReceiverList.ETriggerAll(value);
			}
		}
	//ENDOF inherited abstract property implementation
	}
}