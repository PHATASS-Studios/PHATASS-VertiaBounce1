namespace PHATASS.SettingSystem
{
	[System.Serializable]
	public class MBSettingsPackageActionSlap
	:	MBSettingsPackageRadialActionBase<
			SettingsPackageActionSlap,
			ISettingsPackageActionSlap>,
		ISettingsPackageActionSlap
	{
	//ISettingsPackageActionSlap
		public ISettingForce forceSetting
		{ get { return backingField.forceSetting; }}
	//ENDOF
	}
}

