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
	public class ParserTests
	{
		[Test]
		public void TSQLDependencyParser_SimpleConstructor()
		{
			TSQLDependencyParser parser = new TSQLDependencyParser(
				"Data Source=.;Integrated Security=True");
		}
	}
}
