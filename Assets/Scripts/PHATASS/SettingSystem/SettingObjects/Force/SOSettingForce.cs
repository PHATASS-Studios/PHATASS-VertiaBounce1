using System.Collections.Generic;
using PHATASS.Utils.Types.Mergeables;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

namespace PHATASS.SettingSystem
{
	[System.Serializable]
	[UnityEngine.CreateAssetMenu(
		fileName = "ForceSettingObject",
		menuName = "PHATASS settings/Setting objects/Force setting",
		order = 1
	)]
	public class SOSettingForce : UnityEngine.ScriptableObject, ISettingForce
	{
	//IMerger<ISettingForce>
		ISettingForce IMerger<ISettingForce>.Merge (IList<ISettingForce> mergeables)
		{ return ((ISettingForce) this.backingField).Merge(mergeables); }
	//ENDOF IMerger<ISettingForce>

	//ISettingForce implementation
		float ISettingForce.value { get { return this.backingField.value; }}
	//ENDOF ISettingForce

	//constructor
		public SOSettingForce (SettingForce forceSetting)
		{
			this.backingField = forceSetting;
		}
	//ENDOF constructor

	//private fields
		[UnityEngine.SerializeField]
		[UnityEngine.Tooltip("Force definition")]
		private SettingForce backingField;
	//ENDOF private fields
	}
}