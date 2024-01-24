using UnityEngine;

public class QuitOnEscButton : MonoBehaviour
{
	private void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{ Application.Quit(); }
	}
}
