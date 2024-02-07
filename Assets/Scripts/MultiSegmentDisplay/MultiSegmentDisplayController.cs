using System.Collections.Generic;

using UnityEngine;


using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

namespace PHATASS.Miscellaneous.MultiSegmentDisplay
{
// Class handling an entire phrase of a Multi-Segment display
//	It manages a series of MultiSegmentCharacterController
//	Can be requested a change of the displayed phrase and the string will be split and sent to each character
	public class MultiSegmentDisplayController :
		MonoBehaviour,
		IMultiSegmentDisplay
	{
	//serialized fields
		[Tooltip("Text alignment")]
		[SerializeField]
		private ESegmentDisplayTextAlignment textAlignment;

		[Tooltip("Displayed string. Altering this in the editor sets intially displayed text, but altering this at runtime DOES NOTHING. You need to access IMultiSegmentDisplay.value, like so:\n{\n  ((IMultiSegmentDisplay) component).value = \"string\";\n}")]
		[SerializeField]
		private string _currentString;
		private string currentString
		{
			get { return this._currentString; }
			set
			{
				this._currentString = value;
				this.ApplyString();
			}
		}

		[Tooltip("List of sub-renderers. Received string will be split into characters and each delivered to these character displays.\n\nTHESE MUST BE PLACED LEFT-TO-RIGHT IN THE SCENE")]
		[SerializeField]
		[SerializedTypeRestriction(typeof(IMultiSegmentCharacter))]
		private List<UnityEngine.Object> _characterSubDisplays = null;
		private IList<IMultiSegmentCharacter> _characterSubDisplaysAccessor = null;
		private IList<IMultiSegmentCharacter> characterSubDisplays
		{
			get
			{
				//create accessor if unavailable
				if (this._characterSubDisplaysAccessor == null && this._characterSubDisplays != null)
				{ this._characterSubDisplaysAccessor = new PHATASS.Utils.Types.Wrappers.UnityObjectListCastedAccessor<IMultiSegmentCharacter>(this._characterSubDisplays); }

				return this._characterSubDisplaysAccessor;
			}
		}

		[Tooltip("Color of each segment renderer that is ON")]
		[SerializeField]
		private Color _onColor = Color.red;
		private Color onColor
		{
			get { return this._onColor; }
			set { this._onColor = value; }
		}

		[Tooltip("Color of each segment renderer that is OFF")]
		[SerializeField]
		private Color _offColor = Color.black;
		private Color offColor
		{
			get { return this._offColor; }
			set { this._offColor = value; }
		}
	//ENDOF serialized fields

	//IMultiSegmentDisplay
		string PHATASS.Utils.Types.Values.IValue<string>.value
		{
			get { return this.currentString; }
		}

		string PHATASS.Utils.Types.Values.IValueMutable<string>.value
		{
			get { return this.currentString; }
			set { this.currentString = value; }
		}

		Color IOnOffColorReceiver.onColor
		{
			get { return this.onColor; }
			set
			{
				this.onColor = value;
				this.ApplyColors();
			}
		}

		Color IOnOffColorReceiver.offColor
		{
			get { return this.offColor; }
			set
			{
				this.offColor = value;
				this.ApplyColors();
			}
		}

		// sets the on/off color states in a single call
		//	if a single color and a floating value is provided, offColor is calculated by multiplying onColor by offColorMultiplier
		void IOnOffColorReceiver.SetColor (Color onColor, float offColorMultiplier)
		{ this.SetColor(onColor: onColor, offColorMultiplier: offColorMultiplier); }
		void IOnOffColorReceiver.SetColor (Color onColor, Color offColor)
		{ this.SetColor(onColor: onColor, offColor: offColor); }
	//ENDOF IMultiSegmentDisplay


	//MonoBehaviour lifecycle
		private void Start ()
		{
			this.ApplyColors();
			this.ApplyString();
		}
	//ENDOF MonoBehaviour

	//private members
	  // color setter methods
		// sets the on/off color states in a single method call
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

			this.ApplyColors();
		}

		//applies current colors to child displays
		private void ApplyColors ()
		{
			foreach (IMultiSegmentCharacter subDisplay in this.characterSubDisplays)
			{ subDisplay?.SetColor(onColor, offColor); }
		}



	  //string setter methods
		//applies the string currentString to the display, according to string alignment settings
		private void ApplyString ()
		{
			string[] substrings = this.SplitSubstrings(this.currentString);

			Debug.Log("substrings: " + substrings.Length);

			int substringIndex = 0;
			int substringCount = substrings.Length;
			int displayIndex = 0;
			int displayCount = this.characterSubDisplays.Count;

			if (this.textAlignment == ESegmentDisplayTextAlignment.rightAligned)
			{
				displayIndex = displayCount - 1;
				substringIndex = substringCount -1;

				while (displayIndex >= 0)
				{
//					Debug.Log("writing substring " + i + " (\"" + substrings[i] + "\") @pos " + displayIndex);
					if (substringIndex >= 0)
					{ this.characterSubDisplays[displayIndex].value = substrings[substringIndex]; }
					else
					{ this.characterSubDisplays[displayIndex].value = null; }

					displayIndex--;
					substringIndex--;
				}
			}
			else //leftAligned
			{
				displayIndex = 0;
				substringIndex = 0;

				while (displayIndex < displayCount)
				{
					if (substringIndex < substringCount)
					{ this.characterSubDisplays[displayIndex].value = substrings[substringIndex]; }
					else
					{ this.characterSubDisplays[displayIndex].value = null; }

					displayIndex++;
					substringIndex++;
				}
			}

		}

		private string[] SplitSubstrings (string original)
		{
			//TEMPORARY SOLUTION: Just split the string into characters
			//[TO-DO]: detect periods and {curlyBracket} commands
			string[] substrings = new string[original.Length];
			for (int i = 0, iLimit = original.Length; i < iLimit; i++)
			{
				substrings[i] = original[i].ToString();
			}

			return substrings;
		}
	//ENDOF private

	//private enumerations
		[System.Serializable]
		private enum ESegmentDisplayTextAlignment
		{
			rightAligned = 0,
			leftAligned = 1
		}
	//ENDOF private
	}
}