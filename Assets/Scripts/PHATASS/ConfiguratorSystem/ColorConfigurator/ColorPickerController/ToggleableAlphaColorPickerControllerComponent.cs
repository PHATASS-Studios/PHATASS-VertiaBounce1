using UnityEngine;

using IToggleable = PHATASS.Utils.Types.Toggleables.IToggleable;

namespace PHATASS.ConfiguratorSystem
{
	public class ToggleableAlphaColorPickerControllerComponent : ColorPickerControllerComponent, IToggleable
	{
	//Serialized fields
		[SerializeField]
		[Range(0f, 1f)]
		[Tooltip("Alpha value given to sprites when toggled TRUE")]
		private float alphaOn = 1.0f;

		[SerializeField]
		[Range(0f, 1f)]
		[Tooltip("Alpha value given to sprites when toggled FALSE")]
		private float alphaOff = 0.6f;

		[SerializeField]
		private bool state = true;
	//ENDOF Serialized

	//IToggleable
		bool IToggleable.state
		{
			get { return this.state; }
			set
			{
				this.state = value;
				//when changing desired alpha, automatically update target color
				this.colorValue = this.colorValue;
			}
		}
	//ENDOF IToggleable

	//MonoBehaviour lifecycle
	//ENDOF MonoBehaviour

	//overridden properties
		//Setting the color value now adjusts alpha as desired
		protected override Color colorValue
		{
			get { return base.colorValue; }
			set
			{
				value.a = this.state ? this.alphaOn : this.alphaOff;
				base.colorValue = value;
			}
		}
	//ENDOF overrides
	}
}