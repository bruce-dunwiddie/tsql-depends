using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TSQL;
using TSQL.Tokens;

namespace TSQL.Depends
{
	internal class TSQLIdentifierParser
	{
		public TSQLMultiPartIdentifier Parse(
			string identifier)
		{
			return Parse(
				TSQLTokenizer.ParseTokens(
					identifier,
					includeWhitespace : false));
		}

		public TSQLMultiPartIdentifier Parse(
			List<TSQLToken> tokens)
		{
			TSQLMultiPartIdentifier identifier = new TSQLMultiPartIdentifier();

			IEnumerator<TSQLToken> partList = tokens
				.Where(t =>
					t.Type != TSQLTokenType.Whitespace &&
					t.Type != TSQLTokenType.SingleLineComment &&
					t.Type != TSQLTokenType.MultilineComment)
				.Reverse()
				.GetEnumerator();

			if (partList.MoveNext() &&
				// currently, the Finder can return a multicolumn reference, e.g. p.*, as a possible reference
				// so we need to make sure to throw this out as an object identifier
				partList.Current.Type == TSQLTokenType.Identifier)
			{
				identifier.ObjectName = partList.Current.AsIdentifier.Name;
			}
			else
			{
				return null;
			}

			if (partList.MoveNext())
			{
				// period
			}

			if (partList.MoveNext())
			{
				if (partList.Current.Type == TSQLTokenType.Identifier ||
					partList.Current.Type == TSQLTokenType.SystemIdentifier)
				{
					identifier.SchemaName = partList.Current.AsIdentifier.Name;

					partList.MoveNext();
				}
				// else period
			}

			if (partList.MoveNext())
			{
				if (partList.Current.Type == TSQLTokenType.Identifier ||
					partList.Current.Type == TSQLTokenType.SystemIdentifier)
				{
					identifier.DatabaseName = partList.Current.AsIdentifier.Name;

					partList.MoveNext();
				}
				// else period
			}

			if (partList.MoveNext())
			{
				identifier.ServerName = partList.Current.AsIdentifier.Name;
			}

			return identifier;
		}
	}
}
