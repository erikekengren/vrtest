using ImportExportTest.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportExportTest.Core.Data
{
	public interface IImportReader : IDisposable
	{
		bool Read();

		IDataItem GetNextItem();
	}
}
