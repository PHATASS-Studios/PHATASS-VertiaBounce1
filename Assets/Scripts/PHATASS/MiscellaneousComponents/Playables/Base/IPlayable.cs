namespace PHATASS.Miscellaneous.Playables
{
//interface representing an object that propagates a Play() call to one or all of a list of playables
	public interface IPlayable
	{
		public void Play ();		//default play behaviour
	}

	public interface IPlayablePropagator : IPlayable
	{
		public void PlayAll ();				//calls Play() on all playables
		public void PlayOne (int target);	//calls Play() on playable with target index. If index < 0, one is chosen randomly
		public void PlayRandom ();			//calls Play() on one random playable
	}
}