using UnityEngine;

namespace PHATASS.Miscellaneous
{
// Triggerable MonoBehaviour that sets the enabled state of several GameObjects when a public trigger method is called
	public class SetGameObjectsEnabledTriggerBehaviour : MonoBehaviour
	{
	//Serialized fields
		[Tooltip("These GameObject will be SetActive(TRUE) when TriggerSetGameObjectsEnabled() is triggered")]
		[SerializeField]
		private GameObject[] setEnabledGameObjects;

		[Tooltip("These GameObject will be SetActive(FALSE) when TriggerSetGameObjectsEnabled() is triggered")]
		[SerializeField]
		private GameObject[] setDisabledGameObjects;
	//ENDOF Serialized fields

	//Public events exposed for unity serialization
		public void TriggerSetGameObjectsEnabled ()
		{
			foreach (GameObject targetGameObject in this.setEnabledGameObjects)
			{ targetGameObject.SetActive(true); }

			foreach (GameObject targetGameObject in this.setEnabledGameObjects)
			{ targetGameObject.SetActive(true); }
		}
	//ENDOF Public events

	//private members
	//ENDOF private
	}
}
