namespace PHATASS.SettingSystem
{
	//Interface specific to action of type IActionSlap
	//Defines accessors to every setting specific to this type of action
	public interface ISettingsPackageActionSlap
		: ISettingsPackageRadialAction<ISettingsPackageActionSlap>
	{
		ISettingForce forceSetting {get;}
	}
}
