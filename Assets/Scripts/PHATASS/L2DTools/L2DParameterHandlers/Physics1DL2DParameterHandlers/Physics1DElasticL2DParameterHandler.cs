using UnityEngine;

//using static PHATASS.Utils.Extensions.FloatExtensions;

using Physics = PHATASS.Utils.Physics;

using IPhysicsBody1D = PHATASS.Utils.Physics.Physics1D.IPhysicsBody1D;
using IForceReceiver1D = PHATASS.Utils.Physics.Physics1D.IForceReceiver1D;
using IKineticBody1D = PHATASS.Utils.Physics.Physics1D.IKineticBody1D;

using DoubleRange = PHATASS.Utils.Types.Ranges.DoubleRange;
//using IFloatRange = PHATASS.Utils.Types.Ranges.IFloatRange;

//using CubismParameter = Live2D.Cubism.Core.CubismParameter;

//using static PHATASS.Utils.Extensions.CubismParameterExtensions;

//*=======================================================

namespace PHATASS.L2DTools
{
// This component handles setting/getting an L2D parameter value, using 1D physics momentum
// It does so by using the following physics components:
//	> PhysicsBody1D_SimpleMomentumBased: handles property inertia and drag
//	> FixedSpringJoint1D:	pulls property back towards the center
//	> OuterLimitPhysics1DConstraint: limits property to the L2D parameter's limit values
//	> ConstantBrakePhysics1DEffector: Constantly slows down current momentum

	[DefaultExecutionOrder(10000)]
	public class Physics1DElasticL2DParameterHandler :
		SimpleL2DParameterHandler,
		IPhysicsBody1D
	{
	//Serialized fields
		[Tooltip("Properties of the physics body handling parameter value's kinetic behaviour")]
		[SerializeField]
		private Physics.Physics1D.PhysicsBody1D_SimpleMomentumBased _physicsBody;
		private Physics.Physics1D.IUpdatablePhysicsBody1D physicsBody { get { return this._physicsBody;}}

		[Tooltip("Joint handling parameter value's restitution spring")]
		[SerializeField]
		private Physics.Physics1D.FixedSpringJoint1D _springJoint;
		private Physics.Physics1D.IFixedJoint1D springJoint { get { return this._springJoint;}}

		[Tooltip("Outer limit constraining physics behaviour to the parameter's valid range")]
		[SerializeField]
		private Physics.Physics1D.OuterLimitPhysics1DConstraint _outerLimit;
		private Physics.Physics1D.IPhysics1DLimitConstraint outerLimit { get { return this._outerLimit;}}

		[Tooltip("Braking force applied in opposite direction to current momentum each second")]
		[SerializeField]
		private Physics.Physics1D.ConstantBrakePhysics1DEffector _constantBrake;
		private Physics.Physics1D.IPhysics1DEffector constantBrake { get { return this._constantBrake;}}

		[Tooltip("If true, will continually re-update this handler's internal value from target L2DParameterHandler. Otherwise will only get parameter value on start.")]
		[SerializeField]
		private bool continuousRefreshFromL2DParameter = true;
	//ENDOF Serialized fields


	//IPhysicsBody1D
		// scalar (unsigned) value representing total kinetic energy in the body, in Joules
		double Physics.IKineticBody.totalEnergyMagnitude { get { return this.physicsBody.totalEnergyMagnitude; }}

		// Inertial mass of this body. Velocity = (momentum / mass) = (√(2 * KineticEnergy / mass))
		double Physics.IKineticBody.mass
		{
			get { return this.physicsBody.mass; }
			set { this.physicsBody.mass = value; }
		}

		// momentum present in the body (m*v)
		double Physics.IKineticBodyNDimensional<double>.momentum
		{
			get { return this.physicsBody.momentum; }
			set { this.physicsBody.momentum = value; }
		}

		// Value representing current position/value of this kinetic body
		double Physics.IKineticBodyNDimensional<double>.position
		{
			get { return this.physicsBody.position; }
			set { this.physicsBody.position = value; }
		}

		//Adds a 1D momentum to the body, with a force & direction depending on momentum value and sign.
		void Physics.IForceReceiverNDimensional<double>.AddMomentum (double momentum)
		{ this.physicsBody.AddMomentum(momentum); }

		// While asleep = true, the following is true:
		//	> A body's energy/momentum is considered 0
		//	> All physics operations that operate upon an object's current energy/momentum can be skipped
		//	> Physics operations that add forces should NOT be skipped. They will awake the object as necessary.
		bool Physics.ISleepable.asleep
		{ get { return this.physicsBody.asleep; } }
	//ENDOF IPhysicsBody1D

	//MonoBehaviour
		private void Awake ()
		{ this.InitializeComponents(); }

		private void InitializeComponents ()
		{
			if (this.physicsBody.mass == 0f)
			{
				//Debug.LogWarning(this.name + "Physics1DElasticL2DParameterHandler.physicsBody.mass MUST NOT BE ZERO.");
				this.physicsBody.mass = 0.01f;
			}
			this.SetPhysicsBodyValueFromL2DParameter();
			this.ValidateComponentSubReferences();
		}


		private void LateUpdate ()
		{
			this.PhysicsUpdate(UnityEngine.Time.deltaTime);
		}
	//ENDOF MonoBehaviour

	//protected class members
	//ENDOF protected class members

	//private members
		private void PhysicsUpdate (float timeStep)
		{
			//Debug.Log("Physics1DElasticL2DParameterHandler.PhysisUpdate() <============================>");
			this.ValidateComponentSubReferences();

			//ensure physics body's properties match the parameter's properties before processing physics update
			if(this.continuousRefreshFromL2DParameter)
			{ this.SetPhysicsBodyValueFromL2DParameter(); }

			//first apply restitution spring
			this.springJoint.Update(timeStep);

			//Then update the physics kinetic body
			this.physicsBody.Update(timeStep);

			//lastly apply boundary constraints, then constant brake
			this.outerLimit.Update(timeStep);
			this.constantBrake.Update(timeStep);

			//once physics have been calculated, apply new value
			this.ApplyPhysicsBodyValue();
		}

		private void SetPhysicsBodyValueFromL2DParameter ()
		{
			//Debug.Log("SetPhysicsBodyValueFromL2DParameter");
			if (((float) this.physicsBody.position) != this.unclampedValue)
			{ this.physicsBody.position = (double) this.unclampedValue; }

		}
		//applies physics body position to parameter's value
		private void ApplyPhysicsBodyValue ()
		{
			this.unclampedValue = (float) this.physicsBody.position;
		}


		private void ValidateComponentSubReferences ()
		{
		//validate outer limit object exists
			if (
				this.outerLimit.outerLimit == null
			||	(float) this.outerLimit.outerLimit.minimum != this.minimumValue
			||	(float) this.outerLimit.outerLimit.maximum != this.maximumValue
			) {
				this.outerLimit.outerLimit = new DoubleRange(minimum: (double) this.minimumValue, maximum: (double) this.maximumValue);
			}

		//check every physics component references the central physics body
			if (this.springJoint.primarySubject != this.physicsBody)
			{ this.springJoint.primarySubject = this.physicsBody; }

			if (this.outerLimit.primarySubject != this.physicsBody)
			{ this.outerLimit.primarySubject = this.physicsBody; }

			if (this.constantBrake.primarySubject != this.physicsBody)
			{ this.constantBrake.primarySubject = this.physicsBody; }
		}
	}
}
//*/