using UnityEngine;

namespace PHATASS.ConfiguratorSystem
{
	public class GradientColorConfiguratorRandomizerComponent : BaseColorConfiguratorRandomizerComponent
	{
	//Serialized fields
	//ENDOF Serialized

	//MonoBehaviour
	//ENDOF MonoBehaviour

	//method overrides
	//ENDOF overrides

	//private methods
		protected override void ApplyColor (IColorConfigurator colorConfigurator, Color color)
		{ colorConfigurator.color = color; }
	//ENDOF private methods
	}
}