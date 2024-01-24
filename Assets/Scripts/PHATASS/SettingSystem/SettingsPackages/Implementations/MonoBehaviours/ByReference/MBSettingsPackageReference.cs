using System.Collections.Generic;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

using ControllerCache = PHATASS.ControllerSystem.ControllerCache;

using UnityEngine;

namespace PHATASS.SettingSystem
{
	[System.Serializable]
	public class MBSettingsPackageReference
		//<TPackageClass, TPackageInterface>
	:	UnityEngine.MonoBehaviour
	{
	//private fields
		[Tooltip("List of settings packages to assign to objects")]
		[UnityEngine.SerializeField]
		[SerializedTypeRestriction(typeof (ISettingsPackage))]
		private List<UnityEngine.Object> _settingsPackageList = null;
		private IList<ISettingsPackage> _settingsPackageListAccessor = null;
		private IList<ISettingsPackage> settingsPackageList
		{
			get
			{
				//create accessor if unavailable
				if (this._settingsPackageListAccessor == null && this._settingsPackageList != null)
				{
					this._settingsPackageListAccessor =
						new PHATASS.Utils.Types.Wrappers.UnityObjectListCastedAccessor<ISettingsPackage>(this._settingsPackageList);
				}

				return this._settingsPackageListAccessor;
			}
		}

	//ENDOF private fields

	//MonoBehaviour lifecycle
		protected virtual void Awake ()
		{

			this.ApplySettingsPackagesForContext(this.gameObject);
		}
	//ENDOF MonoBehaviour lifecycle

	//private methods
		protected void ApplySettingsPackagesForContext (GameObject context)
		{
			foreach (ISettingsPackage settingsPackage in this.settingsPackageList)
			{
				if (settingsPackage == null)
				{
					UnityEngine.Debug.LogWarning(string.Format(
						"MBSettingsPackageReference \"{0}\" settings package un-initialized - or wrong type",
						this.gameObject.name
					));
				}
				else
				{
					this.RegisterSettingsPackageForContext(
						package: settingsPackage,
						context: context
					);
				}
			}
		}

		private void RegisterSettingsPackageForContext (ISettingsPackage package, GameObject context)
		{
			ControllerCache.settingsProvider.RegisterSettingsPackage(
				package: package,
				context: context
			);

		}
	//ENDOF private methods
	}
}