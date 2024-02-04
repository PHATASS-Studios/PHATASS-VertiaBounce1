using UnityEngine;

namespace PHATASS.Miscellaneous.MultiSegmentDisplay
{
// Interface representing an element capable of controlling the rendering of a string or substring
//	Allows altering the displayed string and the color of the segments
//	
	public interface IOnOffColorReceiver
	{
		Color onColor { get; set; }
		Color offColor { get; set; }

		// sets the on/off color states in a single call
		//	if a single color and a floating value is provided, offColor is calculated by multiplying onColor by offColorMultiplier
		void SetColor (Color onColor, float offColorMultiplier);
		void SetColor (Color onColor, Color offColor);
	}
}