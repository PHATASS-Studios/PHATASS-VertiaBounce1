using UnityEngine;

using ScreenCapturer = PHATASS.Utils.ScreenUtils.ScreenCapturer;

namespace PHATASS.Miscellaneous
{
	public class SnapshotTaker : MonoBehaviour
	{
	//serialized fields
		[SerializeField]
		[Tooltip("This RenderTexture will be downloaded as PNG when TakeSnapshotEvent is called")]
		private RenderTexture sourceRenderTexture;

		[SerializeField]
		[Tooltip("File name for the downloaded png file")]
		private string filename = "snapshot";
	//ENDOF serialized fields

	//public events
		public void TakeSnapshotEvent ()
		{ this.TakeSnapshot(); }
	//ENDOF public events

	//private
		private void TakeSnapshot ()
		{
			DownloadSnapshot(this.sourceRenderTexture);
		}

		private void DownloadSnapshot (RenderTexture renderTexture)
		{
			Debug.LogError("!! SnapshotTaker.DownloadSnapshot() unimplemented!!!!!");
		//!!	PHATASS.Utils.ImageExporters.PngExporter.ExportAsPNG(renderTexture, this.filename);
		}
	//ENDOF private
	}
}