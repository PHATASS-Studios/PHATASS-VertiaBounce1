using System.Collections.Generic;

using UnityEngine;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

namespace PHATASS.ConfiguratorSystem
{
	public abstract class BaseColorConfiguratorRandomizerComponent : BaseConfiguratorRandomizerComponent
	{
	//Serialized fields
		[Tooltip("Managed configurators")]
		[SerializeField]
		[SerializedTypeRestriction(typeof(IColorConfigurator))]
		private List<UnityEngine.Object> _managedConfigurators = null;
		private IList<IColorConfigurator> _managedConfiguratorsAccessor = null;
		protected IList<IColorConfigurator> managedConfigurators
		{
			get
			{
				//create accessor if unavailable
				if (this._managedConfiguratorsAccessor == null && this._managedConfigurators != null)
				{ this._managedConfiguratorsAccessor = new PHATASS.Utils.Types.Wrappers.UnityObjectListCastedAccessor<IColorConfigurator>(this._managedConfigurators); }

				return this._managedConfiguratorsAccessor;
			}
		}

		[Tooltip("Range of possible colors accepted")]
		[SerializeField]
		protected Gradient gradient;
	//ENDOF Serialized

	//MonoBehaviour
	//ENDOF MonoBehaviour

	//method overrides
		protected override void Randomize ()
		{
			Color color = this.gradient.Evaluate(UnityEngine.Random.value);

			foreach (IColorConfigurator colorConfigurator in this.managedConfigurators)
			{
				if (colorConfigurator != null) { this.ApplyColor(colorConfigurator, color); }
			}
		}
	//ENDOF overrides

	//private methods
		protected abstract void ApplyColor (IColorConfigurator colorConfigurator, Color color);
	//ENDOF private methods
	}
}