using System.Collections.Generic;

using PHATASS.Utils.Types.Mergeables;

using UnityEngine;

namespace PHATASS.SettingSystem
{
	[System.Serializable]
	[UnityEngine.CreateAssetMenu(
		fileName = "CollisionRadiusSettingObject",
		menuName = "PHATASS settings/Setting objects/Collision Radius settings",
		order = 1
	)]
	// Collision check radius - multiplies by screen size if screenScaled = true
	public class SOSettingCollisionRadius : ScriptableObject, ISettingCollisionRadius
	{
	//IMerger<ISettingCollisionRadius>
		ISettingCollisionRadius IMerger<ISettingCollisionRadius>.Merge (IList<ISettingCollisionRadius> mergeables)
		{ return ((ISettingCollisionRadius) this.backingField).Merge(mergeables); }
	//ENDOF IMerger<ISettingCollisionRadius>

	//ISettingCollisionRadius implementation
		float ISettingCollisionRadius.radius
		{ get { return this.backingField.radius; }}

		//returns the result of the collision check defined in this collision radius around origin
		Collider[] ISettingCollisionRadius.GetCollidersInRange (Transform originTransform)
		{ return this.backingField.GetCollidersInRange(originTransform); }
		Collider[] ISettingCollisionRadius.GetCollidersInRange (Vector3 originPosition)
		{ return this.backingField.GetCollidersInRange(originPosition); }

		//returns the N closest components of type TComponent as defined by this settings package
		TComponent[] ISettingCollisionRadius.GetComponentsInRange <TComponent> (Transform originTransform)
		{ return this.backingField.GetComponentsInRange<TComponent>(originTransform); }
		TComponent[] ISettingCollisionRadius.GetComponentsInRange <TComponent> (Vector3 originPosition)
		{ return this.backingField.GetComponentsInRange<TComponent>(originPosition); }

		//returns N closest components of type TPriorizable sorted by priority and distance (priority first, ties by distance)
		TPriorizable[] ISettingCollisionRadius.GetComponentsInRangeByPriority <TPriorizable> (Transform originTransform)
		{ return this.backingField.GetComponentsInRangeByPriority<TPriorizable>(originTransform); }
		TPriorizable[] ISettingCollisionRadius.GetComponentsInRangeByPriority <TPriorizable> (Vector3 originPosition)
		{ return this.backingField.GetComponentsInRangeByPriority<TPriorizable>(originPosition); }

		//finds and counts the number of valid colliders within range of the collision check.
		//will stop counting at the limit defined by maximumCollisions
		int ISettingCollisionRadius.CountCollidersInRange (Transform originTransform)
		{ return this.backingField.CountCollidersInRange(originTransform); }
		int ISettingCollisionRadius.CountCollidersInRange (Vector3 originPosition)
		{ return this.backingField.CountCollidersInRange(originPosition); }
	//ENDOF ISettingCollisionRadius

	//constructor
		public SOSettingCollisionRadius (SettingCollisionRadius radiusSettingField)
		{
			this._backingField = radiusSettingField;
		}
	//ENDOF constructor

	//private fields
		[SerializeField]
		[Tooltip("Collision radius settings definition")]
		private SettingCollisionRadius _backingField;
		private ISettingCollisionRadius backingField { get { return this._backingField; }}
	//ENDOF private fields

	//private properties
	//ENDOF private properties

	//private methods
	//ENDOF private methods
	}
}