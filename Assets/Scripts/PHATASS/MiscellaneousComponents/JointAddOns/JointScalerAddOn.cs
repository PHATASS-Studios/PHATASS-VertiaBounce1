using UnityEngine;

using static PHATASS.Utils.Extensions.Vector3Extensions;

namespace PHATASS.Miscellaneous
{
	public class JointScalerAddOn : MonoBehaviour
	{
	//serialized fields
		[SerializeField]
		[Tooltip("The scale of this transform will be considered as the reference scale. When that transform's global scale grows, the joints will \"grow\" proportionally.")]
		private Transform referenceTransform;

		[SerializeField]
		[Tooltip("If true, will scale the joints anchor position offset.")]
		private bool scaleAnchor = true;

		[SerializeField]
		[Tooltip("If true, will scale the joints connectedAnchor position offset.")]
		private bool scaleConnectedAnchor = true;

		//[TO-DO]: MAYBE add more properties to scale?

		[SerializeField]
		[Tooltip("These are the joints that will be scaled up and down as referenceTransform's global scale changes. Note reference scale is calculated on Awake(), so this list must not be updated at run-time")]
		private Joint[] targetJoints;
	//ENDOF serialized fields

	//MonoBehaviour
		private void Awake ()
		{
			if (this.referenceTransform == null) { this.referenceTransform = this.GetComponent<Transform>(); }

			this.InitializeReferenceCaches();
		}

		private void InitializeReferenceCaches ()
		{
			this.anchorReferenceCache = new Vector3[this.targetJoints.Length];
			this.connectedAnchorReferenceCache = new Vector3[this.targetJoints.Length];

			Vector3 currentScale = this.referenceScaleVector3;

			for (int i = 0, iLimit = this.targetJoints.Length; i < iLimit; i++)
			{
				//first it is necessary to disable auto-configuration to avoid it overriding final scale
				this.targetJoints[i].autoConfigureConnectedAnchor = false;

				this.anchorReferenceCache[i] = this.targetJoints[i].anchor.EDivideBy(currentScale);
				this.connectedAnchorReferenceCache[i] = this.targetJoints[i].connectedAnchor.EDivideBy(currentScale);
			}
		}

		private void LateUpdate ()
		{
			this.UpdateJointsScale();
		}

		private void UpdateJointsScale ()
		{
			Vector3 currentScale = this.referenceScaleVector3;
			for (int i = 0, iLimit = this.targetJoints.Length; i < iLimit; i++)
			{
				if (this.scaleAnchor)
				{ this.targetJoints[i].anchor = this.anchorReferenceCache[i].EMultiplyBy(currentScale); }

				if (this.scaleConnectedAnchor)
				{ this.targetJoints[i].connectedAnchor = this.connectedAnchorReferenceCache[i].EMultiplyBy(currentScale); }
			}
		}
	//ENDOF MonoBehaviour

	//private members
		//calculates and returns current reference scale
		private Vector3 referenceScaleVector3
		{ get {
			// !! Maybe lossy scale is not good enough - maybe it is
			return this.referenceTransform.localScale;
		}}

		//these caches store the desired relevant values for a baseline scale of 1
		private Vector3[] anchorReferenceCache;
		private Vector3[] connectedAnchorReferenceCache;
	//ENDOF private
	}
}