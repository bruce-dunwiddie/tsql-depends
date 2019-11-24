using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using TSQL.Depends.Scripts;

namespace TSQL.Depends.Models
{
	internal class ModelBuilder
	{
		public ModelBuilder(string connectionString)
		{
			ConnectionString = connectionString;
		}

		private string ConnectionString { get; set; }

		public TSQLServerProperties GetServerProperties()
		{
			return GetModel<TSQLServerProperties>(
				Script.GetServerProperties,
				(reader, model) =>
				{
					model.Name = reader["server_name"].ToString();

					model.Collation = reader["collation_name"].ToString();

					model.CollationCodePage = (int)reader["collation_code_page"];

					model.CollationIgnoreCase = (bool)reader["collation_ignore_case"];
				});
		}

		public TSQLSessionProperties GetSessionProperties()
		{
			return GetModel<TSQLSessionProperties>(
				Script.GetSessionProperties,
				(reader, model) =>
				{
					model.DatabaseName = reader["database_name"].ToString();

					model.DefaultSchema = reader["default_schema_name"].ToString();
				});
		}

		public List<TSQLServer> GetServers()
		{
			return GetModelList<TSQLServer>(
				Script.GetServers,
				(reader, model) =>
				{
					model.Name = reader["server_name"].ToString();

					model.IsLinked = (bool)reader["is_linked"];
				});
		}

		public List<TSQLDatabase> GetDatabases()
		{
			return GetModelList<TSQLDatabase>(
				Script.GetDatabases,
				(reader, model) =>
					{
						model.Name = reader["database_name"].ToString();

						model.Collation = reader["collation_name"].ToString();

						model.CollationCodePage = (int)reader["collation_code_page"];

						model.CollationIgnoreCase = (bool)reader["collation_ignore_case"];

						model.Objects = GetObjects(model.Name);

						model.Columns = GetColumns(model.Name);

						model.Synonyms = GetSynonyms(model.Name);
					});
		}

		private List<TSQLObject> GetObjects(string databaseName)
		{
			return GetModelList<TSQLObject>(
				Script.GetObjects,
				(reader, model) =>
					{
						model.DatabaseName = databaseName;

						model.SchemaName = reader["schema_name"].ToString();

						model.Name = reader["object_name"].ToString();

						model.ObjectID = (int)reader["object_id"];

						model.Type = ParseAsObjectType(reader["object_type"].ToString());
					},
				databaseName);
		}

		private List<TSQLColumn> GetColumns(string databaseName)
		{
			return GetModelList<TSQLColumn>(
				Script.GetColumns,
				(reader, model) =>
				{
					model.ObjectID = (int)reader["object_id"];

					model.Name = reader["column_name"].ToString();

					model.ColumnID = (int)reader["column_id"];
				},
				databaseName);
		}

		private List<TSQLSynonym> GetSynonyms(string databaseName)
		{
			return GetModelList<TSQLSynonym>(
				Script.GetSynonyms,
				(reader, model) =>
				{
					model.DatabaseName = databaseName;

					model.SchemaName = reader["schema_name"].ToString();

					model.Name = reader["synonym_name"].ToString();

					model.ObjectID = (int)reader["object_id"];

					TSQLMultiPartIdentifier baseObject = new TSQLIdentifierParser().Parse(
						reader["base_object_name"].ToString());

					model.BaseServerName = baseObject.ServerName;

					model.BaseDatabaseName = baseObject.DatabaseName;

					model.BaseSchemaName = baseObject.SchemaName;

					model.BaseObjectName = baseObject.ObjectName;
				},
				databaseName);
		}

		private T GetModel<T>(
			string SQLScript,
			Action<SqlDataReader, T> mapping,
			string databaseName = null) where T : class, new()
		{
			return GetModelList<T>(
				SQLScript,
				mapping,
				databaseName).FirstOrDefault();
		}

		private List<T> GetModelList<T>(
			string SQLScript,
			Action<SqlDataReader, T> mapping,
			string databaseName = null) where T : new()
		{
			List<T> model = new List<T>();

			using (SqlConnection conn = new SqlConnection(ConnectionString))
			using (SqlCommand command = new SqlCommand(SQLScript, conn))
			{
				conn.Open();

				if (databaseName != null)
				{
					using (SqlCommand useDatabase = new SqlCommand($"USE [{databaseName}];", conn))
					{
						useDatabase.ExecuteNonQuery();
					}
				}

				using (SqlDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						T modelItem = new T();

						mapping(reader, modelItem);

						model.Add(modelItem);
					}
				}
			}

			return model;
		}

		private static Dictionary<string, TSQLObjectType> objectTypeParseMappings = new Dictionary<string, TSQLObjectType>()
			{
				{"AF", TSQLObjectType.CLRAggregateFunction},
				{"C", TSQLObjectType.CheckConstraint},
				{"D", TSQLObjectType.Default},
				{"F", TSQLObjectType.ForeignKey},
				{"FN", TSQLObjectType.ScalarFunction},
				{"FS", TSQLObjectType.CLRScalarFunction},
				{"FT", TSQLObjectType.CLRTableValuedFunction},
				{"IF", TSQLObjectType.InlineTableValuedFunction},
				{"IT", TSQLObjectType.InternalTable},
				{"P", TSQLObjectType.StoredProcedure},
				{"PC", TSQLObjectType.CLRStoredProcedure},
				{"PG", TSQLObjectType.PlanGuide},
				{"PK", TSQLObjectType.PrimaryKey},
				{"R", TSQLObjectType.Rule},
				{"RF", TSQLObjectType.ReplicationFilter},
				{"S", TSQLObjectType.SystemTable},
				{"SN", TSQLObjectType.Synonym},
				{"SO", TSQLObjectType.Sequence},
				{"SQ", TSQLObjectType.ServiceQueue},
				{"TA", TSQLObjectType.CLRTrigger},
				{"TF", TSQLObjectType.TableValuedFunction},
				{"TR", TSQLObjectType.Trigger},
				{"TT", TSQLObjectType.TableType},
				{"U", TSQLObjectType.Table},
				{"UQ", TSQLObjectType.UniqueConstraint},
				{"V", TSQLObjectType.View},
				{"X", TSQLObjectType.ExtendedStoredProcedure},
				{"SP", TSQLObjectType.SecurityPolicy}
			};

		private static TSQLObjectType ParseAsObjectType(string type)
		{
			return objectTypeParseMappings[type.Trim()];
		}
	}
}
