using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TSQL.Depends.Models;

namespace TSQL.Depends
{
	internal class TSQLObjectTypeMapper
	{
		public TSQLObjectType GetMappedType(TSQLOfficialObjectType objectType)
		{
			return mappings[objectType];
		}

		public static bool HasMapping(TSQLOfficialObjectType objectType)
		{
			return mappings.ContainsKey(objectType);
		}

		private static Dictionary<TSQLOfficialObjectType, TSQLObjectType> mappings =
			new Dictionary<TSQLOfficialObjectType, TSQLObjectType>()
		{
			{TSQLOfficialObjectType.CLRAggregateFunction, TSQLObjectType.Function},
			{TSQLOfficialObjectType.CLRScalarFunction, TSQLObjectType.Function},
			{TSQLOfficialObjectType.CLRStoredProcedure, TSQLObjectType.StoredProcedure},
			{TSQLOfficialObjectType.CLRTableValuedFunction, TSQLObjectType.Function},
			{TSQLOfficialObjectType.ExtendedStoredProcedure, TSQLObjectType.StoredProcedure},
			{TSQLOfficialObjectType.ExternalTable, TSQLObjectType.Table},
			{TSQLOfficialObjectType.InlineTableValuedFunction, TSQLObjectType.Function},
			{TSQLOfficialObjectType.ScalarFunction, TSQLObjectType.Function},
			{TSQLOfficialObjectType.StoredProcedure, TSQLObjectType.StoredProcedure},
			{TSQLOfficialObjectType.Table, TSQLObjectType.Table},
			{TSQLOfficialObjectType.TableValuedFunction, TSQLObjectType.Function},
			{TSQLOfficialObjectType.View, TSQLObjectType.View}
		};
	}
}
