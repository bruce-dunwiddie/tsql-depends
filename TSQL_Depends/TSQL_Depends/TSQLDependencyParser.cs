using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TSQL.Depends.Models;
using TSQL.Tokens;

namespace TSQL.Depends
{
	public class TSQLDependencyParser
	{
		public TSQLDependencyParser(
			string connectionString)
		{
			// https://eng.uber.com/queryparser/ explains possibilities out of a similar effort

			// also https://github.com/uber/queryparser/blob/master/FUTURE.md
			// invalid references
			// type checking

			ModelBuilder builder = new ModelBuilder(
				connectionString);

			Resolver = new TSQLReferenceResolver(
				builder.GetServerProperties(),
				builder.GetSessionProperties(),
				builder.GetServers(),
				builder.GetDatabases());
		}

		private TSQLReferenceResolver Resolver { get; }

		public List<TSQLObjectDependency> GetDependencies(
			string sqlScript)
		{
			List<List<TSQLToken>> references =
				new TSQLReferenceFinder()
					.GetReferences(sqlScript);

			IEnumerable<TSQLObject> resolvedObjects =
				references
					.Select(reference =>
						Resolver.Resolve(reference))
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

			return dependencies;
		}
	}
}
