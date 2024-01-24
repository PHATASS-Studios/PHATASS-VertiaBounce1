using UnityEngine;
using Particle = UnityEngine.ParticleSystem.Particle;

//using static PHATASS.Utils.Extensions.FrameIndependentSmoothingExtensions;
using static PHATASS.Utils.Extensions.FloatExtensions;

namespace PHATASS.Miscellaneous.FX
{
	public class ParticleFXSmoothLerpTowardsBeforeDespawn2DBehaviour : MonoBehaviour
	{
	//Serialized fields
		[Tooltip("Managed particle system. This particleSystem's particles will be managed.")]
		[SerializeField]
		private ParticleSystem managedParticleSystem;

		[Tooltip("Destination transform. Particles will lerp their towards this position")]
		[SerializeField]
		private Transform destinationTransform;

		[Tooltip("Lerp will begin when the particle reaches this remaining lifetime in seconds. Each particle will lerp towards target during its last N seconds of life.")]
		[SerializeField]
		private float animationTime = 1f;

		[Tooltip("Lerp rate will increase from 0 at life left = animation time up to 1 at life left = 0. It will then be scaled by this value before lerping particle position")]
		[SerializeField]
		private float lerpRateScale = 0.5f;
	//ENDOF serialized fields

	//MonoBehaviour lifecycle
		private void Awake ()
		{
			this.particles = new Particle[this.managedParticleSystem.main.maxParticles];
			this.particleCount = 0;
		}

		private void Update ()
		{
			this.GetParticleCache();
			this.ProcessParticleCache();
			this.SetParticleCache();
		}
	//ENDOF MonoBehaviour

	//private members
		private Vector3 targetPosition { get { return this.destinationTransform.position; }}

		private Particle[] particles;
		private int particleCount;

		private void GetParticleCache ()
		{ this.particleCount = this.managedParticleSystem.GetParticles(this.particles); }

		private void SetParticleCache ()
		{ this.managedParticleSystem.SetParticles(this.particles, this.particleCount); }

		private void ProcessParticleCache ()
		{
			for (int i = 0, iLimit = this.particleCount; i < iLimit; i++)
			{
				float timeLeft = this.particles[i].remainingLifetime;
				//Debug.Log("particle remaining Time: " + timeLeft);
				//if (timeLeft < 0) { Debug.LogWarning("particle lifetime under 0"); }

				if (timeLeft < this.animationTime)
				{
					this.particles[i].position = this.LerpPosition(
						from: this.particles[i].position,
						to: this.targetPosition,
						timeLeft: timeLeft
					);
				}
			}
		}

		private Vector3 LerpPosition (Vector3 from, Vector3 to, float timeLeft)
		{
			float rate = ((this.animationTime - timeLeft) / this.animationTime) * this.lerpRateScale;
			//Debug.Log("timeLeft: " + timeLeft + " rate: " + rate);

			return new Vector3 (
				x: UnityEngine.Mathf.Lerp(from.x, to.x, rate),
				y: UnityEngine.Mathf.Lerp(from.y, to.y, rate),
				z: from.z
			);
				/*
				x: from.x.EFrameIndependentLerp(towards: to.x, rate: rate),
				y: from.y.EFrameIndependentLerp(towards: to.y, rate: rate),
				z: from.z //z: from.z.EFrameIndependentLerp(towards: to.z, rate: rate),
				*/
		}
	//ENDOF private
	}
}