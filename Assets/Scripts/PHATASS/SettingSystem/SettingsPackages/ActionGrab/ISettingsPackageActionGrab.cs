namespace PHATASS.SettingSystem
{
	//Interface specific to actions inheriting IActionGrab
	//Defines accessors to every setting specific to this type of action
	public interface ISettingsPackageActionGrab<TPackageInterface>
		: ISettingsPackageRadialAction<TPackageInterface>
		where TPackageInterface : ISettingsPackageActionGrab<TPackageInterface>
	{
		//ISettingJoint grabJointSetting {get;} //REMOVED: Grab joint is now provided by the grabbable, case-by-case
	}
}