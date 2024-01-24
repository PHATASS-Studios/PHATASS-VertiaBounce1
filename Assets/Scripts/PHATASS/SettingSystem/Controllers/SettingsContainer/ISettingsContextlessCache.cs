namespace PHATASS.SettingSystem
{
	public partial class SettingsContainer
	{
		//interface defining the specifics of use of a settings package cache
		private interface ISettingsContextlessCache : ISettingsContainer
		{
			//returns a list with all packages that fulfill given type
			System.Collections.Generic.IList<TPackageInterface> GetCachedPackages <TPackageInterface> ()
				where TPackageInterface : ISettingsPackage<TPackageInterface>;
		}
	}
}