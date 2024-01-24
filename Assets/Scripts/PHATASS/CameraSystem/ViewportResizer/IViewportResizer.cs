using Rect = UnityEngine.Rect;

namespace PHATASS.CameraSystem
{
// Clips a rect into a different, generally smaller rect
//	IToggleable.state represents wether this clipping is enabled or not
//	while IToggleable.state = false input values will be returned unchanged
	public interface IViewportResizer :
		PHATASS.Utils.Types.Toggleables.IToggleable,
		IRectResizer
	{}

//[TO-DO]: move this interface somewhere safer?
	public interface IRectResizer
	{
		// Returns a clipped version of input rect
		Rect Resize (Rect inputRect);

		// Returns the opposite operation of Resize()
		Rect InverseResize (Rect inputRect);
	}
}