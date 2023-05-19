﻿using Catalog.API.Data;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catalog.APITests.Fixtures
{
    public class DbFixture : IDisposable
    {
        public DbFixture()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            DbContextSettings = new DbContextSettings(
                config.GetValue<string>("DatabaseSettings:ConnectionString"),
                $"test_db_{Guid.NewGuid()}",
                config.GetValue<string>("DatabaseSettings:CollectionName")
                );

            this.DbContext = new ProductContext(DbContextSettings);
        }
        public DbContextSettings DbContextSettings { get; }

        public ProductContext DbContext { get; }
        public void Dispose()
        {
            var client = new MongoClient(DbContextSettings.ConnectionString);
            client.DropDatabase(DbContextSettings.DatabaseName);
        }
    }
}
