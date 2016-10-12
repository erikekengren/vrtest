using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportExportTest.Tests
{
	public static class TestDataHelper
	{
		#region Variables

		private const string _randomStringCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

		private static readonly Random _random = new Random();

		#endregion

		#region Public Methods

		public static string GetRandomString(int length, string characterSelectionString = _randomStringCharacters)
		{
			StringBuilder sb = new StringBuilder(length);

			int arrayLength = characterSelectionString.Length - 1;

			for (int i = 0; i < length; i++)
				sb.Append(characterSelectionString[_random.Next(arrayLength)]);


			return sb.ToString();
		}

		#endregion
	}
}
