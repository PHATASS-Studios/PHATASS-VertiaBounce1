namespace PHATASS.CameraSystem
{
// Limits for a 2D viewport. Clamps a viewport rect so it always falls within the acceptable scene.
// IRectValue.value returns a rect representing the calculated viewport's maximum limits
	public interface IViewportLimits :
		PHATASS.Utils.Types.Constraints.IRectConstraint,
		PHATASS.Utils.Types.Values.IRectValue
	{
		//camera giving the reference for the viewport's aspect ratio
		//if null/not set automatically picks Camera.main
		UnityEngine.Camera referenceCamera { set; }
	}
}