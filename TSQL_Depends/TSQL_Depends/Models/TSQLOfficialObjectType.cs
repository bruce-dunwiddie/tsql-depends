namespace TSQL.Depends.Models
{
	internal enum TSQLOfficialObjectType
	{
        /// <summary>
        /// AF = Aggregate function (CLR)
        /// </summary>
        CLRAggregateFunction = 0,

        /// <summary>
        /// C = CHECK constraint
        /// </summary>
		CheckConstraint = 1,

        /// <summary>
        /// D = DEFAULT (constraint or stand-alone)
        /// </summary>
		Default = 2,

        /// <summary>
        /// F = FOREIGN KEY constraint
        /// </summary>
		ForeignKey = 3,

        /// <summary>
        /// FN = SQL scalar function
        /// </summary>
		ScalarFunction = 4,

        /// <summary>
        /// FS = Assembly (CLR) scalar-function
        /// </summary>
		CLRScalarFunction = 5,

        /// <summary>
        /// FT = Assembly (CLR) table-valued function
        /// </summary>
		CLRTableValuedFunction = 6,

        /// <summary>
        /// IF = SQL inline table-valued function
        /// </summary>
		InlineTableValuedFunction = 7,

        /// <summary>
        /// IT = Internal table
        /// </summary>
		InternalTable = 8,

        /// <summary>
        /// P = SQL Stored Procedure
        /// </summary>
		StoredProcedure = 9,

        /// <summary>
        /// PC = Assembly (CLR) stored-procedure
        /// </summary>
		CLRStoredProcedure = 10,

        /// <summary>
        /// PG = Plan guide
        /// </summary>
		PlanGuide = 11,

        /// <summary>
        /// PK = PRIMARY KEY constraint
        /// </summary>
		PrimaryKey = 12,

        /// <summary>
        /// R = Rule (old-style, stand-alone)
        /// </summary>
		Rule = 13,

        /// <summary>
        /// RF = Replication-filter-procedure
        /// </summary>
		ReplicationFilter = 14,

        /// <summary>
        /// S = System base table
        /// </summary>
		SystemTable = 15,

        /// <summary>
        /// SN = Synonym
        /// </summary>
		Synonym = 16,

        /// <summary>
        /// SO = Sequence object
        /// </summary>
		Sequence = 17,

        #region Applies to: SQL Server 2012 through SQL Server 2014.

        /// <summary>
        /// SQ = Service queue
        /// </summary>
		ServiceQueue = 18,

        /// <summary>
        /// TA = Assembly (CLR) DML trigger
        /// </summary>
		CLRTrigger = 19,

        /// <summary>
        /// TF = SQL table-valued-function
        /// </summary>
		TableValuedFunction = 20,

        /// <summary>
        /// TR = SQL DML trigger
        /// </summary>
		Trigger = 21,

        /// <summary>
        /// TT = Table type
        /// </summary>
		TableType = 22,

        /// <summary>
        /// U = Table (user-defined)
        /// </summary>
		Table = 23,

        /// <summary>
        /// UQ = UNIQUE constraint
        /// </summary>
		UniqueConstraint = 24,

        /// <summary>
        /// V = View
        /// </summary>
		View = 25,

        /// <summary>
        /// X = Extended stored procedure
        /// </summary>
		ExtendedStoredProcedure = 26,

        #endregion

        #region undocumented

        /// <summary>
        /// SP = Security Policy
        /// </summary>
		SecurityPolicy = 27,

		#endregion

		/// <summary>
		/// EC = Edge constraint
		/// </summary>
		EdgeConstraint = 28,

		/// <summary>
		/// ET = External Table
		/// </summary>
		ExternalTable = 29
	}
}
