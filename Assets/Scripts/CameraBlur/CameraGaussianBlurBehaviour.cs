using UnityEngine;

namespace PHATASS.CameraSystem.CameraFX
{
	[ExecuteInEditMode]
	public class CameraGaussianBlurBehaviour :
		MonoBehaviour,
		PHATASS.Utils.Types.Values.IFloatValueMutable
	{
	//serialized
		[SerializeField]
		private float sigma;

		[SerializeField]
		private GaussianQuality quality;
	//ENDOF serialized

	//IFloatValueMutable
		float PHATASS.Utils.Types.Values.IValue<float>.value { get { return this.sigma; }}
		float PHATASS.Utils.Types.Values.IValueMutable<float>.value
		{
			get { return this.sigma; }
			set { this.sigma = value; }
		}
	//ENDOF IFloatValueMutable

	//MonoBehaviour lifecycle
		private void OnRenderImage (RenderTexture source, RenderTexture destination)
		{
			if (this.sigma == 0)
			{
				Graphics.Blit(source, destination);
				//RenderTexture.active = source;
				return;
			}

			this.material.SetFloat("_Sigma", this.sigma);
			Graphics.Blit(source, destination, this.material);
		}

		private void OnValidate() { this.Init (); }
		private void OnEnable() { this.Init (); }
	//ENDOF MonoBehaviour

	//private members
		private Shader shader;
		private Material material;

		private void Init()
		{
			this.shader = Shader.Find("blurs-remibodin/naive_gaussian_blur");
		
			if (this.shader.isSupported == false)
			{
				this.enabled = false;
				Debug.LogWarning("CameraGaussianBlur: Shader not supported");
				return;
			}

			this.material = new Material(this.shader);
			this.material.EnableKeyword(this.quality.ToString());
		}
	//ENDOF private

	//private enums
		private enum GaussianQuality
		{
			LITTLE_KERNEL,
			MEDIUM_KERNEL//,
			//BIG_KERNEL	//BIG_KERNEL disabled: "Some graphic's driver crash with Algo.NAIVE and Quality.BIG_KERNEL"
		};
	//ENDOF enums
	}

}