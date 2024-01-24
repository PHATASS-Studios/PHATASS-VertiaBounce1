
namespace PHATASS.ActionSystem
{
	public abstract class ActionRadialBase <TSettingsPackage> :
		ActionBase<TSettingsPackage>
		where TSettingsPackage : PHATASS.SettingSystem.ISettingsPackageRadialAction<TSettingsPackage>
	{
	// Acquisition of element(s) in range
		//returns the N closest components of type TComponent as defined by this settings package
		protected TComponent[] GetComponentsInRange <TComponent> ()
		//	where TComponent : UnityEngine.Component
		{ return this.defaultSettings.actionRadiusSetting.GetComponentsInRange<TComponent>(this.tool.transform); }

		//returns N closest components of type TPriorizable sorted by priority and distance (priority first, ties by distance)
		protected TPriorizable[] GetComponentsInRangeByPriority <TPriorizable> ()
			where TPriorizable : PHATASS.Utils.Types.Priorizables.IPriorizable<TPriorizable>
		{ return this.defaultSettings.actionRadiusSetting.GetComponentsInRangeByPriority<TPriorizable>(this.tool.transform); }

		//Gets all of the colliders within range, according to default (contextless) action settings.
		protected UnityEngine.Collider[] GetCollidersInRange ()
		{ return this.defaultSettings.actionRadiusSetting.GetCollidersInRange(this.tool.transform); }

		//Counts all of the colliders within range, according to default (contextless) action settings.
		protected int CountCollidersInRange ()
		{ return this.defaultSettings.actionRadiusSetting.CountCollidersInRange(this.tool.transform); }
	//ENDOF
	}
}