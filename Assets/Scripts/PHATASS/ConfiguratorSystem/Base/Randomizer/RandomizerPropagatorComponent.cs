using UnityEngine;

using System.Collections.Generic;
using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

namespace PHATASS.ConfiguratorSystem
{
	public class RandomizerPropagatorComponent :
		BaseConfiguratorRandomizerComponent,
		IConfiguratorRandomizer
	{
	//serialized fields
		[SerializeField]
		[SerializedTypeRestriction(typeof(IRandomizer))]
		private List<UnityEngine.Object> _managedRandomizers = null;
		private IList<IRandomizer> _managedRandomizersAccessor = null;
		private IList<IRandomizer> managedRandomizers
		{
			get
			{
				//create accessor if unavailable
				if (this._managedRandomizersAccessor == null && this._managedRandomizers != null)
				{ this._managedRandomizersAccessor = new PHATASS.Utils.Types.Wrappers.UnityObjectListCastedAccessor<IRandomizer>(this._managedRandomizers); }

				return this._managedRandomizersAccessor;
			}
		}
	//ENDOF serialized fields

	//method overrides
		protected override void Randomize ()
		{
			foreach (IRandomizer randomizer in this.managedRandomizers)
			{ randomizer.Randomize(); }
		}
	//ENDOF overrides
	}
}