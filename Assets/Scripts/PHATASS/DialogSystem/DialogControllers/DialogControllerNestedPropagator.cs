using UnityEngine;

using System.Collections.Generic;
//using IEnumerator = System.Collections.IEnumerator;

using Averages = PHATASS.Utils.MathUtils.Averages;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

using IToggleable = PHATASS.Utils.Types.Toggleables.IToggleable;
using IAnimatedToggleable = PHATASS.Utils.Types.Toggleables.IAnimatedToggleable;
using DParameterlessDelegate = PHATASS.Utils.Types.Toggleables.DParameterlessDelegate;

namespace PHATASS.DialogSystem.DialogControllers
{
	public class DialogControllerNestedPropagator :
		MonoBehaviour,
		IDialogController
	{
	//serialized fields
		[Tooltip("Target IFloatEventReceiver components whose value will be randomized each kick")]
		[SerializeField]
 		[SerializedTypeRestriction(typeof(IDialogController))]
		private List<UnityEngine.Object> _childDialogList = null;
		private IList<IDialogController> _childDialogListAccessor = null;
		private IList<IDialogController> childDialogList
		{ get {
			//create accessor if unavailable
			if (this._childDialogListAccessor == null && this._childDialogList != null)
			{ this._childDialogListAccessor = new PHATASS.Utils.Types.Wrappers.UnityObjectListCastedAccessor<IDialogController>(this._childDialogList); }

			return this._childDialogListAccessor;
		}}

		[Tooltip("Portrait enabled while this set of dialogs is active. Children portraits are ignored.")]
		[SerializeField]
		[SerializedTypeRestriction(typeof(IToggleable))]
		private UnityEngine.Object? _portrait = null;
		private IToggleable portrait
		{ get {
			if (this._portrait == null) { return null; }
			else { return this._portrait as IToggleable; }
		}}
	//ENDOF serialized

	//IDialogController
		// Toggles this element on or off, or returns the currently DESIRED state
		bool IToggleable.state
		{
			get { return this.state; }
			set { this.state = value; }
		}

		// Returns the state of the transition between enabled (1f) and disabled (0f)
		//	> 0 means fully disabled/false
		//	> 1 means fully enabled/true
		//	> Decimal values between 0 and 1 represents the current progress of the change between those states
		float IAnimatedToggleable.analogTransitionProgress
		{ get { return this.analogTransitionProgress; }}

		// Returns true ONLY if any transition between states has finished AND current state is requiredState
		bool IAnimatedToggleable.StrictStateCheck (bool requiredState)
		{ return this.StrictStateCheck(requiredState); }

		// Transitions current state to desiredState, and triggers finishingCallback when transition finished
		//	returns false if transition fails because initial state == desiredState, true otherwise
		//	finishingCallback can be null, in which case it won't get invoked
		bool IAnimatedToggleable.TransitionStateWithCallback (bool desiredState, DParameterlessDelegate finishingCallback)
		{ return this.TransitionStateWithCallback(desiredState, finishingCallback); }

		// Immediately force set given state
		void IAnimatedToggleable.ForceSetState (bool desiredState)
		{ this.ForceSetState(desiredState); }

		IToggleable IDialogController.portrait { get { return this.portrait; }}
	//ENDOF IDialogController

	//MonoBehaviour lifecycle
		//[TO-DO] MAYBE JUST MAYBE remove autoinitialization
		private void Awake ()
		{
			if (this.childDialogList == null || this.childDialogList.Count <= 0)
			{ this.AutoInitializeChildList(); }
		}
		private void AutoInitializeChildList ()
		{
			List<UnityEngine.Object> foundDialogList = new List<UnityEngine.Object>();
			for (int i = 0, iLimit = transform.childCount; i < iLimit; i++)
			{
				UnityEngine.Object foundDialog = transform.GetChild(i).GetComponent<IDialogController>() as UnityEngine.Object;
				if (foundDialog != null)
				{
					foundDialogList.Add(foundDialog);
				}
			}

			this._childDialogList = foundDialogList;
			this._childDialogListAccessor = null;
		}
	//ENDOF MonoBehaviour

	//private members
		// state.get() only takes into account the state of the FIRST element
		private bool state
		{
			get
			{
				if (this.childDialogList == null || this.childDialogList.Count <= 0) { return false; }
				return this.childDialogList[0].state;
			}
			set
			{
				foreach (IDialogController childDialog in this.childDialogList)
				{ childDialog.state = value; }
			}
		}

		// analogTransitionProgress takes an average of every child progress
		private float analogTransitionProgress
		{
			get
			{
				int iLimit = this.childDialogList.Count;
				if (this.transitionProgressCache == null || this.transitionProgressCache.Length != iLimit)
				{ this.transitionProgressCache = new float[iLimit]; }

				for(int i = 0; i < iLimit; i++)
				{
					this.transitionProgressCache[i] = this.childDialogList[i].analogTransitionProgress;
				}

				return Averages.FloatArithmeticAverage(this.transitionProgressCache);
			}
		}
		private float[] transitionProgressCache = null;

		// StrictStateCheck() requires EVERY child to pass an StrictStateCheck
		private bool StrictStateCheck (bool requiredState)
		{
			if (this.childDialogList == null || this.childDialogList.Count <= 0) { return false; }
			foreach (IDialogController childDialog in this.childDialogList)
			{
				if (!childDialog.StrictStateCheck(requiredState))
				{ return false; }
			}
			return true;
		}

		// ForceSetState propagates the call to every child
		private void ForceSetState (bool desiredState)
		{
			foreach (IDialogController childDialog in this.childDialogList)
			{ childDialog.ForceSetState(desiredState); }
		}

		private bool TransitionStateWithCallback (bool desiredState, DParameterlessDelegate finishingCallback)
		{
			if (
				desiredState == true && this.queuedOnEnableCallback != null
			||	desiredState == false && this.queuedOnDisableCallback != null
			|| this.StrictStateCheck(desiredState)
			)
			{ return false; }

			DParameterlessDelegate requiredCallback;

			if (desiredState == true)
			{
				this.queuedOnEnableCallback = finishingCallback;
				enablesLeft = this.childDialogList.Count;
				requiredCallback = () =>
				{
					enablesLeft--;
					if (enablesLeft <= 0) { this.TriggerAllChildrenEnabledCallback(); }
				};
			}
			else
			{
				this.queuedOnDisableCallback = finishingCallback;
				disablesLeft = this.childDialogList.Count;
				requiredCallback = () =>
				{
					disablesLeft--;
					if (disablesLeft <= 0) { this.TriggerAllChildrenDisabledCallback(); }
				};
			}

			foreach (IDialogController dialog in this.childDialogList)
			{ dialog.TransitionStateWithCallback(desiredState, requiredCallback); }

			return true;
		}

		private float enablesLeft = 0;
		private float disablesLeft = 0;

		private DParameterlessDelegate queuedOnEnableCallback = null;
		private DParameterlessDelegate queuedOnDisableCallback = null;

		private  void TriggerAllChildrenEnabledCallback ()
		{
			this.queuedOnEnableCallback?.Invoke();
			this.queuedOnEnableCallback = null;
		}
		private  void TriggerAllChildrenDisabledCallback ()
		{
			this.queuedOnDisableCallback?.Invoke();
			this.queuedOnDisableCallback = null;
		}
	//ENDOF private
	}
}