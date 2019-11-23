using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using TSQL.Depends.Models;

namespace Tests
{
	/// <summary>
	///		Generates the serialized version of the real AdventureWorks database that
	///		can be deserialized and used for test scenarios.
	/// </summary>
	public class SerializeAdventureWorks
	{
		public static void Main(string[] args)
		{
			ModelBuilder model = new ModelBuilder(
				"Data Source=.;Integrated Security=True");

			TSQLDatabase AdventureWorks =
				model.GetDatabases().Where(db => db.Name == "AdventureWorks2017").Single();

			using (StreamWriter file = File.CreateText("../../Databases/AdventureWorks2017.json"))
			{
				new JsonSerializer()
				{
					Formatting = Formatting.Indented
				}.Serialize(file, AdventureWorks);
			}
		}
	}
}
