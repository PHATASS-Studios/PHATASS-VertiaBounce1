using System.Collections.Generic;

using PHATASS.Utils.Types.Mergeables; //IMergeable<T>
using PHATASS.Utils.Types.Priorizables; //IPriorizable<T>
using System; //IComparable<T>

namespace PHATASS.SettingSystem
{
	//base class for any settings package defined as a ScriptableObject
	[System.Serializable]
	public abstract class SOSettingsPackageBase
		<TPackageClass, TPackageInterface>
	:	UnityEngine.ScriptableObject,
		ISettingsPackage <TPackageInterface>
		where TPackageInterface : ISettingsPackage <TPackageInterface>
		where TPackageClass : TPackageInterface
	{
	//IComparable <TPackageInterface>
		int IComparable<TPackageInterface>.CompareTo (TPackageInterface other)
		{ return backingField.CompareTo(other); }
	//ENDOF IComparable <TPackageInterface>

	//IPriorizable <TPackageInterface>
		int IPriorizable<TPackageInterface>.priority
		{ get { return backingField.priority; }}
	//ENDOF IPriorizable <TPackageInterface>

	//IMerger <TPackageInterface>
		TPackageInterface IMerger<TPackageInterface>.Merge (IList<TPackageInterface> mergeables)
		{ return backingField.Merge(mergeables); }
	//ENDOF IMerger

	//private fields
		[UnityEngine.SerializeField]
		protected TPackageClass backingField;
	//ENDOF
	}
}
