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

		}

		public TSQLObject Resolve(
			List<TSQLToken> tokens)
		{
			return null;
		}
	}
}
