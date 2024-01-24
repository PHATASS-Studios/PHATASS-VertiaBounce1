using UnityEngine;

using CubismParameter = Live2D.Cubism.Core.CubismParameter;

using static PHATASS.Utils.Extensions.CubismParameterExtensions;

namespace PHATASS.L2DTools
{
//Component handling setting and getting an L2D parameter value
// Constantly sets the value from a serialized field
//	Meant to be used with animators
	[DefaultExecutionOrder(-10000)]
	public class FromSerializedL2DParameterHandler :
		SimpleL2DParameterHandler
	{
	//Serialized fields
		[Tooltip("The value of he managed cubismParameter will be set on every LateUpdate to this serialized value. Mainly meant to be used with Animators.")]
		[SerializeField]
		private float desiredValue;

		[Tooltip("If true, desiredValue will be used as a normalized (0.0f-1.0f) representation of the full possible range of the parameter")]
		[SerializeField]
		private bool asNormalized = true;
	//ENDOF Serialized fields


	//MonoBehaviour lifecycle
		private void LateUpdate ()
		{
			if (this.asNormalized)
			{ this.normalizedValue = this.desiredValue; }
			else
			{ this.absoluteValue = this.desiredValue; }
		}
	//ENDOF MonoBehaviour lifecycle
	}
}