using UnityEngine;

namespace PHATASS.Miscellaneous.Kickers
{
	public class OnOffFlickerGameObjectManual : OnOffFlickerManualActivationBase
	{
	//private fields and properties
		[SerializeField]
		private GameObject targetGameObject = null;
	//ENDOF private fields and properties

	//inherited abstract property implementation
		protected override bool state
		{
			set
			{
				if (targetGameObject.activeSelf != value)
				{ targetGameObject.SetActive(value); }
			}
		}
	//ENDOF inherited abstract property implementation
	}
}