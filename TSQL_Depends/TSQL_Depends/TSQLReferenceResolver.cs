using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TSQL.Depends.Models;
using TSQL.Tokens;

namespace TSQL.Depends
{
	internal class TSQLReferenceResolver
	{
		public TSQLReferenceResolver(
			TSQLServerProperties serverProps,
			TSQLSessionProperties sessionProps,
			List<TSQLServer> servers,
			List<TSQLDatabase> databases)
		{
			ServerProperties = serverProps;
			SessionProperties = sessionProps;
			Servers = servers;
			Databases = databases;
		}

		private TSQLServerProperties ServerProperties { get; }
		private TSQLSessionProperties SessionProperties { get; }
		private List<TSQLServer> Servers { get; }
		private List<TSQLDatabase> Databases { get; }

		public TSQLObject Resolve(
			List<TSQLToken> tokens)
		{
			TSQLObject referencedObject = null;

			TSQLMultiPartIdentifier identifier = 
				new TSQLIdentifierParser()
					.Parse(tokens);

			// if we didn't find a valid identifier object, then the identifier will return null
			// so the resolver needs to then also return null
			// e.g. p.*

			if (identifier == null)
			{
				return null;
			}

			if (identifier.ServerName != null)
			{
				TSQLServer referencedServer = Servers
					.Where(s =>
						{
							if (ServerProperties.CollationIgnoreCase)
							{
								return s.Name.Equals(
									identifier.ServerName,
									StringComparison.InvariantCultureIgnoreCase);
							}
							else
							{
								return s.Name == identifier.ServerName;
							}
						}).SingleOrDefault();

				if (referencedServer != null)
				{
					if (referencedServer.IsLinked)
					{
						// assume their reference is valid for now
						return new TSQLObject()
							{
								ServerName = identifier.ServerName,
								DatabaseName = identifier.DatabaseName,
								SchemaName = identifier.SchemaName,
								Name = identifier.ObjectName
							};
					}
					// else let the logic fall through for now
				}
				else
				{
					// couldn't find corresponding server of 4 part identifier
					return null;
				}
			}

			// TODO: refactor code below for performance and review maintainability
			// just writing it out the easy way for now to work through logic
			
			// this only works if the server is not a linked server
			string databaseLookup = identifier.DatabaseName ?? SessionProperties.DatabaseName;

			TSQLDatabase referencedDatabase = Databases
				.Where(db =>
					{
						if (ServerProperties.CollationIgnoreCase)
						{
							return db.Name.Equals(
								databaseLookup, 
								StringComparison.InvariantCultureIgnoreCase);
						}
						else
						{
							return db.Name == databaseLookup;
						}
					}
				).SingleOrDefault();

			if (referencedDatabase != null)
			{
				string schemaLookup = identifier.SchemaName ?? SessionProperties.DefaultSchema;

				referencedObject = referencedDatabase.Objects
					.Where(obj =>
						{
							if (referencedDatabase.CollationIgnoreCase)
							{
								return
									obj.SchemaName.Equals(
										schemaLookup,
										StringComparison.InvariantCultureIgnoreCase) &&
									obj.Name.Equals(
										identifier.ObjectName,
										StringComparison.InvariantCultureIgnoreCase);
							}
							else
							{
								return
									obj.SchemaName == schemaLookup &&
									obj.Name == identifier.ObjectName;
							}
						}).SingleOrDefault();
			}

			return referencedObject;
		}
	}
}
