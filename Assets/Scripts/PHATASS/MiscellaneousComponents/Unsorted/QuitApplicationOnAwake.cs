using UnityEngine;

public class QuitApplicationOnAwake : MonoBehaviour
{
	void Awake ()
	{
		Application.Quit();
	}
}
