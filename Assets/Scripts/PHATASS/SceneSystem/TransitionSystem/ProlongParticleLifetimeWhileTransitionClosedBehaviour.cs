using System.Collections.Generic;
using System.Collections;
using IDisposable = System.IDisposable;

using UnityEngine;
using Particle = UnityEngine.ParticleSystem.Particle;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

using PreallocatedParticleSystemWrapper = PHATASS.Utils.Types.Wrappers.PreallocatedParticleSystemWrapper;
using IParticleAccessor = PHATASS.Utils.Types.Wrappers.IParticleAccessor;

namespace PHATASS.SceneSystem.TransitionSystem
{
	public class ProlongParticleLifetimeWhileTransitionClosedBehaviour : MonoBehaviour
	{
//serialized fields
		private const string classTooltip = "While this transition's completelyClosed == true, all managed particle system's existing particles lifetime will be prolonged by the frame's delta time each frame.";
		[Tooltip(classTooltip)]
		[SerializeField]
		[SerializedTypeRestriction(typeof(ITransitionController))]
		private UnityEngine.Object? _transitionController;
		private ITransitionController transitionController { get { return this._transitionController as ITransitionController; }}

		[Tooltip(classTooltip)]
		[SerializeField]
		private PreallocatedParticleSystemWrapper[] managedParticleSystems;
//ENDOF serialized

//MonoBehaviour
		private void Update ()
		{
			if (this.transitionController.StrictStateCheck(false))
			{
				this.ProlongParticleLifetime(Time.deltaTime);
			}
		}
//ENDOF MonoBehaviour

//private
		private void ProlongParticleLifetime (float time)
		{
			foreach (IParticleAccessor particleSource in this.managedParticleSystems)
			{
				Particle[] particles = particleSource.particles;
				for (int i = 0, iLimit = particleSource.particleCount; i < iLimit; i++)
				{
					particles[i].remainingLifetime += time;
				}
				particleSource.particles = particles;
			}
		}
//ENDOF private

//obsolete broken implementation
	/*
		private Particle[] livingParticles;

		private void RefreshLivingParticleList ()
		{
		//first gather every list of managed particles separately
			List<Particle[]> particleBufferList = new List<Particle[]>();
			Particle[] particleBufferPreliminary = new Particle[0];
			int particleCount = 0;

			foreach (ParticleSystem particleSystem in this.managedParticleSystems)
			{
				particleSystem.GetParticles(particleBufferPreliminary);

				particleCount += particleBufferPreliminary.Length;
				particleBufferList.Add(particleBufferPreliminary);
			}

		//finally congeal buffered particle lists into a single array 
			this.livingParticles = new Particle[particleCount];
			particleCount = 0;
			foreach (Particle[] buffer in particleBufferList)
			{
				buffer.CopyTo(this.livingParticles, particleCount);
				particleCount += buffer.Length;
			}
		}

		private void ProlongParticleLifetime (float time)
		{
			//if continous refresh, forgo cache and get a fresh iterator
			if (this.refreshParticleListConstantly) { this.RefreshLivingParticleList(); }

			for (int i = 0, iLimit = this.livingParticles.Length; i < iLimit; i++)
			{
				this.livingParticles[i].remainingLifetime += time;
				//particle.remainingLifetime += time;
			}
		}
		*/

	}
}