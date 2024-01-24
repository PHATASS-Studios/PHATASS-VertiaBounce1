namespace PHATASS.SettingSystem
{
	[System.Serializable]
	[UnityEngine.CreateAssetMenu(
		fileName = "SettingsPackageActionGrabSurface",
		menuName = "PHATASS settings/Packages/Action Grab Surface settings",
		order = 1
	)]
	public class SOSettingsPackageActionGrabSurface
	:	SOSettingsPackageActionGrabBase <
			SettingsPackageActionGrabSurface,
			ISettingsPackageActionGrabSurface>,
		ISettingsPackageActionGrabSurface
	{}
}