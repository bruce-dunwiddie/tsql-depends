using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using NUnit.Framework;

using TSQL;
using TSQL.Depends;
using TSQL.Depends.Models;
using TSQL.Tokens;

using Tests.Properties;

namespace Tests
{
	[TestFixture]
	public class ResolverTests
	{
		private static TSQLDatabase AdventureWorks =
			JsonConvert.DeserializeObject<TSQLDatabase>(
				Resources.AdventureWorks2017);

		[Test]
		public void TSQLDependencyParser_ThreePartSameCase()
		{
			List<TSQLToken> tokens = TSQLTokenizer.ParseTokens(
				"AdventureWorks2017.Production.Product");

			TSQLReferenceResolver resolver = new TSQLReferenceResolver(
				new TSQLServerProperties()
				{
					CollationIgnoreCase = true
				},
				new TSQLSessionProperties()
				{
					DatabaseName = "tempdb",
					DefaultSchema = "dbo"
				},
				new List<TSQLServer>()
				{

				},
				new List<TSQLDatabase>()
				{
					AdventureWorks
				});

			TSQLObject referencedObject = resolver.Resolve(tokens);

			Assert.AreEqual("SampleServer", referencedObject.ServerName);
			Assert.AreEqual("AdventureWorks2017", referencedObject.DatabaseName);
			Assert.AreEqual("Production", referencedObject.SchemaName);
			Assert.AreEqual("Product", referencedObject.Name);
			Assert.AreEqual(TSQLObjectType.Table, referencedObject.Type);
			Assert.AreEqual(482100758, referencedObject.ObjectID);
		}

		[Test]
		public void TSQLDependencyParser_TwoPartSameCase()
		{
			List<TSQLToken> tokens = TSQLTokenizer.ParseTokens(
				"Production.Product");

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

			TSQLObject referencedObject = resolver.Resolve(tokens);

			Assert.AreEqual("SampleServer", referencedObject.ServerName);
			Assert.AreEqual("AdventureWorks2017", referencedObject.DatabaseName);
			Assert.AreEqual("Production", referencedObject.SchemaName);
			Assert.AreEqual("Product", referencedObject.Name);
			Assert.AreEqual(TSQLObjectType.Table, referencedObject.Type);
			Assert.AreEqual(482100758, referencedObject.ObjectID);
		}

		[Test]
		public void TSQLDependencyParser_DefaultSchema()
		{
			List<TSQLToken> tokens = TSQLTokenizer.ParseTokens(
				"Product");

			TSQLReferenceResolver resolver = new TSQLReferenceResolver(
				new TSQLServerProperties()
				{
					CollationIgnoreCase = true
				},
				new TSQLSessionProperties()
				{
					DatabaseName = "AdventureWorks2017",
					DefaultSchema = "Production"
				},
				new List<TSQLServer>()
				{

				},
				new List<TSQLDatabase>()
				{
					AdventureWorks
				});

			TSQLObject referencedObject = resolver.Resolve(tokens);

			Assert.AreEqual("SampleServer", referencedObject.ServerName);
			Assert.AreEqual("AdventureWorks2017", referencedObject.DatabaseName);
			Assert.AreEqual("Production", referencedObject.SchemaName);
			Assert.AreEqual("Product", referencedObject.Name);
			Assert.AreEqual(TSQLObjectType.Table, referencedObject.Type);
			Assert.AreEqual(482100758, referencedObject.ObjectID);
		}

		[Test]
		public void TSQLDependencyParser_IgnoreCase()
		{
			List<TSQLToken> tokens = TSQLTokenizer.ParseTokens(
				"production.product");

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

			TSQLObject referencedObject = resolver.Resolve(tokens);

			Assert.AreEqual("SampleServer", referencedObject.ServerName);
			Assert.AreEqual("AdventureWorks2017", referencedObject.DatabaseName);
			Assert.AreEqual("Production", referencedObject.SchemaName);
			Assert.AreEqual("Product", referencedObject.Name);
			Assert.AreEqual(TSQLObjectType.Table, referencedObject.Type);
			Assert.AreEqual(482100758, referencedObject.ObjectID);
		}

		[Test]
		public void TSQLDependencyParser_CaseSpecificSuccess()
		{
			List<TSQLToken> tokens = TSQLTokenizer.ParseTokens(
				"dbo.Test");

			TSQLReferenceResolver resolver = new TSQLReferenceResolver(
				new TSQLServerProperties()
				{
					CollationIgnoreCase = true
				},
				new TSQLSessionProperties()
				{
					DatabaseName = "TestDB",
					DefaultSchema = "dbo"
				},
				new List<TSQLServer>()
				{

				},
				new List<TSQLDatabase>()
				{
					new TSQLDatabase()
					{
						Name = "TestDB",
						CollationIgnoreCase = false,
						Objects = new List<TSQLObject>()
						{
							new TSQLObject()
							{
								ServerName = "TestServer",
								DatabaseName = "TestDB",
								SchemaName = "dbo",
								Name = "Test",
								ObjectID = 123,
								Type = TSQLObjectType.Table
							}
						}
					}
				});

			TSQLObject referencedObject = resolver.Resolve(tokens);

			Assert.AreEqual("TestServer", referencedObject.ServerName);
			Assert.AreEqual("TestDB", referencedObject.DatabaseName);
			Assert.AreEqual("dbo", referencedObject.SchemaName);
			Assert.AreEqual("Test", referencedObject.Name);
			Assert.AreEqual(TSQLObjectType.Table, referencedObject.Type);
			Assert.AreEqual(123, referencedObject.ObjectID);
		}

		[Test]
		public void TSQLDependencyParser_CaseSpecificFail()
		{
			List<TSQLToken> tokens = TSQLTokenizer.ParseTokens(
				"dbo.test");

			TSQLReferenceResolver resolver = new TSQLReferenceResolver(
				new TSQLServerProperties()
				{
					CollationIgnoreCase = true
				},
				new TSQLSessionProperties()
				{
					DatabaseName = "TestDB",
					DefaultSchema = "dbo"
				},
				new List<TSQLServer>()
				{

				},
				new List<TSQLDatabase>()
				{
					new TSQLDatabase()
					{
						Name = "TestDB",
						CollationIgnoreCase = false,
						Objects = new List<TSQLObject>()
						{
							new TSQLObject()
							{
								DatabaseName = "TestDB",
								SchemaName = "dbo",
								Name = "Test",
								ObjectID = 123,
								Type = TSQLObjectType.Table
							}
						}
					}
				});

			TSQLObject referencedObject = resolver.Resolve(tokens);

			Assert.IsNull(referencedObject);
		}

		[Test]
		public void TSQLDependencyParser_LinkedServer()
		{
			List<TSQLToken> tokens = TSQLTokenizer.ParseTokens(
				"RemoteServer.SomeTestDB.dbo.test");

			TSQLReferenceResolver resolver = new TSQLReferenceResolver(
				new TSQLServerProperties()
				{
					CollationIgnoreCase = true
				},
				new TSQLSessionProperties()
				{
					DatabaseName = "TestDB",
					DefaultSchema = "dbo"
				},
				new List<TSQLServer>()
				{
					new TSQLServer()
					{
						Name = "RemoteServer",
						IsLinked = true
					}
				},
				new List<TSQLDatabase>()
				{
					new TSQLDatabase()
					{
						Name = "TestDB",
						CollationIgnoreCase = false,
						Objects = new List<TSQLObject>()
						{
							new TSQLObject()
							{
								DatabaseName = "TestDB",
								SchemaName = "dbo",
								Name = "Test",
								ObjectID = 123,
								Type = TSQLObjectType.Table
							}
						}
					}
				});

			TSQLObject referencedObject = resolver.Resolve(tokens);

			Assert.AreEqual("RemoteServer", referencedObject.ServerName);
			Assert.AreEqual("SomeTestDB", referencedObject.DatabaseName);
			Assert.AreEqual("dbo", referencedObject.SchemaName);
			Assert.AreEqual("test", referencedObject.Name);
		}
	}
}
