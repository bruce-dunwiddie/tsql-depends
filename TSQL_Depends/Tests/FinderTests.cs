using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using TSQL.Tokens;
using TSQL.Depends;

namespace Tests
{
	[TestFixture]
	class FinderTests
	{
		[Test]
		public void TSQLReferenceFinder_MultiColumnReference()
		{
			TSQLReferenceFinder finder = new TSQLReferenceFinder();

			List<List<TSQLToken>> references = finder.GetReferences(
				@"
				SELECT p.*
				INTO #products
				FROM
				Production.Product p;");

			Assert.AreEqual(3, references.Count);

			// regression test
			CompareTokenList(
				new string[] { 
					"p",
					".",
					// make sure * doesn't get dropped
					"*"
				},
				references[0]);

			CompareTokenList(
				new string[] {
					"#products"
				},
				references[1]);

			CompareTokenList(
				new string[] {
					"Production",
					".",
					"Product"
				},
				references[2]);
		}

		private void CompareTokenList(
			string[] expected, 
			List<TSQLToken> actual)
		{
			Assert.AreEqual(
				expected.Length, 
				actual.Count, 
				"Token list count does not match.");

			for (int index = 0; index < expected.Length; index++)
			{
				Assert.AreEqual(
					expected[index], 
					actual[index].Text);
			}
		}
	}
}
