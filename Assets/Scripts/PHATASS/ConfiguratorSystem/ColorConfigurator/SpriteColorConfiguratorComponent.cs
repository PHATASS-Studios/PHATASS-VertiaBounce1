using UnityEngine;

using static PHATASS.Utils.Extensions.IEnumerableExtensions;

namespace PHATASS.ConfiguratorSystem
{
	[System.Serializable]
	public class SpriteColorConfiguratorComponent :
		BaseConfiguratorComponent,
		IColorConfigurator
	{
	//serialized fields
		[SerializeField]
		[Tooltip("Color tint applied to desired sprites")]
		private Color tint = Color.white;

		[SerializeField]
		[Tooltip("Sprite(s) this configurator alters")]
		private SpriteRenderer[] managedSpriteRenderers;
	//ENDOF serialized fields

	//ISpriteColorConfigurator
		Color IColorConfigurator.color
		{
			get { return this.tint; }
			set
			{
				this.tint = value;
				this.UpdateConfiguration();
			}
		}
	//ENDOF ISpriteColorConfigurator	

	//MonoBehaviour lifecycle
		protected override void Awake ()
		{
			if (!this.managedSpriteRenderers.EMExistsAndContainsAnything())
			{ Debug.LogError("BaseSpriteRendererConfiguratorComponent " + this.gameObject.name + " managedSpriteRenderers missing!"); }

			base.Awake();
		}
	//ENDOF MonoBehaviour lifecycle

	//inherited method overrides
	//ENDOF overrides

	//private methods
		protected override void ApplyState ()
		{
			foreach (SpriteRenderer spriteRenderer in this.managedSpriteRenderers)
			{
				if (spriteRenderer != null)
				{ spriteRenderer.color = this.tint; }
			}
		}
	//ENDOF private methods
	}
}