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
			try
			{
				return GetModelList<TSQLServer>(
					Script.GetServers,
					(reader, model) =>
					{
						model.Name = reader["server_name"].ToString();

						model.IsLinked = (bool)reader["is_linked"];
					});
			}
			catch (SqlException exception) when (exception.Message.ToLower()
				                                     .Contains("invalid object name 'sys.servers'"))
			{
				return new List<TSQLServer>();
			}
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

						//model.Columns = GetColumns(model.Name);

						//model.Synonyms = GetSynonyms(model.Name);
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

		// https://docs.microsoft.com/en-us/sql/relational-databases/system-catalog-views/sys-objects-transact-sql?view=sql-server-ver15

		private static readonly Dictionary<string, TSQLOfficialObjectType> objectTypeParseMappings = new Dictionary<string, TSQLOfficialObjectType>()
			{
				{"AF", TSQLOfficialObjectType.CLRAggregateFunction},
				{"C", TSQLOfficialObjectType.CheckConstraint},
				{"D", TSQLOfficialObjectType.Default},
				{"F", TSQLOfficialObjectType.ForeignKey},
				{"FN", TSQLOfficialObjectType.ScalarFunction},
				{"FS", TSQLOfficialObjectType.CLRScalarFunction},
				{"FT", TSQLOfficialObjectType.CLRTableValuedFunction},
				{"IF", TSQLOfficialObjectType.InlineTableValuedFunction},
				{"IT", TSQLOfficialObjectType.InternalTable},
				{"P", TSQLOfficialObjectType.StoredProcedure},
				{"PC", TSQLOfficialObjectType.CLRStoredProcedure},
				{"PG", TSQLOfficialObjectType.PlanGuide},
				{"PK", TSQLOfficialObjectType.PrimaryKey},
				{"R", TSQLOfficialObjectType.Rule},
				{"RF", TSQLOfficialObjectType.ReplicationFilter},
				{"S", TSQLOfficialObjectType.SystemTable},
				{"SN", TSQLOfficialObjectType.Synonym},
				{"SO", TSQLOfficialObjectType.Sequence},
				{"SQ", TSQLOfficialObjectType.ServiceQueue},
				{"TA", TSQLOfficialObjectType.CLRTrigger},
				{"TF", TSQLOfficialObjectType.TableValuedFunction},
				{"TR", TSQLOfficialObjectType.Trigger},
				{"TT", TSQLOfficialObjectType.TableType},
				{"U", TSQLOfficialObjectType.Table},
				{"UQ", TSQLOfficialObjectType.UniqueConstraint},
				{"V", TSQLOfficialObjectType.View},
				{"X", TSQLOfficialObjectType.ExtendedStoredProcedure},
				{"SP", TSQLOfficialObjectType.SecurityPolicy},
				{"EC", TSQLOfficialObjectType.EdgeConstraint},
				{"ET", TSQLOfficialObjectType.ExternalTable}
			};

		private static TSQLOfficialObjectType ParseAsObjectType(string type)
		{
			return objectTypeParseMappings[type.Trim()];
		}
	}
}
