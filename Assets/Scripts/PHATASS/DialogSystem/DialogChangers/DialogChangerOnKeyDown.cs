using UnityEngine;

namespace PHATASS.DialogSystem.DialogChangers
{
	public class DialogChangerOnKeyDown : DialogChangerBase
	{
	//serialized fields
		[Tooltip("Key to listen to. In the Update this key is pressed down, dialog change will trigger (with provided delay)")]
		[SerializeField]
		private UnityEngine.KeyCode keyboardKey;
	//ENDOF serialized

	//MonoBehaviour lifecycle
		public void Update ()
		{
			if (Input.GetKeyDown(this.keyboardKey))
			{ ChangeDialog(); }
		}
	//ENDOF MonoBehaviour lifecycle
	}
}