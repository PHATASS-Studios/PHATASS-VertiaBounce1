using SerializeFieldAttribute = UnityEngine.SerializeField;
using TooltipAttribute = UnityEngine.TooltipAttribute;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

using ControllerCache = PHATASS.ControllerSystem.ControllerCache;
using Debug = UnityEngine.Debug;

using Color = UnityEngine.Color;

using IColorPickerTool = PHATASS.ToolSystem.Tools.IColorPickerTool;

namespace PHATASS.ConfiguratorSystem
{
	public class ColorPickerControllerComponent :
		UnityEngine.MonoBehaviour,
		IColorPickerController,
		IColorConfigurator
	{
	//IColorPickerController
		Color IColorPickerController.previewColor { set { this.SetPreview(value); }}
		Color IColorPickerController.finalChoice { set { this.SetColorChoice(value); }}
	//ENDOF IColorPickerController

	//IColorConfigurator
		Color IColorConfigurator.color
		{
			get { return this.colorValue; }
			set { this.colorValue = value; }
		}
	//ENDOF IColorConfigurator

	//serialized fields
		[SerializeField]
		[Tooltip("Prefab sample of the color picker to instantiate")]
		[SerializedTypeRestriction(typeof(IColorPickerTool))]
		private UnityEngine.Object? _prefabColorPicker;
		private IColorPickerTool prefabColorPicker { get { return this._prefabColorPicker as IColorPickerTool; }}

		[SerializeField]
		[Tooltip("This is the handler in charge of effectuating the color change")]
		[SerializedTypeRestriction(typeof(IColorConfigurator))]
		private UnityEngine.Object? _subordinateColorConfigurator;
		private IColorConfigurator subordinateColorConfigurator { get { return this._subordinateColorConfigurator as IColorConfigurator; }}

		[SerializeField]
		[Tooltip("Time (in seconds) preview-only color changes will last for unless updated")]
		private float previewMaximumTime = 0.2f;
	//ENDOF serialized

	//public events
		//When this is called the color picking process will begin
		public void DoBeginColorPick ()
		{
			this.initialColor = this.subordinateColorConfigurator.color;
			this.CreatePicker();
		}
	//ENDOF public events

	//MonoBehaviour lifecycle
		private void Awake ()
		{
			if (this.prefabColorPicker == null) { Debug.LogError("ColorPickerControllerComponent.prefabColorPicker has not been correctly set!!"); }
			if (this.subordinateColorConfigurator == null) { Debug.LogError("ColorPickerControllerComponent.subordinateColorConfigurator has not been correctly set!!"); }

		}

		private void Update ()
		{
			this.CountDownPreview();
		}
	//ENDOF MonoBehaviour lifecycle

	//private properties
		protected virtual Color colorValue
		{
			get { return this.subordinateColorConfigurator.color; }
			set { this.subordinateColorConfigurator.color = value; }
		}
	//ENDOF private properties

	//private fields
		private Color initialColor;

		private float previewTimer = 0f;
		private bool isPreviewing = false;
	//private fields

	//private methods
		private void CreatePicker ()
		{
			IColorPickerTool picker = (ControllerCache.toolManager.SetAsActiveTool(
				tool: this.prefabColorPicker,
				forceInstantiateCopy: true
			) as IColorPickerTool);

			picker.linkedController = this;
		}

		protected void SetColorChoice (Color desiredColor)
		{
			this.colorValue = desiredColor;
			this.initialColor = desiredColor;
			this.isPreviewing = false;
		}

		private void SetPreview (Color previewColor)
		{
			this.colorValue = previewColor;
			this.previewTimer = this.previewMaximumTime;
			this.isPreviewing = true;
		}

		private void RevertPreview ()
		{
			this.colorValue = this.initialColor;
			this.isPreviewing = false;
		}

		private void CountDownPreview ()
		{
			if (this.isPreviewing)
			{
				if (this.previewTimer > 0)
				{ this.previewTimer -= UnityEngine.Time.unscaledDeltaTime; }
				else
				{ this.RevertPreview(); }
			}
		}
	//ENDOF private methods
	}
}