using UnityEngine;

using IToggleable = PHATASS.Utils.Types.Toggleables.IToggleable;


namespace PHATASS.ConfiguratorSystem
{
	public class AnimatorBoolTogglerComponent : MonoBehaviour, IToggleableConfigurator
	{
	//Serialized fields
		[SerializeField]
		private Animator animator;

		[Tooltip("Variable name of the boolean state to set on the animator")]
		[SerializeField]
		private string boolVariableName = "AnimatorBoolVariableName";
	//ENDOF Serialized fields

	//IToggleable
		bool IToggleable.state { get { return this.state; } set { this.state = value; }}
		private bool state
		{
			get { return this.animator.GetBool(this.boolVariableHash); }
			set { this.animator.SetBool(this.boolVariableHash, value); }
		}
	//ENDOF IToggleable

	//IToggleableConfigurator
		bool IToggleableConfigurator.ToggleEnabled ()
		{
			this.state = !this.state;
			return this.state;
		}
	//ENDOF IToggleableConfigurator

	//MonoBehaviour lifecycle
		private void Awake ()
		{
			if (this.animator == null)
			{
				this.animator = this.GetComponent<Animator>();
				if (this.animator == null) { Debug.LogWarning(this.name + " AnimatorBoolTogglerComponent animator not set"); }
			}

			this.boolVariableHash = Animator.StringToHash(this.boolVariableName);
		}
	//ENDOF MonoBehaviour

	//private fields
		private int boolVariableHash;
	//ENDOF private fields
	}
}