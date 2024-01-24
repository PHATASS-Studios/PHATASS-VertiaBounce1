using System.Collections.Generic;
using PHATASS.Utils.Types.Mergeables;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

namespace PHATASS.SettingSystem
{
	[System.Serializable]
	[UnityEngine.CreateAssetMenu(
		fileName = "JointDefinitionObject",
		menuName = "PHATASS settings/Setting objects/Joint definition",
		order = 1
	)]
	public class SOSettingJoint : UnityEngine.ScriptableObject, ISettingJoint
	{
	//IMerger<ISettingJoint>
		ISettingJoint IMerger<ISettingJoint>.Merge (IList<ISettingJoint> mergeables)
		{ return ((ISettingJoint) this.backingField).Merge(mergeables); }
	//ENDOF IMerger<ISettingJoint>

	//ISettingJoint implementation
		UnityEngine.ConfigurableJoint ISettingJoint.sampleJoint { get { return this.backingField.sampleJoint; }}
	//ENDOF ISettingJoint implementation

	//constructor
		public SOSettingJoint (SettingJoint jointSetting)
		{
			this.backingField = jointSetting;
		}
	//ENDOF constructor

	//private fields
		[UnityEngine.SerializeField]
		[UnityEngine.Tooltip("Joint definition")]
		private SettingJoint backingField;
	//ENDOF private fields
	}
}
