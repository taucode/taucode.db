﻿using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TauCode.Db.SqlServer;

namespace TauCode.Db.Tests.SqlServer
{
    [TestFixture]
    public abstract class TestBase
    {
        protected IDbInspector DbInspector;
        protected IDbConnection Connection;

        protected const string ConnectionString = @"Server=.\mssqltest;Database=rho.test;User Id=testadmin;Password=1234;";

        protected virtual void OneTimeSetUpImpl()
        {
            this.Connection = new SqlConnection(ConnectionString);
            this.Connection.Open();
            this.DbInspector = new SqlServerInspector(Connection);

            this.DbInspector.DropAllTables();
            this.ExecuteDbCreationScript();
        }

        protected abstract void ExecuteDbCreationScript();

        protected virtual void OneTimeTearDownImpl()
        {
            this.Connection.Dispose();
        }

        protected virtual void SetUpImpl()
        {
            this.DbInspector.DeleteDataFromAllTables();
        }

        protected virtual void TearDownImpl()
        {
        }

        [OneTimeSetUp]
        public void OneTimeSetUpBase()
        {
            this.OneTimeSetUpImpl();
        }

        [OneTimeTearDown]
        public void OneTimeTearDownBase()
        {
            this.OneTimeTearDownImpl();
        }

        [SetUp]
        public void SetUpBase()
        {
            this.SetUpImpl();
        }

        [TearDown]
        public void TearDownBase()
        {
            this.TearDownImpl();
        }

        protected void CreateTables()
        {
            var script = TestHelper.GetResourceText("rho.script-create-tables.sql");
            this.Connection.ExecuteCommentedScript(script);
        }

        protected void DropTables()
        {
            var script = TestHelper.GetResourceText("rho.script-drop-tables.sql");
            this.Connection.ExecuteCommentedScript(script);
        }

        protected dynamic GetRow(string tableName, object id)
        {
            using (var command = this.Connection.CreateCommand())
            {
                command.CommandText = $@"SELECT * FROM [{tableName}] WHERE [id] = @p_id";
                var parameter = command.CreateParameter();
                parameter.ParameterName = "p_id";
                parameter.Value = id;
                command.Parameters.Add(parameter);
                var row = DbUtils.GetCommandRows(command).SingleOrDefault();
                return row;
            }
        }

        protected IList<dynamic> GetRows(string tableName)
        {
            using (var command = this.Connection.CreateCommand())
            {
                command.CommandText = $@"SELECT * FROM [{tableName}]";
                var rows = DbUtils.GetCommandRows(command);
                return rows;
            }
        }
    }
}
