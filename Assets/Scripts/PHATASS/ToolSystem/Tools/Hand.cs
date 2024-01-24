using UnityEngine;

using IAction = PHATASS.ActionSystem.IAction;
using IActionUseInteractor = PHATASS.ActionSystem.IActionUseInteractor;

using ActionGrabElement = PHATASS.ActionSystem.ActionGrabElement;
using ActionGrabSurface = PHATASS.ActionSystem.ActionGrabSurface;
using ActionSlap = PHATASS.ActionSystem.ActionSlap;
using ActionUseInteractor = PHATASS.ActionSystem.ActionUseInteractor;


namespace PHATASS.ToolSystem.Tools
{
	public class Hand : ToolBase
	{
	//serialized fields
		[Tooltip("Click-and-release will result in a slap if held time is smaller than this value")]
		[SerializeField]
		private float maximumTimeHeldForSlap = 0.2f;
	//ENDOF serialized fields

	//private fields and properties
		private float inputHeldTime = 0.0f;
	//ENDOF private fields and proerties

	//MonoBehaviour Lifecycle implementation
	//ENDOF MonoBehaviour Lifecycle implementation
		
	//ToolBase implementation
		protected override void InputStarted ()
		{
			//upon first starting an input, try to determine if an special zone action is required
			//if not, try to initiate a grab.
			inputHeldTime = 0.0f;
			TryActions();
		}

		protected override void InputHeld ()
		{
			inputHeldTime += Time.deltaTime;
		}

		protected override void InputEnded ()
		{
			TrySlap();
		}
	//ENDOF ToolBase implementation

	//private method implementation
		//try possible actions in order
		private void TryActions ()
		{
			if (SetAction<ActionUseInteractor>()) return;
			if (SetAction<ActionGrabElement>()) return;
			if (SetAction<ActionGrabSurface>()) return;
		}

		//upon release perform a slap if input was held for short enough and previous action is not an ActionUseInteractor
		private void TrySlap ()
		{
			if (inputHeldTime <= this.maximumTimeHeldForSlap)
			{
				if ((action as IActionUseInteractor)?.IsValid() == true)
				{ return; }	//if previous action is a valid UseInteractor forgo slap
				SetAction<ActionSlap>();
				Debug.Log("Slappin'");
			}
		}
	//ENDOF private method implementation
	}
}

