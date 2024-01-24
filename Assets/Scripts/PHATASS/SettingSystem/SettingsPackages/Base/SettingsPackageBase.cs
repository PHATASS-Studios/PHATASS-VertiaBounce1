using System;
using System.Collections.Generic;
using PHATASS.Utils.Types.Mergeables;
using PHATASS.Utils.Types.Priorizables;

using static PHATASS.Utils.Types.Priorizables.PriorizableExtensions;

using Debug = UnityEngine.Debug;

#nullable enable
namespace PHATASS.SettingSystem
{
	//Base settings package
	//	TPackageClass must define the last implementation level class type
	//	TPackageInterface must define the last implementation level of the package
	public abstract class SettingsPackageBase <TPackageClass, TPackageInterface> :
		ISettingsPackage <TPackageInterface>
		where TPackageInterface :
			class,
			ISettingsPackage<TPackageInterface>
		where TPackageClass :
			SettingsPackageBase<TPackageClass, TPackageInterface>,
			TPackageInterface,
			new()
	{
	//IComparable<TPackageInterface>
		//IComparable comparison method
		int IComparable<TPackageInterface>.CompareTo (TPackageInterface other)
		{
			/*
			Debug.Log(string.Format("{0}.CompareTo({1})", typeof(TPackageInterface), other));
			if (other == null) { Debug.Log("!> other is NULL"); }
			//*/

			if (other == null) { return 1; }

			return this.priority.CompareTo(other.priority);
		}

		/*
		//comparison operators overload. Compares between objects of this class and IPriorizable<T> of a similar type T
		public static bool operator > (SettingsPackageBase <TPackageClass, TPackageInterface> operand1, IPriorizable<TPackageInterface> operand2)
		{ return operand1.CompareTo(operand2) > 0; }

		public static bool operator < (SettingsPackageBase <TPackageClass, TPackageInterface> operand1, IPriorizable<TPackageInterface> operand2)
		{ return operand1.CompareTo(operand2) < 0; }

		public static bool operator >= (SettingsPackageBase <TPackageClass, TPackageInterface> operand1, IPriorizable<TPackageInterface> operand2)
		{ return operand1.CompareTo(operand2) >= 0; }

		public static bool operator <= (SettingsPackageBase <TPackageClass, TPackageInterface> operand1, IPriorizable<TPackageInterface> operand2)
		{ return operand1.CompareTo(operand2) <= 0; }

		public static bool operator == (SettingsPackageBase <TPackageClass, TPackageInterface> operand1, IPriorizable<TPackageInterface> operand2)
		{ return operand1.CompareTo(operand2) == 0; }

		public static bool operator != (SettingsPackageBase <TPackageClass, TPackageInterface> operand1, IPriorizable<TPackageInterface> operand2)
		{ return operand1.CompareTo(operand2) != 0; }
		//*/
	//ENDOF IComparable<TPackageInterface>

	//IPriorizable <TPackageInterface>
		//dictates this object's priority over other priorizables
		[UnityEngine.Tooltip("Priority of this settings package - higher priority packages precede")]
		[UnityEngine.SerializeField]
		private int _priority;
		private int priority { get { return this._priority; }}
		int IPriorizable<TPackageInterface>.priority { get { return this.priority; }}
	//ENDOF IPriorizable <TPackageInterface>

	//IMerger<TPackageInterface>
		//returns an instance of TPackageInterface combining a list of mergeables
		//sorts by priority
			//base class only ensures a new copy of this settings object is created
		TPackageInterface IMerger<TPackageInterface>.Merge (IList<TPackageInterface> mergeables)
		{
			//Debug.Log(string.Format("SettingsPackageBase.Merge({0}) parameter count: {1}", mergeables, mergeables.Count));

			return ((TPackageInterface) this.MergeUnsorted(mergeables.ESortByPriority()));
		}
	//ENDOF IMerger<TPackageInterface>

	//overridable protected methods
		//merges an array of TPackageInterface mergeables into a TPackageClass object
		//Must be overridden by invoking base.MergePackagesStep() and modifying returned object
		protected virtual TPackageClass MergeUnsorted (IList<TPackageInterface> mergeables)
		{
			//Debug.Log("SettingsPackageBase.MergeUnsorted()");
			//Create an empty object at top level class inheritance
			return new TPackageClass();
		}
	//ENDOF overridable protected methods
	}
}			
#nullable restore
