using UnityEngine;

using IToggleable = PHATASS.Utils.Types.Toggleables.IToggleable;
using IBoolValue = PHATASS.Utils.Types.Values.IBoolValue;

namespace PHATASS.CameraSystem.ScreenUtilityComponents
{
// Class handling full screen toggling
//		remember UnityEngine requires all full-screen activation requests done on press instead of on release or merely scripted
//	exposes:
//		IToggleable interface - gets or sets current full-screen state (false: disabled - true: enabled)
//		IBoolValue interface - gets current full-screen state (false: disabled - true: enabled)
//		DoFullScreenToggle() public event - Alternates full-screen state
	public class FullScreenToggler :
		MonoBehaviour,
		IToggleable,
		IBoolValue
	{
	//IToggleable
		bool IToggleable.state
		{
			get { return this.state; }
			set { this.state = value; }
		}
	//ENDOF IToggleable

	//IBoolValue
		bool PHATASS.Utils.Types.Values.IValue<bool>.value
		{ get { return this.state; }}
	//ENDOF IBoolValue

	//MonoBehaviour lifecycle
	//ENDOF MonoBehaviour

	//public events
		//Alternates sound state
		public void DoFullScreenToggle ()
		{ this.state = !this.state; }
	//ENDOF public events

	//private
		private bool state
		{
			get { return Screen.fullScreen; }
			set { Screen.fullScreen = value; }
		}
	//ENDOF private
	}
}