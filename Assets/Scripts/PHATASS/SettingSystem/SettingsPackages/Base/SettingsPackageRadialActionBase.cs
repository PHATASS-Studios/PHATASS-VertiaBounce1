using System.Collections.Generic;
using Debug = UnityEngine.Debug;

using static PHATASS.Utils.Extensions.IListExtensions;

using PHATASS.Utils.Types.Mergeables;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

#nullable enable
namespace PHATASS.SettingSystem
{
	//Base settings package for actions with a radius of effect
	//Defines accessors to every setting specific to actions with a defined radius of effect
	public abstract class SettingsPackageRadialActionBase <TPackageClass, TPackageInterface>
	:
		SettingsPackageActionBase<TPackageClass, TPackageInterface>,
		ISettingsPackageRadialAction<TPackageInterface>
		where TPackageInterface :
			class?,
			ISettingsPackageRadialAction<TPackageInterface>
		where TPackageClass :
			SettingsPackageRadialActionBase<TPackageClass, TPackageInterface>,
			TPackageInterface,
			new()
	{
	//ISettingsPackageRadialActionBase implementation
	  //actionRadiusSetting properties and setters
		[UnityEngine.Tooltip("Action radius")]
		[UnityEngine.SerializeField]
		[SerializedTypeRestriction(typeof(ISettingCollisionRadius))]
		private UnityEngine.Object _actionRadiusSetting;
		public ISettingCollisionRadius actionRadiusSetting
		{ 
			get { return this._actionRadiusSetting as ISettingCollisionRadius; }
			private set { this._actionRadiusSetting = value as UnityEngine.Object; }
		}
	//ENDOF ISettingsPackageRadialActionBase implementation

	//private static methods
		//this method alows other instances of the same class to set private properties
		private static void SetActionRadius (TPackageClass package,	ISettingCollisionRadius collisionRadius)
		{ package.actionRadiusSetting = collisionRadius; }

		//gets the result of merging every radius in the mergeables array
		private static ISettingCollisionRadius GetMergedActionRadius (IList<TPackageInterface> mergeables)
		{
			if (mergeables.Count == 0) { return default(ISettingCollisionRadius); }

			//gather every stack item in a new array
			ISettingCollisionRadius[] radiusArray = new ISettingCollisionRadius[mergeables.Count];
			for (int i = 0, iLimit = mergeables.Count; i < iLimit; i++)
			{ radiusArray[i] = mergeables[i].actionRadiusSetting; }

			//return the result of merging that array
			return radiusMerger.Merge(radiusArray);
		}

		//sets package radius as the merge of every radius in mergeables
		private static void MergeActionRadius (TPackageClass package, IList<TPackageInterface> mergeables)
		{
			SetActionRadius(package, GetMergedActionRadius(mergeables));
		}
	//ENDOF private static

	//overridable protected methods
		//merges an array of TPackageInterface mergeables into a TPackageClass object
		//Must be overridden by invoking base.MergePackagesStep() and modifying returned object
		protected override TPackageClass MergeUnsorted (IList<TPackageInterface> mergeables)
		{
			//Debug.Log("SettingsPackageRadialActionBase.MergeUnsorted()");
			TPackageClass package = base.MergeUnsorted(mergeables);
			MergeActionRadius(package, mergeables);
			return package;
		}
	//ENDOF overridable protected methods

	//private properties
		//[FACTORY] provider for a base merger object for IMergeables of type ISettingCollisionRadius
		private static IMerger<ISettingCollisionRadius> _radiusMerger = null;
		private static IMerger<ISettingCollisionRadius> radiusMerger
		{ get {
			if (_radiusMerger == null) { _radiusMerger = new SettingCollisionRadius(); }
			return _radiusMerger;
		}}
	//ENDOF private properties
	}
}
#nullable restore
