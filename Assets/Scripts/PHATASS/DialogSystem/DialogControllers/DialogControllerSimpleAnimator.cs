using UnityEngine;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

using IToggleable = PHATASS.Utils.Types.Toggleables.IToggleable;

namespace PHATASS.DialogSystem.DialogControllers
{
	public class DialogControllerSimpleAnimator :
		PHATASS.Utils.Types.Toggleables.BaseAnimatorToggleableBehaviour,
		IDialogController
	{
	//serialized fields
		[Tooltip("Portrait enabled while this dialog is active.")]
		[SerializeField]
		[SerializedTypeRestriction(typeof(IToggleable))]
		private UnityEngine.Object? _portrait = null;
		private IToggleable portrait
		{ get {
			if (this._portrait == null) { return null; }
			else { return this._portrait as IToggleable; }
		}}
	//ENDOF serialized

	//IDialogController
			IToggleable IDialogController.portrait { get { return this.portrait; }}
	//ENDOF IDialogController
	}
}