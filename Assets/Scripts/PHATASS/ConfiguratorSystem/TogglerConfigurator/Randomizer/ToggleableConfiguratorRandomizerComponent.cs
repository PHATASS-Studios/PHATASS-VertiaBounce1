using System.Collections.Generic;
using UnityEngine;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

using IToggleable = PHATASS.Utils.Types.Toggleables.IToggleable;

namespace PHATASS.ConfiguratorSystem
{
	public class ToggleableConfiguratorRandomizerComponent : BaseConfiguratorRandomizerComponent
	{
	//Serialized fields
		[Tooltip("Managed toggleables")]
		[SerializeField]
		[SerializedTypeRestriction(typeof(IToggleable))]
		private List<UnityEngine.Object> _managedToggleables = null;
		private IList<IToggleable> _managedToggleablesAccessor = null;
		private IList<IToggleable> managedToggleables
		{
			get
			{
				//create accessor if unavailable
				if (this._managedToggleablesAccessor == null && this._managedToggleables != null)
				{ this._managedToggleablesAccessor = new PHATASS.Utils.Types.Wrappers.UnityObjectListCastedAccessor<IToggleable>(this._managedToggleables); }

				return this._managedToggleablesAccessor;
			}
		}

		[Tooltip("Probability of TRUE. 0 means always FALSE, 1 means always TRUE")]
		[SerializeField]
		[Range(0f, 1f)]
		private float trueChance = 0.5f;
	//ENDOF Serialized


	//method overrides
		protected override void Randomize ()
		{
			bool result = 
				(this.trueChance <= 0f) ? false :
				(this.trueChance >= 1f) ? true :
				UnityEngine.Random.value <= this.trueChance;

			foreach (IToggleable toggleable in this.managedToggleables)
			{
				if (toggleable != null) { toggleable.state = result; }
			}
		}
	//ENDOF overrides
	}
}