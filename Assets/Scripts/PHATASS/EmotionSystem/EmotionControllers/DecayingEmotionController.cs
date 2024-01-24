using System.Collections.Generic;
using UnityEngine;

using IStimulable = PHATASS.EmotionSystem.IStimulable;
using IStimulus = PHATASS.EmotionSystem.IStimulus;

using static PHATASS.Utils.Extensions.FloatExtensions;
using static PHATASS.Utils.Extensions.FrameIndependentSmoothingExtensions;

using static PHATASS.Utils.Events.IEventReceiverEnumerableExtensions;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

namespace PHATASS.EmotionSystem
{
	//ORGASMS EVERYWHERE RITE NAO
	public class DecayingEmotionController : MonoBehaviour, IEmotion
	{
	//serialized fields
		[Tooltip("Stimulus upper buildup limit - stimulus will only accumulate up to this maximum value")]
		[SerializeField]
		private float stimulusUpperLimit = 100f;

		[Tooltip("Stimulus lower buildup limit - stimulus will only accumulate down to this minimum value")]
		[SerializeField]
		private float stimulusLowerLimit = 0f;

		[Tooltip("Stimulus balance value - will drop towards this value gradually")]
		[SerializeField]
		private float stimulusBalanceValue = 0f;

		[Tooltip("Scale of desensitization when receiving an stimulus. Desensitization = (intensity * scale) / depth")]
		[SerializeField]
		private float desensitizationScale = 0.01f;

		[Tooltip("Maximum desensitization value. Desensitization starts at 0 and increases with stimulus. Stimulus change = stimulus / (1 + (desensitization / depth))")]
		[SerializeField]
		private float maxDesensitization = 4f;

		[Tooltip("Stimulus decays at this rate per second")]
		[SerializeField]
		private float stimulusDecayRate = 0.05f;

		[Tooltip("Desensitization drops at this rate per second")]
		[SerializeField]
		private float desensitizationDecayRate = 0.1f;

		[Tooltip("Decay rate multiplier curve. While NOT receiving stimulus, this multiplier changes according to this curve over time. While receiving stimulus, this multiplier resets to T=0. Intensity represents intensity decay, Depth represents desensitization decay")]
		[SerializeField]
		private StimulusCurve decayRateIdleMultiplierCurve = 1f;

		[Tooltip("After processing a received stimulus, re-propagate a copy of it to these stimulables. This stimulus intensity is dampened by desensitization, its depth is left intact ")]
		[SerializeField]
		[SerializedTypeRestriction(typeof(IStimulable))]
		private List<UnityEngine.Object> _repropagationListeners = null;
		private IList<IStimulable> _repropagationListenersAccessor = null;
		private IList<IStimulable> repropagationListeners
		{
			get
			{//create accessor if unavailable
				if (this._repropagationListenersAccessor == null && this._repropagationListeners != null)
				{ this._repropagationListenersAccessor = new PHATASS.Utils.Types.Wrappers.UnityObjectListCastedAccessor<IStimulable>(this._repropagationListeners); }

				return this._repropagationListenersAccessor;
			}
		}
	//ENDOF serialized fields

	//IStimulable implementation
		void PHATASS.Utils.Events.ISimpleEventReceiver<IStimulus>.Event (IStimulus Param0)
		{ this.Stimulate(Param0); }
	//ENDOF IStimulable

	//IEmotion implementation
		float IEmotion.buildUp { get { return this.stimulusBuildUp; }}
		float IEmotion.normalizedBuildUp { get { return this.normalizedBuildUp; }}
		private float normalizedBuildUp
		{ get {	return (this.stimulusBuildUp - this.stimulusLowerLimit) / (this.stimulusUpperLimit - this.stimulusLowerLimit); }}
	//ENDOF IEmotion

	//private fields
		[SerializeField]
		protected float stimulusBuildUp = 0f;	// Current total stimulus accumulated by this controller
		[SerializeField]
		protected float desensitization = 0f;	// Current desensitization multiplier, as implemented @ DephtToDesensitizationModifier(depth)

		// Time since last stimulus was received - used to modify decay rates according to the AnimationCurve decayRateIdleMultiplierCurve
		private float timeSinceLastStimulus = 0f;
	//ENDOF private fields	

	//MonoBehaviour lifecycle
		protected virtual void Update ()
		{
			this.UpdateDecay(Time.deltaTime);
			this.timeSinceLastStimulus += Time.deltaTime; //Update stimulus timer after decay so recent events cause the shortest possible time
		}
	//ENDOF MonoBehaviour lifecycle	

	//private properties
		private IStimulus decayRateMultiplier
		{ get {
			return this.decayRateIdleMultiplierCurve.Evaluate(this.timeSinceLastStimulus);
		}}
	//ENDOF private properties

	//private methods
		private void UpdateDecay (float time)
		{
			IStimulus decayRateMultiplier = this.decayRateMultiplier;
			//Debug.LogWarning("UpdateDecay");
			this.stimulusBuildUp = this.stimulusBuildUp.EFrameIndependentDamp(this.stimulusDecayRate * decayRateMultiplier.intensity, time);
			this.desensitization = this.desensitization.EFrameIndependentDamp(this.desensitizationDecayRate * decayRateMultiplier.depth, time);
		}

		protected virtual void Stimulate (IStimulus stimulus)
		{
			this.timeSinceLastStimulus = 0f;

			float intensityGain = this.StimulusToBuildUp(stimulus);
			float desensitizationGain = this.StimulusToDesensitization(stimulus);
			/*
			Debug.Log("Stimulus: " + stimulus);
			Debug.Log("Buildup gain: " + intensityGain);
			Debug.Log("desensitization gain: " + desensitizationGain);
			//*/
			this.stimulusBuildUp += intensityGain;
			this.desensitization += desensitizationGain;

			if (this.desensitization > this.maxDesensitization)
			{ this.desensitization = this.maxDesensitization; }

			this.RePropagate(new Stimulus(intensityGain, stimulus.depth));
		}

		protected float StimulusToBuildUp (IStimulus stimulus)
		{
			return stimulus.intensity / (1 + (this.desensitization * this.DephtToDesensitizationModifier(stimulus.depth)));
		}

		protected float StimulusToDesensitization (IStimulus stimulus)
		{	//Maybe this should reduce further desensitization by current desensitization, but I think I like it
			return stimulus.intensity * this.desensitizationScale * this.DephtToDesensitizationModifier(stimulus.depth);
		}

		private float DephtToDesensitizationModifier (float depth)
		{
			if (depth >= 0) { return 1/(1 + depth); }
			else { return 1 + depth.EAbs(); }
		}

		private void RePropagate (IStimulus stimulus)
		{ this.repropagationListeners.ETriggerAll(stimulus); }
	//ENDOF private methods
	}
}