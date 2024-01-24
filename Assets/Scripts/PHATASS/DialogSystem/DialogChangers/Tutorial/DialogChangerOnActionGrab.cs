using Debug = UnityEngine.Debug;

using ControllerCache = PHATASS.ControllerSystem.ControllerCache;

using IActionGrab = PHATASS.ActionSystem.IActionGrab;

namespace PHATASS.DialogSystem.DialogChangers
{
	public class DialogChangerOnActionGrab : DialogChangerOnConditionHeldBase
	{
	//base class abstract method implementation
		protected override bool CheckHeldCondition ()
		{

			if (ControllerCache.toolManager == null)
			{
				Debug.LogWarning ("DialogChangerOnActionGrab: tool manager not found");
				return false;
			}

			//fetch active action cast as a grabbing action
			 IActionGrab action = ControllerCache.toolManager.activeTool.activeAction as IActionGrab;
			 
			//if action does not cast sucessfully return false
			if (action == null) { return false; }
			return true; //action.ongoing; //Maybe return true only if action specifies it is active?
		}
	//ENDOF base class abstract method implementation
	}
}