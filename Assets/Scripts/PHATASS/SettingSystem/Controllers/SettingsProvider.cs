using System.Collections.Generic;
using Debug = UnityEngine.Debug;

using GameObject = UnityEngine.GameObject;

using Type = System.Type;

//[UNTESTED] [TO-DO] test this shiet
namespace PHATASS.SettingSystem
{
	//[TO-DO] [IMPORTANT] cache settings package by type and context

	//Action settings provider class
	//stores a list of ISettingsPackage objects and finds and returns requested settings packages
	[UnityEngine.DefaultExecutionOrder(-200)] //this MonoBehaviour needs to execute before settings packages try to register
	public partial class SettingsProvider
	:
		PHATASS.ControllerSystem.MonoBehaviourControllerBase<ISettingsProvider>,
		ISettingsProvider
	{
	//MonoBehaviour lifecycle
		protected override void Awake ()
		{
			base.Awake();
			this.container = this.CreateContainer();
		}
	//ENDOF MonoBehaviour

	//ISettingsContainer implementation
		//Composes and returns a settings package adequate to context GameObject
		//this means default settings altered according to gameObject-specific settings
		TPackageInterface ISettingsContainer.GetSettingsPackage <TPackageInterface>
			(GameObject context)
			//where TPackageInterface : ISettingsPackage<TPackageInterface>;
		{ return this.container.GetSettingsPackage<TPackageInterface>(context); }

		//Stores a settings package for given context
		void ISettingsContainer.RegisterSettingsPackage (System.Object package, GameObject context)
		{ 
			if (context == this.gameObject) { context = null; }
			this.container.RegisterSettingsPackage(package, context);
		}
		void ISettingsContainer.RegisterSettingsPackage <TPackageInterface>
			(TPackageInterface package, GameObject context)
			//where TPackageInterface : ISettingsPackage<TPackageInterface>;
		{
			if (context == this.gameObject) { context = null; }
			this.container.RegisterSettingsPackage<TPackageInterface>(package, context);
		}
	//ENDOF ISettingsContainer implementation

	//private fields
		private ISettingsContainer container;
	//ENDOF private fields

	//private methods
		//[FACTORY] ISettingsContainer
		private ISettingsContainer CreateContainer ()
		{ return new SettingsContainer (); }
	//ENDOF private methods
	}
}