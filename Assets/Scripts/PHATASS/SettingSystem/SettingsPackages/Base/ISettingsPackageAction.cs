using IUpdatableCallbackStack = PHATASS.Utils.Callbacks.IUpdatableCallbackStack;

namespace PHATASS.SettingSystem
{
	//Base interface for any action settings
	public interface ISettingsPackageAction <TPackageInterface> :
		ISettingsPackage <TPackageInterface>
		where TPackageInterface : ISettingsPackageAction <TPackageInterface>
	{	
		IUpdatableCallbackStack callbacks { get; }
	}
}
