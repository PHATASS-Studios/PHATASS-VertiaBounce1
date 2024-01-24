using System.Collections.Generic;

using static PHATASS.Utils.Extensions.IListExtensions;

using PHATASS.Utils.Types.Mergeables;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

namespace PHATASS.SettingSystem
{
	//Settings package specific to actions of type IActionSlap
	[System.Serializable]
	public class SettingsPackageActionSlap :
		SettingsPackageRadialActionBase<SettingsPackageActionSlap, ISettingsPackageActionSlap>,
		ISettingsPackageActionSlap
	{
	//ISettingsPackageActionSlap interface implementation
		[UnityEngine.Tooltip("Slap base force at the center of slap. Farther points will receive less force.")]
		[UnityEngine.SerializeField]
		[SerializedTypeRestriction(typeof(ISettingForce))]
		private UnityEngine.Object _forceSetting;
		public ISettingForce forceSetting
		{
			get { return _forceSetting as ISettingForce; }
			private set { _forceSetting = value as UnityEngine.Object; }
		}		
	//ENDOF ISettingsPackageActionSlap interface implementation

	//private static methods
		private static void SetForceSetting (
			SettingsPackageActionSlap package,
			ISettingForce force
		) {
			package.forceSetting = force;
		}

		//gets the result of merging every radius in the mergeables array
		private static ISettingForce GetMergedForceSetting (IList<ISettingsPackageActionSlap> mergeables)
		{
			if (mergeables.Count == 0) { return default(ISettingForce); }

			//gather every callback stack item in a new array
			ISettingForce[] settingArray = new ISettingForce[mergeables.Count];
			for (int i = 0, iLimit = mergeables.Count; i < iLimit; i++)
			{ settingArray[i] = mergeables[i].forceSetting; }

			//return the result of merging that array
			return forceMerger.Merge(settingArray);
		}

		//sets package radius as the merge of every radius in mergeables
		private static void MergeForceSetting (SettingsPackageActionSlap package, IList<ISettingsPackageActionSlap> mergeables)
		{
			SetForceSetting(package, GetMergedForceSetting(mergeables));
		}
	//ENDOF private static methods

	//overridable protected methods
		//merges an array of TPackageInterface mergeables into a TPackageClass object
		//Must be overridden by invoking base.MergePackagesStep() and modifying returned object
		protected override SettingsPackageActionSlap MergeUnsorted (IList<ISettingsPackageActionSlap> mergeables)
		{
			SettingsPackageActionSlap package = base.MergeUnsorted(mergeables);
			MergeForceSetting(package, mergeables);
			return package;
		}
	//ENDOF overridable protected methods

	//private properties
		//[FACTORY] provider for a base merger object for IMergeables of type ISettingForce
		private static IMerger<ISettingForce> _forceMerger = null;
		private static IMerger<ISettingForce> forceMerger
		{ get {
			if (_forceMerger == null) { _forceMerger = new SettingForce(); }
			return _forceMerger;
		}}
	//ENDOF private properties
	}
}