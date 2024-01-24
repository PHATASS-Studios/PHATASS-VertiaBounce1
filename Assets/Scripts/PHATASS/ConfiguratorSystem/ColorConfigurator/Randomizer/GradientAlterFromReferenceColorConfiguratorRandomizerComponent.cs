using UnityEngine;

using static PHATASS.Utils.Extensions.ColorExtensions;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

namespace PHATASS.ConfiguratorSystem
{
	[DefaultExecutionOrder(800)]
	public class GradientAlterFromReferenceColorConfiguratorRandomizerComponent : BaseColorConfiguratorRandomizerComponent
	{
	//Serialized fields
		[Tooltip("Managed configurators")]
		[SerializeField]
		[SerializedTypeRestriction(typeof(IColorConfigurator))]
		private UnityEngine.Object _referenceColorConfigurator;
		private IColorConfigurator referenceColorConfigurator
		{ get { return (this._referenceColorConfigurator as IColorConfigurator); }}

		[Tooltip("Ratio of new color to keep. 0 = original color, 1 = new color")]
		[Range(0f, 1f)]
		[SerializeField]
		private float newColorRatio = 0.25f;

		[Tooltip("If true, original alpha will be kept unaltered")]
		[SerializeField]
		private bool keepOriginalAlpha = true;
	//ENDOF Serialized

	//MonoBehaviour
	//ENDOF MonoBehaviour

	//method overrides
	//ENDOF overrides

	//private methods
		protected override void ApplyColor (IColorConfigurator colorConfigurator, Color color)
		{
			Color newColor = referenceColorConfigurator.color;
			newColor = newColor.ELerpValue(color, this.newColorRatio);
			newColor = newColor.ELerpSaturation(color, this.newColorRatio);

			if (this.keepOriginalAlpha) { newColor.a = colorConfigurator.color.a; }
			else { newColor.a = UnityEngine.Mathf.Lerp(referenceColorConfigurator.color.a, colorConfigurator.color.a, this.newColorRatio); }
			colorConfigurator.color = newColor;
		}
	//ENDOF private methods
	}
}