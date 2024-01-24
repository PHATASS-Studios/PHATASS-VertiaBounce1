using GameObject = UnityEngine.GameObject;

using IAction = PHATASS.ActionSystem.IAction;

namespace PHATASS.SettingSystem
{
	//Interface defining a class that handles multiple setting controllers
	public interface ISettingsContainer
	{
		//Composes and returns a settings package adequate to context GameObject
		//this means default settings altered according to gameObject-specific settings
		TPackageInterface GetSettingsPackage <TPackageInterface>
			(GameObject context = null)
			where TPackageInterface : ISettingsPackage<TPackageInterface>;

		//Stores a settings package for given context, generic and non-generic overloads
		void RegisterSettingsPackage (System.Object package, GameObject context = null);
		void RegisterSettingsPackage <TPackageInterface>
			(TPackageInterface package, GameObject context = null)
			where TPackageInterface : ISettingsPackage<TPackageInterface>;
	}
}