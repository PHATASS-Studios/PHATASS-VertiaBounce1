using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

using IUpdatableCallbackStack = PHATASS.Utils.Callbacks.IUpdatableCallbackStack;

namespace PHATASS.SettingSystem
{
	//base class for action-related settings packages defined as a ScriptableObject
	[System.Serializable]
	public abstract class MBSettingsPackageActionBase
		<TPackageClass, TPackageInterface>
	:	MBSettingsPackageBase <TPackageClass, TPackageInterface>,
		ISettingsPackageAction <TPackageInterface>
		where TPackageInterface : ISettingsPackageAction <TPackageInterface>
		where TPackageClass : TPackageInterface
	{
	//ISettingsPackageAction
		public IUpdatableCallbackStack callbacks { get { return backingField.callbacks; } }
	//ENDOF
	}
}