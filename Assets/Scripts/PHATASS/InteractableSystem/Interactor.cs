using UnityEngine;

using ControllerCache = PHATASS.ControllerSystem.ControllerCache;

using EButtonInputState = PHATASS.InputSystem.EButtonInputState;

using ISettingsPackageActionUseInteractor = PHATASS.SettingSystem.ISettingsPackageActionUseInteractor;

namespace PHATASS.InteractableSystem
{
	public class Interactor : MonoBehaviour, IInteractor
	{
	//IInteractor implementation
		//process input. returns true if an interactable is in range
		bool IInteractor.Input (EButtonInputState state) { return this.Input(state); }
		private bool Input (EButtonInputState state)
		{
			IInteractable interactable = this.FindInteractable();
			//Debug.Log (" > interactor input: " + state + " " + interactable);
			interactable?.Interact(state);
			return (interactable != null);
		}

		//find if hovering over a valid interactable
		bool IInteractor.IsHovering () { return this.IsHovering(); }//{ return this.IsHovering(); }
		private bool IsHovering ()
		{ return (this.FindInteractable() != null); }

		//propagate hovering call to IInteractable under the interactor
		//returns true if an IInteractable was found
		bool IInteractor.PropagateHover () { return this.PropagateHover(); }
		private bool PropagateHover ()
		{
			IInteractable interactable = this.FindInteractable();
			interactable?.Hovered();
			return (interactable != null);
		}
	//ENDOF IInteractor implementation

	//private fields
		private ISettingsPackageActionUseInteractor defaultSettings;
	//ENDOF private fields

	//MonoBehaviour lifecycle
		private void Start ()
		{
			defaultSettings = ControllerCache.settingsProvider
				.GetSettingsPackage<ISettingsPackageActionUseInteractor>();
		}
	//ENDOF MonoBehaviour lifecycle

	//private methods
		//finds one interactable around this interactor's tool transform
		private IInteractable FindInteractable ()
		{
			IInteractable[] interactableArray = defaultSettings.actionRadiusSetting
				.GetComponentsInRangeByPriority<IInteractable>(transform);

			return (interactableArray.Length > 0)
				? interactableArray[0]
				: null;
		}
	//ENDOF private methods
	}
}