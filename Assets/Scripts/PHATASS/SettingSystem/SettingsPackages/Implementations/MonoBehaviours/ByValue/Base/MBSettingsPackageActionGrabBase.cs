namespace PHATASS.SettingSystem
{
	[System.Serializable]
	public class MBSettingsPackageActionGrabBase
		<TPackageClass, TPackageInterface>
	:	MBSettingsPackageRadialActionBase <TPackageClass, TPackageInterface>,
		ISettingsPackageActionGrab <TPackageInterface>
		where TPackageInterface : ISettingsPackageActionGrab <TPackageInterface>
		where TPackageClass : TPackageInterface
	{
	//ISettingsPackageActionGrab
		/*
		public ISettingJoint grabJointSetting
		{ get { return backingField.grabJointSetting; }}
		*/
	//ENDOF
	}
}