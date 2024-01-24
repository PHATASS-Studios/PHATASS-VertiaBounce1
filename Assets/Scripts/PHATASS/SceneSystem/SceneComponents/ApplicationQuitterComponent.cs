using UnityEngine;

using CursorLocker = PHATASS.InputSystem.CursorLocker;

using ControllerCache = PHATASS.ControllerSystem.ControllerCache;

using IInputController = PHATASS.InputSystem.IInputController;
using EButtonInputState = PHATASS.InputSystem.EButtonInputState;

namespace PHATASS.SceneSystem
{
	//this component tries to free the user from the APP: Either close the app or release the cursor
	public class ApplicationQuitterComponent : MonoBehaviour
	{
	//Serialized fields
		[SerializeField]
		private Animator animator;

		[Tooltip("Name of the bool animator variable to set to true to deploy warning panel")]
		[SerializeField]
		private string shownAnimatorBoolName = "Shown";

		[Tooltip("Time the warning panel stays open - during this time pressing ESC again releases the cursor")]
		[SerializeField]
		private float warningPanelOpenTime = 2f;
	//ENDOF Serialized

	//private fields
		private float panelShownTimer = 0f;
	//ENDOF private fields

	//private properties
		private bool panelShown
		{
			get { return this._panelShown; }
			set
			{
				if (this._panelShown != value)
				{
					this.animator.SetBool(this.shownAnimatorBoolName, value);
					this._panelShown = value;
				}
				if (value == true)
				{ this.panelShownTimer = this.warningPanelOpenTime; }
			}
		}
		private bool _panelShown = false;
	//ENDOF private properties

	//Base implementation
		private void Awake ()
		{
			if (this.animator == null) { this.animator = this.GetComponent<Animator>(); }

			//init animator state
			this.animator.SetBool(this.shownAnimatorBoolName, this.panelShown);
		}

		private void Update ()
		{
			if (this.panelShown)
			{
				this.panelShownTimer -= Time.deltaTime;
				if (this.panelShownTimer <= 0)
				{ this.panelShown = false; }
			}

	#if UNITY_WEBGL
			if (ControllerCache.inputController.quitButton == EButtonInputState.Held)
	#else
			if (ControllerCache.inputController.quitButton == EButtonInputState.Started)
	#endif
			{

				if(!this.panelShown)
				{
					this.panelShown = true;
					this.FirstQuitAttempt();
				}
				else
				{
					this.QuitConfirmed();
				}	
			}
		}
	//ENDOF Base implementation

	//Generic implementation
	//ENDOF generic implementation

	//WEBGL version implementation
	#if UNITY_WEBGL
		private void FirstQuitAttempt () { CursorLocker.cursorLocked = false; }

		private void QuitConfirmed () { CursorLocker.cursorLocked = false; }
	//ENDOF WEBGL

	//fallback definition
	#else
		private void FirstQuitAttempt () { CursorLocker.cursorLocked = false; }

		private void QuitConfirmed () { Application.Quit(); }
	#endif
	//ENDOF fallback
	}
}