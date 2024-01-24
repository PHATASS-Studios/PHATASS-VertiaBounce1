using UnityEngine;

using TMP_Text = TMPro.TMP_Text;

using IIntAccumulator = PHATASS.Utils.Types.IIntAccumulator;

using SerializedTypeRestrictionAttribute = PHATASS.Utils.Attributes.SerializedTypeRestrictionAttribute;

namespace PHATASS.Miscellaneous
{
	public class IntAccumulatorToTextMeshProLabel : UnityEngine.MonoBehaviour
	{
		[Tooltip("The integer value of this IIntAccumulator is written to textArea TextMeshPro component")]
		[SerializeField]
		[SerializedTypeRestriction(typeof(IIntAccumulator))]
		private UnityEngine.Object _accumulator = null;
		private IIntAccumulator accumulator
		{ get { return this._accumulator as IIntAccumulator; }}

		[SerializeField]
		private TMP_Text textArea;

		private void Update ()
		{
			this.textArea.SetText(this.accumulator.value.ToString());
		}

		private void Reset ()
		{
			if (this.textArea == null) { this.textArea = this.GetComponent<TMP_Text>(); }
		}
	}
}