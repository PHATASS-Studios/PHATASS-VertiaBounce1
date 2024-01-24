namespace PHATASS.ConfiguratorSystem
{
	//Interface representing a randomizer - only serves the purpose of triggering a randomization when invoking Randomize()
	public interface IConfiguratorRandomizer : IRandomizer {}

	//IConfiguratorRandomizer should be an empty extension of an IRandomizer interface - move definitions wherever they belong as need arises
	public interface IRandomizer
	{
		void Randomize ();
	}
}
