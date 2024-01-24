using System.Collections.Generic;


using UnityEngine;
using UnityEvent = UnityEngine.Events.UnityEvent;
using TMP_Text = TMPro.TMP_Text;


using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

using IPlayable = PHATASS.Miscellaneous.Playables.IPlayable;

using IFloatRange = PHATASS.Utils.Types.Ranges.IFloatRange;
using RandomFloatRange = PHATASS.Utils.Types.Ranges.RandomFloatRange;

namespace PHATASS.Miscellaneous
{
// "Typewriter" addon MonoBehaviours for Text Mesh Pro (TMP_Text) text renderer MonoBehaviours
//	On activation, this component empties the managed text field and types it out character by character
//	Triggers desired list of events and playables when typing a character and when the full string is finished
	//[RequireComponent(typeof(TMP_Text))]
	public class TextMeshProTypeoutAddOn : MonoBehaviour
	{
	//constants
		//list of characters considered whitespace
		private const string whitespaceCharacterList = " 	\n";
	//ENDOF constants

	//serialized fields
		[Tooltip("TextMeshPro text object managed. This object's text will be blanked and re-written character by character.")]
		[SerializeField]
		private TMP_Text managedTextRenderer;
	
		[Tooltip("If this is true, typeout will trigger too when disabling and re-enabling. Otherwise, it will only trigger on Start()")]
		[SerializeField]
		private bool triggerOnEnable = true;

		[Tooltip("List of IPlayable objects triggered whenever a new character is typed")]
		[SerializeField]
 		[SerializedTypeRestriction(typeof(IPlayable))]
		private List<UnityEngine.Object> _playablesOnCharacterTyped = null;
		private IList<IPlayable> _playablesOnCharacterTypedAccessor = null;
		private IList<IPlayable> playablesOnCharacterTyped
		{
			get
			{
				//create accessor if unavailable
				if (this._playablesOnCharacterTypedAccessor == null && this._playablesOnCharacterTyped != null)
				{ this._playablesOnCharacterTypedAccessor = new PHATASS.Utils.Types.Wrappers.UnityObjectListCastedAccessor<IPlayable>(this._playablesOnCharacterTyped); }

				return this._playablesOnCharacterTypedAccessor;
			}
		}

		//[TO-DO]: Maybe add a list of ICharacterEventReceiver triggered for each new character too?

		[Tooltip("These UnityEvent calls will be triggered whenever a new character is typed.")]
		[SerializeField]
		private UnityEvent unityEventsOnCharacterTyped;

		[Tooltip("Time interval elapsed between each character being written (in seconds)")]
		[SerializeField]
		private RandomFloatRange _characterTypedInterval = null;
		private IFloatRange characterTypedInterval { get { return this._characterTypedInterval; }}

		[Tooltip("Initial delay before the first character is typed (in seconds)")]
		[SerializeField]
		private float initialDelay = 0.25f;

		[Tooltip("If this is true, callbacks will NOT be triggered for whitespace characters.")]
		[SerializeField]
		private bool ignoreCallbacksOnWhitespace = true;

	//ENDOF serialized

	//MonoBehaviour lifecycle
		private void Reset ()
		{
			if (this.managedTextRenderer == null)
			{ this.managedTextRenderer = this.GetComponent<TMP_Text>(); }

			/*if (string.IsNullOrEmpty(this.desiredString))
			{ this.desiredString = this.managedTextRenderer.text; }*/
		}

		private void Start ()
		{
			this.ReInitialize();
		}

		private void OnEnable ()
		{ if (this.triggerOnEnable) { this.ReInitialize(); }}

		private void Update ()
		{
			if (!this.stringFinished)
			{ this.UpdateAndCheckTimer(); }
		}
	//ENDOF MonoBehaviour

	//private fields and properties
		private string desiredString = null;

		private float timer;
		private int index;
		private int stringLength;

		private char nextCharacter { get { return this.desiredString[this.index]; }}
		private bool stringFinished { get { return this.index >= this.stringLength; }}
	//ENDOF fields and properties

	//private methods
		//Resets this to an empty and pre-triggered state
		private void ReInitialize ()
		{
			//if string uninitialized, initialize it now
			if (this.desiredString == null)
			{ this.desiredString = this.managedTextRenderer.text; }

			this.timer = this.initialDelay;
			this.index = 0;
			this.stringLength = this.desiredString.Length;
			Debug.Log("ReInitialize()");
			Debug.Log(" OriginalString: \"" + this.desiredString +"\"");
			Debug.Log("	stringLength: " + this.stringLength);
			this.managedTextRenderer.text = "";
		}

		private void UpdateAndCheckTimer ()
		{
			this.timer -= Time.deltaTime;
			//Debug.Log("timer: " + this.timer);
			if (this.timer <= 0f)
			{
				this.TypeNext();
				this.timer += this.characterTypedInterval.random;
			}
		}

		//TypeNext advances the index and requests typing the next character - unless end of phrase reached.
		//	special consideration required for style markers - they need to be recognized here separately
		private void TypeNext ()
		{
			/*
			Debug.LogWarning("TypeNext()");
			Debug.LogWarning(this.index);
			Debug.LogWarning(this.stringLength);
			Debug.LogWarning(this.stringFinished);
			*/
			if(this.stringFinished)	{ return; }

			//Debug.LogWarning("Type");
			this.Type(this.nextCharacter);
			this.index++;
		}

		//Adds a character at the end of the queue. Triggers callbacks unless the character is included in whitespaceCharacterList
		//https://learn.microsoft.com/en-us/dotnet/api/system.string.chars?view=net-7.0
		//Maybe need to use StringInfo class rather than character for handling unicode and style markers?
		private void Type (char character)
		{
			//Debug.LogWarning(character);
			if (!this.ignoreCallbacksOnWhitespace || !whitespaceCharacterList.Contains(character))
			{ CharacterTypedCallbacks(); }

			this.managedTextRenderer.text = string.Concat(this.managedTextRenderer.text, character);
		}

		//triggers the callbacks corresponding to a character typed
		private void CharacterTypedCallbacks ()
		{
			this.unityEventsOnCharacterTyped.Invoke();

			foreach (IPlayable playable in this.playablesOnCharacterTyped)
			{ playable.Play(); }
		}

	//ENDOF private methods
	}
}