using UnityEngine;

using IIntAccumulator = PHATASS.Utils.Types.IIntAccumulator;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

namespace PHATASS.Miscellaneous
{
	public class IntAccumulatorToLegacyTextLabel : UnityEngine.MonoBehaviour
	{
		[Tooltip("The integer value of this IIntAccumulator is written to textArea legacy UI.Text")]
		[SerializeField]
		[SerializedTypeRestriction(typeof(IIntAccumulator))]
		private UnityEngine.Object _accumulator = null;
		private IIntAccumulator accumulator
		{ get { return this._accumulator as IIntAccumulator; }}

		[SerializeField]
		private UnityEngine.UI.Text textArea;

		private void Update ()
		{
			this.textArea.text = this.accumulator.value.ToString();
		}
	}
}