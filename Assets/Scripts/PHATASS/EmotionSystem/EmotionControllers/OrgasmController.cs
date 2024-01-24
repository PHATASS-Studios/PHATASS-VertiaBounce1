using UnityEngine;

using System.Collections.Generic;

using static PHATASS.Utils.Events.IEventReceiverEnumerableExtensions;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

namespace PHATASS.EmotionSystem
{
// Class specifically handling Orgasm. It is a decaying IEmotion with a climax state
//	Exposes the normalized progress towards climax as IFloatValue 
//	[To-Do]: Port most of this to a base ClimaxableEmotionController
	public class OrgasmController :
		DecayingEmotionController,
		PHATASS.Utils.Types.Values.IFloatValue
	{
	//IFloatValue
		float PHATASS.Utils.Types.Values.IValue<float>.value { get { return this.climaxRate; }}
		private float climaxRate { get { return this.stimulusBuildUp / this.orgasmTriggerThreshold; }}
	//ENDOF IFloatValue

	//serialized fields
		[Tooltip("When stimulus reaches the threshold, orgasm is triggered")]
		[SerializeField]
		private float orgasmTriggerThreshold = 100f;

		[Tooltip("On emotion climas, trigger these IStimulusEventReceiver with an stimulus corresponding to accumulated emotion build up (intensity) and desensitization (depth)")]
		[SerializeField]
		[SerializedTypeRestriction(typeof(IStimulusEventReceiver))]
		private List<UnityEngine.Object> _onClimaxStimulusEventReceivers = null;
		private IList<IStimulusEventReceiver> _onClimaxStimulusEventReceiversAccessor = null;
		private IList<IStimulusEventReceiver> onClimaxStimulusEventReceivers
		{
			get
			{
				//create accessor if unavailable
				if (this._onClimaxStimulusEventReceiversAccessor == null && this._onClimaxStimulusEventReceivers != null)
				{ this._onClimaxStimulusEventReceiversAccessor = new PHATASS.Utils.Types.Wrappers.UnityObjectListCastedAccessor<IStimulusEventReceiver>(this._onClimaxStimulusEventReceivers); }

				return this._onClimaxStimulusEventReceiversAccessor;
			}
		}

		[Tooltip("UnityEvents fired on reaching orgasm")]
		[SerializeField]
		private UnityEngine.Events.UnityEvent onClimaxUnityEvents;

		[Tooltip("total accumulated pleasure since last orgasm. This is released during orgasm and dictates its intensity")]
		[SerializeField]
		private float totalPleasure = 0f;
	//ENDOF serialized fields


	//MonoBehaviour lifecycle
		protected override void Update ()
		{
			this.CheckClimax();
			base.Update();
		}
	//ENDOF MonoBehaviour lifecycle		

	//overrides
	/*	protected override void UpdateDecay (float time)
		{
			this.stimulusBuildUp = this.FramerateIndependentDamp(this.stimulusBuildUp, this.stimulusDecayRate, time);
			this.desensitization = this.FramerateIndependentDamp(this.desensitization, this.desensitizationDecayRate, time);
		}
*/
		protected override void Stimulate (IStimulus stimulus)
		{
			this.totalPleasure += this.StimulusToPleasure(stimulus);

			base.Stimulate(stimulus);

			this.CheckClimax();
		}

		private void CheckClimax ()
		{
			if (this.stimulusBuildUp >= this.orgasmTriggerThreshold)
			{
				this.Climax();
			}
		}

		private void Climax ()
		{
			this.TriggerClimaxEvents();
			this.ResetBuildUp();
		}

		private void ResetBuildUp ()
		{
			this.stimulusBuildUp = 0f;
			this.desensitization = 0f;
			this.totalPleasure = 0f;	
		}

		private void TriggerClimaxEvents ()
		{
			this.onClimaxUnityEvents.Invoke();
			this.onClimaxStimulusEventReceivers.ETriggerAll(this.GetBuildUpAsStimulus());
		}
	//ENDOF overrides

	//private
		private float StimulusToPleasure (IStimulus stimulus)
		{ return this.StimulusToBuildUp(stimulus); } 

		private IStimulus GetBuildUpAsStimulus ()
		{ return new Stimulus(intensity: this.stimulusBuildUp, depth: this.desensitization); }
	//ENDOF private
	}
}