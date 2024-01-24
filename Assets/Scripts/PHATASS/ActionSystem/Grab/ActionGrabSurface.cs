using TSettingsPackage = PHATASS.SettingSystem.ISettingsPackageActionGrabSurface;

namespace PHATASS.ActionSystem
{
	//Grabbing action subclass: single element
	public class ActionGrabSurface :
		ActionGrabBase<TSettingsPackage>,
		IActionGrabSurface
	{}
}