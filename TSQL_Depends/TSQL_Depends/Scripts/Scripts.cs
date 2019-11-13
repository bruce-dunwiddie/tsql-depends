using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TSQL.Depends.Scripts
{
	internal static class Script
	{
		public static string GetColumns
		{
			get
			{
				return GetScript("GetColumns.sql");
			}
		}

		private static string GetScript(string name)
		{
			using (StreamReader reader = new StreamReader(
				Assembly
					.GetExecutingAssembly()
					.GetManifestResourceStream(
						typeof(Script).Namespace + "." + name)))
			{
				return reader.ReadToEnd();
			}
		}
	}
}
