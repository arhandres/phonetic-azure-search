using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using PhoneticAzureSearch.Model;
using System;
using System.Collections.Generic;

namespace PhoneticAzureSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
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
    }
}
