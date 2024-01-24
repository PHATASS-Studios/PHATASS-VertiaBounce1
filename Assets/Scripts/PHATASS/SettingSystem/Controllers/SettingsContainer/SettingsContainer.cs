using System.Collections.Generic;
using Debug = UnityEngine.Debug;

using GameObject = UnityEngine.GameObject;

using Type = System.Type;

//[UNTESTED] [TO-DO] test this shiet
namespace PHATASS.SettingSystem
{
	//settings objects container class
	//stores a list of ISettingsPackage objects
	//finds and returns requested settings packages by required context and type
	public partial class SettingsContainer
	: ISettingsContainer
	{
	//Constructor
		public SettingsContainer ()
		{
			this.InitializeCaches();
		}

		//initialize backing dictionary
		private void InitializeCaches ()
		{
			this.contextSettingsDictionary = new Dictionary<GameObject, ISettingsContextlessCache>();
			this.cache = this.CreateSettingsCache();
			this.defaults = this.CreateContextlessCache();
		}
	//ENDOF Constructor

	//ISettingsProvider implementation
		//Composes and returns a settings package adequate to context GameObject
		//this means default settings altered according to gameObject-specific settings
		TPackageInterface ISettingsContainer.GetSettingsPackage <TPackageInterface>
			(GameObject context)
			//where TPackageInterface : ISettingsPackage<TPackageInterface>;
		{ return this.GetCachedPackage<TPackageInterface>(context); }

		//Stores a settings package for given context
		void ISettingsContainer.RegisterSettingsPackage (System.Object package, GameObject context)
		{ this.RegisterSettingsPackage(package, context); }
		void ISettingsContainer.RegisterSettingsPackage <TPackageInterface> (TPackageInterface package, GameObject context)
			//where TPackageInterface : ISettingsPackage<TPackageInterface>;
		{ this.RegisterSettingsPackage(package, context); }
	//ENDOF ISettingsProvider implementation

	//private fields
		private ISettingsCache cache;
		private IDictionary<GameObject, ISettingsContextlessCache> contextSettingsDictionary;
		private ISettingsContextlessCache defaults;
	//ENDOF private fields

	//private methods
		//tries to fetch a package from cache. if not available, composes and caches the package
		private TPackageInterface GetCachedPackage <TPackageInterface> (GameObject context)
			where TPackageInterface : ISettingsPackage<TPackageInterface>
		{
			//try to find cached package
			if (this.cache.ContainsSettingsPackage<TPackageInterface>(context))
			{
				//Debug.Log(string.Format("SettingsContainer: found cached package (type: {0}, context: {1})", typeof(TPackageInterface).ToString(), context));
				return this.cache.GetSettingsPackage<TPackageInterface>(context);
			}

			//if package is not cached, compose, cache, and return a new one
			//Debug.LogWarning(string.Format("SettingsContainer: NO CACHE FOUND (type: {0}, context: {1})", typeof(TPackageInterface).ToString(), context));
			TPackageInterface package = this.ComposeSettingsPackage<TPackageInterface>(context);
			this.cache.RegisterSettingsPackage<TPackageInterface>(package, context);
			return package;
		}

		//Composes and returns a settings package adequate to context GameObject
		//this means default settings altered according to gameObject-specific settings
		private TPackageInterface ComposeSettingsPackage <TPackageInterface>
			(GameObject context)
			where TPackageInterface : ISettingsPackage<TPackageInterface>
		{ 
			/*
			Debug.Log(
				string.Format("GetSettingsPackage<{0}>(context: {1})",
					typeof(TPackageInterface).ToString(),
					context));
			//*/
			IList<TPackageInterface> defaultSettings = this.GetDefaultSettings<TPackageInterface>();

			IList<TPackageInterface> contextualSettings = this.FindContextualSettings<TPackageInterface>(context);
			
			//Debug.Log(string.Format(" > contextualSettings: {0}", contextualSettings));

			//ensure a default setting exists for type
			if (defaultSettings.Count == 0)
			{
				Debug.LogError(string.Format("SettingsProvier.GetSettingsPackage<{0}>() no defaults available for type", typeof(TPackageInterface).ToString()));
				return default(TPackageInterface);
			}
			
			//compose a list with a combination of defaults and contextual overrides
			TPackageInterface[] fullSettingsList =
				new TPackageInterface[defaultSettings.Count + contextualSettings.Count];

			defaultSettings.CopyTo(fullSettingsList, 0);
			contextualSettings.CopyTo(fullSettingsList, defaultSettings.Count);
			//[TO-DO] maybe replacing these copytos with an IEnumerator that iterates over one after the other we can make the merge of these lists more efficient?

			return this.MergeUnsorted<TPackageInterface>(fullSettingsList);
		}

		//Stores a settings package for given context
		private void RegisterSettingsPackage (System.Object package, GameObject context)
		{
			//first separate default settings
			if (context == null)
			{
				this.defaults.RegisterSettingsPackage(package, context);
				return;
			}
			
			//if a cache did not exist for given object, create one anew
			if (!this.contextSettingsDictionary.ContainsKey(context))
			{
				this.contextSettingsDictionary.Add(context, this.CreateContextlessCache());
			}

			//register settings package in the appropriate cache
			this.contextSettingsDictionary[context].RegisterSettingsPackage(package, context);

			//reset cache so changes will apply on next request
			this.cache.Reset();
		}

		//returns defaults package for given type
		private IList<TPackageInterface> GetDefaultSettings <TPackageInterface> ()
			where TPackageInterface : ISettingsPackage<TPackageInterface>
		{
			return this.defaults.GetCachedPackages<TPackageInterface>();
		}

		//returns contextual settings package for given type and given object
		private IList<TPackageInterface> FindContextualSettings <TPackageInterface> (GameObject context)
			where TPackageInterface : ISettingsPackage<TPackageInterface>
		{
			//if no context available, or no override registered, return nothing
			if (context == null || !this.contextSettingsDictionary.ContainsKey(context))
			{ return new TPackageInterface[0]; }

			return this.contextSettingsDictionary[context].GetCachedPackages<TPackageInterface>();
		}

		//returns a merged representation of given list of settings packages
		private TPackageInterface MergeUnsorted <TPackageInterface> (IList<TPackageInterface> packages)
			where TPackageInterface : ISettingsPackage<TPackageInterface>
		{
			if (packages.Count == 0) 
			{ return default(TPackageInterface); }
			return packages[0].Merge(packages);
		}

		//[FACTORY] ISettingsCache
		private ISettingsCache CreateSettingsCache ()
		{
			return new SettingsCache();
		}

		//[FACTORY] ISettingsContextlessCache
		private ISettingsContextlessCache CreateContextlessCache ()
		{
			return new SettingsContextlessCache();
		}
	//ENDOF private methods
	}
}