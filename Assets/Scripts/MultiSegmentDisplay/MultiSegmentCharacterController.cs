using UnityEngine;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

namespace PHATASS.Miscellaneous.MultiSegmentDisplay
{
// Class handling a single-character Multi-Segment sub-display
//	can take an IMultiSegmentConfiguration object dictating which characters are lit/unlit
//	on/off color can be set
//
	public class MultiSegmentCharacterController :
		MonoBehaviour,
		IMultiSegmentCharacter		
	{
	//serialized fields
		[Tooltip("List of renderers handling each segment, by strict index order top-to-bottom left-to-right")]
		[SerializeField]
		private SpriteRenderer[] segmentRendererList;

		[Tooltip("Color of each segment renderer that is ON")]
		[SerializeField]
		private Color onColor = Color.red;

		[Tooltip("Color of each segment renderer that is OFF")]
		[SerializeField]
		private Color offColor = Color.black;

/*	//Serializable implementation of configuration discarded - can't or musn't store configuration file as UnityEngine.Object
		[Tooltip("IMultiSegmentCharacterConfiguration representing  each sub-element's on/off state")]
		[SerializeField]
		[SerializedTypeRestriction(typeof(IMultiSegmentCharacterConfiguration))]
		private UnityEngine.Object? _configuration = null;
		private IMultiSegmentCharacterConfiguration configuration
		{ get {
			if (this._configuration == null) { return null; }
			else { return this._configuration as IMultiSegmentCharacterConfiguration; }
		}}
*/
	//ENDOF serialized fields

	//IMultiSegmentCharacter
		IMultiSegmentCharacterConfiguration IMultiSegmentCharacter.configuration
		{
			get { return this.configuration; }
			set
			{
				this.configuration = value;
			}
		}

		Color IOnOffColorReceiver.onColor
		{
			get { return this.onColor; }
			set
			{
				this.onColor = value;
				this.Refresh();
			}
		}

		Color IOnOffColorReceiver.offColor
		{
			get { return this.offColor; }
			set
			{
				this.offColor = value;
				this.Refresh();
			}}

		// sets the on/off color states in a single call
		//	if a single color and a floating value is provided, offColor is calculated by multiplying onColor by offColorMultiplier
		void IOnOffColorReceiver.SetColor (Color onColor, float offColorMultiplier)
		{ this.SetColor(onColor: onColor, offColorMultiplier: offColorMultiplier); }
		void IOnOffColorReceiver.SetColor (Color onColor, Color offColor)
		{ this.SetColor(onColor: onColor, offColor: offColor); }
	//ENDOF IMultiSegmentCharacter

	//MonoBehaviour lifecycle
		private void Start ()
		{
			this.Refresh();
		}
	//ENDOF MonoBehaviour

	//private members
		private IMultiSegmentCharacterConfiguration _configuration = null;
		private IMultiSegmentCharacterConfiguration configuration
		{
			get { return this._configuration; }
			set
			{
				this._configuration = value;
				this.Refresh();
			}
		}

		private void Refresh ()
		{
			for (int i = 0, iLimit = this.segmentRendererList.Length; i < iLimit; i++)
			{
				if (
					this.configuration != null && this.configuration.segmentState != null	// first we need to ensure configuration data exists
					&&	i < this.configuration.segmentState.Count							// second, we need check current element is inside valid range
					&& this.configuration.segmentState[i] == true					// last, if segment state info exists and it's true, this segment is on
				) {
					this.segmentRendererList[i].color = this.onColor;
				}
				else
				{
					this.segmentRendererList[i].color = this.offColor;
				}
			}
		}

		// sets the on/off color states in a single call
		//	if a single color and a floating value is provided, offColor is calculated by multiplying onColor by offColorMultiplier
		private void SetColor (Color onColor, float offColorMultiplier)
		{
			this.SetColor(
				onColor: onColor,
				offColor: new Color(
					r: onColor.r * offColorMultiplier,
					g: onColor.g * offColorMultiplier,
					b: onColor.b * offColorMultiplier,
					a: onColor.a
			));
		}
		private void SetColor (Color onColor, Color offColor)
		{
			this.onColor = onColor;
			this.offColor = offColor;
			this.Refresh();
		}
	//ENDOF private members
	}
}