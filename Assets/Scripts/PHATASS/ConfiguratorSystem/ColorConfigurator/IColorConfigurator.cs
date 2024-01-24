using Color = UnityEngine.Color;

namespace PHATASS.ConfiguratorSystem
{
	public interface IColorConfigurator : IConfigurator
	{
		Color color { get; set; }
	}
}