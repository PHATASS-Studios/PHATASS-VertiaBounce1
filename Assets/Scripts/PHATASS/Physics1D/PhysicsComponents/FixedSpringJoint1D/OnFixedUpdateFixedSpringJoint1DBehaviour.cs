using UnityEngine;

namespace PHATASS.Utils.Physics.Physics1D
{
// FixedUpdate MonoBehaviour implementation of a 1D physics body as implemented in PHATASS.Utils.Physics
	public class OnFixedUpdateFixedSpringJoint1DBehaviour : BaseFixedSpringJoint1DBehaviour
	{
	//MonoBehaviour lifecycle
		private void FixedUpdate ()
		{ this.PhysicsUpdate(Time.fixedDeltaTime); }
	//ENDOF MonoBehaviour lifecycle
	}
}