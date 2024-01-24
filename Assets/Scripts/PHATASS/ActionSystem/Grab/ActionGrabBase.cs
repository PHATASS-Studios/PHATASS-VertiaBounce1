using UnityEngine; //Physics, Transform, SpringJoint, ...

using static PHATASS.Utils.Extensions.TransformJointRiggingExtensions;
using static PHATASS.Utils.Extensions.JointConfigurationExtensions;

using ControllerCache = PHATASS.ControllerSystem.ControllerCache;

//using AnimationNames = PHATASS.Constants.AnimationNames;
using EButtonInputState = PHATASS.InputSystem.EButtonInputState;

//using static PHATASS.Utils.Extensions.JointConfigurationExtensions;
//using static PHATASS.Utils.Extensions.ColliderConfigurationExtensions;  //Collider.EMGetColliderTransformOffset(this Collider);

namespace PHATASS.ActionSystem
{
	//Base grabbing action
	//must be extended providing a settings types
	public abstract class ActionGrabBase<TGrabSettingsPackage> :
		ActionRadialBase<TGrabSettingsPackage>,
		IActionGrab
		where TGrabSettingsPackage : PHATASS.SettingSystem.ISettingsPackageActionGrab
			<TGrabSettingsPackage>
	{
	//protected method overrides
		//returns true if this action is currently doing something, like maintaining a grab or repeating a slapping pattern
		protected override bool ongoing
		{ get { return (base.ongoing || this.grabActive); }}

		//returns true if this action is currently doing something, like maintaining a grab or repeating a slapping pattern
		//Will be true if base.ongoing (because automated) or if we have a joint list acting upon the world
		//receive state of corresponding input medium
		protected override void Input (EButtonInputState state)
		{
			if (state == EButtonInputState.Started)
			{
				this.InitiateGrab();
			}
			if (state == EButtonInputState.Ended)
			{
				this.FinishGrab();
			}
		}

		//clears and finishes the action
		protected override void Clear ()
		{
			this.RemoveJoints();
			base.Clear();
		}

		//returns true if the action can be legally activated at its position
		protected override bool IsValid ()
		{
			/////////////////////////////////////////////////////////////////////////////////////////////////////
			//[TO-DO] optimize initialization by keeping a copy of bone list?
			//[TO-DO] consider OverlapCircleNonAlloc for fast validity checks too
			/////////////////////////////////////////////////////////////////////////////////////////////////////			
			return (this.CountCollidersInRange() > 0);
		}

		//action update. Must be called once per frame.
		protected override void ActionUpdate ()
		{ }

		//try to set in automatic state. Returns true on success
		protected override bool Automate ()
		{
			this.auto = this.grabActive;
			return this.auto;
		}

		protected override void DeAutomate ()
		{
			this.auto = false;
		}
	//ENDOF protected method overrides

	//private methods
	  //Grab Action Implementation
		//list of currently in-use joints
		private UnityEngine.Component[] jointList;
		//determines if grab is active
		private bool grabActive { get { return (this.jointList != null); }}

		//initiate grabbing action
		private void InitiateGrab ()
		{
			this.CreateJoints(grabbables: this.GetComponentsInRange<IGrabbable>());
		}

		//End grabbing action
		private void FinishGrab ()
		{
			this.Clear();
		}
	  //end Grab Action Implementation

	  //Grab Action support methods
		// Creates the joints necessary to grab a list of grabbables
		private void CreateJoints (IGrabbable[] grabbables)
		{
			//force purge joint list and create a new list
			this.RemoveJoints();
			this.jointList = new UnityEngine.Component[grabbables.Length];

			//create and store the joints
			for (int i = 0, iLimit = grabbables.Length; i < iLimit; i++)
			{ this.jointList[i] = grabbables[i].CreateGrabJoint(this.tool.gameObject); }
		}

		//Remove all joints currently in use
		private void RemoveJoints ()
		{
			if (this.jointList != null)
			{
				foreach (UnityEngine.Component joint in this.jointList)
				{ UnityEngine.Object.Destroy(joint); }
				this.jointList = null;
			}
		}
	  //ENDOF Grab Action support methods
	}
}
/* removed code delete
		//create joints required from the tool gameobject to each target
		private void CreateJoints (Collider[] targets)
		{
			//clear joint list and create a new list
			this.RemoveJoints();
			this.jointList = new ConfigurableJoint[targets.Length];

			//create a joint for each target
			for (int i = 0, iLimit = targets.Length; i < iLimit; i++)
			{
				this.jointList[i] = this.CreateJoint(targets[i]);
			}
		}

		//Create a joint linked to a specific transform
		private ConfigurableJoint CreateJoint (Collider target)
		{
			if (target == null) { Debug.Log("ActionGrabBase.CreateJoint(): received null target, can't create joint"); return null; }

			//fetch settings specific to target gameobject context
			TGrabSettingsPackage settings = ControllerCache.settingsProvider
				.GetSettingsPackage<TGrabSettingsPackage>(target.gameObject);

			//Create the new joint, immediately applying its configuration, and return it
			return this.tool.transform.ESetupJointConnectingTo(
					target: target.transform,
					sample: settings.grabJointSetting.sampleJoint
				);
		}

*/