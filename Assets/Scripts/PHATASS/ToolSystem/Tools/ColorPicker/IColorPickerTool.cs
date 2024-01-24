using IColorPickerController = PHATASS.ConfiguratorSystem.IColorPickerController;

namespace PHATASS.ToolSystem.Tools
{
	public interface IColorPickerTool : ITool
	{
		IColorPickerController linkedController { get; set; }
	}
}