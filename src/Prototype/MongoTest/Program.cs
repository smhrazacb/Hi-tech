// See https://aka.ms/new-console-template for more information
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoTest.Entities;

Console.WriteLine("Hello, World!");
var dbClient = new MongoClient("mongodb://127.0.0.1:27017");
var dbList = dbClient.ListDatabases().ToList();
IMongoDatabase db = dbClient.GetDatabase("CatalogDb");

//var command = new BsonDocument { { "dbstats", 1 } };
//var result = db.RunCommand<BsonDocument>(command);
//Console.WriteLine(result.ToJson());


var categories = db.GetCollection<Category>("Categories");
var documents = categories.Find(a => a.CategoryName == "Mechanical").ToList();
documents = categories.Find(a => a.SubCategory.Product.Price == 900).ToList();


//var gr = categories.Aggregate()
//    .Group(
//    a => a.CategoryName,
//    b=> new 
//    {
//        CategoryC = b.Key, 
//        Count = b.Count(),
//    }
//    )
//    .ToList();

var gsr = categories.Aggregate()
    .Group(
    a => new { a.CategoryName, a.SubCategory.SubCategoryName},
    b => new
    {
        SubCategory = b.Key.CategoryName,
        Category = b.Key.SubCategoryName,
        Count = b.Count(),
    }
    )
    .ToList();

Console.WriteLine();