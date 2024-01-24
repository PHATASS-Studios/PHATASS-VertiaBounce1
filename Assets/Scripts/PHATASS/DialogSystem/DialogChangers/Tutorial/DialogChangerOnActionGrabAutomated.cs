using System.Collections.Generic;
using Debug = UnityEngine.Debug;

using ControllerCache = PHATASS.ControllerSystem.ControllerCache;

using ITool = PHATASS.ToolSystem.Tools.ITool;

namespace PHATASS.DialogSystem.DialogChangers
{
	public class DialogChangerOnActionGrabAutomated : DialogChangerOnConditionHeldBase
	{
	//serialized fields and properties
		private IList<ITool> toolList
		{
			get	{ return ControllerCache.toolManager.tools;	}
		}
	//ENDOF serialized fields and properties

	//base class abstract method implementation
		protected override bool CheckHeldCondition ()
		{
			foreach (ITool tool in toolList)
			{
				if (tool.auto) { return true; }
			}
			return false;
		}
	//ENDOF base class abstract method implementation
	}
}