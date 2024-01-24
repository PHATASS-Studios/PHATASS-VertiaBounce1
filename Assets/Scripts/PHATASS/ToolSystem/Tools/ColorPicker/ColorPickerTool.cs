using IColorPickerController = PHATASS.ConfiguratorSystem.IColorPickerController;

using ControllerCache = PHATASS.ControllerSystem.ControllerCache;
using ScreenCapturer = PHATASS.Utils.ScreenUtils.ScreenCapturer;

using Color = UnityEngine.Color;

namespace PHATASS.ToolSystem.Tools
{
	public class ColorPickerTool : ToolBase, IColorPickerTool
	{
	//serialized fields
		[UnityEngine.SerializeField]
		[UnityEngine.Tooltip("time, in seconds, between each update of the preview color change")]
		private float previewUpdateInterval = 0.15f;
	//ENDOF serialized fields

	//IColorPickerTool
		IColorPickerController IColorPickerTool.linkedController
		{
			get { return this.controller; }
			set { this.controller = value; }
		}
			private IColorPickerController controller;
	//ENDOF IColorPickerTool

	//MonoBehaviour Lifecycle implementation
		protected override void Update ()
		{
			this.UpdatePreview();
			base.Update();
		}
	//ENDOF MonoBehaviour Lifecycle implementation

	//ToolBase implementation
		protected override void InputStarted ()
		{
		}

		protected override void InputHeld ()
		{
		}

		protected override void InputEnded ()
		{
			this.FinalizeChoice();
		}
	//ENDOF ToolBase implementation

	//method overrides
		protected override void LostFocus ()
		{
			this.SelfDestruct();
		}
	//ENDOF method overrides

	//private properties
		private UnityEngine.Vector2 colorPickingScreenPosition
		{ get { return ControllerCache.viewportController.WorldSpaceToScreenSpace(this.transform.position); }}
	//ENDOF private properties

	//private fields
		private float previewUpdateTimer = 0f;
		private bool active = true;
	//ENDOF private fields

	//private methods
		private void UpdatePreview ()
		{
			if (!active) { return; }

			if (this.previewUpdateTimer > 0)
			{ this.previewUpdateTimer -= UnityEngine.Time.unscaledDeltaTime; }
			else
			{
				this.previewUpdateTimer += this.previewUpdateInterval;
				this.StartCoroutine(ScreenCapturer.GetScreenPixelColorAsync(
					pixelPosition: this.colorPickingScreenPosition,
					callback: (Color color) => { this.controller.previewColor = color; }
				));
			}
		}

		private void FinalizeChoice ()
		{
			if (!active) { return; }

			active = false;
			this.StartCoroutine(ScreenCapturer.GetScreenPixelColorAsync(
				pixelPosition: this.colorPickingScreenPosition,
				callback: (Color color) =>
				{
					this.controller.finalChoice = color;
					this.SelfDestruct();
				}
			));
		}
	//ENDOF private methods
	}
}
