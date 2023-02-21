// See https://aka.ms/new-console-template for more information
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoTest.Entities;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

Console.WriteLine("Hello, World!");


var regions = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(x => new RegionInfo(x.LCID).EnglishName).Distinct().ToList();


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