namespace PHATASS.DialogSystem.DialogChangers
{
	public abstract class DialogChangerOnConditionBase : DialogChangerBase
	{
	//serialized fields
		[UnityEngine.SerializeField]
		private bool selfDisableOnTrigger = true;
	//ENDOF serialized fields

	//MonoBehaviour lifecycle
		public void Update ()
		{
			if (CheckCondition())
			{
				ChangeDialog();
				TrySelfDisable();
			}
		}
	//ENDOF MonoBehaviour lifecycle

	//private methods
		private void TrySelfDisable ()
		{
			if (selfDisableOnTrigger)
			{
				this.enabled = false;
			}
		}
	//ENDOF private methods

	//abstract method declaration
		//this method should return true when the condition for dialog change is fulfilled
		protected abstract bool CheckCondition ();
	//ENDOF abstract method declaration
	}
}