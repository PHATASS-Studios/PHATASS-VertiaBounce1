using UnityEngine;
using UnityEvent = UnityEngine.Events.UnityEvent;
using Particle = UnityEngine.ParticleSystem.Particle;


namespace PHATASS.Utils.Events
{
	public class ParticleEventToUnityEvent : ParticleEventReceiverBase 
	{
	//Serialized fields
		[Tooltip("Events fired when receiving a particle event")]
		[SerializeField]
		private UnityEvent eventsFired;
	//ENDOF Serialized fields

	//overrides
		protected override void Event (Particle param0)
		{
			eventsFired.Invoke();
		}
	//ENDOF overrides
	}
}
