namespace PHATASS.AudioSystem.Global
{
// Static class handling global sound volume
//	It allows enabling/disabling, toggling, and checking sound enabled/muted state
	public static class GlobalSoundVolumeHandler
	{
	// Toggles sound enabled/disabled state, and returns the resulting state (false = muted, true = sound enabled)
		public static bool Toggle ()
		{
			SetState(!state);
			return state;
		}

	// Sets sound state. false = muted, true = enable sound. Does nothing if state is the same as current state
	//	returns true if state actually changed.
		public static bool SetState (bool desiredState)
		{
			if (state == desiredState) { return false; }

			//sets current volume
			UnityEngine.AudioListener.volume = (desiredState)
				?	1.0f
				:	0.0f;

			return true;
		}

	// Current global sound state: false = muted, true = sound enabled
	//	returns true if current audioListener volume is above zero
		public static bool state
		{ get { return UnityEngine.AudioListener.volume > 0.0f; }}

	//private static
	//ENDOF private static
	}
}