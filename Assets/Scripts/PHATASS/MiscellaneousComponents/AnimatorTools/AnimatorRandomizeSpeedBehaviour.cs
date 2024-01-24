using SerializeField = UnityEngine.SerializeField;

using Animator = UnityEngine.Animator;
using Random = UnityEngine.Random;

using RandomFloatRange = PHATASS.Utils.Types.Ranges.RandomFloatRange;
using ILimitedRangeFloat = PHATASS.Utils.Types.Ranges.ILimitedRange<float>;

namespace PHATASS.Miscellaneous.AnimatorTools
{
	//This gives the animator a random delay
	public class AnimatorRandomizeSpeedBehaviour : UnityEngine.MonoBehaviour
	{
	//serialized fields
		[SerializeField]
		private Animator[] animators;

		[SerializeField]
		private RandomFloatRange _speedModifierRange = new RandomFloatRange(0.75f, 1.25f);
		private ILimitedRangeFloat speedModifierRange { get { return this._speedModifierRange; }}
	//ENDOF serialized fields

	//MonoBehaviour lifecycle
		private void Start ()
		{
			this.RandomizeAnimators();

			UnityEngine.Object.Destroy(this);
		}
	//ENDOF MonoBehaviour lifecycle

	//private methods
		//delays this animator
		private void RandomizeAnimators ()
		{
			float speedModifier = this.speedModifierRange.random;

			foreach (Animator animator in this.animators)
			{
				animator.speed *= speedModifier;
			}
		}
	//ENDOF private methods
	}
}