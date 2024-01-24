using UnityEngine;

using System.Collections.Generic;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;
using TRandomFloatRange = PHATASS.Utils.Types.Ranges.ILimitedRange<System.Single>;

using IFloatEventReceiver = PHATASS.Utils.Events.IFloatEventReceiver;

namespace PHATASS.Miscellaneous.Kickers
{
	public abstract class KickerOnConditionRandomFloatEvent : KickerOnConditionHeldOnFixedUpdateBase
	{
	//serialized properties 
		[Tooltip("Range of minimum and maximum intensity values")]
		[SerializeField]
		private PHATASS.Utils.Types.Ranges.RandomFloatRange _intensityRange;
		private TRandomFloatRange intensityRange { get { return this._intensityRange; }}

		[Tooltip("If true, the same random value will be applied to every managed controller each kick. If false, a different random value will be generated for each separate controller.")]
		[SerializeField]
		private bool sameValueForAll = true;

		[Tooltip("Target IFloatEventReceiver components whose value will be randomized each kick")]
		[SerializeField]
 		[SerializedTypeRestriction(typeof(IFloatEventReceiver))]
		private List<UnityEngine.Object> _eventReceiverList = null;
		private IList<IFloatEventReceiver> _eventReceiverListAccessor = null;
		private IList<IFloatEventReceiver> eventReceiverList
		{
			get
			{
				//create accessor if unavailable
				if (this._eventReceiverListAccessor == null && this._eventReceiverList != null)
				{ this._eventReceiverListAccessor = new PHATASS.Utils.Types.Wrappers.UnityObjectListCastedAccessor<IFloatEventReceiver>(this._eventReceiverList); }

				return this._eventReceiverListAccessor;
			}
		}
	//ENDOF serialized properties 

	//inherited method overrides
		public override void Kick ()
		{
			float randomValue = 0f;
			if (this.sameValueForAll) { randomValue = this.intensityRange.random; }

			foreach (IFloatEventReceiver eventReceiver in this.eventReceiverList)
			{
				if (!this.sameValueForAll) { randomValue = this.intensityRange.random; }
				eventReceiver.Event(randomValue);
			}
		}
	//ENDOF method overrides

	//MonoBehaviour Lifecycle
	//ENDOF MonoBehaviour Lifecycle
	}
}