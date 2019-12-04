﻿using System.Data;

namespace TauCode.Db.SqlServer
{
    public class SqlServerInspector : SqlServerInspectorBase
    {
        #region Constructor

        public SqlServerInspector(IDbConnection connection)
            : base(connection)
        {
        }

        #endregion

        #region Overridden

        protected override string TableTypeForTable => "BASE TABLE";

        protected override SqlServerTableInspectorBase CreateTableInspectorImpl(string realTableName)
        {
            return new SqlServerTableInspector(this.Connection, realTableName);
        }

        //protected override ICruder CreateCruder()
        //{
        //    return new SqlServerCruder(this.Connection);
        //}

        public override IScriptBuilder CreateScriptBuilder()
        {
            return new SqlServerScriptBuilder
            {
                CurrentOpeningIdentifierDelimiter = '[',
            };
        }

        #endregion
    }
}