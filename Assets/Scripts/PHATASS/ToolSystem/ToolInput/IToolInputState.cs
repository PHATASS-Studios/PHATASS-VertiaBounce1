using IInputMovementDelta = PHATASS.InputSystem.IInputMovementDelta;
using EButtonInputState = PHATASS.InputSystem.EButtonInputState;

namespace PHATASS.ToolSystem
{
	// Interface defining the values relevant to a tool's input
	// Generally injected into a tool as an intermediary/adaptor between it and related ToolManagerBase
	public interface IToolInputState : IInputMovementDelta
	{
		EButtonInputState primaryInputState { get; }	// state of primary input (left click, or touch lifecycle state)
	}
}