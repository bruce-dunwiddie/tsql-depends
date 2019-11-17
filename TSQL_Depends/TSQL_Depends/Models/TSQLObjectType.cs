using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSQL.Depends.Models
{
	internal enum TSQLObjectType
	{
		/*
		AF = Aggregate function (CLR)
		C = CHECK constraint
		D = DEFAULT (constraint or stand-alone)
		F = FOREIGN KEY constraint
		FN = SQL scalar function
		FS = Assembly (CLR) scalar-function
		FT = Assembly (CLR) table-valued function
		IF = SQL inline table-valued function
		IT = Internal table
		P = SQL Stored Procedure
		PC = Assembly (CLR) stored-procedure
		PG = Plan guide
		PK = PRIMARY KEY constraint
		R = Rule (old-style, stand-alone)
		RF = Replication-filter-procedure
		S = System base table
		SN = Synonym
		SO = Sequence object
		Applies to: SQL Server 2012 through SQL Server 2014.
		SQ = Service queue
		TA = Assembly (CLR) DML trigger
		TF = SQL table-valued-function
		TR = SQL DML trigger
		TT = Table type
		U = Table (user-defined)
		UQ = UNIQUE constraint
		V = View
		X = Extended stored procedure
		*/

		// undocumented
		// SP = Security Policy

		CLRAggregateFunction,
		CheckConstraint,
		Default,
		ForeignKey,
		ScalarFunction,
		CLRScalarFunction,
		CLRTableValuedFunction,
		InlineTableValuedFunction,
		InternalTable,
		StoredProcedure,
		CLRStoredProcedure,
		PlanGuide,
		PrimaryKey,
		Rule,
		ReplicationFilter,
		SystemTable,
		Synonym,
		Sequence,
		ServiceQueue,
		CLRTrigger,
		TableValuedFunction,
		Trigger,
		TableType,
		Table,
		UniqueConstraint,
		View,
		ExtendedStoredProcedure,
		SecurityPolicy
	}
}
