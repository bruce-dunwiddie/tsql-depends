using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using TSQL.Depends;

namespace Tests
{
	[TestFixture]
	public class IdentifierParserTests
	{
		[Test]
		public void TSQLIdentifierParser_TwoPart()
		{
			TSQLMultiPartIdentifier identifier = new TSQLIdentifierParser().Parse(
				"[Production].[Product]");

			Assert.IsNull(identifier.ServerName);
			Assert.IsNull(identifier.DatabaseName);
			Assert.AreEqual("Production", identifier.SchemaName);
			Assert.AreEqual("Product", identifier.ObjectName);
		}

		[Test]
		public void TSQLIdentifierParser_FourPart()
		{
			TSQLMultiPartIdentifier identifier = new TSQLIdentifierParser().Parse(
				"[TestServer].[AdventureWorks2017].[Production].[Product]");

			Assert.AreEqual("TestServer", identifier.ServerName);
			Assert.AreEqual("AdventureWorks2017", identifier.DatabaseName);
			Assert.AreEqual("Production", identifier.SchemaName);
			Assert.AreEqual("Product", identifier.ObjectName);
		}

		[Test]
		public void TSQLIdentifierParser_DefaultSchema()
		{
			TSQLMultiPartIdentifier identifier = new TSQLIdentifierParser().Parse(
				"[AdventureWorks2017]..[Product]");

			Assert.IsNull(identifier.ServerName);
			Assert.AreEqual("AdventureWorks2017", identifier.DatabaseName);
			Assert.IsNull(identifier.SchemaName);
			Assert.AreEqual("Product", identifier.ObjectName);
		}
	}
}
