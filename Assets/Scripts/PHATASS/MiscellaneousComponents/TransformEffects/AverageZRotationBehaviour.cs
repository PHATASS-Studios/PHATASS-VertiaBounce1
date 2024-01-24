using UnityEngine;

using static PHATASS.Utils.Types.Angles.IAngle2DFactory;

namespace PHATASS.Miscellaneous.TransformEffects
{
	public class AverageZRotationBehaviour : MonoBehaviour
	{
	//serialized fields
		[Tooltip("Transforms whose Z rotation will be set to the average Z rotation of sample transforms")]
		[SerializeField]
		private Transform[] subjectTransforms;

		[Tooltip("Sample transforms and their weights")]
		[SerializeField]
		private WeightedTransform[] sampleTransforms;

		[Tooltip("If true use transforms local rotation instead of global rotation")]
		[SerializeField]
		private bool useLocalRotation = false;

		[Tooltip("Degrees used for X and Y axis")]
		[SerializeField]
		private float defaultDegreesX = 0f;
		
		[Tooltip("Degrees used for X and Y axis")]
		[SerializeField]
		private float defaultDegreesY	 = 0f;
	//ENDOF serialized

	//MonoBehaviour lifecycle
		private void Update ()
		{
			this.UpdateRotations();
		}
	//ENDOF MonoBehaviour lifecycle

	//private methods
		private void UpdateRotations ()
		{
			Quaternion desiredRotation = this.CalculateAverageRotation();

			foreach (Transform subjectTransform in this.subjectTransforms)
			{
				if (this.useLocalRotation) { subjectTransform.localRotation = desiredRotation; }
				else  { subjectTransform.rotation = desiredRotation; }
			}
		}

		private Quaternion CalculateAverageRotation ()
		{
			float totalDegrees = 0f;
			float totalWeight = 0f;

			foreach (WeightedTransform sampleTransform in this.sampleTransforms)
			{
				//scale the rotation by weight and store total weight to calculate final average
				float sampleDegrees = this.useLocalRotation
					? sampleTransform.transform.localRotation.eulerAngles.z
					: sampleTransform.transform.rotation.eulerAngles.z;

				//ensure degrees stay in the 0-360 range
				sampleDegrees = sampleDegrees.EDegreesToAngle2D().degrees;

				totalDegrees += sampleDegrees * sampleTransform.weight;
				totalWeight += sampleTransform.weight;
			}

			//float totalAverage { get { return totalDegrees/totalWeight; }}

			return Quaternion.Euler(new Vector3(
				x: this.defaultDegreesX,
				y: this.defaultDegreesY,
				z: totalDegrees/totalWeight
			));
		}
	//ENDOF private methods

	//private types
		[System.Serializable]
		private class WeightedTransform
		{
			[SerializeField]
			public Transform transform = null;

			[SerializeField]
			public float weight = 1f;

			public WeightedTransform (Transform transform, float weight)
			{
				this.transform = transform;
				this.weight = weight;
			}

		}
	//ENDOF private types
	}
}