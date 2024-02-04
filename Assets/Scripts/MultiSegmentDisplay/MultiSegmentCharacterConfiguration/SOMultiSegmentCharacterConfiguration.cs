using UnityEngine;
namespace PHATASS.Miscellaneous.MultiSegmentDisplay
{
// ScriptableObject representing the configuration of a single Multi-Segment character display
//	used to deliver the desired configuration to MultiSegmentCharacterController objects handling a Multi-Segment display
//
	[System.Serializable]
	[CreateAssetMenu(
		fileName = "Multi-Segment-Config (7-Segment) [CHAR].asset",
		menuName = "PHATASS/Multi-Segment display/Multi-Segment character configuration",
		order = 0
	)]
	public class SOMultiSegmentCharacterConfiguration :
		UnityEngine.ScriptableObject,
		IMultiSegmentCharacterConfiguration
	{
	//serialized fields
		[SerializeField]
		[Tooltip("Character or string represented by this configuration.")]
		private string index = "-";

		[SerializeField]
		[Tooltip("On/Off state for every segment in this configuration. Distribution Top-To-Bottom, Left-To-Right.\nDEFAULT:\n 0: Top\n 1: Top Left\n 2: Top Right\n 3: Central\n 4: Bottom Left\n 5: Bottom Right\n 6: Bottom")]
		private bool[] segmentState = { false, false, false, false, false, false, false };
	//ENDOF serialized fields
		
	//IMultiSegmentCharacterConfiguration
		// Character represented by this configuration (used as index too)
		string IMultiSegmentCharacterConfiguration.index
		{ get { return this.index; }}

		//list of states for every available segment
		System.Collections.Generic.IList<bool> IMultiSegmentCharacterConfiguration.segmentState
		{ get { return this.segmentState; }}
	//ENDOF IMultiSegmentCharacterConfiguration
	}
}