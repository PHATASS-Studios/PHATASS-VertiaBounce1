using UnityEngine;
using PHATASS.Utils.Types.Values;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

namespace PHATASS.Utils.Types.Values.ValueTools
{
// this behaviour exposes a value that is dictated by a given curve, by taking the point in time T from a referenced IFloatValue
//	Exposes IFloatValue
	public class FloatValueFromCurveBehaviour:
		BaseFloatValueFromCurveBehaviour,
		IFloatValue
	{}
}
