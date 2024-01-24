using UnityEngine;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;
using IFloatValue = PHATASS.Utils.Types.Values.IFloatValue;

using SerializableAnimatorVariableIdentifier = PHATASS.Utils.Types.SerializableAnimatorVariableIdentifier;

namespace PHATASS.Miscellaneous.AnimatorTools
{
	public class AnimatorSetFloatFromIFloatValueBehaviour : MonoBehaviour
	{
	//serialized fields
		[Tooltip("Each of these animators will have their float variable named as defined by floatVariableName set to the value of sourceValue.")]
		[SerializeField]
		private Animator[] managedAnimators;

		[Tooltip("This is the animator variable name that will be set to the value of sourceValue.")]
		[SerializeField]
		private SerializableAnimatorVariableIdentifier floatVariableName = "AnimatorVariableName";
		//private IAnimatorVariableIdentifier floatVariableName { get { return this._floatVariableName; }}

		[Tooltip("IFloatValue whose value is written to the desired animator variable.")]
		[SerializeField]
		[SerializedTypeRestriction(typeof(IFloatValue))]
		private UnityEngine.Object? _sourceValue = null;
		private IFloatValue sourceValue
		{ get {
			if (this._sourceValue == null) { return null; }
			else { return this._sourceValue as IFloatValue; }
		}}
	//ENDOF serialized fields

	//MonoBehaviour lifecycle
		private void LateUpdate ()
		{ this.ApplyValue(); }
	//ENDOF MonoBehaviour

	//private methods
		private void ApplyValue ()
		{
			if (this.managedAnimators == null || this.sourceValue == null || this.managedAnimators.Length == 0)
			{
			//	Debug.Log("AnimatorFloatFromIFloatValueBehaviour no linked animators or no source value.");
				return;
			}

			foreach (Animator animator in this.managedAnimators)
			{
				animator.SetFloat(this.floatVariableName, this.sourceValue.value);
			}
		}
	//ENDOF private methods
	}
}