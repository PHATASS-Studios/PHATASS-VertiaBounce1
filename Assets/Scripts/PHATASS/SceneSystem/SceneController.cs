using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

using ControllerCache = PHATASS.ControllerSystem.ControllerCache;

namespace PHATASS.SceneSystem
{
	public class SceneController :
		PHATASS.ControllerSystem.MonoBehaviourControllerBase <ISceneController>,
		ISceneController
	{
	//Constants and enum definitions
		//private const float sceneLoadMinimum = 0.9f;
		private static class SceneNumbers
		{
			public static readonly int LAUNCHER = 0;	//unused, included for consistency
			public static readonly int TRANSITION = 1;
			public static readonly int MAINMENU = 2;
			public static readonly int QUITTER = 3;
		}
	//ENDOF Constants and enum definitions

	//serialized fields
		[Tooltip("first transition (launcher to first scene) will have this minimum waiting time in seconds during the loading cue")]
		[SerializeField]
		private float firstTransitionMinimumWait = 1.0f;
	//ENDOF serialized

	//static properties and methods
		//initialize method manually launchs the curtains layer scene through unityengine's SceneManager
		public static void Initialize ()
		{
			SceneManager.LoadScene(SceneNumbers.TRANSITION, LoadSceneMode.Additive);
		}
	//ENDOF static properties and methods

	//private fields and properties
		private bool busy = false;	//kept true while performing a scene change
	//ENDOF private fields and properties

	//MonoBehaviour lifecycle implementation
		//on first instantiation, load the menu under the curtain
		public void Start ()
		{
			Debug.Log("Initial change scene");
			ChangeScene(SceneNumbers.MAINMENU, this.firstTransitionMinimumWait);
		}
	//ENDOF MonoBehaviour lifecycle implementation

	//ISceneController implementation
		//input is enabled if curtains are open or non-existent
		public bool inputEnabled 
		{
			get
			{
				if (ControllerCache.transitionController == null) { return true; }
				return !ControllerCache.transitionController.StrictStateCheck(false);
			}
		}

		//[TO-DO]: ChangeScene should return a value indicating success on scene change, or failure
		public void ChangeScene (int targetScene, float minimumWait = 0.0f)
		{
			if (busy) { return; }
			StartCoroutine(ChangeSceneAsync(targetScene, minimumWait));
		}
	//ENDOF ISceneController implementation

	//private methods
		private IEnumerator ChangeSceneAsync (int targetScene, float minimumWait = 0.0f)
		{
			//lock on a busy state to avoid stacked coroutines
			busy = true;

			//close the curtains
			ControllerCache.transitionController.state = false;

			//start song change
			ControllerCache.musicController?.PlaySceneSong(targetScene);

			Debug.Log("one");

			//wait until curtains are closed
			while (!ControllerCache.transitionController.StrictStateCheck(false))
			{ yield return null; }

			Debug.Log("two");

			//unload previous scene before deploying next
			AsyncOperation unloadingScene =	UnloadActiveScene();
			unloadingScene.allowSceneActivation = true;
			if (unloadingScene != null)
			{
				while (!unloadingScene.isDone) { yield return null; }
				Resources.UnloadUnusedAssets();
			}

			Debug.Log("three");

			//start loading next scene
			AsyncOperation loadingScene = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Additive);

			yield return new WaitForSeconds(minimumWait);
			while (!loadingScene.isDone) { yield return null; }

			Debug.Log("caramba");

			//once next scene is ready set it as active
			SetActiveScene(targetScene);

			//finally open the curtains and wait until they're done
			ControllerCache.transitionController.state = true;

			while (ControllerCache.transitionController.StrictStateCheck(false))
			{ yield return null; }

			busy = false;
		}

		private AsyncOperation UnloadActiveScene ()
		{
			if (SceneManager.GetActiveScene().buildIndex == SceneNumbers.TRANSITION)
			{
				Debug.LogWarning("Cannot unload curtain scene - ignoring request");
				return null;
			}
			return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
		}

		private void SetActiveScene (int targetScene)
		{
			SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(targetScene));
		}

	//ENDOF private methods
	}
}