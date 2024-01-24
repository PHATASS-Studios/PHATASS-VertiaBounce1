using UnityEngine;

using static PHATASS.Utils.Extensions.IEnumerableExtensions;

using IToggleable = PHATASS.Utils.Types.Toggleables.IToggleable;

namespace PHATASS.ConfiguratorSystem
{
	[System.Serializable]
	public class RigElementDisablerConfiguratorComponent :
		BaseConfiguratorComponent,
		IToggleableConfigurator
	{
	//class constants
		const float MINIMUM_RIGIDBODY_MASS = 0.00000001f;
	//ENDOF class constants

	//serialized fields
		[SerializeField]
		[Tooltip("Sprite(s) this configurator enables/disables")]
		private SpriteRenderer[] managedSpriteRenderers;

		[SerializeField]
		[Tooltip("Rigidbody(s) this configurator enables/disables")]
		protected Rigidbody[] managedRigidbodies;

		[SerializeField]
		[Tooltip("Sets initial enabled/disabled state")]
		private bool _enabledState = true;
		private bool enabledState
		{
			get { return this._enabledState; }
			set
			{
				if (value != this._enabledState)
				{
					this._enabledState = value;
					this.UpdateConfiguration();
				}
			}
		}
	//ENDOF serialized fields

	//IToggleable implementation
		//enabled/disabled state of managed item. true if enabled/active
		bool IToggleable.state
		{
			get { return this.enabledState; }
			set { this.enabledState = value; }
		}
	//ENDOF IToggleable implementation

	//IToggleableConfigurator
		//alternates the enabled/disabled state of managed item. Returns the resulting state after change
		bool IToggleableConfigurator.ToggleEnabled ()
		{
			this.enabledState = !this.enabledState;
			return this.enabledState;	
		}
	//ENDOF IToggleableConfigurator	

	//MonoBehaviour lifecycle
		protected override void Awake ()
		{
			if (!this.managedSpriteRenderers.EMExistsAndContainsAnything() && !this.managedRigidbodies.EMExistsAndContainsAnything())
			{ Debug.LogError("ObjectDisablerConfiguratorComponent " + this.gameObject.name + " managed components not initialized!"); }

			this.InitializeRigidbodyMassCache();

			base.Awake();
		}
	//ENDOF MonoBehaviour lifecycle

	//inherited method overrides
		protected override  void ApplyState ()
		{
			foreach (SpriteRenderer spriteRenderer in this.managedSpriteRenderers)
			{
				spriteRenderer.enabled = this.enabledState;
			}

			for (int i = 0, iLimit = this.managedRigidbodies.Length; i < iLimit; i++)
			{
				this.managedRigidbodies[i].mass = (this.enabledState) ? this.rigidbodyMassCache[i] : MINIMUM_RIGIDBODY_MASS;
			}
		}
	//ENDOF overrides

	//private fields
		private float[] rigidbodyMassCache;	//the base masses of each managed rigidbody is stored here
	//ENDOF private fields

	//private methods
		private void InitializeRigidbodyMassCache ()
		{
			this.rigidbodyMassCache = new float[this.managedRigidbodies.Length];

			for (int i = 0, iLimit = this.managedRigidbodies.Length; i < iLimit; i++)
			{
				this.rigidbodyMassCache[i] = this.managedRigidbodies[i].mass;
			}
		}
	//ENDOF private methods
	}
}