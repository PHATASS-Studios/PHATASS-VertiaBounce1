using PHATASS.Utils.Types.Mergeables;

namespace PHATASS.SettingSystem
{
	public interface ISetting<TSetting> :
		IMergeable<TSetting>
		where TSetting : ISetting<TSetting>
	{
	}
}