using ButtonControl = UnityEngine.InputSystem.Controls.ButtonControl;
using KeyControl = UnityEngine.InputSystem.Controls.KeyControl;

namespace PHATASS.InputSystem
{
	//convert a ButtonControl into an object representing simply its started, ended, held, or none states
	public static class UnityInputSystemExtensions
	{
		public static EButtonInputState EGetButtonState (this ButtonControl control)
		{
			if (control.wasPressedThisFrame) { return EButtonInputState.Started; }
			if (control.wasReleasedThisFrame) { return EButtonInputState.Ended; }
			if (control.isPressed) { return EButtonInputState.Held; }
			return EButtonInputState.None;
		}
		//private static EButtonInputState EGetButtonState (this KeyControl control) { return control.EGetButtonState(); }
	}
}