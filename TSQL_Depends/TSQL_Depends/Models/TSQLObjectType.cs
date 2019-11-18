namespace TSQL.Depends.Models
{
	internal enum TSQLObjectType
	{
        /// <summary>
        /// AF = Aggregate function (CLR)
        /// </summary>
        CLRAggregateFunction,

        /// <summary>
        /// C = CHECK constraint
        /// </summary>
		CheckConstraint,

        /// <summary>
        /// D = DEFAULT (constraint or stand-alone)
        /// </summary>
		Default,

        /// <summary>
        /// F = FOREIGN KEY constraint
        /// </summary>
		ForeignKey,

        /// <summary>
        /// FN = SQL scalar function
        /// </summary>
		ScalarFunction,

        /// <summary>
        /// FS = Assembly (CLR) scalar-function
        /// </summary>
		CLRScalarFunction,

        /// <summary>
        /// FT = Assembly (CLR) table-valued function
        /// </summary>
		CLRTableValuedFunction,

        /// <summary>
        /// IF = SQL inline table-valued function
        /// </summary>
		InlineTableValuedFunction,

        /// <summary>
        /// IT = Internal table
        /// </summary>
		InternalTable,

        /// <summary>
        /// P = SQL Stored Procedure
        /// </summary>
		StoredProcedure,

        /// <summary>
        /// PC = Assembly (CLR) stored-procedure
        /// </summary>
		CLRStoredProcedure,

        /// <summary>
        /// PG = Plan guide
        /// </summary>
		PlanGuide,

        /// <summary>
        /// PK = PRIMARY KEY constraint
        /// </summary>
		PrimaryKey,

        /// <summary>
        /// R = Rule (old-style, stand-alone)
        /// </summary>
		Rule,

        /// <summary>
        /// RF = Replication-filter-procedure
        /// </summary>
		ReplicationFilter,

        /// <summary>
        /// S = System base table
        /// </summary>
		SystemTable,

        /// <summary>
        /// SN = Synonym
        /// </summary>
		Synonym,

        /// <summary>
        /// SO = Sequence object
        /// </summary>
		Sequence,

        #region Applies to: SQL Server 2012 through SQL Server 2014.

        /// <summary>
        /// SQ = Service queue
        /// </summary>
		ServiceQueue,

        /// <summary>
        /// TA = Assembly (CLR) DML trigger
        /// </summary>
		CLRTrigger,

        /// <summary>
        /// TF = SQL table-valued-function
        /// </summary>
		TableValuedFunction,

        /// <summary>
        /// TR = SQL DML trigger
        /// </summary>
		Trigger,

        /// <summary>
        /// TT = Table type
        /// </summary>
		TableType,

        /// <summary>
        /// U = Table (user-defined)
        /// </summary>
		Table,

        /// <summary>
        /// UQ = UNIQUE constraint
        /// </summary>
		UniqueConstraint,

        /// <summary>
        /// V = View
        /// </summary>
		View,

        /// <summary>
        /// X = Extended stored procedure
        /// </summary>
		ExtendedStoredProcedure,

        #endregion

        #region undocumented

        /// <summary>
        /// SP = Security Policy
        /// </summary>
		SecurityPolicy

        #endregion
	}
}
