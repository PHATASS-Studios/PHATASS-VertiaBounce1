using IToggleable = PHATASS.Utils.Types.Toggleables.IToggleable;

namespace PHATASS.ConfiguratorSystem
{
	public interface IToggleableConfigurator :
		IToggleable,
		IConfigurator
	{
		//bool IToggleable.toggleState { get; set; }
		//enabled/disabled state of managed item. true if enabled/active

		bool ToggleEnabled ();	//alternates the enabled/disabled state of managed item. Returns the resulting state after change
	}
}