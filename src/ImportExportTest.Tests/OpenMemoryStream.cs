using System.IO;

namespace ImportExportTest.Tests
{
	public class OpenMemoryStream : MemoryStream
	{
		protected override void Dispose(bool disposing)
		{
			//base.Dispose(disposing);
		}

		public override void Close()
		{
			//base.Close();
		}

		public void ResetToStartPosition()
		{
			this.Seek(0, SeekOrigin.Begin);
		}

		public void DisposeActual(bool disposing)
		{
			base.Dispose(disposing);
		}

		public void CloseActual()
		{
			base.Close();
		}
	}
}
