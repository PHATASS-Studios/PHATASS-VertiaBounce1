using UnityEngine;

using SerializableAnimatorVariableIdentifier = PHATASS.Utils.Types.SerializableAnimatorVariableIdentifier;

namespace PHATASS.SceneSystem.TransitionSystem
{
	public class CurtainTransitionController :
	//[TO-DO]: Actually inherit from AnimatorBasedTransitionController
		AnimatorBasedTransitionController, ///PHATASS.ControllerSystem.MonoBehaviourControllerBase <ITransitionController>,
		ITransitionController
	{
	//serialized fields
		[SerializeField]
		private GameObject spotlightContainer = null;

		[SerializeField]
		private Transform rightSheetUpperNode = null;
		[SerializeField]
		private Transform leftSheetUpperNode = null;
		[SerializeField]
		private Transform rightSheetLowerNode = null;
		[SerializeField]
		private Transform leftSheetLowerNode = null;
	//ENDOF serialized field

	//MonoBehaviour overrides
	//ENDOF MonoBehaviour

	//overrides
		// Curtain-specific transition controller additionally checks that all curtain sheet nodes are physically covering each other 
		protected override bool StrictStateCheck (bool requiredState)
		{

			// If checking for closed, check if both upper and lower nodes are beyond eachother instead
			if (!requiredState)
			{
				return
					this.rightSheetLowerNode.position.x < this.leftSheetLowerNode.position.x &&
					this.rightSheetUpperNode.position.x < this.leftSheetUpperNode.position.x;
			}
			return base.StrictStateCheck(requiredState);
		}
	//ENDOF private
	}
}