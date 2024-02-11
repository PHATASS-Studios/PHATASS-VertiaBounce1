using UnityEngine;

namespace PHATASS.CameraSystem.CameraFX
{
	[ExecuteInEditMode]
	public class CameraPostProcessBehaviour : MonoBehaviour
	{
	//serialized
		[SerializeField]
		private Material material;
	//ENDOF serialized

	//MonoBehaviour lifecycle
		void OnRenderImage (RenderTexture source, RenderTexture destination)
		{
			Graphics.Blit(source, destination, material);
		}
	//ENDOF MonoBehaviour
	}
}