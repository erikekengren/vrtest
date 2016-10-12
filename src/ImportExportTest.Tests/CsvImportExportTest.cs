using ImportExportTest.Core;
using ImportExportTest.Core.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportExportTest.Tests
{
	[TestFixture]
    public class CsvImportExportTest
    {
		#region Variables

		private List<string> _tempFiles = new List<string>();
		
		#endregion

		#region Setup and Teardown

		[TestFixtureTearDown]
		public void TestCleanup()
		{
			foreach (string tempFile in _tempFiles)
			{
				try
				{
					if (File.Exists(tempFile))
						File.Delete(tempFile);
				}
				catch { }
			}
		}
		
		#endregion

		#region Tests

		[Test]
		[Category("Fail")]
		public void Should_throw_on_missing_file()
		{
			string fileName = GetTempFilePath();

			CsvImporter importer = null;

			Assert.Throws<FileNotFoundException>(() => { importer = new CsvImporter(fileName, null); });
		}

		[Test]
		[Category("Fail")]
		public void Should_throw_on_empty_filename()
		{
			CsvImporter importer = null;

			Assert.Throws<ArgumentException>(() => { importer = new CsvImporter(string.Empty, null); });
		}

		[Test]
		[Category("Pass")]
		public void Should_read_columns_accurately_from_first_line()
		{
			string fileName = GetTempFilePath();

			Random random = new Random();

			int columnCount = random.Next(2, 8);
			List<string> columns = new List<string>(columnCount);

			for (int i = 0; i < columnCount; i++)
				columns.Add(TestDataHelper.GetRandomString(random.Next(3,8)));

			// Write test data
			using (FileStream outputStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				using (StreamWriter writer = new StreamWriter(outputStream))
				{
					writer.WriteLine(string.Join(",", columns.ToArray()));
				}
			}

			using (CsvImporter importer = new CsvImporter(fileName, null))
			{
				while (importer.Read())
				{
					IDataItem dataItem = importer.GetNextItem();
				}
			}
		}

		[Test]
		[Category("Pass")]
		public void Should_read_rows_accurately_from_first_line()
		{
			string fileName = GetTempFilePath();

			Random random = new Random();

			int columnCount = random.Next(2, 8);
			int rowCount = 3;

			List<string> columns = new List<string>(columnCount);

			for (int i = 0; i < columnCount; i++)
				columns.Add(TestDataHelper.GetRandomString(random.Next(3, 8)));

			// Write test data
			List<string> row = new List<string>(columnCount);
			using (FileStream outputStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				using (StreamWriter writer = new StreamWriter(outputStream))
				{
					writer.WriteLine(string.Join(",", columns.ToArray()));

					for (int i = 0; i < rowCount; i++)
					{
						row.Clear();

						for (int j = 0; j < columnCount; j++)
							row.Add(TestDataHelper.GetRandomString(random.Next(3, 8)));

						writer.WriteLine(string.Join(",", row.ToArray()));
					}
				}
			}

			using (CsvImporter importer = new CsvImporter(fileName, null))
			{
				while (importer.Read())
				{
					IDataItem dataItem = importer.GetNextItem();
				}
			}
		}

		#endregion

		#region Private Medthods

		private string GetTempFilePath()
		{
			string fileName = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

			_tempFiles.Add(fileName);

			return fileName;
		}

		#endregion
    }
}
