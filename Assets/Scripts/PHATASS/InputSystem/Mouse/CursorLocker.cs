using UnityEngine;

namespace PHATASS.InputSystem
{
	//Cursor locker static class definition
	public static class CursorLocker
	{
		public static bool cursorLocked 
		{
			get { return CursorLocker._cursorLocked; }
			set
			{
				CursorLocker._cursorLocked = value;
				Cursor.visible = !value;
				Cursor.lockState = (value) ? CursorLockMode.Locked : CursorLockMode.None;
			}
		} 
		private static bool _cursorLocked = false;

				//Cursor.lockState = CursorLockMode.None;
				//Cursor.lockState = CursorLockMode.Locked;
	}
	//ENDOF Cursor locker
}