using UnityEngine;

namespace PHATASS.InteractableSystem
{
	//button that stays locked on after activation
	public class InteractableTriggerLockOnUse : InteractableTriggerOnRelease
	{
	//private fields and properties
		private bool lockedOn = false;
		protected override bool hovered
		{
			get { return (this.lockedOn || base.hovered); }
			set { base.hovered = (this.lockedOn || value); }
		}
		protected override bool pressed
		{
			get { return (this.lockedOn || base.pressed); }
			set { base.pressed = (this.lockedOn || value); }
		}
	//ENDOF private fields and properties

	//overrides implementation
		protected override void InteractableTriggered ()
		{
			if (this.lockedOn) { return; }
			this.lockedOn = true;
			base.InteractableTriggered();
		}
	//overrides implementation
	}
}