using UnityEngine;

using static PHATASS.Utils.Extensions.FloatExtensions;
using static PHATASS.Utils.Extensions.AnimatorListExtensions;

using IFloatValue = PHATASS.Utils.Types.Values.IFloatValue;
using IFloatValueMutable = PHATASS.Utils.Types.Values.IFloatValueMutable;

using SerializableAnimatorVariableIdentifier = PHATASS.Utils.Types.SerializableAnimatorVariableIdentifier;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

namespace PHATASS.CameraSystem.CameraFX
{
	public class CameraBlurAndRefocusOnZoomBehaviour :
		MonoBehaviour
	{
	//serialized
		[Tooltip("Source IFloatValue representing current frame's zoom delta")]
		[SerializeField]
		[SerializedTypeRestriction(typeof(IFloatValue))]
		private UnityEngine.Object? _zoomDeltaValue = null;
		private IFloatValue zoomDeltaValue
		{ get {
			if (this._zoomDeltaValue == null) { return null; }
			else { return this._zoomDeltaValue as IFloatValue; }
		}}

		[Tooltip("Zoom delta multiplier. Zoom change will be scaled by this value.")]
		[SerializeField]
		private float focusLossMultiplier = 2.0f;

		[Tooltip("Focus loss hard limit. Focus loss will never go above this value, in the positive or negative.")]
		[SerializeField]
		private float focusLossLimit = 20f;

		[Tooltip("Receiver IFloatValueMutable meant to receive the current blur intensity value")]
		[SerializeField]
		[SerializedTypeRestriction(typeof(IFloatValueMutable))]
		private UnityEngine.Object? _blurValueReceiver = null;
		private IFloatValueMutable blurValueReceiver
		{ get {
			if (this._blurValueReceiver == null) { return null; }
			else { return this._blurValueReceiver as IFloatValueMutable; }
		}}

		[Tooltip("Delay in seconds before Auto-Focus kicks in")]
		[SerializeField]
		private float autoFocusDelay = 0.1f;

		[Tooltip("Focus regain per second during auto-focus")]
		[SerializeField]
		private float autoFocusSpeed = 20f;

		/*
		[Tooltip("Animator linked to autofocus")]
		//*/
		[Tooltip("Animators linked to autofocus.")]
		[SerializeField]
		protected Animator[] animators;

		[Tooltip("Animator bool set to true/false when actively focusing in.")]
		[SerializeField]
		private SerializableAnimatorVariableIdentifier isFocusingAnimatorBool = "IsFocusing";
	//ENDOF serialized

	//MonoBehaviour lifecycle
		private void LateUpdate ()
		{
			this.UpdateFocus();
		}
	//ENDOF MonoBehaviour

	//private members
		private float currentBlur = 0f;
		private float autoFocusWaitTimer = 0f;

		private float currentFocusLoss
		{ get { return this.zoomDeltaValue.value * this.focusLossMultiplier; }}

		private float currentFocusRegain
		{ get { return this.autoFocusSpeed * Time.deltaTime; }}

		private bool autoFocusIsActive
		{
			get
			{
				return (this.autoFocusWaitTimer <= 0f && this.blurValueReceiver.value != 0);
			}
		}

		private bool isFocusingAnimatorStatus = false;

		private void UpdateFocus ()
		{
			if (this.zoomDeltaValue.value != 0f)
			{
				this.autoFocusWaitTimer = autoFocusDelay;
				currentBlur += currentFocusLoss;
			}
			else if (autoFocusWaitTimer >= 0f)
			{ this.autoFocusWaitTimer -= Time.deltaTime; }

			if (this.autoFocusIsActive)
			{
				currentBlur = currentBlur.EStepTowards(target: 0f, step: currentFocusRegain);
				this.SetAnimatorState(true);
			}
			else
			{ this.SetAnimatorState(false); }


			this.ClampBlur();	
			this.ApplyBlur();
		}

		private void ClampBlur ()
		{
			if (this.currentBlur > this.focusLossLimit)
			{ this.currentBlur = this.focusLossLimit; }
			else if (this.currentBlur < (this.focusLossLimit * -1))
			{ this.currentBlur = this.focusLossLimit * -1; }
		}

		private void ApplyBlur ()
		{
			this.blurValueReceiver.value = this.currentBlur;
		}

		private void SetAnimatorState (bool state)
		{
			if (state == this.isFocusingAnimatorStatus) { return; }

			this.animators.ESetBool(varName: this.isFocusingAnimatorBool, value: state);
			this.isFocusingAnimatorStatus = state;
		}
	//ENDOF private
	}

}