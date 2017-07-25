using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MongoDB
{
    class Program
    {
        public static void Search(IMongoCollection<BsonDocument> collection)
        {
            var filterBuilder = Builders<BsonDocument>.Filter;
            var filter = filterBuilder.Empty;
            string field = string.Empty;
            string value = string.Empty;
            Console.WriteLine("Please first enter the field and then enter the value you are searching for");
            var readLine = Console.ReadLine();
            if (readLine != null) field = readLine.ToLower();
            readLine = Console.ReadLine();
            if (readLine != null)
                value = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(readLine.ToLower());
            Console.WriteLine("What kind of a filter would you like gt/gte/lt/lte/eq");
            string filterVariation = Console.ReadLine().ToLower();
            switch (filterVariation)
            {
                case "gt":
                    filter &= filterBuilder.Gt(field, value);
                    break;
                case "gte":
                    filter &= filterBuilder.Gte(field, value);
                    break;
                case "lt":
                    filter &= filterBuilder.Lt(field, value);
                    break;
                case "lte":
                    filter &= filterBuilder.Lte(field, value);
                    break;
                case "eq":
                    filter &= filterBuilder.Eq(field, value);
                    break;
                default:
                    break;
            }
            Console.WriteLine("would you like to add filter  y/n");
            while (string.Compare(Console.ReadLine(), "y", StringComparison.OrdinalIgnoreCase) == 0)
            {
                Console.WriteLine("Please first enter the field and then enter the value you are searching for");
                readLine = Console.ReadLine();
                if (readLine != null) field = readLine.ToLower();
                readLine = Console.ReadLine();
                if (readLine != null)
                    value = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(readLine.ToLower());

                Console.WriteLine("What kind of a filter would you like gt/gte/lt/lte/eq");
                filterVariation = Console.ReadLine().ToLower();
                switch (filterVariation)
                {
                    case "gt":
                        filter &= filterBuilder.Gt(field, value);
                        break;
                    case "gte":
                        filter &= filterBuilder.Gte(field, value);
                        break;
                    case "lt":
                        filter &= filterBuilder.Lt(field, value);
                        break;
                    case "lte":
                        filter &= filterBuilder.Lte(field, value);
                        break;
                    case "eq":
                        filter &= filterBuilder.Eq(field, value);
                        break;
                    default:
                        break;
                }


                Console.WriteLine("would you like to add another filter  y/n");
            }

            var document = collection.Find(filter);
            Console.WriteLine(document.ToList().Count() + "Restaurants have been found");
            document.Sort(Builders<BsonDocument>.Sort.Ascending("name"));
            int counter = 1;
            foreach (BsonDocument item in document.ToList())
            {
                Console.WriteLine("\nRestaurant " + counter);
                Console.WriteLine(item.GetElement("name").ToString());
                Console.WriteLine(item.GetElement("borough").ToString());
                Console.WriteLine(item.GetElement("cuisine").ToString());
                Console.WriteLine(item.GetElement("restaurant_id").ToString());
                counter++;
            }
        }

        static void Main(string[] args)
        {

            MongoClient client;

            try
            {
                client = new MongoClient("mongodb://localhost:27017");
            }
            catch 
            {
                Console.WriteLine("Could ot connect to mongo server pls make sure theconnection is open");
                return;
            }

            var database = client.GetDatabase("Test");
            var collection = database.GetCollection<BsonDocument>("Restaurants");
            var filterBuilder = Builders<BsonDocument>.Filter;
            FilterDefinition<BsonDocument> filter;
            Console.WriteLine("What would you like to do");
            var readLine = Console.ReadLine(); 
            if (string.Compare(readLine,"search",StringComparison.OrdinalIgnoreCase)==0)
            {              
                Search(collection);
            }

            else if (string.Compare(readLine, "add", StringComparison.OrdinalIgnoreCase) == 0)
            {
                Console.WriteLine("Enter your firstname and your last name pls");
                var keyValuePairs = new List<string>();
                keyValuePairs.Add("firstname");
                keyValuePairs.Add(Console.ReadLine());
                keyValuePairs.Add("lastname");
                keyValuePairs.Add(Console.ReadLine());
               /* bool a = true;
                while (a)
                {
                    string temporary = Console.ReadLine();
                    if (temporary != null && String.Compare(temporary, "trololo", StringComparison.OrdinalIgnoreCase) != 0)
                    {
                        keyValuePairs.Add(temporary);
                    }
                    else
                    {
                        a = false;
                    }
                }*/

                var documentToAdd = new BsonDocument
                {
                    { "nameofDB", "MongoDB" },
                    { "typeofDB", "Database" },
                    { keyValuePairs[0],  CultureInfo.CurrentCulture.TextInfo.ToTitleCase(keyValuePairs[1]) },
                    { keyValuePairs[2],  CultureInfo.CurrentCulture.TextInfo.ToTitleCase(keyValuePairs[3]) },
                    
                };
                collection.InsertOne(documentToAdd);

            }
            else if (string.Compare(readLine, "update", StringComparison.OrdinalIgnoreCase) == 0)
            {
                Search(collection);
                Console.WriteLine("Please enter the restaurant_id of the restaurant you want to edit and then field and value to edit");
                filter = Builders<BsonDocument>.Filter.Eq("restaurant_id", Console.ReadLine());
                string field = Console.ReadLine();
                string value = Console.ReadLine();
                var update = Builders<BsonDocument>.Update.Set(field, value);
                collection.UpdateOne(filter, update);
            }




            Console.ReadKey();
            

        }
    }
}
