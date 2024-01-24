namespace PHATASS.SettingSystem
{
	[System.Serializable]
	[UnityEngine.CreateAssetMenu(
		fileName = "SettingsPackageActionGrabElement",
		menuName = "PHATASS settings/Packages/Action Grab Element settings",
		order = 1
	)]
	public class SOSettingsPackageActionGrabElement
	:	SOSettingsPackageActionGrabBase <
			SettingsPackageActionGrabElement,
			ISettingsPackageActionGrabElement>,
		ISettingsPackageActionGrabElement
	{}
}