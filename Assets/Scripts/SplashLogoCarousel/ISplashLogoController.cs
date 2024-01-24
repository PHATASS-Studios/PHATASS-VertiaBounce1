using Vector2 = UnityEngine.Vector2;

namespace External.LogoSplash
{
	public interface ISplashLogoController
	{
		Vector2 targetPosition {set;}	//target position to lerp to
		Vector2 targetScale {set;}		//target scale to lerp towards
		float targetAlpha {set;}		//target sprite alpha to lerp towards

		Vector2 forcePosition {set;}
		Vector2 forceScale {set;}
		float forceAlpha {set;}
	}
}