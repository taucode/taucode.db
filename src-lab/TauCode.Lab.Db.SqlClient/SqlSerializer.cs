﻿using System.Data;
using TauCode.Db;

namespace TauCode.Lab.Db.SqlClient
{
    public class SqlSerializer : DbSerializerBase
    {
        public SqlSerializer(IDbConnection connection)
            : base(connection)

        {
        }
        public override IDbUtilityFactory Factory => SqlUtilityFactory.Instance;
    }
}
