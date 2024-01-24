using PHATASS.Utils.Types.Mergeables;

namespace PHATASS.SettingSystem
{
	//base interface for any settings package
	public interface ISettingsPackage <TPackageInterface> :
		ISettingsPackage,
		IPriorizableMergeable<TPackageInterface>
		where TPackageInterface : ISettingsPackage <TPackageInterface>
	{
	}

	public interface ISettingsPackage {}
}