using UnityEngine;

using static PHATASS.Utils.Types.Values.IVector2ValueExtensions;

using Vector2Axis = PHATASS.Utils.Types.Values.Vector2Axis;

using Values = PHATASS.Utils.Types.Values;
using IVector2Value = PHATASS.Utils.Types.Values.IVector2Value;
using IFloatValue = PHATASS.Utils.Types.Values.IFloatValue;
using IDoubleValue = PHATASS.Utils.Types.Values.IDoubleValue;
using IIntValue = PHATASS.Utils.Types.Values.IIntValue;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

namespace PHATASS.Miscellaneous.ValueAdapters
//[TO-DO]: change namespace to PHATASS.Utils.Types.Values.ValueTools?
{
// IFloatValue behaviour exposing a single axis of a given IVector2Value
//	value is casted and accessible as float, double, and int
//	when casting to int, this value is rounded towards zero to the nearest integral value
	public class Vector2ToSingleAxisValueAdapterBehaviour :
		MonoBehaviour,
		IFloatValue,
		IDoubleValue,
		IIntValue
	{
	//serialized fields
		[Tooltip("IVector2Value source. Chosen axis of this Vector2 value will be exposed as IFloatValue/IDoubleValue/IIntValue.")]
		[SerializeField]
		[SerializedTypeRestriction(typeof(IVector2Value))]
		private UnityEngine.Object? _vector2ValueSource = null;
		private IVector2Value vector2ValueSource
		{ get {
			if (this._vector2ValueSource == null) { return null; }
			else { return this._vector2ValueSource as IVector2Value; }
		}}

		[Tooltip("Axis this adapter exposes as as IFloatValue/IDoubleValue/IIntValue.")]
		[SerializeField]
		private Vector2Axis desiredAxis;
	//ENDOF serialized fields

	//IFloatValue
		float Values.IValue<float>.value { get { return this.desiredAxisValue; }}
	//ENDOF IFloatValue

	//IDoubleValue
		double Values.IValue<double>.value { get { return (double) this.desiredAxisValue; }}
	//ENDOF IDoubleValue

	//IIntValue
		int Values.IValue<int>.value { get { return (int) this.desiredAxisValue; }}
	//ENDOF IIntValue

	//private members
		private float desiredAxisValue
		{ get { return this.vector2ValueSource.EVector2ValueToAxisFloatValue(this.desiredAxis).value; }}
	//ENDOF private
	}
}