using UnityEngine;
using UnityEngine.Events;

namespace PHATASS.ConfiguratorSystem
{
	[System.Serializable]
	[DefaultExecutionOrder(1000)]
	public abstract class BaseConfiguratorComponent :
		MonoBehaviour,
		IConfigurator
	{
	//serialized fields
		[SerializeField]
		[Tooltip("Events fired each time this item's configuration is changed")]
		private UnityEvent onConfigurationChangedEvent;

		[SerializeField]
		[Tooltip("If continuousUpdate is true, configuration will be re-applied each frame. This is useful to make direct changes to serialized backing fields automatically apply on real time. EVENTS DO NOT FIRE IN THIS CASE. onConfigurationChangedEvent is only invoked if setter properties/methods are invoked.")]
		private bool continuousUpdate = true;
	//ENDOF serialized fields

	//MonoBehaviour lifecycle
		protected virtual void Awake ()
		{
			this.ApplyState();
		}

		private void Update ()
		{
			if (this.continuousUpdate)
			{ this.ApplyState(); }
		}
	//ENDOF MonoBehaviour lifecycle

	//protected virtual
		protected virtual void UpdateConfiguration ()
		{
			this.ApplyState();
			this.OnConfigurationChanged();
		}

		protected virtual void OnConfigurationChanged ()
		{
			this.onConfigurationChangedEvent.Invoke();
		}
	//ENDOF abstract methods

	//abstract declarations
		protected abstract void ApplyState ();
	//ENDOF abstract declarations
	}
}
