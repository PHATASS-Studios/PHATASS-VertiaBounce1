using UnityEngine;

namespace PHATASS.Miscellaneous.MultiSegmentDisplay
{
// Interface representing an element capable of controlling the rendering of a string or substring
//	Allows altering the displayed string and the color of the segments
//	
	public interface IMultiSegmentDisplay :
		PHATASS.Utils.Types.Values.IStringValueMutable,	//IStringValueMutable allows setting/getting currently displayed string/substring
		IOnOffColorReceiver		//IOnOffColorReceiver allows handling
	{
	}
}