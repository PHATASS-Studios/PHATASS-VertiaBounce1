using Color = UnityEngine.Color;

namespace PHATASS.ConfiguratorSystem
{
	public interface IColorPickerController
	{
		Color previewColor { set; }
		Color finalChoice { set; }

		//[??] Maybe we need a PickCancelled() and/or StartListening() methods?
	}
}