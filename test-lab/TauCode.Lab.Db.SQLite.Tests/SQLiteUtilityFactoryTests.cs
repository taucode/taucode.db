﻿using NUnit.Framework;
using System.Data;
using System.Data.SQLite;
using System.IO;
using TauCode.Db;

// todo clean
namespace TauCode.Lab.Db.SQLite.Tests
{
    [TestFixture]
    public class SQLiteUtilityFactoryTests
    {
        [Test]
        public void Members_DifferentArguments_HaveExpectedProps()
        {
            // Arrange
            IDbUtilityFactory utilityFactory = SQLiteUtilityFactory.Instance;

            // get SQLite stuff loaded.
            using (new SQLiteConnection())
            {   
            }

            // Act
            //var dbProviderName = utilityFactory.DbProviderName;

            //IDbConnection connection = utilityFactory.CreateConnection();
            IDbConnection connection = new SQLiteConnection();
            var tuple = TestHelper.CreateSQLiteConnectionString();
            var filePath = tuple.Item1;
            var connectionString = tuple.Item2;
            connection.ConnectionString = connectionString;
            connection.Open();

            IDbDialect dialect = utilityFactory.GetDialect();

            IDbScriptBuilder scriptBuilder = utilityFactory.CreateScriptBuilder(null);

            IDbInspector dbInspector = utilityFactory.CreateDbInspector(connection, null);

            IDbTableInspector tableInspector = utilityFactory.CreateTableInspector(connection, null, "language");

            IDbCruder cruder = utilityFactory.CreateCruder(connection, null);

            IDbSerializer dbSerializer = utilityFactory.CreateDbSerializer(connection, null);

            // Assert
            //Assert.That(dbProviderName, Is.EqualTo("SQLite"));
            Assert.That(connection, Is.TypeOf<SQLiteConnection>());
            Assert.That(dialect, Is.SameAs(SQLiteDialect.Instance));

            Assert.That(scriptBuilder, Is.TypeOf<SQLiteScriptBuilder>());
            Assert.That(scriptBuilder.CurrentOpeningIdentifierDelimiter, Is.EqualTo('['));

            Assert.That(dbInspector, Is.TypeOf<SQLiteInspector>());
            Assert.That(tableInspector, Is.TypeOf<SQLiteTableInspector>());
            Assert.That(cruder, Is.TypeOf<SQLiteCruder>());
            Assert.That(dbSerializer, Is.TypeOf<SQLiteSerializer>());

            // Finalize
            connection.Dispose();
            try
            {
                File.Delete(filePath);
            }
            catch
            {
                // dismiss
            }
        }
    }
}