using ITool = PHATASS.ToolSystem.Tools.ITool;

using UnityEngine;

using ControllerCache = PHATASS.ControllerSystem.ControllerCache;

using IVector2Constraint = PHATASS.Utils.Types.Constraints.IVector2Constraint;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

namespace PHATASS.EmotionSystem.Aimables
{
	[System.Serializable]
	public struct AimTargetTrackerActiveTool : IAimTargetTracker
	{
	//serializable fields
		[SerializeField]
		[Tooltip("[IVector2Constraint] When active tool is INSIDE trackingZoneLimit, but NOT inside deadZoneLimit, tracking is ENABLED. If null, tracking is always enabled.")]
		[SerializedTypeRestriction(typeof(IVector2Constraint))]
		private UnityEngine.Object? _trackingZoneLimit;
		private IVector2Constraint trackingZoneLimit
		{ get {
			if (this._trackingZoneLimit == null) { return null; }
			return this._trackingZoneLimit as IVector2Constraint;
		}}

		[SerializeField]
		[Tooltip("[IVector2Constraint] When active tool is INSIDE deadZoneLimit, tracking is DISABLED. If null, this is ignored.")]
		[SerializedTypeRestriction(typeof(IVector2Constraint))]
		private UnityEngine.Object? _deadZoneLimit;
		private IVector2Constraint deadZoneLimit
		{ get {
			if (this._deadZoneLimit == null) { return null; }
			return this._deadZoneLimit as IVector2Constraint;
		}}
	//ENDOF serializable

	//IAimTargetTracker
		//Returns false if target tracking is meant to be inactive. True if active.
		bool IAimTargetTracker.trackingActive { get { return this.trackingActive; }}

		//Returns the position of the target to aim towards
		// while trackingActive = false, behaviour is undefined/up to the implementor
		Vector3 IAimTargetTracker.targetPosition { get { return this.targetPosition; }}
	//ENDOF IAimTargetTracker

	//constructor
	/*	public AimTargetTrackerActiveTool (IVector2Constraint trackingZone = null, IVector2Constraint deadZone = null)
		{
			trackingZoneLimit = trackingZone;
			deadZoneLimit = deadZone;
		}*/
	//ENDOF constructor

	//private
		private ITool activeTool { get { return ControllerCache.toolManager.activeTool; }}

		private bool trackingActive
		{
			get
			{
				return this.ToolIsInValidRange(this.activeTool);
			}
		}

		private Vector3 targetPosition
		{
			get
			{
				if (this.activeTool == null)
				{ return Vector3.zero; }
				else
				{
					return this.activeTool.position;
				}
			}
		}

		//Returns true if tool exists and is inside valid range
		private bool ToolIsInValidRange (ITool tool)
		{
			//if no active tool, we can't track nothing, so no tracking
			if (tool == null)
			{ return false; }

			//cache tool position
			Vector3 toolPosition = tool.position;

			//if tool is within the dead zone (if it exists), tracking is disabled
			if (this.deadZoneLimit != null && this.deadZoneLimit.Contains(toolPosition))
			{ return false; }

			//if no tracking zone delimiter, tracking is enabled
			if (this.trackingZoneLimit == null)
			{ return true; }

			//if tool is inside the tracking zone (but not inside the dead zone), tracking is enabled
			if (this.trackingZoneLimit.Contains(toolPosition))
			{ return true; }
			
			//if none of the above conditions is fulfilled, tracking is disabled
			return false;
		}
	//ENDOF private
	}
}