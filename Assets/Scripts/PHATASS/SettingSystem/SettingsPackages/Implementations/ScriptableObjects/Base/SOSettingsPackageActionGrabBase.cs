namespace PHATASS.SettingSystem
{
	[System.Serializable]
	public class SOSettingsPackageActionGrabBase
		<TPackageClass, TPackageInterface>
	:	SOSettingsPackageRadialActionBase <TPackageClass, TPackageInterface>,
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