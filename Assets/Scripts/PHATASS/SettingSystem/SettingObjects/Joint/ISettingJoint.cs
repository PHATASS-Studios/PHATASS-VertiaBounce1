using UnityEngine;

namespace PHATASS.SettingSystem
{
	public interface ISettingJoint :
		ISetting<ISettingJoint>
	{
		ConfigurableJoint sampleJoint {get;}
	}
}