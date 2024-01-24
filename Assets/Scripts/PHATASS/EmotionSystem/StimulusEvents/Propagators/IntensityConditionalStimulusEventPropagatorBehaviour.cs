using UnityEngine;

namespace PHATASS.EmotionSystem
{
// Stimulus event conditional repropagator
//	Checks that stimulus received meets minimum and/or maximum intensity/depth requirements
//	If all enabled conditions are met, stimulus is propagated
//	If no conditions are enabled, stimulus is always propagated
	public class IntensityConditionalStimulusEventPropagatorBehaviour :
		BaseConditionalStimulusEventPropagatorBehaviour
	{
	//serialized fields
		[Tooltip("If true, stimulus will need to have an intensity equal or greater to intensityMinimum.")]
		[SerializeField]
		private bool checkIntensityMinimum = true;

		[Tooltip("This is the minimum intensity value required, if checkIntensityMinimum is true.")]
		[SerializeField]
		private float intensityMinimum;

		[Tooltip("If true, stimulus will need to have an intensity equal or smaller than intensityMaximum.")]
		[SerializeField]
		private bool checkIntensityMaximum = true;

		[Tooltip("This is the maximum intensity value required, if checkIntensityMaximum is true.")]
		[SerializeField]
		private float intensityMaximum;

		[Tooltip("If true, stimulus will need to have an depth equal or greater to depthMinimum.")]
		[SerializeField]
		private bool checkDepthMinimum = false;

		[Tooltip("This is the minimum depth value required, if checkDepthMinimum is true.")]
		[SerializeField]
		private float depthMinimum;

		[Tooltip("If true, stimulus will need to have an depth equal or smaller than depthMaximum.")]
		[SerializeField]
		private bool checkDepthMaximum = false;

		[Tooltip("This is the maximum depth value required, if checkDepthMaximum is true.")]
		[SerializeField]
		private float depthMaximum;
	//ENDOF serialized

	//overrides
		// Propagation condition checking method. Whenever an event is received, re-propagation will only be performed if this returns true.
		protected override bool CheckCondition (IStimulus param0)
		{
			if (this.checkIntensityMinimum && param0.intensity < this.intensityMinimum)
			{ return false; }

			if (this.checkIntensityMaximum && param0.intensity > this.intensityMaximum)
			{ return false; }

			if (this.checkDepthMinimum && param0.depth < this.depthMinimum)
			{ return false; }
		
			if (this.checkDepthMaximum && param0.depth > this.depthMaximum)
			{ return false; }
			
			return true;
		}
	//ENDOF overrides

	//private
	//ENDOF private
	}
}