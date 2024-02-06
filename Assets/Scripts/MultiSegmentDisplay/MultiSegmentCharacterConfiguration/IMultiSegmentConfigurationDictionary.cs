using System.Collections.Generic;

namespace PHATASS.Miscellaneous.MultiSegmentDisplay
{
// interface representing a dictionary of character configurations for Multi-Segment character displays
//	Can be requested the corresponding IMultiSegmentCharacterConfiguration for a given string index
//	
	public interface IMultiSegmentConfigurationDictionary
	{
		// Returns the IMultiSegmentCharacterConfiguration corresponding to given index. Returns null if it doesn't exist.
		IMultiSegmentCharacterConfiguration GetCharacterConfiguration (string index);
	}
}