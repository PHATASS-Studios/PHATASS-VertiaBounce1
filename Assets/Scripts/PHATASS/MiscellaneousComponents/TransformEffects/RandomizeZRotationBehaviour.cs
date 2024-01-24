using UnityEngine;

using static PHATASS.Utils.Extensions.Vector3Extensions;

using TFloatRange = PHATASS.Utils.Types.Ranges.ILimitedRange<System.Single>;

namespace PHATASS.Miscellaneous.TransformEffects
{
	public class RandomizeZRotationBehaviour : MonoBehaviour
	{
	//Serialized fields
		[Tooltip("Z Rotation of these transforms will be randomized when triggered")]
		[SerializeField]
		private Transform[] targetTransformList;

		[Tooltip("Z axis rotation will be limited to this range")]
		[SerializeField]
		private PHATASS.Utils.Types.Ranges.RandomFloatRange _degreesRange;
		protected TFloatRange degreesRange { get { return this._degreesRange; }}

		[Tooltip("If this is true, the same value will be applied to every item. Otherwise, a new value will be generated for each.")]
		[SerializeField]
		private bool sameValueForAll = true;

		[Tooltip("Wether this will trigger on Start()")]
		[SerializeField]
		private bool triggerOnStart = true;

		[Tooltip("Wether this will trigger on OnEnable()")]
		[SerializeField]
		private bool triggerOnEnable = true;
	//ENDOF Serialized

	//public events
		public void RandomizeZRotationEvent ()
		{ this.RandomizeValues(); }
	//ENDOF public events

	//MonoBehaviour lifecycle
		private void Start ()
		{
			if (this.triggerOnStart)
			{ this.RandomizeValues(); }
		}

		private void OnEnable ()
		{
			if (this.triggerOnEnable)
			{ this.RandomizeValues(); }
		}
	//ENDOF MonoBehaviour

	//private members
		private void RandomizeValues ()
		{
			Quaternion randomValue = this.GenerateRandomRotation();

			foreach (Transform targetTransform in this.targetTransformList)
			{
				if (!this.sameValueForAll)
				{ randomValue = this.GenerateRandomRotation(); }

				targetTransform.rotation = randomValue;
			}
		}

		private Quaternion GenerateRandomRotation ()
		{
			return Quaternion.Euler(x: 0, y: 0, z: this.degreesRange.random);
		}
	//ENDOF members
	}
}