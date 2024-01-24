using System.Collections.Generic;
using Debug = UnityEngine.Debug;

using static PHATASS.Utils.Extensions.IListExtensions;

using PHATASS.Utils.Types.Mergeables;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

#nullable enable
namespace PHATASS.SettingSystem
{
	//Settings package specific to actions of type IActionGrab
	[System.Serializable]
	public abstract class SettingsPackageActionGrabBase <TPackageClass, TPackageInterface>
	:
		SettingsPackageRadialActionBase<TPackageClass, TPackageInterface>,
		ISettingsPackageActionGrab<TPackageInterface>
		where TPackageInterface :
			class?,
			ISettingsPackageActionGrab<TPackageInterface>
		where TPackageClass :
			SettingsPackageActionGrabBase<TPackageClass, TPackageInterface>,
			TPackageInterface,
			new()
	{

	//ISettingsPackageActionGrab interface implementation
	//ENDOF ISettingsPackageActionGrab interface implementation
		
	//overridable protected methods
	//ENDOF overridable protected methods

	//private properties
	//ENDOF private properties
	}
}
#nullable restore