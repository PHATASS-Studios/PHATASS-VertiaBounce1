namespace PHATASS.ControllerSystem
{
	//[TO-DO] Consider renaming IPublicController?
	public interface IController
	{
		//should return false when stale
		bool isValid {get;}
	}
}