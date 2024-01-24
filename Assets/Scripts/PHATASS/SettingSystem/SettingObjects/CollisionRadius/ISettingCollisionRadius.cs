using UnityEngine;

namespace PHATASS.SettingSystem
{
	public interface ISettingCollisionRadius :
		ISetting<ISettingCollisionRadius>
	{
		// Collision check radius
		float radius {get;}

		//returns the result of the collision check defined in this collision radius around origin
		//ordered by closest first and up to the limits defined in this settings package
		Collider[] GetCollidersInRange (Transform originTransform);
		Collider[] GetCollidersInRange (Vector3 originPosition);

		//returns the N closest components of type TComponent as defined by this settings package
		TComponent[] GetComponentsInRange <TComponent> (Transform originTransform);// where TComponent : UnityEngine.Component;
		TComponent[] GetComponentsInRange <TComponent> (Vector3 originPosition);// where TComponent : UnityEngine.Component;

		//returns N closest components of type TPriorizable sorted by priority and distance (priority first, ties by distance)
		TPriorizable[] GetComponentsInRangeByPriority <TPriorizable> (Transform originTransform)
			where TPriorizable : PHATASS.Utils.Types.Priorizables.IPriorizable<TPriorizable>;
		TPriorizable[] GetComponentsInRangeByPriority <TPriorizable> (Vector3 originPosition)
			where TPriorizable :  PHATASS.Utils.Types.Priorizables.IPriorizable<TPriorizable>;

		//finds and counts the number of valid colliders within range of the collision check.
		int CountCollidersInRange (Transform originTransform);
		int CountCollidersInRange (Vector3 originPosition);
	}
}