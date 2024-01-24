namespace PHATASS.DialogSystem.DialogControllers
{
	public interface IDialogController :
		PHATASS.Utils.Types.Toggleables.IAnimatedToggleable
	{
		PHATASS.Utils.Types.Toggleables.IToggleable portrait { get; }
	}
}