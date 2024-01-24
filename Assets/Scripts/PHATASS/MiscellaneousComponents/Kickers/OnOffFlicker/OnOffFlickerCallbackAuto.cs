using UnityEngine;

using CallbackContexts = PHATASS.Utils.Callbacks.CallbackContexts;

using UpdatableCallbackStack = PHATASS.Utils.Callbacks.UpdatableCallbackStack;
using IUpdatableCallback = PHATASS.Utils.Callbacks.IUpdatableCallback;
using ICallbackContext = PHATASS.Utils.Callbacks.ICallbackContext;

namespace PHATASS.Miscellaneous.Kickers
{
	public class OnOffFlickerCallbackAuto : OnOffFlickerAutoFireBase
	{
	//MonoBehaviour lifecycle overrides
		public override void Awake ()
		{
			base.Awake();
			this.callbacks = (IUpdatableCallback) this.callbacksSerialized;
		}

		//On update, execute callback update if state requires it
		public override void Update ()
		{
			base.Update();
			if (this.executionState) { this.callbacks.ExecuteUpdate(this.callbackContext); }
		}
	//ENDOF MonoBehaviour lifecycle overrides		

	//inherited abstract property implementation
		protected override bool state
		{
			set { this.SetExecutionState(value); }
		}
	//ENDOF inherited abstract property implementation

	//private properties
		private ICallbackContext callbackContext
		{ get { return CallbackContexts.FromGameObject(this.gameObject); }}
	//ENDOF private properties

	//private fields
		//true while callbacks are in execution/updating
		[SerializeField]
		private UpdatableCallbackStack callbacksSerialized = null;
		private IUpdatableCallback callbacks;
		private bool executionState = false;
	//ENDOF private fields

	//private methods
		private void SetExecutionState (bool targetState)
		{
			//only change if target state is different from current state
			if (this.executionState == targetState) { return; }

			//execute starting or finishing callback depending on state change
			if (targetState) { this.callbacks.ExecuteStart(this.callbackContext); }
			else { this.callbacks.ExecuteEnded(this.callbackContext); }

			this.executionState = targetState;
		}
	//ENDOF private methods
	}
}