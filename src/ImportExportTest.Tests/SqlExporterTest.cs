using ImportExportTest.Core;
using ImportExportTest.Core.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportExportTest.Tests
{
	[TestFixture]
	public class SqlExporterTest
    {
		#region Tests

		[Test]
		[Category("Pass")]
		public void Should_update_database_with_export()
		{
			string connectionString = ConfigurationManager.ConnectionStrings["TestDb"].ConnectionString;

			IDictionary<string, string> mappings = new Dictionary<string, string>();

			mappings.Add("Make", "Make");
			mappings.Add("Model", "Model");
			mappings.Add("Base Price", "BasePrice");
			mappings.Add("Introduced", "Introduced");
			mappings.Add("Cylinders", "Cylinders");
			mappings.Add("Convertible", "IsConvertible");


			List<ParsedDataItem> dataItemList = new List<ParsedDataItem>();

			ParsedDataItem dataItem = new ParsedDataItem();

			dataItem["Make"] = "Audi";
			dataItem["Model"] = "A6 2.8";
			dataItem["Base Price"] = "\"42,400\"";
			dataItem["Introduced"] = "05/02/1996";
			dataItem["Cylinders"] = "6";
			dataItem["Convertible"] = "FALSE";

			dataItemList.Add(dataItem);


			dataItem = new ParsedDataItem();

			dataItem["Make"] = "BMW";
			dataItem["Model"] = "Z3M";
			dataItem["Base Price"] = "\"54,000\"";
			dataItem["Introduced"] = "01/06/1997";
			dataItem["Cylinders"] = "6";
			dataItem["Convertible"] = "TRUE";

			dataItemList.Add(dataItem);


			dataItem = new ParsedDataItem();

			dataItem["Make"] = "Ferrari";
			dataItem["Model"] = "550";
			dataItem["Base Price"] = "\"204,390\"";
			dataItem["Introduced"] = "01/09/1996";
			dataItem["Cylinders"] = "12";
			dataItem["Convertible"] = "FALSE";

			dataItemList.Add(dataItem);


			dataItem = new ParsedDataItem();

			dataItem["Make"] = "TVR";
			dataItem["Model"] = "Griffith 500";
			dataItem["Base Price"] = "\"58,500\"";
			dataItem["Introduced"] = "03/08/1996";
			dataItem["Cylinders"] = "8";
			dataItem["Convertible"] = "TRUE";

			dataItemList.Add(dataItem);


			using (SqlExporter sqlExporter = new SqlExporter(connectionString, "TestTable", mappings))
			{
				foreach (IDataItem item in dataItemList)
					sqlExporter.WriteItem(item);
			}
		}

		#endregion
    }
}
