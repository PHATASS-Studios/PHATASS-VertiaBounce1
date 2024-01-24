using UnityEngine;

namespace PHATASS.Miscellaneous
{
//MonoBehaviour that triggers a given UnityEvent when any form of input is detected
	public class UnityEventTriggerOnInput : MonoBehaviour
	{
	//serialized fields
		[Tooltip("Event triggered when any input is detected")]
		[SerializeField]
		private UnityEngine.Events.UnityEvent DoOnAnyInputEvent;
	
		[Tooltip("Unless this is true, this event handler will self-disable after first trigger")]
		[SerializeField]
		private bool repeatable = false;
	//ENDOF serialized fields

	//MonoBehaviour
		private void Update ()
		{
			if (this.anyInput)
			{
				this.TriggerEvent();
				this.CheckRepeat();
			}
		}
	//ENDOF MonoBehaviour


	//private members
		private bool anyInput
		{ get { return UnityEngine.Input.anyKey; }}

		private void TriggerEvent ()
		{
			this.DoOnAnyInputEvent.Invoke();
		}

		private void CheckRepeat ()
		{
			if (!this.repeatable)
			{ this.enabled = false; }
		}
	//ENDOF private members
	}
}
