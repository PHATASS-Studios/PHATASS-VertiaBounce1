using UnityEngine;

namespace External.LogoSplash
{
	// Class managing a splash of logos appearing in order into a circular array
	public class RadialLogoCarousel : MonoBehaviour
	{
	//serialized fields
		[SerializeField]
		private float initialDelay = 0.1f;

		[SerializeField]
		private float startingAngle = 0f;

		[SerializeField]
		private LogoCarouselStep[] initialAnimationSteps;

		[SerializeField]
		private LogoCarouselStep[] finalAnimationSteps;		

		[SerializeField]
		private LogoLerpController[] logoArray;

		[Tooltip("Callbacks executed when animation is finished")]
		[SerializeField]
		private UnityEngine.Events.UnityEvent DoOnAnimationFinished;
	//ENDOF serialized fields

	//private fields
		[SerializeField]
		private float timer = 0.0f;

		private int currentLogo = 0; //indicates which logo is currently animating. if == logoArray.Length, means every logo should be at final position
		private int currentStep = 0; //current step of animation

		private bool running = true;
	//ENDOF private fields

	//MonoBehaviour lifecycle
		public void Start ()
		{
			InitializeLogos();
			timer = initialDelay;
			currentLogo = 0;
			currentStep = -1;
		}

		public void Update ()
		{
			if (!running) { return; }

			//execute every animation step in order for each logo. finally execute final animation for every logo at once
			if (timer <= 0f)
			{
				currentStep++;

				// if we are still processing initial animations check to loop through each logo
				if (currentStep >= initialAnimationSteps.Length && currentLogo < logoArray.Length)
				{
					currentLogo++;
					currentStep = 0;
				}
				ApplyAnimationStep(index: currentLogo, step: currentStep);
			}

			timer -= Time.deltaTime;
		}		
	//ENDOF MonoBehaviour

	//private methods
		private void InitializeLogos ()
		{
			for (int i = 0, iLimit = logoArray.Length; i < iLimit; i++) 
			{
				logoArray[i].forcePosition = PositionByRadiusAndIndex(
					radius: initialAnimationSteps[0].radius,
					index: i
				);
				logoArray[i].forceScale = Vector2.one * initialAnimationSteps[0].scale;
				logoArray[i].forceAlpha = initialAnimationSteps[0].alpha;
			}
		}

		private void ApplyAnimationStep (int index, int step)
		{
			//if index is outside of range, we are in the final animation
			if (index >= logoArray.Length)
			{
				//if outside of the final animation range, we've finished the splash
				if (step >= finalAnimationSteps.Length)
				{
					SplashFinished();
				}
				else for (int i = 0, iLimit = logoArray.Length; i < iLimit; i++)
				{
					timer = ApplyStep(index: i, step: finalAnimationSteps[step]);
				}
			}
			else
			{
				timer += ApplyStep(index: index, step: initialAnimationSteps[step]);
			}
		}

		private float ApplyStep (int index, LogoCarouselStep step)
		{
			logoArray[index].targetPosition = PositionByRadiusAndIndex(index: index, radius: step.radius);
			logoArray[index].targetScale = Vector2.one * step.scale;
			logoArray[index].targetAlpha = step.alpha;

			return step.duration;
		} 

		private float AngleByIndex (int index)
		{
			return startingAngle + ((360/logoArray.Length) * index);
		}

		private Vector2 PositionByRadiusAndIndex (int index, float radius)
		{
			return new Vector2(
				x: Mathf.Sin(Mathf.Deg2Rad * AngleByIndex(index)) * radius,
				y: Mathf.Cos(Mathf.Deg2Rad * AngleByIndex(index)) * radius
			);
		}

		private void SplashFinished ()
		{
			this.running = false;
			Debug.LogWarning("RadialLogoCarousel.SplashFinished()");
			this.DoOnAnimationFinished.Invoke();
		}
	//ENDOF private methods
	}

	[System.Serializable]
	public struct LogoCarouselStep
	{
		public float radius;
		public float duration;
		public float alpha;
		public float scale;
	}
}