using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImportExportTest.Core.Data
{
	public class ParsedDataItem :  IDataItem
	{
		#region Variables

		private readonly IDictionary<string, object> _properties = new Dictionary<string, object>();

		private CultureInfo _culture;
		
		#endregion

		#region Constructor

		public ParsedDataItem(CultureInfo culture)
		{
			_culture = culture;
		}

		public ParsedDataItem() : this(Thread.CurrentThread.CurrentUICulture)
		{
		}

		#endregion

		#region Public Methods

		public object this[string key]
		{
			get
			{
				if (_properties.ContainsKey(key))
					return _properties[key];

				throw new ArgumentOutOfRangeException();
			}
			set
			{
				SetValue(key, value);
			}
		}
		
		#endregion

		#region Private Methods

		private bool SetValue(string key, object value)
		{
			if (value is string)
			{
				string valueAsString = value as string;

				if (valueAsString.StartsWith("\"") && valueAsString.StartsWith("\""))
				{
					_properties[key] = valueAsString.Remove(valueAsString.Length - 1, 1).Remove(0, 1);

					return true;
				}


				int valueAsInt;

				if (int.TryParse(valueAsString, out valueAsInt))
				{
					_properties[key] = valueAsInt;

					return true;
				}


				bool valueAsBool;

				if (bool.TryParse(valueAsString, out valueAsBool))
				{
					_properties[key] = valueAsBool;

					return true;
				}


				DateTime valueAsDateTime;

				if (DateTime.TryParse(valueAsString, _culture.DateTimeFormat, DateTimeStyles.AssumeLocal, out valueAsDateTime))
				{
					_properties[key] = valueAsDateTime;

					return true;
				}


				_properties[key] = valueAsString;
			}
			else
				_properties[key] = value;

			return true;
		}
		
		#endregion
	}
}
