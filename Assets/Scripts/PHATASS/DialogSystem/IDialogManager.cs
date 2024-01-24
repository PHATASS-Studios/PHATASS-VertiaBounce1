using IDialogController = PHATASS.DialogSystem.DialogControllers.IDialogController;

namespace PHATASS.DialogSystem
{
	public interface IDialogManager
	{
		void SetActiveDialog (IDialogController targetDialog);
	}

}