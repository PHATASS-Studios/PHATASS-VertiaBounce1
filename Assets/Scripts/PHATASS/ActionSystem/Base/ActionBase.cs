using ControllerCache = PHATASS.ControllerSystem.ControllerCache;

using ITool = PHATASS.ToolSystem.Tools.ITool;

using EButtonInputState = PHATASS.InputSystem.EButtonInputState;

using GameObject = UnityEngine.GameObject;

/*DEBUG*/using Debug = UnityEngine.Debug;/*DEBUG*/

namespace PHATASS.ActionSystem
{
	public abstract class ActionBase <TSettingsPackage> :
		IAction
		where TSettingsPackage : PHATASS.SettingSystem.ISettingsPackageAction<TSettingsPackage>
	{
	//IAction implementation
		//returns true if this action is currently doing something, like maintaining a grab or repeating a slapping pattern
		//base class only knows an action is ongoing if automatized. Children classes may determine additional conditions
		bool IAction.ongoing { get { return this.ongoing; }}
		protected virtual bool ongoing { get { return this.auto; }}

		//wether action is to automatically repeat
		bool IAction.auto { get { return this.auto; }}
		protected bool auto { get { return this._auto; } set { this._auto = value; }}
		private bool _auto = false;

		//initialize the action with a reference to the parent tool
		bool IAction.Initialize (ITool parentTool) { return this.Initialize(parentTool); }
		protected virtual bool Initialize (ITool parentTool)
		{
			Debug.Log("Action initializing:" + this + " received: " + parentTool);
			this.tool = parentTool;
			return this.IsValid();
		}

		//receive state of corresponding input medium
		void IAction.Input (EButtonInputState state) { this.Input(state); }
		protected abstract void Input (EButtonInputState state);

		//action update. Must be called once per frame.
		void IAction.ActionUpdate () { this.ActionUpdate(); }
		protected virtual void ActionUpdate () {}

		//try to set in automatic state. Returns true on success
		bool IAction.Automate () { return this.Automate(); }
		protected virtual bool Automate () { return false; }

		//stop automation
		void IAction.DeAutomate () { this.DeAutomate(); }
		protected virtual void DeAutomate () {}

		//clears and finishes the action
		void IAction.Clear () { this.Clear(); }
		protected virtual void Clear ()
		{
			//this.auto = false; //unnecessary the action is gonna be destroyed anyway
			this.tool.ActionEnded(this); //Tells the parent to properly dispose of this eel-ement
		}

		//returns true if this action is valid for this hand (targets in range 'n such)
		bool IAction.IsValid () { return this.IsValid(); }
		protected virtual bool IsValid () { return false; }
	//ENDOF IAction implementation

	//protected properties
		//Default settings per-type
		protected TSettingsPackage defaultSettings
		{ get { return ControllerCache.settingsProvider.GetSettingsPackage<TSettingsPackage>(); }}
	//ENDOF protected properties

	//protected fields
		protected ITool tool;
	//ENDOF protected fields

	//protected methods
	   	// > Callbacks implementation
		//executes start callbacks for every linked object
		protected void DoCallbacksStart ()
		{
			//foreach 
		}

		protected void DoCallbackStart (GameObject context)
		{}

		//executes callbacks update for every linked object
		protected void DoCallbacksUpdate ()
		{}

		protected void DoCallbackUpdate (GameObject context)
		{}

		//executes callbacks ended for every linked object
		protected void DoCallbacksEnded ()
		{}

		protected void DoCallbackEnded (GameObject context)
		{}
	  //end callbacks implementation
	//ENDOF protected methods
	}
}