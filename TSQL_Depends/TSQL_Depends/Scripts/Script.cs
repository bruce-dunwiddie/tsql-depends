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

		public static string GetDatabases
		{
			get
			{
				return GetScript("GetDatabases.sql");
			}
		}

		public static string GetObjects
		{
			get
			{
				return GetScript("GetObjects.sql");
			}
		}

		public static string GetServerProperties
		{
			get
			{
				return GetScript("GetServerProperties.sql");
			}
		}

		public static string GetServers
		{
			get
			{
				return GetScript("GetServers.sql");
			}
		}

		public static string GetSessionProperties
		{
			get
			{
				return GetScript("GetSessionProperties.sql");
			}
		}

		public static string GetSynonyms
		{
			get
			{
				return GetScript("GetSynonyms.sql");
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
