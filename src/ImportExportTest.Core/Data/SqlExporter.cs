using ImportExportTest.Core.Data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportExportTest.Core.Data
{
	public class SqlExporter : IExportWriter
	{
		#region Variables

		private IDictionary<string, string> _columnMappings = new Dictionary<string, string>();

		private string _tableName;

		private SqlConnection _connection;
		private SqlTransaction _transaction;

		private string _commandText;

		#endregion

		#region Constructor

		public SqlExporter(string connectionString, string tableName, IDictionary<string, string> columnMappings)
		{
			if (string.IsNullOrWhiteSpace(connectionString))
				throw new ArgumentException("Parameter \"connectionString\" cannot be null or empty");

			if (string.IsNullOrWhiteSpace(tableName))
				throw new ArgumentException("Parameter \"tableName\" cannot be null or empty");

			if (columnMappings == null || columnMappings.Count == 0)
				throw new ArgumentException("Parameter \"columnMappings\" cannot be null or empty");

			_tableName = tableName;

			_columnMappings = columnMappings;

			_connection = new SqlConnection(connectionString);
		}

		#endregion

		#region Destructor

		~SqlExporter()
		{
			Dispose();
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			if (_connection != null)
			{
				_connection.Close();
				_connection = null;
			}

			GC.SuppressFinalize(this);
		}

		#endregion

		#region IExportWriter Members

		public void WriteItem(IDataItem dataItem)
		{
			SqlCommand command = CreateSqlCommand(dataItem);

			if (_connection.State == ConnectionState.Closed)
				_connection.Open();

			command.ExecuteNonQuery();
		}

		#endregion

		#region Private Methods

		private SqlCommand CreateSqlCommand(IDataItem dataItem)
		{
			if (_commandText == null)
			{
				List<string> parameterList = new List<string>();
				List<string> valueList = new List<string>();

				string commandText = string.Format("INSERT INTO [{0}] (", _tableName);

				foreach (KeyValuePair<string, string> column in _columnMappings)
				{
					string parameterName = column.Value.Replace(" ", string.Empty);

					parameterList.Add("[" + column.Value + "]");

					valueList.Add("@" + parameterName);
				}

				_commandText = commandText + string.Join(",", parameterList.ToArray()) + ") VALUES (" + string.Join(",", valueList.ToArray()) + ")";
			}

			SqlCommand command = new SqlCommand(_commandText, _connection);

			if (_transaction != null)
				command.Transaction = _transaction;

			foreach (KeyValuePair<string, string> column in _columnMappings)
			{
				string parameterName = column.Value.Replace(" ", string.Empty);

				command.Parameters.AddWithValue(parameterName, dataItem[column.Key]);
			}

			return command;
		}

		#endregion
	}
}
