using UnityEngine;

using CubismParameter = Live2D.Cubism.Core.CubismParameter;

using static PHATASS.Utils.Extensions.FrameIndependentSmoothingExtensions;
using static PHATASS.Utils.Extensions.FloatExtensions;

namespace PHATASS.L2DTools
{
	public class L2DParameterFromMouseYBehaviour : MonoBehaviour
	{
		[SerializeField]
		private CubismParameter cubismParameter;

		[SerializeField]
		private float lerpRate = 0.999f;

		[SerializeField]
		private float restingPoint = 0.5f;

		[SerializeField]
		private bool resetWhileNotClicked = true;

		[SerializeField]
		private float resetLerpRate = 0.9f;


		private float mouseYToScreenNormalized
		{
			get { return Input.mousePosition.y / Screen.height; }
		}

		private float minimum { get { return this.cubismParameter.MinimumValue; }}
		private float maximum { get { return this.cubismParameter.MaximumValue; }}
		private float value
		{
			get { return this.cubismParameter.Value; }
			set { this.cubismParameter.Value = value; }
		}



		private void LateUpdate ()
		{
			if (Input.GetMouseButton(0))
			{ this.SetL2DParameterValue(this.mouseYToScreenNormalized, this.lerpRate); }
			else if (resetWhileNotClicked)
			{ this.SetL2DParameterValue(this.restingPoint, this.resetLerpRate); }
		}

		//sets an L2D's parameter value to the equivalent of received normalized value
		//for 0, sets the value at the minimum, for 1, sets the value at its maximum
		private void SetL2DParameterValue (float normalizedRatio, float rate)
		{
			normalizedRatio = normalizedRatio.EClamp(minimum: 0f, maximum: 1f);
			//Debug.Log("mousePosition: " + Input.mousePosition);
			float desiredValue = this.minimum + ((this.maximum - this.minimum) * normalizedRatio);
			//Debug.Log("SetL2DParameterValue: " + normalizedRatio + " > " + desiredValue);
			if (lerpRate < 1f) { desiredValue = this.value.EFrameIndependentLerp(towards: desiredValue, rate: rate); }
			this.value = desiredValue;
		}
	}
}