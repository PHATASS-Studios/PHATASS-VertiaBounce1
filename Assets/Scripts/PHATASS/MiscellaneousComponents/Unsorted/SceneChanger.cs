using UnityEngine;

using ControllerCache = PHATASS.ControllerSystem.ControllerCache;

public class SceneChanger : MonoBehaviour
{
	public void GoToScene (int targetScene)
	{
		ControllerCache.sceneController.ChangeScene(targetScene);
	}
}
