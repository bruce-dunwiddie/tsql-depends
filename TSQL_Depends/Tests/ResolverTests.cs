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

				},
				new TSQLSessionProperties()
				{

				},
				new List<TSQLServer>()
				{

				},
				new List<TSQLDatabase>()
				{
					AdventureWorks
				});

			TSQLObject referencedObject = resolver.Resolve(tokens);

			Assert.AreEqual("AdventureWorks2017", referencedObject.DatabaseName);
			Assert.AreEqual("Production", referencedObject.SchemaName);
			Assert.AreEqual("Product", referencedObject.Name);
			Assert.AreEqual(TSQLObjectType.Table, referencedObject.Type);
			Assert.AreEqual(482100758, referencedObject.ObjectID);
		}
	}
}
