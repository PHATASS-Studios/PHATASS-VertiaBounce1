using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

using static PHATASS.Utils.Enumerables.TransformEnumerables;

using ControllerCache = PHATASS.ControllerSystem.ControllerCache;

using UnityEngine;

namespace PHATASS.SettingSystem
{
	[System.Serializable]
	public class MBSettingsPackageReferenceAllChildren
	:	MBSettingsPackageReference
	{
	//private fields
		[Tooltip("If this is true, THIS GameObject will be included. Otherwise only all children will be included.")]
		[UnityEngine.SerializeField]
		private bool includeThisTransform;
	//ENDOF private fields

	//Overrides
		protected override void Awake ()
		{
			foreach (Transform transform in this.transform.EToRecursiveChildrenEnumerable(this.includeThisTransform))
			{
				this.ApplySettingsPackagesForContext(transform.gameObject);
			}
		}
	//ENDOF Overrides
	}
}