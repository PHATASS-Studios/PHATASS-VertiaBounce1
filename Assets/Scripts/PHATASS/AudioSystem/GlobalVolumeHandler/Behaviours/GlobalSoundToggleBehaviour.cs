using UnityEngine;

using IToggleable = PHATASS.Utils.Types.Toggleables.IToggleable;
using IBoolValue = PHATASS.Utils.Types.Values.IBoolValue;

namespace PHATASS.AudioSystem.Global
{
// Class exposing access to the global sound handler as a behaviour
//	exposes:
//		IToggleable interface - gets or sets current sound state (false: muted - true: enabled)
//		IBoolValue interface - gets current sound state (false: muted - true: enabled)
//		DoSoundToggle() public event - Alternates sound state
	public class GlobalSoundToggleBehaviour :
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
		public void DoSoundToggle ()
		{ GlobalSoundVolumeHandler.Toggle(); }
	//ENDOF public events

	//private
		private bool state
		{
			get { return GlobalSoundVolumeHandler.state; }
			set { GlobalSoundVolumeHandler.SetState(value); }
		}
	//ENDOF private
	}
}