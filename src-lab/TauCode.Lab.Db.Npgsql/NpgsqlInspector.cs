﻿using System.Collections.Generic;
using System.Data;
using System.Linq;
using TauCode.Db;

namespace TauCode.Lab.Db.Npgsql
{
    public class NpgsqlInspector : DbInspectorBase
    {
        #region Constants

        public const string DefaultSchema = "public";

        private const string TableTypeForTable = "BASE TABLE";

        #endregion

        #region Constructor

        public NpgsqlInspector(IDbConnection connection, string schema)
            : base(connection, schema ?? DefaultSchema)
        {
        }

        #endregion

        public override IDbUtilityFactory Factory => NpgsqlUtilityFactory.Instance;

        protected override IReadOnlyList<string> GetTableNamesImpl(string schema)
        {
            using var command = this.Connection.CreateCommand();
            var sql =
                $@"
SELECT
    T.table_name TableName
FROM
    information_schema.tables T
WHERE
    T.table_type = @p_tableType AND
    T.table_schema = @p_schema
";

            command.AddParameterWithValue("p_tableType", TableTypeForTable);
            command.AddParameterWithValue("p_schema", this.Schema);

            command.CommandText = sql;

            var tableNames = DbTools
                .GetCommandRows(command)
                .Select(x => (string)x.TableName)
                .ToArray();

            return tableNames;
        }
    }
}