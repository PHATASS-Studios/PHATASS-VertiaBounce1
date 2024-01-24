using UnityEngine;

using IEnumerator = System.Collections.IEnumerator;

using RandomFloatRange = PHATASS.Utils.Types.Ranges.RandomFloatRange;
using TRandomFloatRange = PHATASS.Utils.Types.Ranges.ILimitedRange<System.Single>;

namespace PHATASS.Miscellaneous.Kickers
{
	public abstract class OnOffFlickerBase : KickerOnConditionHeldOnUpdateBase
	{
	//serialized fields
		[Tooltip("time each flick up stays active")]
		[SerializeField]
		private RandomFloatRange _randomUptimeRange = new RandomFloatRange(1, 1);
		private TRandomFloatRange randomUptimeRange { get { return this._randomUptimeRange; }}

		[Tooltip("This is the value corresponding to a flick down, false by default. The oposite of this is the value for an up flick")]
		[SerializeField]
		private bool downState = false;
	//ENDOF serialized fields 

	//private fields and properties
		//what's considered as the flicker's up state
		private bool upState { get { return !downState; }}

		//timer left
		private float uptimeLeft = 0f;
		protected bool flickIsUp = false;
	//ENDOF private fields and properties

	//abstract property definition
		protected abstract bool state { set; }
	//ENDOF abstract property definition

	//MonoBehaviour lifecycle
		public virtual void Awake ()
		{
			if (!flickIsUp) { state = downState; }
		}
	/*
		public override void Update ()
		{
			base.Update();

		}
	*/
	//ENDOF MonoBehaviour lifecycle

	//IKicker implementation
		//executes a momentary effect
		public override void Kick ()
		{ FlickUp(); }
	//ENDOF IKicker implementation

	//public method definition
		public void FlickUp ()
		{ FlickUp(randomUptimeRange.random); }
		public void FlickUp (float uptime)
		{
			if(!flickIsUp)
			{ StartCoroutine(FlickUpCoroutine()); }
			else
			{ UpdateFlickTimer(); }

			IEnumerator FlickUpCoroutine ()
			{
				uptimeLeft = uptime;
				flickIsUp = true;
				state = upState;

				while (uptimeLeft > 0)
				{
					yield return null;
					uptimeLeft -= Time.deltaTime;
				}

				flickIsUp = false;
				state = downState;
			}

			void UpdateFlickTimer ()
			{
				uptimeLeft = (uptimeLeft > uptime)
								? uptimeLeft
								: uptime;
			}
		}
	//ENDOF public method definition
	}
}