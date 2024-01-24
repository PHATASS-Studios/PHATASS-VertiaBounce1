using System.Collections.Generic;
using UnityEngine;

using ControllerProvider = PHATASS.ControllerSystem.ControllerProvider;
using ControllerCache = PHATASS.ControllerSystem.ControllerCache;

using ITool = PHATASS.ToolSystem.Tools.ITool;

namespace PHATASS.ToolSystem.Managers
{
	public abstract class ToolManagerBase :
		PHATASS.ControllerSystem.MonoBehaviourControllerBase<IToolManager>,
		IToolManager
	{
	//serialized fields
		[SerializeField]
		private Transform toolDefaultSpawnPoint;
	//ENDOF serialized fields

	//MonoBehaviour Lifecycle implementation
		protected override void Awake ()
		{
			base.Awake();
			this._toolList = new List<ITool>();
		}
	//ENDOF MonoBehaviour

	//IToolManager implementation
		// Exposes access to the list of managed tools
		IList<ITool> IToolManager.tools { get { return this.tools; }}
			protected virtual IList<ITool> tools
			{
				get { return this._toolList as IList<ITool>; }
			}
			private List<ITool> _toolList;

		ITool IToolManager.activeTool { get { return this.activeTool; }}
			protected abstract ITool activeTool {get;}

		ITool IToolManager.SetAsActiveTool (ITool tool, Vector2? position, bool forceInstantiateCopy) { return this.SetAsActiveTool(tool, position, forceInstantiateCopy); }
			protected virtual ITool SetAsActiveTool (ITool tool, Vector2? position = null, bool forceInstantiateCopy = false)
			{ return this.InstantiateAsTool(tool, position: position); }

		void IToolManager.ClearActions (ITool tool) { this.ClearActions(tool); }
			protected virtual void ClearActions (ITool tool)
			{
				if (tool != null) { ClearToolActions(tool); }
				else foreach (ITool singleTool in tools) { ClearToolActions(singleTool); }
			}

		// Destroys and unmanages target tool
		void IToolManager.DeleteTool (ITool tool) { this.DeleteTool(tool); }
			protected virtual void DeleteTool (ITool targetTool)
			{
				this.tools.Remove(targetTool);
				targetTool.Delete();
			}	
	//ENDOF IToolManager implementation

	//protected class methods
		//determines if given tool is already being managed
		protected bool IsManagingTool (ITool tool)
		{
			return this.tools.Contains(tool);
		}

		//incorporates given tool to the tool list
		protected void StartManagingTool (ITool tool, bool forced = false)
		{
			if (forced || !this.IsManagingTool(tool))
			{ this.tools.Add(tool); }
		}

		//instantiates a prefab of a tool
		protected virtual ITool InstantiateAsTool (ITool prefabTool, Vector2? position = null)
		{
			//if no position is provided automatically pick a default - either given transform or the center of the screen
			if (position == null)
			{
				if (this.toolDefaultSpawnPoint != null)
				{ position = this.toolDefaultSpawnPoint.position; }
				else 
				{ position = ControllerCache.viewportController.position; }
			}

			//create a copy of the tool and return a reference to its tool script
			ITool newTool = UnityEngine.Object.Instantiate(
				original: prefabTool.gameObject,
				position: (Vector2) position,
				rotation: prefabTool.transform.rotation,
				parent: transform.parent
			).GetComponent<ITool>();

			//add the tool to the managed tool pool
			this.StartManagingTool(newTool, forced: true);

			return newTool;
		}

		protected void ClearToolActions (ITool tool) { tool.ClearAction(); }
	//ENDOF protected class methods
	}
}