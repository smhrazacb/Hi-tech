namespace Catalog.API.Data
{
    public class DbContextSettings
    {
        public DbContextSettings(string connectionString, string databaseName, string collectionName, string eCollectionName)
        {
            ConnectionString = connectionString;
            DatabaseName = databaseName;
            CollectionName = collectionName;
            ECollectionName = eCollectionName;
        }

        public string ConnectionString { get; }
        public string DatabaseName { get; }
        public string CollectionName { get; }
        public string ECollectionName { get; }
    }
}