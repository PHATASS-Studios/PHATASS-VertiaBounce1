using IController = PHATASS.ControllerSystem.IController;
namespace PHATASS.SettingSystem
{
	//Main settings provider interface. It's a settings Container acting as a controller
	public interface ISettingsProvider : IController, ISettingsContainer
	{}
}