using UnityEngine;

using static PHATASS.Utils.Extensions.Vector3Extensions;

using ControllerCache = PHATASS.ControllerSystem.ControllerCache;

using TFloatRange = PHATASS.Utils.Types.Ranges.ILimitedRange<System.Single>;

namespace PHATASS.Miscellaneous.TransformEffects
{
	public class ZRotationFromTransformVelocityBehaviour : MonoBehaviour
	{
	//Serialized fields
		[Tooltip("Will rotate this transform's Z by this transform's X axis velocity")]
		[SerializeField]
		private BaseTransformVelocityCacheBehaviour transformVelocity;

		[Tooltip("If this is true, effect will be reduced as viewport is zoomed in")]
		[SerializeField]
		private bool scaleEffectByViewportScaleFactor = true;

		[SerializeField]
		private float lerpRate = 0.05f;

		[SerializeField]
		private float degreesByMetersPerSecond = 30f;

		[Tooltip("Origin velocity will be scaled by this vector before calculating")]
		[SerializeField]
		private Vector3 axisScaleVector = Vector3.left;

		[Tooltip("Z axis rotation will be limited to this range")]
		[SerializeField]
		private PHATASS.Utils.Types.Ranges.RandomFloatRange _degreesLimit;
		protected TFloatRange degreesLimit { get { return this._degreesLimit; }}
	//ENDOF Serialized

	//MonoBehaviour lifecycle
		private void Start ()
		{
			if (this.transformVelocity == null) { this.transformVelocity = this.GetComponent<OnUpdateTransformVelocityCacheBehaviour>(); }
			this.baseRotation = this.transform.rotation.eulerAngles;
		}

		private void Update ()
		{
			this.UpdateRotation();
		}
	//ENDOF MonoBehaviour

	//private fields
		private Vector3 baseRotation;
	//ENDOF fields

	//private properties
		private float scaleFactor
		{
			get
			{
				return (this.scaleEffectByViewportScaleFactor)
					? ControllerCache.viewportController.viewportScaleFactor
					: 1f;
			}
		}

		private float desiredZOffset
		{
			get
			{
				//angular offset is calculated with the sum of the scaled axis velocities, multiplied by speed and scale
				return this.degreesLimit.Clamp(
					Vector3.Scale(this.axisScaleVector, this.transformVelocity.velocity).EComponentSum()
					* this.degreesByMetersPerSecond / this.scaleFactor
				);
			}
	}

		private Quaternion desiredRotation
		{
			get
			{
				return Quaternion.Euler(
					x: this.baseRotation.x,
					y: this.baseRotation.y,
					z: this.baseRotation.z + this.desiredZOffset
				);
			}
		}
	//ENDOF private properties

	//private methods
		private void UpdateRotation ()
		{
			this.transform.rotation = Quaternion.Slerp(this.transform.rotation, this.desiredRotation, this.lerpRate);
		}
	//ENDOF methods
	}
}