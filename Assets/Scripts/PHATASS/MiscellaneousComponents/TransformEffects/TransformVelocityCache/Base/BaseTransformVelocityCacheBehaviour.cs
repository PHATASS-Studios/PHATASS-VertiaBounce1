using UnityEngine;

using Values = PHATASS.Utils.Types.Values;
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
	public class BaseTransformVelocityCacheBehaviour :
		MonoBehaviour,
		IFloatValue,
		IVector2Value,
		IVector3Value
	{
	//serialized fields
	//ENDOF serialized fields

	//IFloatValue
		float Values.IValue<float>.value { get { return this.velocityMagnitude; }}
	//ENDOF IFloatValue

	//IVector2Value
		Vector2 Values.IValue<Vector2>.value { get { return (Vector2) this.velocity; }}
	//ENDOF IVector2Value

	//IVector3Value
		Vector3 Values.IValue<Vector3>.value { get { return this.velocity; }}
	//ENDOF IVector3Value

	//public properties
		[SerializeField]
		public Vector3 velocity
		{
			get { return this._velocity; }
			private set { this._velocity = value; }
		}
		private Vector3 _velocity;
	//endof properties

	//MonoBehaviour lifecycle
		private void Start ()
		{ this.previousPosition = this.transform.position; }
	//ENDOF MonoBehaviour

	//Inheritable members
		protected void RefreshVelocityCache ()
		{
			this.velocity = (this.transform.position - this.previousPosition) / Time.deltaTime;
			this.previousPosition = this.transform.position;
			//Debug.Log(this.velocity.ToString("F4"));
		}
	//ENDOF Inheritable members
		
	//private members
		private Vector3 previousPosition;

		private float velocityMagnitude { get { return this.velocity.magnitude; }}
	//ENDOF private
	}
}