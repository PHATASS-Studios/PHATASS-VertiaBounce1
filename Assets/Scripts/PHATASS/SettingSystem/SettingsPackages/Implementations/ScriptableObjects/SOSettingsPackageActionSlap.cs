namespace PHATASS.SettingSystem
{
	[System.Serializable]
	[UnityEngine.CreateAssetMenu(
		fileName = "SettingsPackageActionSlap",
		menuName = "PHATASS settings/Packages/Action Slap settings",
		order = 1
	)]
	public class SOSettingsPackageActionSlap
	:	SOSettingsPackageRadialActionBase<
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

