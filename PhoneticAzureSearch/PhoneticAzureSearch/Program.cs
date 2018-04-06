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
            UploadData();
            Console.WriteLine("Data uploaded");

            SearchWithoutPhonetic();

            SearchJustPhonetic();

            SearchJustFuzzyLucene();

            SearchPhoneticAndFuzzyLucene();

            Console.ReadKey();
        }

        static void SearchWithoutPhonetic()
        {
            Console.WriteLine("::::::: SearchWithoutPhonetic :::::::");
            Console.WriteLine();

            var result1 = SearchPhrase("kia", columns: "name");
            PrintResult(result1);

            var result2 = SearchPhrase("qia", columns: "name");
            PrintResult(result2);

            var result3 = SearchPhrase("Mercedes-Benz", columns: "name");
            PrintResult(result3);

            var result4 = SearchPhrase("Mersedes-Bens", columns: "name");
            PrintResult(result4);

            var result5 = SearchPhrase("Mazda", columns: "name");
            PrintResult(result5);

            var result6 = SearchPhrase("Masda", columns: "name");
            PrintResult(result6);

            var result7 = SearchPhrase("Lincoln", columns: "name");
            PrintResult(result7);

            var result8 = SearchPhrase("Lyncoln", columns: "name");
            PrintResult(result8);

            var result9 = SearchPhrase("Lyncon", columns: "name");
            PrintResult(result9);

            var result10 = SearchPhrase("Jeep", columns: "name");
            PrintResult(result10);

            var result11 = SearchPhrase("Geep", columns: "name");
            PrintResult(result11);
        }

        static void SearchJustPhonetic()
        {
            Console.WriteLine("::::::: SearchJustPhonetic :::::::");
            Console.WriteLine();

            var result1 = SearchPhrase("kia", columns: "namePhonetic");
            PrintResult(result1);

            var result2 = SearchPhrase("qia", columns: "namePhonetic");
            PrintResult(result2);

            var result3 = SearchPhrase("Mercedes-Benz", columns: "namePhonetic");
            PrintResult(result3);

            var result4 = SearchPhrase("Mersedes-Bens", columns: "namePhonetic");
            PrintResult(result4);

            var result5 = SearchPhrase("Mazda", columns: "namePhonetic");
            PrintResult(result5);

            var result6 = SearchPhrase("Masda", columns: "namePhonetic");
            PrintResult(result6);

            var result7 = SearchPhrase("Lincoln", columns: "namePhonetic");
            PrintResult(result7);

            var result8 = SearchPhrase("Lyncoln", columns: "namePhonetic");
            PrintResult(result8);

            var result9 = SearchPhrase("Lyncon", columns: "namePhonetic");
            PrintResult(result9);

            var result10 = SearchPhrase("Jeep", columns: "namePhonetic");
            PrintResult(result10);

            var result11 = SearchPhrase("Geep", columns: "namePhonetic");
            PrintResult(result11);
        }

        static void SearchJustFuzzyLucene()
        {
            Console.WriteLine("::::::: SearchJustFuzzyLucene :::::::");
            Console.WriteLine();

            var result1 = SearchPhrase("kia~", lucene: true, columns: "name");
            PrintResult(result1);

            var result2 = SearchPhrase("qia~", lucene: true, columns: "name");
            PrintResult(result2);

            var result3 = SearchPhrase("Mercedes-Benz~", lucene: true, columns: "name");
            PrintResult(result3);

            var result4 = SearchPhrase("Mersedes-Bens~", lucene: true, columns: "name");
            PrintResult(result4);

            var result5 = SearchPhrase("Mazda~", lucene: true, columns: "name");
            PrintResult(result5);

            var result6 = SearchPhrase("Masda~", lucene: true, columns: "name");
            PrintResult(result6);

            var result7 = SearchPhrase("Lincoln~", lucene: true, columns: "name");
            PrintResult(result7);

            var result8 = SearchPhrase("Lyncoln~", lucene: true, columns: "name");
            PrintResult(result8);

            var result9 = SearchPhrase("Lyncon~", lucene: true, columns: "name");
            PrintResult(result9);

            var result10 = SearchPhrase("Jeep~", lucene: true, columns: "name");
            PrintResult(result10);

            var result11 = SearchPhrase("Geep~", lucene: true, columns: "name");
            PrintResult(result11);
        }

        static void SearchPhoneticAndFuzzyLucene()
        {
            Console.WriteLine("::::::: SearchPhoneticAndFuzzyLucene :::::::");
            Console.WriteLine();

            var result1 = SearchPhrase("kia || kia~", lucene: true, columns: "name,namePhonetic");
            PrintResult(result1);

            var result2 = SearchPhrase("qia || qia~", lucene: true, columns: "name,namePhonetic");
            PrintResult(result2);

            var result3 = SearchPhrase("Mercedes-Benz | |Mercedes-Benz~", lucene: true, columns: "name,namePhonetic");
            PrintResult(result3);

            var result4 = SearchPhrase("Mersedes-Bens || Mersedes-Bens~", lucene: true, columns: "name,namePhonetic");
            PrintResult(result4);

            var result5 = SearchPhrase("Mazda || Mazda~", lucene: true, columns: "name,namePhonetic");
            PrintResult(result5);

            var result6 = SearchPhrase("Masda || Masda~", lucene: true, columns: "name,namePhonetic");
            PrintResult(result6);

            var result7 = SearchPhrase("Lincoln || Lincoln~", lucene: true, columns: "name,namePhonetic");
            PrintResult(result7);

            var result8 = SearchPhrase("Lyncoln || Lyncoln~", lucene: true, columns: "name,namePhonetic");
            PrintResult(result8);

            var result9 = SearchPhrase("Lyncon || Lyncon~", lucene: true, columns: "name,namePhonetic");
            PrintResult(result9);

            var result10 = SearchPhrase("Jeep || Jeep~", lucene: true, columns: "name,namePhonetic");
            PrintResult(result10);

            var result11 = SearchPhrase("Geep || Geep~", lucene: true, columns: "name,namePhonetic");
            PrintResult(result11);
        }

        static void PrintResult(DocumentSearchResult<IndexProduct> result)
        {
            Console.WriteLine("Total: {0}", result.Count);
            foreach (var r in result.Results)
                Console.WriteLine("{0} - {1} | score:{2}", r.Document.Id, r.Document.Name, r.Score);

            Console.WriteLine();
        }

        static CustomAnalyzer CreatePhoneticCustomAnalyzer()
        {
            var analyzer = new CustomAnalyzer
            {
                TokenFilters = new List<TokenFilterName> { TokenFilterName.Phonetic, TokenFilterName.AsciiFolding, TokenFilterName.Lowercase },
                Tokenizer = TokenizerName.Standard,
                Name = "PhoneticCustomAnalyzer"
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

        static DocumentSearchResult<IndexProduct> SearchPhrase(string phrase, params string[] columns)
        {
            return SearchPhrase(phrase, false, columns);
        }

        static DocumentSearchResult<IndexProduct> SearchPhrase(string phrase, bool lucene = false, params string[] columns)
        {
            Console.WriteLine("Phrase: {0}", phrase);
            Console.WriteLine("Lucene Syntax: {0}", lucene ? "Yes" : "No");
            Console.WriteLine("Columns: {0}", string.Join(',', columns));

            var indexClient = CreateIndexAndGetClient();

            var data = indexClient.Documents.Search<IndexProduct>(phrase, new SearchParameters()
            {
                SearchFields = new List<string>(columns),
                SearchMode = SearchMode.Any,
                IncludeTotalResultCount = true,
                QueryType = lucene ? QueryType.Full : QueryType.Simple, //For Lucene Syntax,
                Top = 1000
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
