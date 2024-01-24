using System.Collections.Generic;

using Debug = UnityEngine.Debug;
using GameObject = UnityEngine.GameObject;

namespace PHATASS.SettingSystem
{
	public partial class SettingsContainer
	{
		//class containing settings linked to a specific context
		private class SettingsContextlessCache : ISettingsContextlessCache
		{
		//Constructor
			public SettingsContextlessCache (IList<System.Object> settings = null)
			{
				//initialize store on creation
				this.settingsList = (settings != null) ? settings : new List<System.Object>();
			}
		//ENDOF Constructor

		//private fields
			private IList<System.Object> settingsList;
		//ENDOF private fields

		//ISettingsContextlessCache implementation
			//returns a list with all packages that fulfill given type
			IList<TPackageInterface> ISettingsContextlessCache.GetCachedPackages <TPackageInterface> ()
			{ return this.GetCachedPackages<TPackageInterface>(); }
		//ENDOF ISettingsContextlessCache

		//ISettingsContainer implementation
			//Composes and returns a settings package adequate to context GameObject
			TPackageInterface ISettingsContainer.GetSettingsPackage <TPackageInterface>
				(GameObject context)
			{
				IList<TPackageInterface> packageList = this.GetCachedPackages<TPackageInterface>();

				//if no compatible objects found, return null. If exactly 1, return that one, otherwise return a merge of every one
				if (packageList.Count == 1) { return packageList[0]; }
				if (packageList.Count == 0) { return default(TPackageInterface); }
				return packageList[0].Merge(packageList);
				// [TO-DO] Maybe cache the result of this Merge by type too?
			}

			//Stores a settings package for given context
			void ISettingsContainer.RegisterSettingsPackage
				(System.Object package, GameObject context)
			{ this.settingsList.Add(package); }
			void ISettingsContainer.RegisterSettingsPackage <TPackageInterface>
				(TPackageInterface package, GameObject context)
			{ this.settingsList.Add(package); }
		//ENDOF ISettingsContainer

		//private methods
			//returns a list with every cached item that fulfills given type
			private IList<TPackageInterface> GetCachedPackages <TPackageInterface> ()
				where TPackageInterface : ISettingsPackage<TPackageInterface>
			{
				List<TPackageInterface> packageList = new List<TPackageInterface>();

				//for each stored object, test its compatibility with TPackageInterface and store it
				foreach (System.Object entry in this.settingsList)
				{
					if (entry is TPackageInterface)
					{
						packageList.Add((TPackageInterface) entry);
					}
				}

				return packageList;
			}
		//ENDOF private methods
		}
	}
}
