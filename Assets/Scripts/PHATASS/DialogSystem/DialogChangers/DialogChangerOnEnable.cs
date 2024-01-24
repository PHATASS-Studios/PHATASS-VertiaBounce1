namespace PHATASS.DialogSystem.DialogChangers
{
// This component triggers a delayed dialog change on any of the possible monobehaviour lifecycle callbacks
	public class DialogChangerOnEnable : DialogChangerBase
	{
	//MonoBehaviour lifecycle
		public void OnEnable ()
		{
			ChangeDialog();
		}
	//ENDOF MonoBehaviour lifecycle
	}
}