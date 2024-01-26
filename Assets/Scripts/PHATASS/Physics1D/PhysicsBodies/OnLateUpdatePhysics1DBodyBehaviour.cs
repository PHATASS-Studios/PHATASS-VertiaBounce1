using UnityEngine;

namespace PHATASS.Utils.Physics.Physics1D
{
// FixedUpdate MonoBehaviour implementation of a 1D physics body as implemented in PHATASS.Utils.Physics
	public class OnLateUpdatePhysics1DBodyBehaviour : BasePhysics1DBodyBehaviour
	{
	//MonoBehaviour lifecycle
		private void LateUpdate ()
		{ this.PhysicsUpdate(Time.deltaTime); }
	//ENDOF MonoBehaviour lifecycle
	}
}