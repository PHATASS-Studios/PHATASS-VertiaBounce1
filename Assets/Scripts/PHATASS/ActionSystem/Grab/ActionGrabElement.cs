using TSettingsPackage = PHATASS.SettingSystem.ISettingsPackageActionGrabElement;

namespace PHATASS.ActionSystem
{
	//Grabbing action subclass: single element
	public class ActionGrabElement :
		ActionGrabBase<TSettingsPackage>,
		IActionGrabElement
	{}
}