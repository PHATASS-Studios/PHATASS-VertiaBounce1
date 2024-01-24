using CubismParameter = Live2D.Cubism.Core.CubismParameter;

namespace PHATASS.Utils.Extensions
{
	public static class CubismParameterExtensions
	{
	// Sets the current value of a CubismParameter from a normalized (0 to 1) value
		//if 0, sets param to its minimum; if 1, sets param to its maximum
		public static void ESetNormalizedValue (this CubismParameter parameter, float normalizedValue)
		{
			parameter.Value = parameter.MinimumValue + ((parameter.MaximumValue - parameter.MinimumValue) * normalizedValue);
		}

	//Gets the current value as a normalized (0 to 1) value
		public static float EGetNormalizedValue (this CubismParameter parameter)
		{
			return (parameter.Value - parameter.MinimumValue) / (parameter.MaximumValue - parameter.MinimumValue);
		}


	}
}