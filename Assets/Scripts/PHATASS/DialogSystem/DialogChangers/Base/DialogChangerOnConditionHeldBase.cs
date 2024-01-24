namespace PHATASS.DialogSystem.DialogChangers
{
	public abstract class DialogChangerOnConditionHeldBase : DialogChangerOnConditionBase
	{
	//serialized fields and properties
		[UnityEngine.SerializeField]
		private float targetHeldTime = 0.5f;
	//ENDOF serialized fields and properties

	//private fields and properties
		private float currentHeldTime = 0f;
	//ENDOF private fields and properties

	//base class abstract implementation
		//this method should return true when the condition for dialog change is fulfilled
		protected override bool CheckCondition ()
		{
			if (CheckHeldCondition())
			{
				currentHeldTime += UnityEngine.Time.deltaTime;
				if (currentHeldTime >= targetHeldTime)
				{
					return true;
				}
			}
			else
			{
				currentHeldTime = 0;
			}
			return false;
		}
	//ENDOF base class abstract implementation

	//abstract method declaration
		protected abstract bool CheckHeldCondition ();
	//ENDOF abstract method declaration
	}
}