using GameObject = UnityEngine.GameObject;

namespace PHATASS.SettingSystem
{
	public partial class SettingsContainer
	{
		//interface defining the specifics of use of a settings package cache
		private interface ISettingsCache : ISettingsContainer
		{
			//force-empty cache
			void Reset ();

			//returns wether cache contains an entry for target context and type
			bool ContainsSettingsPackage <TPackageInterface>
				(GameObject context = null)
				where TPackageInterface : ISettingsPackage<TPackageInterface>;
		}
	}
}