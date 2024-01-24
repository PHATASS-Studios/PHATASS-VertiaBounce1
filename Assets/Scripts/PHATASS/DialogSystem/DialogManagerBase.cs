using UnityEngine;

using IEnumerator = System.Collections.IEnumerator;

using IDialogController = PHATASS.DialogSystem.DialogControllers.IDialogController;
using IToggleable = PHATASS.Utils.Types.Toggleables.IToggleable;

namespace PHATASS.DialogSystem
{
	public class DialogManagerBase : MonoBehaviour, IDialogManager
	{
		//static namespace
			//[TO-DO] this is unused and singletons are bad. remove?
			//make this inherit from MonoBehaviourControllerBase OR make it a non-global controller?
			//public static DialogManagerBase instance;
		//ENDOF static namespace

		//serialized fields
		//ENDOF serialized fields

		//private fields and properties
			private IDialogController waitingDialog = null;
			private IDialogController activeDialog;
			//private int dialogIndex;

			private IToggleable activePortrait = null;
		//ENDOF private fields and properties

		//MonoBehaviour lifecycle
			//public void Awake ()
			//{	instance = this; }

			public void Start ()
			{
				ResetDialogs();
			}
		//ENDOF MonoBehaviour lifecycle

		//IDialogManager implementation
			public void SetActiveDialog (IDialogController targetDialog)
			{
				//Debug.Log("DialogManagerBase.SetActiveDialog()");
				//if already changing dialogs ignore change request
				if (waitingDialog != null) { return; }

				//also ignore change request if targetDialog is already active
				if (targetDialog == this.activeDialog) { return; }

				//check if we need to activate a different portrait for the next dialog
				if (targetDialog == null || targetDialog.portrait != this.activePortrait)
				{ this.UnSetPortrait(); }

				waitingDialog = targetDialog;
				//if there's another dialog active, request it closes
				if (activeDialog != null)
				{
					activeDialog.TransitionStateWithCallback(false, DelegateOpenNextDialog);
				}
				else	//if no dialog already active just open target dialog
				{
					DelegateOpenNextDialog();
				}

			}
		//ENDOF IDialogManager implementation

		//private method definition
			private void ResetDialogs ()
			{
				waitingDialog = null;
				activeDialog = null;
				IDialogController[] dialogList = GetComponentsInChildren<IDialogController>();
				foreach (IDialogController dialog in dialogList)
				{
					dialog.ForceSetState(false);
				}
			}

			private void DelegateOpenNextDialog ()
			{
				if (this.waitingDialog == null)
				{ this.activeDialog = null; }
				else
				{ StartCoroutine(DelegateOpenNextDialogCoroutine()); }

				IEnumerator DelegateOpenNextDialogCoroutine ()
				{
					//one-frame delay introduced to guarantee next dialog's animator has a chance to resize the panel before frame
					yield return new WaitForEndOfFrame();

					activeDialog = waitingDialog;
					if (waitingDialog != null)
					{
						this.waitingDialog.state = true;
						this.SetPortrait(this.waitingDialog.portrait);
						this.waitingDialog = null;
					}
				}
			}

			private void UnSetPortrait ()
			{
				if (this.activePortrait != null)
				{
					this.activePortrait.state = false;
					this.activePortrait = null;
				}
			}

			private void SetPortrait (IToggleable portrait)
			{
				this.UnSetPortrait();
				if (portrait != null)
				{
					portrait.state = true;
					this.activePortrait = portrait;
				}
			}
		//ENDOF private method definition
	}
}