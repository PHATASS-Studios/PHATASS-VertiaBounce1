using UnityEngine;
using Particle = UnityEngine.ParticleSystem.Particle;

using static PHATASS.Utils.Extensions.RoundingExtensions;
using RoundingPolicy = PHATASS.Utils.Extensions.RoundingExtensions.RoundingPolicy;


using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

namespace PHATASS.Utils.Events
{
	public class ParticleEventStartSizeToIntEvent : ParticleEventPropertyToIntEventBase 
	{
	//Serialized fields
		[Tooltip("Curve determining output value, using particle.startSize as time dimension. Result is rounded to an integer according to roundingPolicy.")]
		[SerializeField]
		private AnimationCurve sizeToValueCurve;

		[Tooltip("Rounding Policy used when transforming the value to an integer.")]
		[SerializeField]
		private RoundingPolicy roundingPolicy = RoundingPolicy.Ceiling;
	//ENDOF Serialized fields

	//overrides
		protected override int ParticleToValue (Particle particle)
		{ return this.sizeToValueCurve.Evaluate(particle.startSize).ERoundToInt(this.roundingPolicy); }
	//ENDOF overrides
	}
}
