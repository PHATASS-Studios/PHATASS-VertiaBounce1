namespace PHATASS.L2DTools
{
// Interface representing a L2D Cubism rig parameter handler (Live2D.Cubism.Core.CubismParameter).
// This element is in charge of handling L2D parameter change and interpolation, and should be used instead of directly writing cubism parameters
	public interface IL2DParameterHandler
/*implement IPhysicsBody1D??*/ // 	PHATASS.Utils.Physics.Physics1D.IPhysicsBody1D	//IPhysicsBody1D provides methods for applying forces to the parameter value
	{
		float absoluteValue { get; set; }	//absolute value of the L2D parameter, as it handled by the cubism rig
		float normalizedValue { get; set; }	//normalized value of the L2D parameter, with 0 corresponding to the absolute minimum value and 1 to the absolute maximum value
	}
}