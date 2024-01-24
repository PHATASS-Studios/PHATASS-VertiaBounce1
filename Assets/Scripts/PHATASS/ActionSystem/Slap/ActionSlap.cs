//using System.Collections.Generic;

using UnityEngine;

using ControllerCache = PHATASS.ControllerSystem.ControllerCache;
using EButtonInputState = PHATASS.InputSystem.EButtonInputState;

using ISettingsPackageActionSlap = PHATASS.SettingSystem.ISettingsPackageActionSlap;

namespace PHATASS.ActionSystem
{
	public class ActionSlap :
		ActionRadialBase<ISettingsPackageActionSlap>,
		IActionSlap
	{
	//ActionBase override implementation
		protected override bool ongoing { get { return (this.slapOngoing || base.ongoing); }}
		private bool slapOngoing {
			get { return this._slapOngoing; }
			set
			{
				this._slapOngoing = value;
				this.slapOngoingSetFrame = Time.frameCount;
			}
		}
		private bool _slapOngoing = false;
		private int slapOngoingSetFrame;

		//receive state of corresponding input medium
		protected override void Input (EButtonInputState state)
		{
			if (state == EButtonInputState.Ended)
			{
				this.PerformSlap();
			}
		}

		//interaction is always valid - slap is generally the fallback state if no other action is valid
		protected override bool IsValid ()
		{
			return true;
			//alternative implementation determining wether there were valid colliders in range
			//return ControllerCache.settingsProvider.slapAreaRadiusSetting.GetCollidersInRange(tool.transform).Length > 0;
		}

		protected override void ActionUpdate ()
		{
			if (this.slapOngoing && this.slapOngoingSetFrame != Time.frameCount)
			{ this.slapOngoing = false; }
		}

		//A slap is an entirely non-automatable one-shot action, so automation methods just report failure
		protected override bool Automate () { return false; }
		protected override void DeAutomate () {}
	//ENDOF ActionBase override implementation

	//private method implementation
		//execute the slapping action
		private void PerformSlap ()
		{
			ISlappable[] slappableList = this.GetComponentsInRange<ISlappable>();

			if (slappableList.Length > 0) { this.slapOngoing = true; }

			foreach (ISlappable slappable in slappableList)
			{
				this.ApplySlap(slappable);
			}
		}

		private void ApplySlap (ISlappable slappable)
		{
			slappable.PushFromPosition(
				originPosition: this.tool.position,
				pushForce: this.defaultSettings.forceSetting.value,
				fallOffCurve: null	//[TO-DO]: maybe add a fallOff curve from settings
			);
		}
/* OLD IMPLEMENTATION - REMOVE
			//fetch colliders in range around the tool
			Collider[] colliderList = this.defaultSettings.actionRadiusSetting
				.GetCollidersInRange(tool.transform);

			if (colliderList.Length > 0) { this.slapOngoing = true; }

			//add a force to each collider in range
			foreach (Collider collider in colliderList)
			{
				this.ApplySlapForce(collider.gameObject);
			}
		}

		//applies force to an individual element according to distance to tool
		private void ApplySlapForce (GameObject targetGameObject)
		{
			ISettingsPackageActionSlap slapSettings = ControllerCache.settingsProvider
				.GetSettingsPackage<ISettingsPackageActionSlap>(targetGameObject);

			Rigidbody targetRigidbody = targetGameObject.GetComponent<Rigidbody>();

			if(targetRigidbody != null) {
				targetRigidbody.AddExplosionForce(
					explosionForce: slapSettings.forceSetting.value,	//float
					explosionPosition: tool.transform.position,	//Vector3
					explosionRadius: slapSettings.actionRadiusSetting.radius,	//float
					upwardsModifier: 0.0f,	//float
					mode: slapSettings.forceSetting.mode	//ForceMode
				);
			}
		}
*/
	//ENDOF private method implementation
	}
}