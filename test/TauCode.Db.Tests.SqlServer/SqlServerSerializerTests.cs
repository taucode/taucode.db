﻿using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Linq;
using TauCode.Db.Data;
using TauCode.Db.Model;
using TauCode.Db.SqlServer;

namespace TauCode.Db.Tests.SqlServer
{
    [TestFixture]
    public class SqlServerSerializerTests : TestBase
    {
        private IDbSerializer _dbSerializer;

        [SetUp]
        public void SetUp()
        {
            _dbSerializer = new SqlServerSerializer(this.Connection);
        }

        [Test]
        public void SerializeTableData_ValidInput_ProducesExpectedResult()
        {
            // Arrange
            var insertScript = TestHelper.GetResourceText("rho.script-insert-data.sql");
            this.Connection.ExecuteCommentedScript(insertScript);

            // Act
            var json = _dbSerializer.SerializeTableData("language");

            // Assert
            var expectedJson = TestHelper.GetResourceText("rho.data-language.json");
            Assert.That(json, Is.EqualTo(expectedJson));
        }

        [Test]
        public void SerializeDbData_ValidInput_ProducesExpectedResult()
        {
            // Arrange
            var insertScript = TestHelper.GetResourceText("rho.script-insert-data.sql");
            this.Connection.ExecuteCommentedScript(insertScript);

            // Act
            var json = _dbSerializer.SerializeDbData(x => x != "foo");

            // Assert
            var expectedJson = TestHelper.GetResourceText("rho.data-db.json");
            Assert.That(json, Is.EqualTo(expectedJson));
        }

        [Test]
        public void SerializeTableMetadata_ValidInput_ProducesExpectedResult()
        {
            // Arrange

            // Act
            var json = _dbSerializer.SerializeTableMetadata("language");

            // Assert
            var expectedJson = TestHelper.GetResourceText("rho.metadata-language.json");
            Assert.That(json, Is.EqualTo(expectedJson));
        }

        [Test]
        public void SerializeDbMetadata_ValidInput_ProducesExpectedResult()
        {
            // Arrange

            // Act
            var json = _dbSerializer.SerializeDbMetadata(x => x != "foo");
            var inverted = JsonConvert.DeserializeObject<DbMold>(json);

            // Assert
            var expectedJson = TestHelper.GetResourceText(".rho.metadata-db.json");
            Assert.That(json, Is.EqualTo(expectedJson));
            Assert.That(inverted.DbProviderName, Is.EqualTo("SqlServer"));
        }

        [Test]
        public void DeserializeTableData_ValidInput_ProducesExpectedResult()
        {
            // Arrange
            var json = TestHelper.GetResourceText("rho.data-language.json");

            // Act
            _dbSerializer.DeserializeTableData("language", json);

            // Assert
            var dictionary = this.GetRows("language").Select(x => new DynamicRow(x))
                .ToDictionary(x => x.GetValue("id"), x => (dynamic)x);

            var it = dictionary[new Guid("2a9ac9e3-eb27-4461-90d4-95a5e6b9d3e8")];
            Assert.That(it.code, Is.EqualTo("it"));
            Assert.That(it.name, Is.EqualTo("Italian"));

            var en = dictionary[new Guid("04990c0d-5d4a-41b9-98e4-103545d094d9")];
            Assert.That(en.code, Is.EqualTo("en"));
            Assert.That(en.name, Is.EqualTo("English"));
        }

        protected override void ExecuteDbCreationScript()
        {
            var script = TestHelper.GetResourceText("rho.script-create-tables.sql");
            this.Connection.ExecuteCommentedScript(script);
        }
    }
}
