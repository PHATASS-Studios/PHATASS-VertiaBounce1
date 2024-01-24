namespace PHATASS.SettingSystem
{
	[System.Serializable]
	[UnityEngine.CreateAssetMenu(
		fileName = "SettingsPackageActionUseInteractor",
		menuName = "PHATASS settings/Packages/Action UseInteractor settings",
		order = 1
	)]
	public class SOSettingsPackageActionUseInteractor
	:	SOSettingsPackageRadialActionBase<
			SettingsPackageActionUseInteractor,
			ISettingsPackageActionUseInteractor>,
		ISettingsPackageActionUseInteractor
	{
	//ISettingsPackageActionUseInteractor
	//ENDOF
	}
}

