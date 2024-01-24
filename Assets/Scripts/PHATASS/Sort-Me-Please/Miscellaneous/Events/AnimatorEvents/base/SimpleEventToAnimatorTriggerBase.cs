using UnityEngine;

namespace PHATASS.Utils.Events
{
	public class SimpleEventToAnimatorTriggerBase <TValueType> : SimpleEventReceiverBase <TValueType>
	{
	//Serialized fields
		[SerializeField]
		private Animator[] animators;

		[SerializeField]
		private PHATASS.Utils.Types.SerializableAnimatorVariableIdentifier _animatorTriggerName = "Trigger";
		private PHATASS.Utils.Types.IAnimatorVariableIdentifier animatorTriggerName { get { return this._animatorTriggerName; }}
	//ENDOF serialized

	//overrides
		protected override void Event (TValueType param0)
		{
			foreach (Animator animator in this.animators)
			{ animator.SetTrigger(this.animatorTriggerName.variableID); }
		}
	//ENDOF overrides
	}
}