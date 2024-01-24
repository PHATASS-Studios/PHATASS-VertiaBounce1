using UnityEngine;

using SerializableAnimatorVariableIdentifier = PHATASS.Utils.Types.SerializableAnimatorVariableIdentifier;

namespace PHATASS.ToolSystem.Tools
{
	//on awake, sets the animator horizontal flip to true or false, alternatively
	[RequireComponent(typeof(Animator))]
	public class ToolFlip : MonoBehaviour
	{
	//serialized fields and properties
		[Tooltip("On Start, this animator bool is set to true or false, in a globally alternating fashion.")]
		[SerializeField]
		private SerializableAnimatorVariableIdentifier horizontalFlipAnimatorBool = "HorizontalFlip";
	//serialized fields and properties

	//static space
		private static int flipCounter;
		//returns false, then true, then false, then true...
		private static bool Flip ()
		{
			return (flipCounter++) % 2 > 0;
		}
	//ENDOF static space

	//instance implementation
		private Animator animator;
		private void Awake ()
		{
			this.animator = this.GetComponent<Animator>();
		} 

		private void Start ()
		{
			this.animator.SetBool(this.horizontalFlipAnimatorBool, ToolFlip.Flip());
			Destroy(this);
		}
	//ENDOF instance implementation
	}
}