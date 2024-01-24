using UnityEngine;
namespace PHATASS.Miscellaneous.Web
{
	public class OpenURLOnEventBehaviour : MonoBehaviour
	{
	//Serialized fields
		[SerializeField]
		private string destinationURL = "";
	//ENDOF Serialized fields

	//public events
		public void OpenURLEvent ()
		{
			if (string.IsNullOrEmpty(this.destinationURL))
			{
				Debug.LogWarning(this.name + " OpenURLOnEventBehaviour.destinationURL not set!!");
				return;
			}

			Application.OpenURL(this.destinationURL);
		}
	//ENDOF public events
	}
}