using Particle = UnityEngine.ParticleSystem.Particle;

namespace PHATASS.Utils.Events
{
	public abstract class SimpleEventReceiverBase <TValueType> : UnityEngine.MonoBehaviour, ISimpleEventReceiver<TValueType>
	{
	//IParticleEventReceiver
		void ISimpleEventReceiver<TValueType>.Event (TValueType param0)
		{ this.Event(param0); }
		protected abstract void Event (TValueType param0);
	//ENDOF IParticleEventReceiver
	}
}