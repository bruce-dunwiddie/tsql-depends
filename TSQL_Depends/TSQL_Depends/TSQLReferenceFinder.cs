using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TSQL.Tokens;

namespace TSQL.Depends
{
	internal class TSQLReferenceFinder
	{
		public List<List<TSQLToken>> GetReferences(
			string sqlScript)
		{
			List<List<TSQLToken>> references = new List<List<TSQLToken>>();

			TSQLTokenizer tokenizer = new TSQLTokenizer(sqlScript)
			{
				IncludeWhitespace = false
			};

			List<TSQLToken> currentReference = new List<TSQLToken>();
			TSQLToken lastToken = null;

			while (tokenizer.MoveNext())
			{
				switch (tokenizer.Current.Type)
				{
					// ignore whitespace
					// ignore comments
					case TSQLTokenType.Identifier:
					case TSQLTokenType.SystemIdentifier:
					case TSQLTokenType.IncompleteIdentifier:
						{
							// if the current reference is empty
							// or the last token was a period
							// add the current token to the current reference token list
							if (lastToken != null &&
								lastToken.IsCharacter(TSQLCharacters.Period))
							{
								if (tokenizer.Current.Type != TSQLTokenType.IncompleteIdentifier)
								{
									currentReference.Add(tokenizer.Current);
								}
							}
							// the current identifier token signals another identifier chain
							else
							{
								if (currentReference.Count > 0)
								{
									references.Add(currentReference);

									currentReference = new List<TSQLToken>();
								}

								if (tokenizer.Current.Type != TSQLTokenType.IncompleteIdentifier &&
									(
										lastToken is null ||
										(
											// if the last token was an identifier
											// and this token is an identifier
											// then this is an alias and don't add it
											lastToken.Type != TSQLTokenType.Identifier &&
											lastToken.Type != TSQLTokenType.SystemIdentifier &&
											
											// if the last token was AS
											// and this token is an identifier
											// then this is an alias and don't add it
											!lastToken.IsKeyword(TSQLKeywords.AS) &&

											// if the last token was a variable
											// then this is a data type in a proc/function definition
											// and don't add it
											lastToken.Type != TSQLTokenType.Variable &&

											// if the last token was SET
											// then this is a SET option keyword and don't add it
											!lastToken.IsKeyword(TSQLKeywords.SET) &&

											// if the last token was WITH
											// then this is a CTE definition and don't add it
											!lastToken.IsKeyword(TSQLKeywords.WITH) &&

											// if the last token was USE
											// then this is a database name and don't add it
											!lastToken.IsKeyword(TSQLKeywords.USE)
										)
									))
								{
									currentReference.Add(tokenizer.Current);
								}
							}

							break;
						}
					case TSQLTokenType.Character:
						{
							if (tokenizer.Current.IsCharacter(TSQLCharacters.Period))
							{
								currentReference.Add(tokenizer.Current);
							}
							else
							{
								// signals the end of an identifier

								if (currentReference.Count > 0)
								{
									references.Add(currentReference);

									currentReference = new List<TSQLToken>();
								}
							}

							break;
						}
					case TSQLTokenType.BinaryLiteral:
					case TSQLTokenType.IncompleteString:
					case TSQLTokenType.Keyword:
					case TSQLTokenType.MoneyLiteral:
					case TSQLTokenType.NumericLiteral:
					case TSQLTokenType.Operator:
					case TSQLTokenType.StringLiteral:
					case TSQLTokenType.SystemVariable:
					case TSQLTokenType.Variable:
						{
							if (tokenizer.Current.Text == "*" &&
								currentReference.Count > 0 &&
								currentReference.Last().Text == ".")
							{
								// e.g. p.*
								currentReference.Add(tokenizer.Current);
							}
							else
							{
								// signals the end of an identifier

								if (currentReference.Count > 0)
								{
									references.Add(currentReference);

									currentReference = new List<TSQLToken>();
								}
							}

							break;
						}
				}

				lastToken = tokenizer.Current;
			}

			if (currentReference.Count > 0)
			{
				references.Add(currentReference);
			}

			return references;
		}
	}
}
