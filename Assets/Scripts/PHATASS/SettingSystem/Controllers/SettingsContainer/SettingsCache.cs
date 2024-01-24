using System.Collections.Generic;

using Type = System.Type;
using KeyTuple = System.Tuple<System.Type, UnityEngine.GameObject>;

using Debug = UnityEngine.Debug;
using GameObject = UnityEngine.GameObject;

namespace PHATASS.SettingSystem
{
	public partial class SettingsContainer
	{
		//class containing settings linked to a specific context
		private class SettingsCache : ISettingsCache
		{
		//Constructor
			public SettingsCache ()
			{
				this.InitializeCache();
			}
		//ENDOF Constructor

		//ISettingsContainer implementation
			//Composes and returns a settings package adequate to context GameObject
			//this means default settings altered according to gameObject-specific settings
			TPackageInterface ISettingsContainer.GetSettingsPackage <TPackageInterface>
				(GameObject context)
				//where TPackageInterface : ISettingsPackage<TPackageInterface>
			{ return (TPackageInterface) this.cache[this.GenerateKey<TPackageInterface>(context)]; }

			//Stores a settings package for given context, generic and non-generic overloads
			void ISettingsContainer.RegisterSettingsPackage <TPackageInterface>
				(TPackageInterface package, GameObject context)
				//where TPackageInterface : ISettingsPackage<TPackageInterface>
			{ this.cache[this.GenerateKey<TPackageInterface>(context)] = package; }
				//non-generic counterpart is non-valid
				void ISettingsContainer.RegisterSettingsPackage (System.Object package, GameObject context)
				{ throw new System.InvalidOperationException("SettingsCache.ISettingsContainer.RegisterSettingsPackage(): non-generic method is not valid."); }
		//ENDOF ISettingsContainer

		//ISettingsCache implementation
			//force-empty cache
			void ISettingsCache.Reset ()
			{ this.InitializeCache(); }

			//returns wether cache contains an entry for target context and type
			bool ISettingsCache.ContainsSettingsPackage <TPackageInterface>
				(GameObject context)
				//where TPackageInterface : ISettingsPackage<TPackageInterface>
			{ 
				return this.cache.ContainsKey(
					this.GenerateKey<TPackageInterface>(context)
				);
			}
		//ENDOF ISettingsCache

		//private fields
			private IDictionary<KeyTuple, ISettingsPackage> cache;
		//ENDOF private fields

		//private methods
			//initializes cache as empty
			private void InitializeCache ()
			{
				//Create an empty cache only if there is not an empty dictionary ready already
				if (this.cache == null || this.cache.Count > 0)
				{ this.cache = this.CreateCache(); }
			}

			//[FACTORY] creates a KeyTuple object from a type and context
			private KeyTuple GenerateKey <TPackageInterface>
				(GameObject context)
				where TPackageInterface : ISettingsPackage<TPackageInterface>
			{ return new KeyTuple(typeof(TPackageInterface), context); }

			//[FACTORY] creates an empty cache dictionary
			private IDictionary<KeyTuple, ISettingsPackage> CreateCache ()
			{ return new Dictionary<KeyTuple, ISettingsPackage>(); }
		//ENDOF private methods
		}
	}
}