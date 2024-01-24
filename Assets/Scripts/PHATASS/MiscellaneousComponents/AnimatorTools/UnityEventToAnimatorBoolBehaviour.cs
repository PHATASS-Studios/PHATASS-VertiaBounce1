using UnityEngine;

namespace PHATASS.Miscellaneous.AnimatorTools
{
//Exposes a single public event that sets specified trigger on a list of Animator components
	public class UnityEventToAnimatorBoolBehaviour : MonoBehaviour
	{
		[SerializeField]
		private Animator[] animators;

		[SerializeField]
		private PHATASS.Utils.Types.SerializableAnimatorVariableIdentifier animatorBoolName = "Trigger";
		
		
		[SerializeField]
		private bool defaultValue = true;

		public void SetAnimatorBoolDefaultEvent ()
		{ this.SetAnimatorBoolEvent(this.defaultValue); }

		public void SetAnimatorBoolEvent (bool value)
		{
			foreach (Animator animator in this.animators)
			{ animator.SetBool(this.animatorBoolName, value); }
		}
	}
}
