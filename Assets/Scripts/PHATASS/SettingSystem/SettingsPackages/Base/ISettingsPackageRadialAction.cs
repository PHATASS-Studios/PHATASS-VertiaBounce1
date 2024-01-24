namespace PHATASS.SettingSystem
{
	//Base settings package for actions with a radius of effect
	//Defines accessors to every setting specific to actions with a defined radius of effect
	public interface ISettingsPackageRadialAction <TPackageInterface>
		: ISettingsPackageAction <TPackageInterface>
		where TPackageInterface : ISettingsPackageRadialAction <TPackageInterface>
	{
		ISettingCollisionRadius actionRadiusSetting {get;}
	}
}