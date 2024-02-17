using UnityEngine;

using IFloatValue = PHATASS.Utils.Types.Values.IFloatValue;


using static PHATASS.Utils.Extensions.RoundingExtensions;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

namespace PHATASS.MiscellaneousComponents
{
	public class ZoomFormattedStringValueFromFloatValue :
		MonoBehaviour,
		PHATASS.Utils.Types.Values.IStringValue
	{
	//serialized
		[Tooltip("Source IFloatValue representing current zoom scale value")]
		[SerializeField]
		[SerializedTypeRestriction(typeof(IFloatValue))]
		private UnityEngine.Object? _zoomScaleValue = null;
		private IFloatValue zoomScaleValue
		{ get {
			if (this._zoomScaleValue == null) { return null; }
			else { return this._zoomScaleValue as IFloatValue; }
		}}

/*
		[Tooltip("Zoom base multiplier. Zoom will be scaled by this value.")]
		[SerializeField]
		private float zoomMultiplier = 2.0f;
//*/
		[Tooltip("Header string. This will be prepended to the formatted zoom value")]
		[SerializeField]
		private string headString = "x";

		[SerializeField]
		private int decimalPlaces = 1;
	//ENDOF serialized

	//IStringValue
		string PHATASS.Utils.Types.Values.IValue<string>.value
		{ get { return this.GetFormattedString(); }}
	//ENDOF IStringValue

	//MonoBehaviour lifecycle
		private void Update () { Debug.Log(this.GetFormattedString()); }
	//ENDOF MonoBehaviour

	//private members

		private float currentZoomValue
		{ get { return (1f  / this.zoomScaleValue.value); }}// * this.zoomMultiplier; }}

		private string GetFormattedString ()
		{
			//float roundedValue = this.currentZoomValue.ERound(digits: 1)
			return $"{this.headString}{this.currentZoomValue:F1}";
		}
	//ENDOF private
	}

}