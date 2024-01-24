using UnityEngine;

using static PHATASS.Utils.Extensions.Vector2Extensions;
using static PHATASS.Utils.Types.Angles.IAngle2DExtensions;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

using IPhysicsBody1D = PHATASS.Utils.Physics.Physics1D.IPhysicsBody1D;

using IAngle2D = PHATASS.Utils.Types.Angles.IAngle2D;
using IAngle2DIntensity = PHATASS.Utils.Events.IAngle2DIntensity;

namespace PHATASS.ActionSystem
{
// ISlappable implementation that translates slap forces to two respective IPhysicsBody1D, one per axis
	public class SlappableToPhysics1DBodies :
		MonoBehaviour,
		ISlappable
	{
	//Serialized fields
		[Tooltip("Physics1D Object that will receive incoming forces corresponding to the X (Horizontal) axis. Will be ignored if not set.")]
		[SerializeField]
		[SerializedTypeRestriction(typeof(IPhysicsBody1D))]
		private UnityEngine.Object? _horizontalAxisBody = null;
		private IPhysicsBody1D horizontalAxisBody
		{ get {
			if (this._horizontalAxisBody == null) { return null; }
			else { return this._horizontalAxisBody as IPhysicsBody1D; }
		}}

		[Tooltip("Physics1D Object that will receive incoming forces corresponding to the Y (Vertical) axis. Will be ignored if not set.")]
		[SerializeField]
		[SerializedTypeRestriction(typeof(IPhysicsBody1D))]
		private UnityEngine.Object? _verticalAxisBody = null;
		private IPhysicsBody1D verticalAxisBody
		{ get {
			if (this._verticalAxisBody == null) { return null; }
			else { return this._verticalAxisBody as IPhysicsBody1D; }
		}}

		[Tooltip("Multiplier value for any forces emitted")]
		[SerializeField]
		private float pushForceMultiplier = 1f;

		[Tooltip("Any push received will be additionally rotated this amount in ºdegrees")]
		[SerializeField]
		private float _angularOffsetDegrees;
		private IAngle2D angularOffset
		{ get { return PHATASS.Utils.Types.Angles.IAngle2DFactory.FromDegrees(this._angularOffsetDegrees); }}
	//ENDOF Serialized
	
	//ISlappable
		void IPushable2D.PushFromPosition (Vector2 originPosition, float pushForce, AnimationCurve fallOffCurve)
		{ this.PushFromPosition(originPosition, pushForce, fallOffCurve); }

		void PHATASS.Utils.Events.ISimpleEventReceiver<IAngle2DIntensity>.Event (IAngle2DIntensity param0)
		{ this.Push(pushAngle: param0.angle, pushForce: param0.intensity); }
	//ENDOF ISlappable

	//private members
		//calculated centerpoint for this element
		private Vector2 selfPosition
		{ get { return this.transform.position; }}

		//calculates distance from center to given point
		private float DistanceToPoint (Vector2 point)
		{ return (this.selfPosition - point).magnitude; }

		// Apply a pushing force calculating the angle as if it was received from originPosition.
		//	If fallOffCurve exists it will be used to further scale pushForce by distance
		private void PushFromPosition (Vector2 originPosition, float pushForce, UnityEngine.AnimationCurve fallOffCurve)
		{
			if (fallOffCurve != null)
			{ pushForce *= fallOffCurve.Evaluate(this.DistanceToPoint(originPosition)); }

			this.Push(pushAngle: originPosition.EFromToAngle2D(this.selfPosition), pushForce: pushForce);
		}

		// Apply a force to available axes at pushAngle direction and scaled by pushForce
		private void Push (IAngle2D pushAngle, float pushForce)
		{
			// Calculate a Vector2 of the forces corresponding to each axis, for given angle, and scaled properly
			Vector2 pushForceVector = (pushAngle + this.angularOffset).EAngle2DToVector2() * pushForce * this.pushForceMultiplier;

			if (this.horizontalAxisBody != null)
			{ this.horizontalAxisBody.AddMomentum(pushForceVector.x); }

			if (this.verticalAxisBody != null)
			{ this.verticalAxisBody.AddMomentum(pushForceVector.y); }
		}
	//ENDOF private members
	}
}