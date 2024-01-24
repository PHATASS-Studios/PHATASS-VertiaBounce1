using UnityEngine;

namespace PHATASS.Miscellaneous.AnimatorTools
{
//Exposes a single public event that sets specified trigger on a list of Animator components
	public class UnityEventToAnimatorTriggerBehaviour : MonoBehaviour
	{
		[SerializeField]
		private Animator[] animators;

		[SerializeField]
		private PHATASS.Utils.Types.SerializableAnimatorVariableIdentifier _animatorTriggerName = "Trigger";
		private PHATASS.Utils.Types.IAnimatorVariableIdentifier animatorTriggerName { get { return this._animatorTriggerName; }}
		
		public void SetAnimatorTrigger ()
		{
			foreach (Animator animator in this.animators)
			{ animator.SetTrigger(this.animatorTriggerName.variableID); }
		}
	}
}
