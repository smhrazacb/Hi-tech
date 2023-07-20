namespace Catalog.API.Data
{
    public class DbContextSettings
    {
        public DbContextSettings(string connectionString, string databaseName, string collectionName)
        {
            ConnectionString = connectionString;
            DatabaseName = databaseName;
            CollectionName = collectionName;
        }

        public string ConnectionString { get; }
        public string DatabaseName { get; }
        public string CollectionName { get; }
    }
}