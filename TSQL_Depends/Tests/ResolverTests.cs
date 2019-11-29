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
			Assert.AreEqual(TSQLOfficialObjectType.Table, referencedObject.Type);
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
			Assert.AreEqual(TSQLOfficialObjectType.Table, referencedObject.Type);
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
			Assert.AreEqual(TSQLOfficialObjectType.Table, referencedObject.Type);
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
			Assert.AreEqual(TSQLOfficialObjectType.Table, referencedObject.Type);
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
								Type = TSQLOfficialObjectType.Table
							}
						}
					}
				});

			TSQLObject referencedObject = resolver.Resolve(tokens);

			Assert.AreEqual("TestServer", referencedObject.ServerName);
			Assert.AreEqual("TestDB", referencedObject.DatabaseName);
			Assert.AreEqual("dbo", referencedObject.SchemaName);
			Assert.AreEqual("Test", referencedObject.Name);
			Assert.AreEqual(TSQLOfficialObjectType.Table, referencedObject.Type);
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
								Type = TSQLOfficialObjectType.Table
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
								Type = TSQLOfficialObjectType.Table
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

		[Test]
		public void TSQLDependencyParser_StartsWithPeriod()
		{
			List<TSQLToken> tokens = TSQLTokenizer.ParseTokens(
				".ErrorLog");

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
			Assert.AreEqual("dbo", referencedObject.SchemaName);
			Assert.AreEqual("ErrorLog", referencedObject.Name);
			Assert.AreEqual(TSQLOfficialObjectType.Table, referencedObject.Type);
			Assert.AreEqual(933578364, referencedObject.ObjectID);
		}

		[Test]
		public void TSQLDependencyParser_AdventureWorksStoredProc()
		{
			TSQLReferenceFinder finder = new TSQLReferenceFinder();

			List<List<TSQLToken>> references = finder.GetReferences(
				Resources.dbo_uspGetBillOfMaterials);

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

			List<TSQLObject> resolvedObjects =
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
					.ThenBy(resolvedObject => resolvedObject.Name)
					.ToList();

			Assert.AreEqual(3, resolvedObjects.Count);

			TSQLObject uspGetBillOfMaterials = resolvedObjects
				.Where(obj => obj.Name == "uspGetBillOfMaterials")
				.Single();

			Assert.AreEqual("SampleServer", uspGetBillOfMaterials.ServerName);
			Assert.AreEqual("AdventureWorks2017", uspGetBillOfMaterials.DatabaseName);
			Assert.AreEqual("dbo", uspGetBillOfMaterials.SchemaName);
			Assert.AreEqual("uspGetBillOfMaterials", uspGetBillOfMaterials.Name);
			Assert.AreEqual(TSQLOfficialObjectType.StoredProcedure, uspGetBillOfMaterials.Type);
			Assert.AreEqual(887674210, uspGetBillOfMaterials.ObjectID);

			TSQLObject billOfMaterials = resolvedObjects
				.Where(obj => obj.Name == "BillOfMaterials")
				.Single();

			Assert.AreEqual("SampleServer", billOfMaterials.ServerName);
			Assert.AreEqual("AdventureWorks2017", billOfMaterials.DatabaseName);
			Assert.AreEqual("Production", billOfMaterials.SchemaName);
			Assert.AreEqual("BillOfMaterials", billOfMaterials.Name);
			Assert.AreEqual(TSQLOfficialObjectType.Table, billOfMaterials.Type);
			Assert.AreEqual(1157579162, billOfMaterials.ObjectID);

			TSQLObject product = resolvedObjects
				.Where(obj => obj.Name == "Product")
				.Single();

			Assert.AreEqual("SampleServer", product.ServerName);
			Assert.AreEqual("AdventureWorks2017", product.DatabaseName);
			Assert.AreEqual("Production", product.SchemaName);
			Assert.AreEqual("Product", product.Name);
			Assert.AreEqual(TSQLOfficialObjectType.Table, product.Type);
			Assert.AreEqual(482100758, product.ObjectID);
		}
	}
}
