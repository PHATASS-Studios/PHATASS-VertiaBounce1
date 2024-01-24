using UnityEngine;

using CubismParameter = Live2D.Cubism.Core.CubismParameter;

using static PHATASS.Utils.Extensions.CubismParameterExtensions;

namespace PHATASS.L2DTools
{
//Component handling setting and getting an L2D parameter value
//	[DefaultExecutionOrder(-10000)]
	public class SimpleL2DParameterHandler :
		MonoBehaviour,
		IL2DParameterHandler
	{
	//Serialized fields
		[Tooltip("Managed Cubism Parameter controller.")]
		[SerializeField]
		private CubismParameter cubismParameter;
	//ENDOF Serialized fields

	//IL2DParameterHandler
		//absolute value of the L2D parameter, as it handled by the cubism rig
		float IL2DParameterHandler.absoluteValue
		{ get { return this.absoluteValue; } set { this.absoluteValue = value; }}

		//normalized value of the L2D parameter, with 0 corresponding to the absolute minimum value and 1 to the absolute maximum value
		float IL2DParameterHandler.normalizedValue
		{ get { return this.normalizedValue; } set { this.normalizedValue = value; }}
	//ENDOF IL2DParameterHandler

	//protected class members
		protected float unclampedValue
		{
			get { return this.cubismParameter.Value; }
			set { this.cubismParameter.Value = value; /*Debug.Log("ParamValueSet: " + value);*/ }
		}
		protected float absoluteValue
		{
			get { return this.cubismParameter.Value; }
			set { this.cubismParameter.Value = System.Math.Clamp(value: value, min: this.minimumValue, max: this.maximumValue); }
		}
		protected float normalizedValue
		{
			get { return this.cubismParameter.EGetNormalizedValue(); }
			set { this.cubismParameter.ESetNormalizedValue(value); }
		}

		protected float minimumValue { get { return this.cubismParameter.MinimumValue; }}
		protected float maximumValue { get { return this.cubismParameter.MaximumValue; }}
	//ENDOF protected class members
	}
}