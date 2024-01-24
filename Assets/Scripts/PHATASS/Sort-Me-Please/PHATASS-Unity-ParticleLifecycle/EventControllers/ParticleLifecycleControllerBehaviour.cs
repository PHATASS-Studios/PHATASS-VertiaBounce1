using System.Collections.Generic;

using UnityEngine;
using Particle = UnityEngine.ParticleSystem.Particle;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

namespace PHATASS.Utils.Events
{
	public class ParticleLifecycleControllerBehaviour : MonoBehaviour
	{
		[Header("This is the particle system lifecycle event controller.\nThe following events will be correspondingly fired for every particle each frame.\n\"IParticleEventReceiver.ParticleEvent(particle)\" method will be invoked for each handler, passing a copy of the triggering particle.")]
	//Serialized fields
		[Tooltip("This is the managed ParticleSystem. Particle lifecycle events will be handled and triggered for each particle in this ParticleSystem")]
		[SerializeField]
		private ParticleSystem managedParticleSystem;

		[Tooltip("Events triggered ONCE for each new particle before their first OnParticleUpdate")]
		[SerializeField]
		[SerializedTypeRestriction(typeof(IParticleEventReceiver))]
		private List<UnityEngine.Object> _onParticleStart = null;
		private IList<IParticleEventReceiver> _onParticleStartAccessor = null;
		private IList<IParticleEventReceiver> onParticleStart
		{ get {
			if (this._onParticleStartAccessor == null && this._onParticleStart != null) //create accessor if unavailable
			{ this._onParticleStartAccessor = new PHATASS.Utils.Types.Wrappers.UnityObjectListCastedAccessor<IParticleEventReceiver>(this._onParticleStart); }
			return this._onParticleStartAccessor;
		}}

		[Tooltip("Events triggered during update for every living particle")]
		[SerializeField]
		[SerializedTypeRestriction(typeof(IParticleEventReceiver))]
		private List<UnityEngine.Object> _onParticleUpdate = null;
		private IList<IParticleEventReceiver> _onParticleUpdateAccessor = null;
		private IList<IParticleEventReceiver> onParticleUpdate
		{ get {
			if (this._onParticleUpdateAccessor == null && this._onParticleUpdate != null) //create accessor if unavailable
			{ this._onParticleUpdateAccessor = new PHATASS.Utils.Types.Wrappers.UnityObjectListCastedAccessor<IParticleEventReceiver>(this._onParticleUpdate); }
			return this._onParticleUpdateAccessor;
		}}

		[Tooltip("Events triggered ONCE, THE NEXT FRAME a particle disappears")]
		[SerializeField]
		[SerializedTypeRestriction(typeof(IParticleEventReceiver))]
		private List<UnityEngine.Object> _onParticleDestroy = null;
		private IList<IParticleEventReceiver> _onParticleDestroyAccessor = null;
		private IList<IParticleEventReceiver> onParticleDestroy
		{ get {
			if (this._onParticleDestroyAccessor == null && this._onParticleDestroy != null) //create accessor if unavailable
			{ this._onParticleDestroyAccessor = new PHATASS.Utils.Types.Wrappers.UnityObjectListCastedAccessor<IParticleEventReceiver>(this._onParticleDestroy); }
			return this._onParticleDestroyAccessor;
		}}

		[Tooltip("The following custom data stream will be used for event handling - REMEMBER TO DISABLE THIS CUSTOM DATA STREAM'S USAGE ON YOUR PARTICLESYSTEM!!")]
		[SerializeField]
		private ParticleSystemCustomData desiredCustomDataStreamID = ParticleSystemCustomData.Custom2;
	//ENDOF Serialized

	//MonoBehaviour lifecycle
		private void Awake ()
		{
			if (this.managedParticleSystem == null) { this.managedParticleSystem = this.gameObject.GetComponent<ParticleSystem>(); }

			this.particleStream = new Particle[this.managedParticleSystem.main.maxParticles];
			this.customDataStream = new List<Vector4>(this.managedParticleSystem.main.maxParticles);
			this.particleCache = new Dictionary<int, ParticleCache>(this.managedParticleSystem.main.maxParticles);
		}

		private void LateUpdate ()
		{
			this.RefreshParticleStream();

			this.UpdateParticles();

			this.RewriteCustomData();
		}
	
		private void UpdateParticles ()
		{
		//do OnStart and OnUpdate during an initial loop over input stream
			for (int i = 0; i < this.streamCount; i++)
			{
		//trigger OnStart and initialize ID for uninitialized
				if (!this.CustomDataIsInitialized(this.customDataStream[i]))
				{
					this.customDataStream[i] = this.GenerateCustomID();
					this.TriggerEvent(particle: this.particleStream[i], receivers: this.onParticleStart);
				}

		//trigger OnUpdate and store/update cached copy
				this.particleCache[this.GetCustomID(this.customDataStream[i])] = new ParticleCache(this.particleStream[i]);
				this.TriggerEvent(particle: this.particleStream[i], receivers: this.onParticleUpdate);
			}

		//finish by checking un-updated particles from the cache to trigger OnDestroy
			//generate a list of keys 
			int[] keys = new int[this.particleCache.Count];
			this.particleCache.Keys.CopyTo(keys, 0);

			foreach (int key in keys)
			{
				ParticleCache item = this.particleCache[key];
				if (!item.isCurrent)
				{
					this.TriggerEvent(particle: item.particle, receivers: this.onParticleDestroy);
					this.particleCache.Remove(key);
				}
			}
		}
	//ENDOF MonoBehaviour

	//private members
		private int uniqueIdCounter = 0;

		private Particle[] particleStream;
		private List<Vector4> customDataStream;
		private int streamCount = 0;

		private Dictionary<int, ParticleCache> particleCache;

		//gets particles and custom data from target particle system, as well as particle count
		private void RefreshParticleStream ()
		{
			this.streamCount = this.managedParticleSystem.GetParticles(this.particleStream);
			//this.managedParticleSystem.GetCustomParticleData(this.customDataStream, this.desiredCustomDataStreamID)
			// temporary debug log, to ensure particle data and custom data are always equally sized - remove or comment this log and replace with previous line
			if (streamCount != this.managedParticleSystem.GetCustomParticleData(this.customDataStream, this.desiredCustomDataStreamID))
			{ Debug.LogError("ParticleLifecycleControllerBehaviour particle stream count and custom data stream count mismatch!!"); }
		}

		//writes updated custom data stream to the particleSystem, so as to store generated IDs
		private void RewriteCustomData ()
		{ this.managedParticleSystem.SetCustomParticleData(this.customDataStream, this.desiredCustomDataStreamID); }

		//CustomData.z represents particle ID. customdata.w is set to 1 to identify it has been given an ID
		private bool CustomDataIsInitialized (Vector4 customData)
		{ return customData.w != 0f; }

		//gets custom ID contained within given customData vector
		private int GetCustomID (Vector4 customData)
		{ return (int) customData.z; }

		//initializes ref passed customData vector with an ID and setting its initialized flag (w component)
		//returns newly given ID
		private Vector4 GenerateCustomID ()
		{
			return new Vector4 (
				x: 0f,
				y: 0f,
				z: ++this.uniqueIdCounter,
				w: 1f
			);
		}

		//triggers the given list of event receivers for given particle
		private void TriggerEvent (Particle particle, IEnumerable<IParticleEventReceiver> receivers)
		{
			foreach (IParticleEventReceiver receiver in receivers)
			{
				try { receiver.Event(particle); }
				catch (System.Exception e) { Debug.LogError(e); }
			}
		}
	//ENDOF private members

	//private sub-types
		private readonly struct ParticleCache
		{
			public readonly Particle particle;
			private readonly int lastUpdate;

			public bool isCurrent { get { return this.lastUpdate == UnityEngine.Time.frameCount; }}

			public ParticleCache (Particle inParticle)
			{
				particle = inParticle;
				lastUpdate = UnityEngine.Time.frameCount;
			}
		}
	//ENDOF sub-types
	}
}