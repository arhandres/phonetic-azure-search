using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneticAzureSearch.Model
{
    [SerializePropertyNamesAsCamelCase]
    public class IndexProduct
    {
        private string _id;
        private string _name;
        private string _description;
        private string _country;

        [System.ComponentModel.DataAnnotations.Key]
        [IsFilterable, IsSearchable, IsSortable]
        public string Id { get => _id; set => _id = value; }

        [IsSearchable, IsSortable]
        [Analyzer(AnalyzerName.AsString.EnLucene)]
        public string Name { get => _name; set => _name = value; }

        [IsSearchable, IsSortable]
        [Analyzer("PhoneticCustomAnalyzer")]
        public string NamePhonetic { get => _name; set => _name = value; }

        [IsSearchable, IsSortable]
        [Analyzer(AnalyzerName.AsString.EnLucene)]
        public string Description { get => _description; set => _description = value; }

        [IsSearchable, IsSortable]
        [Analyzer(AnalyzerName.AsString.EnLucene)]
        public string Country { get => _country; set => _country = value; }
        
        [IsSearchable, IsSortable]
        [Analyzer("PhoneticCustomAnalyzer")]
        public string CountryPhonetic { get => _country; set => _country = value; }
    }
}
