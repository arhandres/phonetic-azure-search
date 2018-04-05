using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Newtonsoft.Json;
using PhoneticAzureSearch.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneticAzureSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            //UploadData();
            //Console.WriteLine("Data uploaded");

            var result1 = SearchPhrase("kia");
            PrintResult(result1);

            var result2 = SearchPhrase("qia");
            PrintResult(result2);


            Console.ReadKey();
        }

        static void PrintResult(DocumentSearchResult<IndexProduct> result)
        {
            Console.WriteLine("Total: {0}", result.Count);
            foreach(var r in result.Results)
                Console.WriteLine("{0} - {1} | score:{2}", r.Document.Id, r.Document.Name, r.Score);

            Console.WriteLine();
        }

        static CustomAnalyzer CreatePhoneticCustomAnalyzer()
        {
            var analyzer = new CustomAnalyzer
            {
                TokenFilters = new List<TokenFilterName> { TokenFilterName.Phonetic, TokenFilterName.AsciiFolding, TokenFilterName.Lowercase },
                Tokenizer = TokenizerName.Standard,
                Name = "PhoneticCustomnAlyzer"
            };

            return analyzer;
        }

        static Index CreateIndexDefinition()
        {
            var phoneticAnalizer = CreatePhoneticCustomAnalyzer();

            var definition = new Index()
            {
                Name = Config.GetValue<string>("AzureSearch:AzureSearchIndexName"),
                Fields = FieldBuilder.BuildForType<IndexProduct>(),
                Analyzers = new List<Analyzer> { phoneticAnalizer },
            };

            return definition;
        }

        static ISearchIndexClient CreateIndexAndGetClient()
        {
            var searchServiceName = Config.GetValue<string>("AzureSearch:AzureSearchServiceName");
            var apiKey = Config.GetValue<string>("AzureSearch:AzureSearchKey");

            var serviceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(apiKey));
            var index = CreateIndexDefinition();

            if (!serviceClient.Indexes.Exists(index.Name))
                serviceClient.Indexes.Create(index);

            var indexClient = serviceClient.Indexes.GetClient(index.Name);

            return indexClient;
        }

        static DocumentSearchResult<IndexProduct> SearchPhrase(string phrase)
        {
            var indexClient = CreateIndexAndGetClient();

            var data = indexClient.Documents.Search<IndexProduct>(phrase, new SearchParameters()
            {
                SearchFields = new List<string>() { "namePhonetic" },
                SearchMode = SearchMode.Any,
                IncludeTotalResultCount = true
            });

            return data;
        }

        static void UploadData()
        {
            var data = CreateBatchData(stepSize: 1000);

            var indexClient = CreateIndexAndGetClient();

            Iterate(data, b =>
            {
                try
                {

                    indexClient.Documents.Index(b);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }, inParallel: true);
        }

        static List<IndexBatch<IndexProduct>> CreateBatchData(int stepSize)
        {
            var data = Encoding.UTF8.GetString(Resource.MOCK_DATA);

            var products = JsonConvert.DeserializeObject<List<IndexProduct>>(data);

            var steps = (int)Math.Ceiling(products.Count / (double)stepSize);

            var batches = new List<IndexBatch<IndexProduct>>();

            for (var i = 0; i < steps; i++)
            {
                var s = (i + 1);

                var result = products.Skip(i * stepSize).Take(stepSize).ToList();

                var documents = result.Select(p => IndexAction.Upload(p)).ToList();

                var batch = IndexBatch.New(documents);

                batches.Add(batch);
            }

            return batches;
        }

        static void Iterate<T>(IEnumerable<T> collecion, Action<T> a, bool inParallel = true)
        {
            if (!inParallel)
            {
                foreach (var item in collecion)
                    a(item);
            }
            else
            {
                Parallel.ForEach(collecion, a);
            }
        }
    }
}
