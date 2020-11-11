﻿using Microsoft.Data.SqlClient;
using System;
using System.Data;
using TauCode.Db;
using TauCode.Db.DbValueConverters;
using TauCode.Db.Model;

namespace TauCode.Lab.Db.SqlClient
{
    public class SqlCruderLab : DbCruderBase
    {
        private const int TimeTypeColumnSize = 4;
        private const int DateTime2TypeColumnSize = 8;
        private const int DateTimeOffsetTypeColumnSize = 10;

        public SqlCruderLab(SqlConnection connection, string schemaName)
            : base(connection, schemaName ?? SqlToolsLab.DefaultSchemaName)
        {
        }

        public override IDbUtilityFactory Factory => SqlUtilityFactoryLab.Instance;

        protected override IDbValueConverter CreateDbValueConverter(ColumnMold column)
        {
            switch (column.Type.Name)
            {
                case "uniqueidentifier":
                    return new GuidValueConverter();

                case "bit":
                    return new BooleanValueConverter();

                case "tinyint":
                    return new ByteValueConverter();

                case "smallint":
                    return new Int16ValueConverter();

                case "int":
                    return new Int32ValueConverter();

                case "bigint":
                    return new Int64ValueConverter();

                case "decimal":
                case "numeric":

                case "money":
                case "smallmoney":
                    return new DecimalValueConverter();

                case "real":
                    return new SingleValueConverter();

                case "float":
                    return new DoubleValueConverter();

                case "date":
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                    return new DateTimeValueConverter();

                case "datetimeoffset":
                    return new DateTimeOffsetValueConverter();

                case "time":
                    return new TimeSpanValueConverter();

                case "char":
                case "varchar":
                case "nchar":
                case "nvarchar":
                    return new StringValueConverter();

                case "binary":
                case "varbinary":
                    return new ByteArrayValueConverter();

                default:
                    throw new NotImplementedException();
            }
        }

        protected override IDbDataParameter CreateParameter(string tableName, ColumnMold column)
        {
            const string parameterName = "parameter_name_placeholder";

            switch (column.Type.Name)
            {
                case "uniqueidentifier":
                    return new SqlParameter(parameterName, SqlDbType.UniqueIdentifier);

                case "bit":
                    return new SqlParameter(parameterName, SqlDbType.Bit);

                case "tinyint":
                    return new SqlParameter(parameterName, SqlDbType.TinyInt);

                case "smallint":
                    return new SqlParameter(parameterName, SqlDbType.SmallInt);

                case "int":
                    return new SqlParameter(parameterName, SqlDbType.Int);

                case "bigint":
                    return new SqlParameter(parameterName, SqlDbType.BigInt);

                case "decimal":
                case "numeric":
                    var parameter = new SqlParameter(parameterName, SqlDbType.Decimal);
                    parameter.Scale = (byte)(column.Type.Scale ?? 0);
                    parameter.Precision = (byte)(column.Type.Precision ?? 0);
                    return parameter;

                case "smallmoney":
                    return new SqlParameter(parameterName, SqlDbType.SmallMoney);

                case "money":
                    return new SqlParameter(parameterName, SqlDbType.Money);

                case "real":
                    return new SqlParameter(parameterName, SqlDbType.Real);

                case "float":
                    return new SqlParameter(parameterName, SqlDbType.Float);

                case "date":
                    return new SqlParameter(parameterName, SqlDbType.Date);

                case "datetime":
                    return new SqlParameter(parameterName, SqlDbType.DateTime);

                case "datetime2":
                    return new SqlParameter(parameterName, SqlDbType.DateTime2, DateTime2TypeColumnSize);

                case "datetimeoffset":
                    return new SqlParameter(parameterName, SqlDbType.DateTimeOffset, DateTimeOffsetTypeColumnSize);

                case "smalldatetime":
                    return new SqlParameter(parameterName, SqlDbType.SmallDateTime);

                case "time":
                    return new SqlParameter(parameterName, SqlDbType.Time, TimeTypeColumnSize);

                case "char":
                    return new SqlParameter(parameterName, SqlDbType.Char, column.Type.Size ?? throw new NotImplementedException());

                case "varchar":
                    return new SqlParameter(parameterName, SqlDbType.VarChar, column.Type.Size ?? throw new NotImplementedException());

                case "nchar":
                    return new SqlParameter(parameterName, SqlDbType.NChar, column.Type.Size ?? throw new NotImplementedException());

                case "nvarchar":
                    return new SqlParameter(parameterName, SqlDbType.NVarChar, column.Type.Size ?? throw new NotImplementedException());

                case "binary":
                    return new SqlParameter(parameterName, SqlDbType.Binary, column.Type.Size ?? throw new NotImplementedException());

                case "varbinary":
                    return new SqlParameter(parameterName, SqlDbType.VarBinary, column.Type.Size ?? throw new NotImplementedException());

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
