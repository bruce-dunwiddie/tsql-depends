using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using TSQL.Depends;
using TSQL.Depends.Models;
using TSQL.Tokens;

using Tests.Properties;

namespace Tests
{
	/// <summary>
	///		Just a simple runner app to display intermediate collections and values
	///		from parsed text.
	/// </summary>
	public class OutputDisplayRunner
	{
		public static void Main(string[] args)
		{
			string sql = @"

";

			TSQLReferenceFinder finder = new TSQLReferenceFinder();

			List<List<TSQLToken>> references = finder
				.GetReferences(sql)
				.GroupBy(reference => String.Join("", reference.Select(r => r.Text)))
				.Select(uniqueGroup => uniqueGroup.First())
				.ToList();

			Console.WriteLine("Identifiers:");

			foreach (var reference in references)
			{
				Console.WriteLine("\t" + String.Join("", reference.Select(r => r.Text)));
			}

			TSQLDatabase AdventureWorks =
				JsonConvert.DeserializeObject<TSQLDatabase>(
					Resources.AdventureWorks2017);

			TSQLReferenceResolver resolver = new TSQLReferenceResolver(
				new TSQLServerProperties()
				{
					CollationIgnoreCase = true
				},
				new TSQLSessionProperties()
				{
					DatabaseName = "AdventureWorks2017",
					DefaultSchema = "dbo"
				},
				new List<TSQLServer>()
				{

				},
				new List<TSQLDatabase>()
				{
					AdventureWorks
				});

			IEnumerable<TSQLObject> resolvedObjects =
				references
					.Select(reference =>
						resolver.Resolve(reference))
					.Where(resolvedObject => resolvedObject != null)
					// only returning ones that are officially handled right now
					.Where(resolvedObject =>
						TSQLObjectTypeMapper.HasMapping(resolvedObject.Type))
					// select distinct
					.GroupBy(resolvedObject => new
					{
						resolvedObject.ServerName,
						resolvedObject.DatabaseName,
						resolvedObject.SchemaName,
						resolvedObject.Name
					})
					.Select(uniqueObject => uniqueObject.First())
					.OrderBy(resolvedObject => resolvedObject.ServerName)
					.ThenBy(resolvedObject => resolvedObject.DatabaseName)
					.ThenBy(resolvedObject => resolvedObject.SchemaName)
					.ThenBy(resolvedObject => resolvedObject.Name);

			Console.WriteLine("Resolved:");

			foreach(var obj in resolvedObjects)
			{
				Console.WriteLine(
					"\t" + obj.Type.ToString() + ": " +
					obj.ServerName + "." +
					obj.DatabaseName + "." +
					obj.SchemaName + "." +
					obj.Name);
			}

			TSQLObjectTypeMapper typeMapper = new TSQLObjectTypeMapper();

			List<TSQLObjectDependency> dependencies =
				resolvedObjects.Select(obj =>
					new TSQLObjectDependency()
					{
						ServerName = obj.ServerName,
						DatabaseName = obj.DatabaseName,
						SchemaName = obj.SchemaName,
						Name = obj.Name,
						ObjectID = obj.ObjectID,
						Type = typeMapper.GetMappedType(obj.Type)
					}).ToList();

			Console.WriteLine("Dependencies:");

			foreach (var obj in dependencies)
			{
				Console.WriteLine(
					"\t" + obj.Type.ToString() + ": " +
					obj.ServerName + "." +
					obj.DatabaseName + "." +
					obj.SchemaName + "." +
					obj.Name);
			}
		}
	}
}
