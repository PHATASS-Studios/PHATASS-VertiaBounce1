using UnityEngine;
using System.Collections.Generic;

using static PHATASS.Utils.Extensions.IListExtensions;
using PHATASS.Utils.Types.Mergeables;

using ControllerCache = PHATASS.ControllerSystem.ControllerCache;

using Comparers = PHATASS.Utils.Comparers;

namespace PHATASS.SettingSystem
{
	[System.Serializable]
	public struct SettingCollisionRadius : ISettingCollisionRadius
	{
	//IMerger<ISettingCollisionRadius>
		//this object has a very simple merge politic: just return the last mergeable that is not null
		ISettingCollisionRadius IMerger<ISettingCollisionRadius>.Merge (IList<ISettingCollisionRadius> mergeables)
		{ return mergeables.EMGetLastNonNull<ISettingCollisionRadius>(); }
	//ENDOF IMerger<ISettingCollisionRadius>

	//ISettingCollisionRadius implementation
		// Collision check radius - multiplies by screen size if screenScaled = true
		float ISettingCollisionRadius.radius { get { return this.effectiveRadius; }}

		//returns the result of the collision check defined in this collision radius around origin
		Collider[] ISettingCollisionRadius.GetCollidersInRange (Transform originTransform)
		{ return this.FindCollidersInRange(originTransform.position); }
		Collider[] ISettingCollisionRadius.GetCollidersInRange (Vector3 originPosition)
		{ return this.FindCollidersInRange(originPosition); }

		//returns the N closest components of type TComponent as defined by this settings package
		TComponent[] ISettingCollisionRadius.GetComponentsInRange <TComponent> (Transform originTransform)
		{ return this.FindComponentsInRange<TComponent>(originTransform.position); }
		TComponent[] ISettingCollisionRadius.GetComponentsInRange <TComponent> (Vector3 originPosition)
		{ return this.FindComponentsInRange<TComponent>(originPosition); }

		//returns N closest components of type TPriorizable sorted by priority and distance (priority first, ties by distance)
		TPriorizable[] ISettingCollisionRadius.GetComponentsInRangeByPriority <TPriorizable> (Transform originTransform)
		{ return this.FindComponentsInRangeByPriority<TPriorizable>(originTransform.position); }
		TPriorizable[] ISettingCollisionRadius.GetComponentsInRangeByPriority <TPriorizable> (Vector3 originPosition)
		{ return this.FindComponentsInRangeByPriority<TPriorizable>(originPosition); }

		//finds and counts the number of valid colliders within range of the collision check.
		//will stop counting at the limit defined by maximumCollisions
		int ISettingCollisionRadius.CountCollidersInRange (Transform originTransform)
		{ return this.CountCollidersInRange(originTransform.position); }
		int ISettingCollisionRadius.CountCollidersInRange (Vector3 originPosition)
		{ return this.CountCollidersInRange(originPosition); }
	//ENDOF ISettingCollisionRadius

	//private static methods
	//endof private static methods

	//private fields
		[SerializeField]
		[Tooltip("Layers to check collision against")]
		private LayerMask layerMask;

		[SerializeField]
		[Tooltip("Collision check radius")]
		private float collisionRadius;// = 1.0f;	

		[SerializeField]
		[Tooltip("If true scale collision radius with screen size")]
		private bool screenScaled;// = true;

		[SerializeField]
		[Tooltip("Maximum number of items returned. closest first. If -1, all available results will be returned")]
		private int maximumCollisions;// = -1; 

		[SerializeField]
		[Tooltip("Wether to include trigger colliders in the search")]
		private bool detectTriggers;// = true;
	//ENDOF private fields

	//private properties
		//calculates collision radius
		private float effectiveRadius { get {
			//If not screenScaled or viewport controller unavailable, return unscaled size
			//if scaled and available controller apply scale
			return (ControllerCache.viewportController == null || !screenScaled)
				?	this.collisionRadius
				:	this.collisionRadius * ControllerCache.viewportController.size;
		}}
	//ENDOF private properties

	//private methods
		//returns the result of the collision check defined in this collision radius around origin
		private Collider[] FindCollidersInRange (Vector3 originPosition)
		{
			//fetch all the colliders in range
			Collider[] colliderArray = this.FetchAllCollidersInRange(originPosition);
			//sort detected colliders by distance 
			System.Array.Sort(
				array: colliderArray,
				comparer: new Comparers.ComparerSortCollidersByDistanceToPosition(originPosition) as IComparer<Collider>
			);
			return this.TrimToSettingLimit<Collider>(colliderArray);
		}

		//returns the result of the collision check defined in this collision, but considering only objects of type TComponent
		private TComponent[] FindComponentsInRange <TComponent> (Vector3 originPosition)// where TComponent : UnityEngine.Component
		{
			//fetch all the components in range
			TComponent[] componentArray = this.ColliderArrayToComponentArray<TComponent>(this.FetchAllCollidersInRange(originPosition));

			//sort detected components by distance 
			System.Array.Sort(
				array: componentArray,
				comparer: new Comparers.ComparerSortComponentsByDistanceToPosition(originPosition) as System.Collections.IComparer
			);
			return this.TrimToSettingLimit<TComponent>(componentArray);
		}

		private TPriorizable[] FindComponentsInRangeByPriority <TPriorizable> (Vector3 originPosition)
			where TPriorizable : PHATASS.Utils.Types.Priorizables.IPriorizable<TPriorizable>
		{
			//fetch all the components in range
			TPriorizable[] componentArray = this.ColliderArrayToComponentArray<TPriorizable>(this.FetchAllCollidersInRange(originPosition));

			//sort detected components by distance 
			System.Array.Sort(componentArray, new Comparers.ComparerSortIPriorizablesByPriorityAndDistanceToPosition<TPriorizable>(originPosition) as IComparer<TPriorizable>);
			return this.TrimToSettingLimit<TPriorizable>(componentArray);
		}


		//takes a list of collider Components and tries to fetch one component of type TComponent from each of those gameObjects
		//output array will be as long as input array or shorter, if components of proper type were not found
		private TComponent[] ColliderArrayToComponentArray<TComponent> (Collider[] colliderArray)
			//where TComponent : UnityEngine.Component
		{
			List<TComponent> componentList = new List<TComponent>();
			foreach (Collider collider in colliderArray)
			{
				TComponent component = collider.gameObject.GetComponent<TComponent>();
				if (component != null) { componentList.Add(component); }
			}
			return componentList.ToArray();
		}

		//fetches ALL the colliders in range - ignoring setting count limit
		private Collider[] FetchAllCollidersInRange (Vector3 originPosition)
		{
			Collider[] colliderArray = Physics.OverlapSphere(
				position: originPosition,
				radius: this.effectiveRadius,
				layerMask: this.layerMask,
				queryTriggerInteraction: (this.detectTriggers)
					? QueryTriggerInteraction.Collide	//if detectTriggers, collide with trigger colliders
					: QueryTriggerInteraction.Ignore	//if !detectTriggers, ignore trigger colliders
			);

			return colliderArray;
		}

		//Trim length of array of results according to maximumCollisions
		private TElement[] TrimToSettingLimit<TElement> (TElement[] startingArray)
		{
			if (this.maximumCollisions < 0 || this.maximumCollisions > startingArray.Length)
			{ return startingArray; }

			TElement[] trimmedArray = new TElement[this.maximumCollisions];
			System.Array.Copy(sourceArray: startingArray, destinationArray: trimmedArray, length: this.maximumCollisions);

			return trimmedArray;
		}

		//finds and counts the number of valid colliders within range of the collision check.
		//will stop counting at the limit defined by maximumCollisions
		private int CountCollidersInRange (Vector3 originPosition)
		{
			Collider[] recountArray;
			//limitless version uses a memory-allocating method
			if (maximumCollisions < 0)
			{
				recountArray = Physics.OverlapSphere(
					position: originPosition,
					radius: this.effectiveRadius,
					layerMask: this.layerMask,
					queryTriggerInteraction: (this.detectTriggers)
						? QueryTriggerInteraction.Collide	//if detectTriggers, collide with trigger colliders
						: QueryTriggerInteraction.Ignore	//if !detectTriggers, ignore trigger colliders
				);
				return recountArray.Length;
			}
			else //limited version uses faster non-allocating method
			{
				recountArray = new Collider[maximumCollisions];
				return Physics.OverlapSphereNonAlloc(
					position: originPosition,
					radius: this.effectiveRadius,
					results: recountArray,
					layerMask: this.layerMask,
					queryTriggerInteraction: (this.detectTriggers)
						? QueryTriggerInteraction.Collide	//if detectTriggers, collide with trigger colliders
						: QueryTriggerInteraction.Ignore	//if !detectTriggers, ignore trigger colliders
				);
			}
		}
	//ENDOF private methods
	}
}