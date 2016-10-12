using ImportExportTest.Core.Data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportExportTest.Core.Data
{
	public class CsvImporter : IImportReader
	{
		#region Variables

		private IDictionary<string, int> _columnMappings = new Dictionary<string, int>();
		private IList<string> _columns = new List<string>();

		private string _filename;

		private Stream _fileStream;
		private StreamReader _fileReader;

		private string _currentLine;

		#endregion

		#region Constructor

		public CsvImporter(string filename, List<string> columns)
			: this(new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read), columns)
		{
		}

		public CsvImporter(Stream inputStream, IList<string> columns)
		{
			_fileStream = inputStream;

			if (_fileStream == null)
				throw new ArgumentException("Parameter \"inputStream\" cannot be null");

			if (!_fileStream.CanRead)
				throw new ArgumentException("Input stream is not readable");


			_columns = columns;


			_fileReader = new StreamReader(_fileStream);


			ReadColumnsFromFile();
		}
		
		#endregion

		#region Destructor

		~CsvImporter()
		{
			Dispose();
		}
		
		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			if (_fileReader != null)
			{
				_fileReader.Close();
				_fileReader = null;
			}

			GC.SuppressFinalize(this);
		}
		
		#endregion

		#region IImportReader Members

		public bool Read()
		{
			if (_fileReader.EndOfStream)
				return false;

			_currentLine = _fileReader.ReadLine();

			return true;
		}

		public Data.IDataItem GetNextItem()
		{
			IList<string> items = ParseLine(_currentLine);

			ParsedDataItem dataItem = new ParsedDataItem();

			foreach (string column in _columns)
				dataItem[column] = items[_columnMappings[column]];


			return dataItem;
		}
		
		#endregion

		#region Private Methods

		private void ReadColumnsFromFile()
		{
			IList<string> parts = ParseLine(_currentLine);

			for (int i = 0; i < parts.Count; i++)
				_columnMappings.Add(parts[i], i);

			if (_columns == null || _columns.Count == 0)
			{
				_columns = new List<string>(_columnMappings.Keys);
			}
			else
			{
				// Correctness over tolerance
				if (_columns.Any(column => !_columnMappings.ContainsKey(column)))
					throw new InvalidDataException("Columns specified contains entries not contained in data file");
			}
		}

		private IList<string> ParseLine(string line)
		{
			string[] parts = _currentLine.Split(',');

			List<string> items = new List<string>();

			string savedItem = null;
			foreach (string part in parts)
			{
				string currentPart = part.Trim();

				if (currentPart.StartsWith("\""))
				{
					savedItem = currentPart;
					continue;
				}

				if (savedItem != null)
				{
					items.Add(savedItem + currentPart);

					savedItem = null;
				}
				else
					items.Add(currentPart);
			}

			return items;
		}
		
		#endregion
	}
}
