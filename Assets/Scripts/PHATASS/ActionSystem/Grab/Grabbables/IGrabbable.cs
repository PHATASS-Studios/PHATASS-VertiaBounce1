namespace PHATASS.ActionSystem
{
//Interface representing an element subsceptible to ActionGrab
//Elements capable of receiving an ActionGrab must implement IGrabbable
//
// It is capable of creating a joint MonoBehaviour component on originGameObject and returning a reference to it
//	in order to rescind that grab the caller just needs to destroy the joint MonoBehaviour
	public interface IGrabbable :
		IActionReceiver
	{
	// Creates and returns a joint on originGameObject. This joint will pull on this grabbable as originGameObject moves.
		// In order to rescind the grab, use UnityEngine.Object.Destroy() to destroy the joint components
		UnityEngine.Component CreateGrabJoint (UnityEngine.GameObject originGameObject);
	}
}