﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportExportTest.Core.Data
{
	public interface IExportWriter : IDisposable
	{
		void WriteItem(IDataItem dataItem);

		bool Commit();
	}
}
