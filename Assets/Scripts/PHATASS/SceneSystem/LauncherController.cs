using System.Collections;
using UnityEngine;

using CursorLocker = PHATASS.InputSystem.CursorLocker;

namespace PHATASS.SceneSystem
{
// Class containing a single public event/method "Launch()" that, when called, locks cursor and initializes scene cycle
//	Meant to be called right after splash is finished in order to create transition scene and transition to menu/intro scene	
	public class LauncherController : MonoBehaviour
	{
		//public GameObject SplashContainer
		public void Launch ()
		{
			CursorLocker.cursorLocked = true;
			SceneController.Initialize();
		}
	}
}