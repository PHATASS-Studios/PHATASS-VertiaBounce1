using UnityEngine;

using IFloatValue = PHATASS.Utils.Types.Values.IFloatValue;
using IVector2Value = PHATASS.Utils.Types.Values.IVector2Value;
using IVector3Value = PHATASS.Utils.Types.Values.IVector3Value;

namespace PHATASS.Miscellaneous.TransformEffects
{
	//this component exposes this transform's velocity over time in M/S
	// exposed interfaces behaviour
	//		IFloatValue		> exposes velocity magnitude (forward speed)
	//		IVector3Value	> exposes velocity on each axis as a Vector3
	//		IVector2Value	> exposes velocity as a Vector2, removing Z axis
	public class OnLateUpdateTransformVelocityCacheBehaviour : BaseTransformVelocityCacheBehaviour
	{
	//MonoBehaviour lifecycle
		private void Update ()
		{ this.RefreshVelocityCache(); }
	//ENDOF MonoBehaviour
	}
}