namespace PHATASS.SceneSystem.TransitionSystem
{
// Interface representing an inter-scene transition controller
//	This is a scene always loaded above content scene that is used to cover transitions when unloading/loading scenes
	public interface ITransitionController :
		PHATASS.ControllerSystem.IController,
		PHATASS.Utils.Types.Toggleables.IAnimatedToggleable
	{
	// Relevant notes
	//	IToggleable.state
	//		> 0 means closed - but not necessarily completely covering the screen
	//		> 1 means open - viewport is uncovered and content scene is shown
	//
	//	IAnimatedToggleable.analogTransitionProgress 
	//		> being 0 (fully closed) no necessarily means this passes an StrictStateCheck as there may be gaps closing with an ellastic delay
	//		> always use StrictStateCheck() to ensure transition is completely closed and fully covering the viewport
	//
	//	IAnimatedToggleable.ForceSetState(bool)
	//		> Transitions MUST NOT suddenly change states, in order to avoid jarring changes
	//		> calling this will usually do the same as setting IToggleable.state
	}
}