using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using ControllerProvider = PHATASS.ControllerSystem.ControllerProvider;
using ControllerCache = PHATASS.ControllerSystem.ControllerCache;

using ITool = PHATASS.ToolSystem.Tools.ITool;

using EButtonInputState = PHATASS.InputSystem.EButtonInputState;
using IMouseInputController = PHATASS.InputSystem.IMouseInputController;

namespace PHATASS.ToolSystem.Managers
{
	public class ToolManagerMouseInput : ToolManagerBase
	{
	//constants
		private const float idleToolFocusHoldTime = 1.0f;
	//ENDOF constants

	//private fields
		[SerializeField]
		private PHATASS.ToolSystem.Tools.ToolBase defaultToolPrefab;

		[SerializeField]
		private int minimumToolCount = 2;

		[SerializeField]
		private PHATASS.ToolSystem.Tools.ToolBase[] initialToolPrefabs = {};

		private IMouseInputController inputController;	//input controller
	//ENDOF private fields

	//private properties
		//input is considered enabled if there are tools and scene controller allows it (or no scene controller)
		private bool inputEnabled
		{
			get
			{
				return
					this.tools != null &&
					this.tools.Any() &&
					(ControllerCache.sceneController == null || ControllerCache.sceneController.inputEnabled);
			}
		}
	//ENDOF private properties

	//IToolManager overrides
		protected override ITool activeTool { get { return this.focusedTool; }}
		private ITool focusedTool;		//highligted and active tool

		protected override ITool SetAsActiveTool (ITool tool, Vector2? position = null, bool forceInstantiateCopy = false)
		{
			//First if target object does not belong to the same scene, we need to instantiate a new copy from it
			if (forceInstantiateCopy || this.gameObject.scene != tool.gameObject.scene)
			{
				if (position == null) { position = this.focusedTool?.position; }

				tool = this.InstantiateAsTool(tool, position);
			}
			this.SetFocused(tool);
			return tool;
		}
	//ENDOF IToolManager implementation

	//MonoBehaviour Lifecycle implementation
		//create a mouse input controller for oneself on start, and register with the central controller
		protected override void Awake ()
		{
			base.Awake();
			this.inputController = this.gameObject.AddComponent<PHATASS.InputSystem.MouseInputControllerInputSystem>() as IMouseInputController;
		}

		protected void Start ()
		{
			this.StartCoroutine(InitializeToolsAfterInputEnabled());
		}

		protected void Update ()
		{
			//Debug.Log(this.inputController.rawDelta);
			if (this.inputEnabled)
			{
				this.ToolCycleCheck();
			}
		}
	//ENDOF MonoBehaviour Lifecycle implementation

	//private method implementation
		//initializes predefined tools as soon as input is enabled by sceneController
		private IEnumerator InitializeToolsAfterInputEnabled()
		{
			//wait until sceneController allows input before initializing tool list
			//skip wait if scene controller does not exist
			if (ControllerCache.sceneController != null)
			{
				while (!ControllerCache.sceneController.inputEnabled)
				{ yield return null; }
			}

			//create initial list of tools in the scene
			foreach (ITool toolPrefab in this.initialToolPrefabs)
			{
				this.InstantiateAsTool(toolPrefab);
			//[TO-DO] This doesn't allow for initial tool positioning! should instead have a list of pre-instantiated ITools to register
				//this.StartManagingTool(initalTool);
			}

			//create additional default tools until minimum is met
			for (int i = this.tools.Count; i < this.minimumToolCount; i++)
			{
				this.InstantiateAsTool(this.defaultToolPrefab);
			}

			//finally focus the first tool instantiated
			this.SetFocused(this.tools[0]);
		}

		//set target tool as focused, every other as un-focused
		private void SetFocused (ITool targetTool)
		{
			this.StartManagingTool(targetTool);

			this.focusedTool = targetTool;

			//send every known tool an update on its status - true if they're focused false otherwise
			foreach (ITool tool in this.tools)
			{
				tool.focused = (tool == targetTool);
				tool.input = (tool.focused == true)
					?	this.inputController
					:	null;
			}
		}

		//checks for input corresponding to hand swap, and performs the action if necessary
		private void ToolCycleCheck ()
		{
			if (this.inputController.GetButtonHeld(1, idleToolFocusHoldTime))
			{ this.CycleTool(prioritizeIdle: true); }
			else if (this.inputController.GetButtonDown(1))
			{ this.CycleTool(prioritizeIdle: false); }
		}

		//cycles active tool - creates or destroys idle hands when unused/required
		private void CycleTool (bool prioritizeIdle = false)
		{
			ITool nextIdle = this.FindIdleTool();

			//unfocus active hand so it can be counted as idle if not automated
			this.activeTool.focused = false;
			int idleCount = this.CountIdleTools();

			if (idleCount == 0) //if no idle hands left, create and focus a new hand
			{
				this.SetFocused(this.InstantiateAsTool(this.defaultToolPrefab));
				return;
			}

			//priorize idle tools if necessary
			if (prioritizeIdle && nextIdle != null)
			{
				this.SetFocused(nextIdle);
				return;
			}

			//if there are additional idle hands and last hand is left idle,
			//then remove last hand and focus the next in order as long as minimum hand count is respected
			if (idleCount > 1 && this.activeTool.idle && this.tools.Count > this.minimumToolCount)
			{
				ITool toDeleteTool = this.activeTool;
				if (prioritizeIdle)
				{
					this.SetFocused(this.InstantiateAsTool(this.defaultToolPrefab));
				}
				else
				{
					FocusNext();
					toDeleteTool = nextIdle;
				}
				this.DeleteTool(toDeleteTool);
				return;
			}
			else
			{ FocusNext(); }

			//set as focused the immediate next tool in the managed list (with loopback)
			void FocusNext ()
			{
				int index = this.tools.IndexOf(this.activeTool);
				index ++;

				if (index >= this.tools.Count)
				{ index = 0; }

				this.SetFocused(this.tools[index]);
			}
		}

		//finds and return the first idle tool. NULL if no idle tool.
		private ITool FindIdleTool ()
		{
			foreach (ITool tool in this.tools)
			{
				if (tool.idle) { return tool; }
			}
			return null;
		}

		//polls existing hands and returns a count of idle tools (tools with no action)
		private int CountIdleTools ()
		{
			int idleCount = 0;
			foreach (ITool tool in this.tools)
			{
				if (tool.idle) { idleCount ++; }
			}
			return idleCount;
		}
	//ENDOF private method implementation

	//protected method overrides
		protected override void DeleteTool (ITool targetTool)
		{
			if (targetTool.focused)
			{
				this.CycleTool(prioritizeIdle: true);
			}
			
			base.DeleteTool(targetTool);
		}
	//ENDOF protected method overrides
	}
}