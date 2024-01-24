using UnityEngine;

namespace PHATASS.ConfiguratorSystem
{
	public abstract class BaseConfiguratorRandomizerComponent : MonoBehaviour, IConfiguratorRandomizer
	{
	//Serialized fields
		[SerializeField]
		[Tooltip("Randomization will trigger during OnAwake if this is true")]
		private bool randomizeOnAwake = true;
	//ENDOF Serialized

	//IRandomizer
		void IRandomizer.Randomize ()
		{ this.Randomize(); }
	//ENDOF IRandomizer

	//public events
		public void DoRandomize ()
		{ this.Randomize(); }

	//MonoBehaviour
		protected virtual void Awake ()
		{
			if (this.randomizeOnAwake) { this.Randomize(); }
		}
	//ENDOF MonoBehaviour

	//abstract methods
		protected abstract void Randomize ();
	//ENDOF abstract
	}
}