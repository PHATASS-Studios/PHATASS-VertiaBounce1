using UnityEngine;

using System.Collections.Generic;

namespace PHATASS.Miscellaneous.MultiSegmentDisplay
{
// ScriptableObject representing the configuration of a single Multi-Segment character display
//	used to deliver the desired configuration to MultiSegmentCharacterController objects handling a Multi-Segment display
//
	[System.Serializable]
	[CreateAssetMenu(
		fileName = "Multi-Segment-Configuration-Dictionary (7-Segment).asset",
		menuName = "PHATASS/Multi-Segment display/Multi-Segment configuration dictionary",
		order = 0
	)]
	public class SOMultiSegmentConfigurationDictionary :
		UnityEngine.ScriptableObject,
		IMultiSegmentConfigurationDictionary
	{
	//serialized fields
		[SerializeField]
		[Tooltip("Available character mappings for this dictionary")]
		private SOMultiSegmentCharacterConfiguration[] characterDictionary;
	//ENDOF serialized fields
		
	//IMultiSegmentCharacterConfiguration
		// Get the character configuration required for given substring
		IMultiSegmentCharacterConfiguration IMultiSegmentConfigurationDictionary.GetCharacterConfiguration (string index)
		{ return this.Find(index); }
	//ENDOF IMultiSegmentCharacterConfiguration

	//private members
		private Dictionary<string, IMultiSegmentCharacterConfiguration> dictionaryCache = null;

		private void GenerateDictionaryCache ()
		{
			this.dictionaryCache = new Dictionary<string, IMultiSegmentCharacterConfiguration>();

			IMultiSegmentCharacterConfiguration castedEntry = null;

			foreach (SOMultiSegmentCharacterConfiguration entry in this.characterDictionary)
			{
				castedEntry = entry as IMultiSegmentCharacterConfiguration;
				this.dictionaryCache.Add(key: castedEntry.index, value: castedEntry);
			}
		}

		private IMultiSegmentCharacterConfiguration Find (string index)
		{
			if (this.dictionaryCache == null) { this.GenerateDictionaryCache(); }
			if (!this.dictionaryCache.ContainsKey(index)) { return null; }
			return this.dictionaryCache[index];
		}
	//ENDOF private members
	}
}