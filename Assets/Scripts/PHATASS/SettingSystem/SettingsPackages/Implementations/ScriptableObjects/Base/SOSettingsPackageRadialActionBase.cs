namespace PHATASS.SettingSystem
{
	//base class for any settings package defined as a ScriptableObject
	[System.Serializable]
	public abstract class SOSettingsPackageRadialActionBase
		<TPackageClass, TPackageInterface>
	:	SOSettingsPackageActionBase <TPackageClass, TPackageInterface>,
		ISettingsPackageRadialAction <TPackageInterface>
		where TPackageInterface : ISettingsPackageRadialAction <TPackageInterface>
		where TPackageClass : TPackageInterface
	{
	//ISettingsPackageRadialAction
		public ISettingCollisionRadius actionRadiusSetting
		{ get { return backingField.actionRadiusSetting; }}
	//ENDOF
	}
}