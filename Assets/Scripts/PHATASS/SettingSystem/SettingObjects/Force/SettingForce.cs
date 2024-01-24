using System.Collections.Generic;

using static PHATASS.Utils.Extensions.IListExtensions;

using PHATASS.Utils.Types.Mergeables;

using TooltipAttribute = UnityEngine.TooltipAttribute;
using SerializeFieldAttribute = UnityEngine.SerializeField;

using ForceMode = UnityEngine.ForceMode;

namespace PHATASS.SettingSystem
{
	//force setting - includes info on force strength and force mode
	[System.Serializable]
	public struct SettingForce : ISettingForce
	{
	//IMerger<ISettingForce>
		//this object has a very simple merge politic: just return the last mergeable that is not null
		ISettingForce IMerger<ISettingForce>.Merge (IList<ISettingForce> mergeables)
		{ return mergeables.EMGetLastNonNull<ISettingForce>(); }
	//ENDOF IMerger<ISettingForce>

	//ISettingForce implementation
		//Force Value
		[Tooltip("Force value")]
		[SerializeField]
		private float _value;
		public float value
		{
			get { return _value; }
			private set { _value = value; }
		}
	//ENDOF ISettingForce implementation

	//Constructor
		/*public SettingForce (float value, ForceMode mode)
		{
			_value = value;
			_mode = mode;
		}*/
	//ENDOF Constructor
	}
}