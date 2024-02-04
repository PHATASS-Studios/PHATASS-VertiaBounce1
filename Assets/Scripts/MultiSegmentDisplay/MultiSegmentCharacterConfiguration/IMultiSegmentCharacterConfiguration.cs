using System.Collections.Generic;

namespace PHATASS.Miscellaneous.MultiSegmentDisplay
{
// interface representing the configuration of a single Multi-Segment character display
//	used to deliver the desired configuration to MultiSegmentCharacterController objects handling a Multi-Segment display
//	
	public interface IMultiSegmentCharacterConfiguration
	{
		string index { get; }		// Character represented by this configuration (used as index too)
		IList<bool> segmentState { get; }	//list of states for every available segment
	}
}