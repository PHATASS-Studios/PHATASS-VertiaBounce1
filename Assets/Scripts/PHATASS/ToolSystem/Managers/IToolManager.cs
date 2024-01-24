using System.Collections.Generic;

using Vector2 = UnityEngine.Vector2;
using ITool = PHATASS.ToolSystem.Tools.ITool;

namespace PHATASS.ToolSystem.Managers
{
	public interface IToolManager : PHATASS.ControllerSystem.IController
	{
		IList<ITool> tools {get;} // Exposes access to the list of managed tools
		ITool activeTool {get;} // This is the focused tool
		//[TO-DO] perhaps it's a good idea to expose this as an array including every active tool?


		// Activates and prepares to manage given tool.
		// If target gameObject is a prefab, a new copy will be instantiated
		// Returns a reference to newly instantiated or given tool
		ITool SetAsActiveTool (ITool tool, Vector2? position = null, bool forceInstantiateCopy = false);

		// Clears all active actions off of given tool.
		// If no tool given, affects every managed tool.
		void ClearActions (ITool tool = null);

		// Destroys and unmanages target tool
		void DeleteTool (ITool tool = null);
	}
}