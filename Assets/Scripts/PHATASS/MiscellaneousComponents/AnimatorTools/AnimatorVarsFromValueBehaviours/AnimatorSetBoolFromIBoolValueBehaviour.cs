using UnityEngine;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;
using IBoolValue = PHATASS.Utils.Types.Values.IBoolValue;

using SerializableAnimatorVariableIdentifier = PHATASS.Utils.Types.SerializableAnimatorVariableIdentifier;

namespace PHATASS.Miscellaneous.AnimatorTools
{
	public class AnimatorSetBoolFromIFloatValueBehaviour : MonoBehaviour
	{
	//serialized fields
		[Tooltip("Each of these animators will have their bool variable named as defined by boolVariableName set to the value of sourceValue.")]
		[SerializeField]
		private Animator[] managedAnimators;

		[Tooltip("This is the animator variable name that will be set to the value of sourceValue.")]
		[SerializeField]
		private SerializableAnimatorVariableIdentifier boolVariableName = "AnimatorVariableName";
		//private IAnimatorVariableIdentifier floatVariableName { get { return this._floatVariableName; }}

		[Tooltip("IBoolValue whose value is written to the desired animator variable.")]
		[SerializeField]
		[SerializedTypeRestriction(typeof(IBoolValue))]
		private UnityEngine.Object? _sourceValue = null;
		private IBoolValue sourceValue
		{ get {
			if (this._sourceValue == null) { return null; }
			else { return this._sourceValue as IBoolValue; }
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
				animator.SetBool(this.boolVariableName, this.sourceValue.value);
			}
		}
	//ENDOF private methods
	}
}