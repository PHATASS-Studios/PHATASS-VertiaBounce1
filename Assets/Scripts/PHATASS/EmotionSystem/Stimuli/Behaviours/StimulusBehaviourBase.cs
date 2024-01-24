using UnityEngine;
using System.Collections.Generic;

using static PHATASS.Utils.Events.IEventReceiverEnumerableExtensions;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

namespace PHATASS.EmotionSystem
{
	public abstract class StimulusBehaviourBase : MonoBehaviour
	{
	//Serialized Fields
		//[Header("Base stimulus value")]
		[Tooltip("List of Stimulus Event Receivers this controller stimulates")]
		[SerializeField]
		[SerializedTypeRestriction(typeof(IStimulusEventReceiver))]
		private List<UnityEngine.Object> _stimulables = null;
		private IList<IStimulusEventReceiver> _stimulablesAccessor = null;
		private IList<IStimulusEventReceiver> stimulables
		{
			get
			{
				//create accessor if unavailable
				if (this._stimulablesAccessor == null && this._stimulables != null)
				{ this._stimulablesAccessor = new PHATASS.Utils.Types.Wrappers.UnityObjectListCastedAccessor<IStimulusEventReceiver>(this._stimulables); }

				return this._stimulablesAccessor;
			}
		}

		[Tooltip("Base stimulus value - it may then be further scaled according to other factors")]
		[SerializeField]
		protected Stimulus baseStimulus = new Stimulus (1f, 1f);
	//ENDOF Serialized

	//private methods
		protected void PropagateStimulus (IStimulus stimulus)
		{
			this.stimulables.ETriggerAll(stimulus);
			/*foreach (IStimulable stimulable in this.stimulables)
			{
				stimulable.Event(stimulus);
			}*/
		}
	//ENDOF private methods

	//inheritable private types

	//ENDOF inheritable private types
	}
}