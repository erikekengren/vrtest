using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportExportTest.Core.Data
{
	public interface IDataItem
	{
		object this[string key] { get; set; }
	}
}
