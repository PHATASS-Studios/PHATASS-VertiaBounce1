using UnityEngine;



using IIntAccumulator = PHATASS.Utils.Types.IIntAccumulator;

namespace PHATASS.Miscellaneous
{
	public class IntAccumulatorBehaviour : MonoBehaviour, IIntAccumulator
	{
	//Serialized fields
		[Tooltip("Accumulator values")]
		[SerializeField]
		private PHATASS.Utils.Types.IntAccumulator _accumulator;
		private IIntAccumulator accumulator;
	//ENDOF Serialized fields

	//IAccumulator<int>
		void PHATASS.Utils.Events.ISimpleEventReceiver<int>.Event (int param0)
		{
			this.accumulator.Event(param0);
			//Debug.Log(this.name + " added: " + param0 + " total: " + this.accumulator.value);
		}
	//ENDOF IAccumulator<int>

	//IValue<int>
		int PHATASS.Utils.Types.Values.IValue<int>.value { get { return this.accumulator.value; }}
	//ENDOF IValue<int>

	//IResettable
		void PHATASS.Utils.Types.IResettable.Reset ()
		{ this.accumulator.Reset(); }
	//ENDOF IResettable

	//MonoBehaviour
		private void Awake ()
		{
			this.accumulator = this._accumulator;
		}
	//ENDOF MonoBehaviour
	}
}
