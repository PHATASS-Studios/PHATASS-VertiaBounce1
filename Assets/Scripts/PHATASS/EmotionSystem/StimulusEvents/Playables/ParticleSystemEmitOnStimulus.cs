using UnityEngine;

namespace PHATASS.EmotionSystem
{
	public class ParticleSystemEmitOnStimulus : MonoBehaviour, IStimulable
	{
	//Serialized fields
		[Tooltip("For each particle paid with an stimulus, one particle will be emitted from EACH of these ParticleSystems")]
		[SerializeField]
		private ParticleSystem[] particleSystems;

		[Tooltip("Size/stimulus particle costs curve. When receiving stimulus, as many particles as big as possible will be emitted. Horizontal axis (time) represents particle cost, vertical axis represents particle size. Time (intensity) values outside this curve will be ignored (below) or clamped. Remaining intensity will be stored for later calls")]
		[SerializeField]
		private AnimationCurve particleSizeStimulusCostCurve = AnimationCurve.Constant(1f, 1f, 1f);
	//ENDOF Serialized fields


	//IStimulable
		void PHATASS.Utils.Events.ISimpleEventReceiver<IStimulus>.Event (IStimulus Param0)
		{ this.Stimulate(Param0); }
	//ENDOF IStimulable

	//MonoBehaviour
		/*
		private void Start ()
		{
			this.LogCurveKeyTimes(this.particleSizeStimulusCostCurve);
		}//*/
	//ENDOF MonoBehaviour

	//private
		private float maxParticleCost
		{ get { return this.particleSizeStimulusCostCurve[this.particleSizeStimulusCostCurve.length-1].time; }}
		private float minParticleCost
		{ get { return this.particleSizeStimulusCostCurve[0].time; }}

		private float stimulusBuildUp = 0f;

		//process a received stimulus
		private void Stimulate (IStimulus stimulus)
		{
			this.stimulusBuildUp += stimulus.intensity;

			//if a large enough amount of stimulus is available, emit as many big particles as possible
			if (this.stimulusBuildUp >= this.maxParticleCost)
			{ this.EmitParticles((int)System.MathF.Truncate(this.stimulusBuildUp / this.maxParticleCost), this.maxParticleCost); }

			//with what's left, emit the largest possible particle
			if (this.stimulusBuildUp >= this.minParticleCost)
			{
				this.EmitParticles(1, (this.stimulusBuildUp > this.maxParticleCost)? this.maxParticleCost : this.stimulusBuildUp);
			}
		}

		//instantiates the number of required particles and discounts their cost
		private void EmitParticles (int particleCount, float particleCost)
		{
			this.stimulusBuildUp -= particleCount * particleCost;

			foreach (ParticleSystem particleSystem in this.particleSystems)
			{ particleSystem.Emit(this.GetParticleParams(particleCost), particleCount); }
		}

		//generates the params for a particle of given cost value
		private ParticleSystem.EmitParams GetParticleParams (float particleCostIndex)
		{
			ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
			emitParams.startSize = this.particleSizeStimulusCostCurve.Evaluate(particleCostIndex);
			return emitParams;
		}
		
	//ENDOF private

	//Debug 
		/*
		private void LogCurveKeyTimes (AnimationCurve curve)
		{
			Debug.Log("curve keys:");
			foreach (Keyframe key in curve.keys)
			{Debug.Log(" > time: " + key.time + " value: " + key.value); }
		}
		//*/
	//ENDOF Debug
	}
}