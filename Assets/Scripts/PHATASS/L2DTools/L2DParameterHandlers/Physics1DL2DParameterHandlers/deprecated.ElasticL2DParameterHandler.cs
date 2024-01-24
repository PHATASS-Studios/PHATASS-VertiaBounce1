/*//[DEPRECATED] Kept only for temporary reference, remove this whenever=======================================================

using UnityEngine;

using static PHATASS.Utils.Extensions.FloatExtensions;

using IPhysicsBody1D = PHATASS.Utils.Physics.Physics1D.IPhysicsBody1D;
using IForceReceiver1D = PHATASS.Utils.Physics.Physics1D.IForceReceiver1D;
using IKineticBody1D = PHATASS.Utils.Physics.Physics1D.IKineticBody1D;

using RandomFloatRange = PHATASS.Utils.Types.Ranges.RandomFloatRange;
using IFloatRange = PHATASS.Utils.Types.Ranges.IFloatRange;

//using CubismParameter = Live2D.Cubism.Core.CubismParameter;

//using static PHATASS.Utils.Extensions.CubismParameterExtensions;


namespace PHATASS.L2DTools
{
// This component handles setting/getting an L2D parameter value, using 1D physics momentum
// [CONSIDER]: Maybe moving physics implementation to a non-monobehaviour struct or class that can be used to handle 1D physics separately
//	[DefaultExecutionOrder(10000)]
	public class ElasticL2DParameterHandler :
		SimpleL2DParameterHandler,
		IPhysicsBody1D
	{
	//Serialized fields
		[Tooltip("Spring dead zone. Parameter values between the upper and lower dead zone will not activate the restitution spring. Default: 0f.")]
		[SerializeField]
		private RandomFloatRange _springDeadZone = 0f;
		private IFloatRange springDeadZone { get { return this._springDeadZone; }}

		[Tooltip("Inertial mass of this parameter. MUST NOT BE ZERO. This dictates how much applied forces alter this parameter's momentum. Default: 1f")]
		[SerializeField]
		private float parameterInertialMass = 1f;

		[Tooltip("Restitution spring force. Spring will pull back towards the center as long as the parameter's value is outside from the dead zone. Formula: springForce = distanceFromDeadZone * restitutionSpring")]
		[SerializeField]
		private float restitutionSpring = 1f;

		[Tooltip("Dampener spring force. Spring will push against this body's momentum the closer it is to the dead zone. Formula: dampenerForce = dampenerSpring * momentum.sign * -1)")]
		[SerializeField]
		private float dampenerSpring = 1f;

		[Tooltip("Bounciness of the edges. Reaching an edge bounces the parameter back with this proportion of the original force. 0f: no bounce, 1f: full force bounce.")]
		[SerializeField]
		private float edgeBounciness = 0f;



		[Tooltip("Sleep threshold - when momentum magnitude is smaller than the body's momentum is removed. Default 1e-19.")]
		[SerializeField]
		private float sleepThreshold = 0.0000000000000000001f;

		[Tooltip("Movement-damping force. Slows current momentum by a fraction of its value. Formula: dragForce = momentum * dragRate * -1f")]
		[SerializeField]
		private float dragRate = 0.05f;
	//ENDOF Serialized fields

	//IPhysicsBody1D
	//IForceReceiver1D
		//Adds a 1D force/momentum to the body, with a force & direction depending on force value and sign.
		void IForceReceiver1D.AddForce (float force) { this.AddForce(force); }
	//ENDOF IForceReceiver1D

	//IKineticBody1D
		//momentum in this body. Sign indicates direction.
		float IKineticBody1D.momentum { get { return this.momentum; } set { this.momentum = value; }}
	//ENDOF IKineticBody1D
	//ENDOF IPhysicsBody1D

	//MonoBehaviour
		private void Start ()
		{
			if (this.parameterInertialMass == 0f)
			{
				Debug.LogWarning(this.name + ".ElasticL2DParameterHandler.parameterInertialMass MUST NOT BE ZERO.");
				this.parameterInertialMass = 0.01f;
			}
		}
		private void LateUpdate ()
		{
			this.PhysicsUpdate();
		}
	//ENDOF MonoBehaviour

	//protected class members
		// Current 1D physics body linear momentum.
		[SerializeField]
		protected float momentum; // { get; set; }	//left as auto property for simplicity, offer explicit implementation if necessary
		protected float velocity { get { return this.momentum / this.parameterInertialMass; }}

		// Force of the restitution spring
		protected float restitutionSpringForce
		{ get { return this.distanceFromDeadZone * this.restitutionSpring * -1f; }}

		// Force loss to drag
		protected float dragForce
		{ get { return this.momentum * this.dragRate * -1f; }}


		protected void AddForce (float force)
		{ this.momentum += force; }
	//ENDOF protected class members

	//private members
		private float distanceFromDeadZone
		{ get { return this.springDeadZone.DistanceFromRange(this.absoluteValue, true); }}

		private void PhysicsUpdate ()
		{
			//First apply the restitution spring
			this.AddForce(this.restitutionSpringForce * Time.deltaTime);
			
			//check for sleeping conditions. If body must sleep, abort the rest of the physics update
			if (this.IsAsleep())
			{
				this.Sleep();
				return;
			}
			
			//move the object's value by accumulated force
			this.ApplyMomentum();

			//Finally apply the force drag and damping
			this.AddForce(this.dragForce * Time.deltaTime);	//drag
			AddDamping();

			//calculates and adds the damping force
			void AddDamping ()
			{
				float dampenerForce = this.dampenerSpring * Time.deltaTime;

				//force set momentum to 0 if dampener is stronger than momentum to avoid infinitesimally small residual momentum caused by rounding errors
				if (dampenerForce >= this.momentum.EAbs()) { this.momentum = 0f; }
				//otherwise, apply the dampener in momentum's opposite direction
				else { this.AddForce(dampenerForce * (momentum.ESign() * -1f)); }
			}
		}


		// Applies one frame of movement according to frame time and accumulated momentum
		private void ApplyMomentum ()
		{

			float nextValue = this.absoluteValue + (this.velocity * Time.deltaTime);

			// detect if edges are surpassed, and reset accumulated momentum to the correct direction 
			if (nextValue <= this.minimumValue)
			{
				nextValue = this.minimumValue;
				this.momentum = this.momentum.EAbs() * edgeBounciness * +1f;
			}
			else if (nextValue >= this.maximumValue)
			{
				nextValue = this.maximumValue;
				this.momentum = this.momentum.EAbs() * edgeBounciness * -1f;
			}

			this.unclampedValue = nextValue; //for efficiency, physics update uses a value setter that skips value clamping
		}

		private bool IsAsleep ()
		{
			return this.momentum.EAbs() < this.sleepThreshold;
		}

		private void Sleep ()
		{ this.momentum = 0f; }
	//ENDOF private members
	}
}
//*/