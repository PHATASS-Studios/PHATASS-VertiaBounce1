using UnityEngine;

using static PHATASS.InputSystem.UnityInputSystemExtensions;

using Mouse = UnityEngine.InputSystem.Mouse;
using Keyboard = UnityEngine.InputSystem.Keyboard;
using ButtonControl = UnityEngine.InputSystem.Controls.ButtonControl;

using IViewportController = PHATASS.CameraSystem.IViewportController;
using ControllerCache = PHATASS.ControllerSystem.ControllerCache;

using IToolInputState = PHATASS.ToolSystem.IToolInputState;
using IInputMovementDelta = PHATASS.InputSystem.IInputMovementDelta;


namespace PHATASS.InputSystem
{
	//Requires package: com.unity.inputsystem and enabling InputSystemPackage input handling in project
	[DefaultExecutionOrder(-100)]//this needs to execute before ToolManager
	public class MouseInputControllerInputSystem :
		PHATASS.ControllerSystem.MonoBehaviourControllerBase<IInputController>,
		IMouseInputController
	{
	//const definitions
		private const int mouseButtonCount = 3;
		private const string mouseXAxisName = "Mouse X";
		private const string mouseYAxisName = "Mouse Y";
	//ENDOF const definitions

	//serialized fields
		[Tooltip("Base physical mouse movement delta multiplier. Configuration changes on delta scale apply on top of this.")]
		[SerializeField]
		private float baseMovementDeltaScale = 0.003f;
		private float movementDeltaScale { get { return this.baseMovementDeltaScale; }}

		[Tooltip("Base physical mouse scrollwheel delta multiplier. Configuration changes on delta scale apply on top of this.")]
		[SerializeField]
		private float baseScrollDeltaScale = 0.005f;
		private float scrollDeltaScale { get { return this.baseScrollDeltaScale; }}
	//ENDOF serialized fields

	//IMouseInputController
		//returns true if mouse button buttonID has been pressed this frame
		//[CONSIDER] Maybe remove GetButtonDown?
		bool IMouseInputController.GetButtonDown (int buttonID) { return this.GetButtonDown(buttonID); }
			private bool GetButtonDown (int buttonID)
			{ return this.GetButtonStateByID(buttonID) == EButtonInputState.Started; }

		//returns true if mouse button buttonID has been held for at least lenght seconds
		//If reset = true or omitted, resets held counter until lifted and pressed again
		bool IMouseInputController.GetButtonHeld (int buttonID, float length, bool reset) { return this.GetButtonHeld(buttonID, length, reset); }
			private bool GetButtonHeld (int buttonID, float length, bool reset)
			{
				if (this.buttonIsHeld[buttonID] && this.buttonHeldTimer[buttonID] >= length)
				{
					if (reset)
					{ this.FinishButtonHeld(buttonID); }

					return true;
				}
				return false;
			}
	//ENDOF IMouseInputController

	//IInputController
		//gets zoom input
		float IInputController.zoomDelta { get { return this.zoomDelta; }}
			private float zoomDelta { get { return -1 * (this.rawZoomDelta * this.scrollDeltaScale); }}
			private float rawZoomDelta { get { return this.mouse.scroll.y.ReadValue(); }}
					/* commented how to get scroll input through keyboard keys
					+ ((Input.GetKey(KeyCode.R))
						? (+ 0.1f)
						: (Input.GetKey(KeyCode.F))
							? (- 0.1f)
							: 0);
					*/

		//will return true if esc key or other quitting input is received
		EButtonInputState IInputController.quitButton
		{ get { return Keyboard.current.escapeKey.EGetButtonState(); }}
	//ENDOF IInputController

	//IToolInputState
		// state of primary input (left click, or touch lifecycle state)
		EButtonInputState IToolInputState.primaryInputState { get { return this.buttonState[0]; }}
	//ENDOF IToolInputState

	//IInputMovementDelta
		//input movement delta for last frame
		Vector2 IInputMovementDelta.rawDelta { get { return this.rawDelta; }}
			private Vector2 rawDelta
			{ get { return new Vector2(x: this.mouse.delta.x.ReadValue(), y: this.mouse.delta.y.ReadValue()); }}

		//delta, transformed into screen space
		Vector2 IInputMovementDelta.screenSpaceDelta { get { return this.screenSpaceDelta; }}
			private Vector2 screenSpaceDelta
			{ get { return ControllerCache.viewportController.ScreenSpaceToWorldSpace(this.rawDelta, worldSpace: false); }}

		//delta, scaled by sensitivity, screen size, and other factors
		Vector2 IInputMovementDelta.scaledDelta { get { return this.scaledDelta; }}
			private Vector2 scaledDelta { get { return this.rawDelta * this.movementDeltaScale * this.screenSizeFactor; }}
	//ENDOF IInputMovementDelta

	//MonoBehaviour lifecycle
		private void Start ()
		{
			this.LockCursor();
		}

		private void Update ()
		{
			this.UpdateButtonsState();
			this.UpdateButtonHeldTimers();
			this.LockCursorIfMouseClicked();
			//this.UpdateCursorLock();
		}
	//ENDOF MonoBehaviour lifecycle

	//private properties
		//managed mouse
		private Mouse mouse { get { return Mouse.current; }}

		// input multiplier based on the size of the screen
		private float screenSizeFactor { get { return ControllerCache.viewportController.size; }}
	//ENDOF private properties

	//private fields
		private EButtonInputState[] buttonState = new EButtonInputState[mouseButtonCount];
		private bool[] buttonIsHeld = new bool[mouseButtonCount];
		private float[] buttonHeldTimer = new float[mouseButtonCount];
	//ENDOF private fields

	//Private methods
		private void UpdateButtonsState ()
		{
			for (int i = 0, iLimit = this.buttonState.Length;  i < iLimit; i++)
			{
				this.buttonState[i] = this.GetButtonStateByID(i);
			}
		}

		// 0> Left click  1> Right click  2> Middle click
		private ButtonControl GetButtonByID (int buttonID)
		{
			if (buttonID == 0) { return this.mouse.leftButton; }
			else if (buttonID == 1) { return this.mouse.rightButton; }
			else if (buttonID == 2) { return this.mouse.middleButton; }	
			return this.mouse.leftButton; //return the left mouse button if outside valid range
		}

		private EButtonInputState GetButtonStateByID (int buttonID)
		{ return this.GetButtonByID(buttonID).EGetButtonState(); }

		private void UpdateButtonHeldTimers ()
		{
			for (int i = 0; i < mouseButtonCount; i++)
			{
				EButtonInputState state = this.GetButtonStateByID(i);
				if (state == EButtonInputState.Started)
				{ InitiateButtonHeld(i); }

				if (state == EButtonInputState.Held || state == EButtonInputState.Started)
				{ UpdateButtonHeld(i); }

				if (state == EButtonInputState.Ended || state == EButtonInputState.None)
				{ FinishButtonHeld(i); }
			}
		}

		private void InitiateButtonHeld (int buttonID)
		{
			this.buttonIsHeld[buttonID] = true;
			this.buttonHeldTimer[buttonID] = 0f;
		}

		private void UpdateButtonHeld (int buttonID)
		{
			if (this.buttonIsHeld[buttonID])
			{
				this.buttonHeldTimer[buttonID] += Time.deltaTime;
			}
		}

		private void FinishButtonHeld (int buttonID)
		{
			this.buttonIsHeld[buttonID] = false;
		}

		private void LockCursorIfMouseClicked ()
		{
			if (!CursorLocker.cursorLocked && (this.buttonState[0] != EButtonInputState.None || this.buttonState[1] != EButtonInputState.None))
			{
				this.LockCursor();
			}
		}

		private void LockCursor ()
		{
			CursorLocker.cursorLocked = true;
		}

		/*
		private void UpdateCursorLock ()
		{
			if (CursorLocker.cursorLocked)
			{
				this.mouse.WarpCursorPosition(new Vector2(x: Screen.width/2, y: Screen.height/2));
			}
		}
		//*/
	//ENDOF Private methods
	}
}