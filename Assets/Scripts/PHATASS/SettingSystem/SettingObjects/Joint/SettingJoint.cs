using System.Collections.Generic;

using static PHATASS.Utils.Extensions.IListExtensions;

using PHATASS.Utils.Types.Mergeables;

using ConfigurableJoint = UnityEngine.ConfigurableJoint;

namespace PHATASS.SettingSystem
{
	[System.Serializable]
	public struct SettingJoint : ISettingJoint
	{
	//IMerger<ISettingJoint>
		//this object has a very simple merge politic: just return the last mergeable that is not null
		ISettingJoint IMerger<ISettingJoint>.Merge (IList<ISettingJoint> mergeables)
		{ return mergeables.EMGetLastNonNull<ISettingJoint>(); }
	//ENDOF IMerger<ISettingJoint>

	//ISettingJoint implementation
		//has value property getter
		public bool hasValue
		{ get { return sampleJoint != null; }}

		//Sample Joint
		[UnityEngine.SerializeField]
		[UnityEngine.Tooltip("Reference to a prefabed ConfigurableJoint, as a configuration sample")]
		private ConfigurableJoint _sampleJoint;
		public ConfigurableJoint sampleJoint
		{
			get { return _sampleJoint; }
			private set { _sampleJoint = value; }
		}
	//ENDOF ISettingJoint

	//Constructor
		/*public SettingJoint (ConfigurableJoint joint)
		{
			_sampleJoint = joint;
		}*/
	//ENDOF Constructor

	//private fields
	//ENDOF private fields
	}
}