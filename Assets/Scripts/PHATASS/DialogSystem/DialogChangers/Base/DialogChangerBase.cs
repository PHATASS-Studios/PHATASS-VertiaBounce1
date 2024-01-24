using UnityEngine;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

using IDialogController = PHATASS.DialogSystem.DialogControllers.IDialogController;
using IDialogManager = PHATASS.DialogSystem.IDialogManager;

namespace PHATASS.DialogSystem.DialogChangers
{
	public class DialogChangerBase : MonoBehaviour
	{
	//serialized fields
		[Tooltip("IDialogController to activate on call")]
		[SerializeField]
		[SerializedTypeRestriction(typeof(IDialogController))]
		private UnityEngine.Object? _defaultTargetDialog = null;
		private IDialogController defaultTargetDialog
		{ get {
			if (this._defaultTargetDialog == null) { return null; }
			else { return this._defaultTargetDialog as IDialogController; }
		}}
		//old implementation: protected PHATASS.DialogSystem.DialogControllers.DialogControllerBase defaultTargetDialog = null;

		[Tooltip("IDialogManager which should receive the dialog change request.")]
		[SerializeField]
		[SerializedTypeRestriction(typeof(IDialogManager))]
		private UnityEngine.Object? _dialogManager = null;
		private IDialogManager dialogManager
		{ get {
			if (this._dialogManager == null) { return null; }
			else { return this._dialogManager as IDialogManager; }
		}}

		[Tooltip("Delay in seconds to wait before performing the dialog change when it is requested.")]
		[SerializeField]
		private float delay = 0.0f;

		[Tooltip("If this is true, triggering this again while this has already been triggered and waiting for delay, delay timer will be reset. Does nothing if delay is 0f.")]
		[SerializeField]
		private bool resetTimerOnActivation = true;
	//ENDOF serialized fields

	//public events
		//requests a dialogManager to activate target dialog
		public void ChangeDialog () { this.ChangeDialog(this.defaultTargetDialog); }
		public void ChangeDialog (IDialogController dialog)
		{
			//Debug.Log("Changing dialog");
			if (this.delay <= 0){ this.DoChangeDialog(dialog); }
			else { this.DelayedDialogChange(dialog); }
		}
	//ENDOF public events methods

	//MonoBehaviour lifecycle
		private void Update ()
		{
			this.CountDownTimer();
		}
	//ENDOF MonoBehaviour

	//private members
		private float timer = 0f;

		private IDialogController queuedDialog;

		//if there is a timer enabled, count down and check it
		private void CountDownTimer ()
		{
			if (this.timer <= 0f)
			{ return; }

			this.timer -= Time.deltaTime;
			if (this.timer <= 0f)
			{ this.DoChangeDialog(this.queuedDialog); }
		}

		private void DelayedDialogChange (IDialogController dialog)
		{
			//if timer's running and either reset is enabled or a different dialog change is requested, do a dialog change anyway
			if (this.timer > 0f
			&& (!this.resetTimerOnActivation && dialog == this.queuedDialog))
			{ 
				return;
			}

			this.timer = this.delay;
			this.queuedDialog = dialog;
		}

		private void DoChangeDialog (IDialogController dialog)
		{
			this.dialogManager.SetActiveDialog(dialog);
		}
	//ENDOF private
	}
}