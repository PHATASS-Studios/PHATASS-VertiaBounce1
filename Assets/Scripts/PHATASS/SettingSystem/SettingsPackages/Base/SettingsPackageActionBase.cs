using System.Collections.Generic;
using Debug = UnityEngine.Debug;

using static PHATASS.Utils.Extensions.IListExtensions;

using PHATASS.Utils.Types.Mergeables;
using IUpdatableCallbackStack = PHATASS.Utils.Callbacks.IUpdatableCallbackStack;
using UpdatableCallbackStack = PHATASS.Utils.Callbacks.UpdatableCallbackStack;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

#nullable enable
namespace PHATASS.SettingSystem
{
	//Base settings package for action settings
	[System.Serializable]
	public class SettingsPackageActionBase <TPackageClass, TPackageInterface>
	:
		SettingsPackageBase <TPackageClass, TPackageInterface>,
		ISettingsPackageAction <TPackageInterface>
		where TPackageInterface :
			class?,
			ISettingsPackageAction<TPackageInterface>
		where TPackageClass :
			SettingsPackageActionBase <TPackageClass, TPackageInterface>,
			TPackageInterface,
			new()
	{
	//ISettingsPackageAction
		[UnityEngine.Tooltip("Callbacks set for the start, update, and end of this action.")]
		[UnityEngine.SerializeField]
		// By-value UpdatableCallbackStack implementation
		private UpdatableCallbackStack _callbacks;
		private IUpdatableCallbackStack callbacks
		{
			get { return _callbacks as IUpdatableCallbackStack; }
			set { _callbacks = value as UpdatableCallbackStack; }
		}
		IUpdatableCallbackStack ISettingsPackageAction<TPackageInterface>.callbacks
		{ get { return this.callbacks; }}
	//ENDOF

	//private static methods
		//this method alows other instances of the same class to set private properties
		private static void SetCallbacks (TPackageClass package, IUpdatableCallbackStack callbackStack)
		{ package.callbacks = callbackStack; }

		//gets the result of merging every callback stack in the mergeables array
		private static IUpdatableCallbackStack GetMergedCallbackStack (IList<TPackageInterface> mergeables)
		{
			if (mergeables.Count == 0) { return default(IUpdatableCallbackStack); }

			//gather every callback stack item in a new array
			IUpdatableCallbackStack[] callbacksArray = new IUpdatableCallbackStack[mergeables.Count];
			for (int i = 0, iLimit = mergeables.Count; i < iLimit; i++)
			{ callbacksArray[i] = mergeables[i].callbacks; }

			//return the result of merging that array
			return callbacksMerger.Merge(callbacksArray);
		}

		//sets package callbacks as the merge of every callback stack in mergeables
		private static void MergeCallbacks (TPackageClass package, IList<TPackageInterface> mergeables)
		{
			SetCallbacks(package, GetMergedCallbackStack(mergeables));
		}
	//ENDOF private static

	//overridable protected methods
		//merges an array of TPackageInterface mergeables into a TPackageClass object
		//Must be overridden by invoking base.MergeUnsorted() and modifying returned object
		protected override TPackageClass MergeUnsorted (IList<TPackageInterface> mergeables)
		{
			//Debug.Log("SettingsPackageActionBase.MergeUnsorted()");
			TPackageClass package = base.MergeUnsorted(mergeables);
			MergeCallbacks(package, mergeables);
			return package;
		}
	//ENDOF overridable protected methods

	//private properties
		//[FACTORY] provider for a base merger object for IMergeables of type IUpdatableCallbackStack
		private static IMerger<IUpdatableCallbackStack> _callbacksMerger = null;
		private static IMerger<IUpdatableCallbackStack> callbacksMerger
		{ get {
			if (_callbacksMerger == null) { _callbacksMerger = new UpdatableCallbackStack(); }
			return _callbacksMerger;
		}}
	//ENDOF private properties
	}
}
#nullable restore
