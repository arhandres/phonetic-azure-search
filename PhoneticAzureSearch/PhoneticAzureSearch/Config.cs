using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PhoneticAzureSearch
{
    public static class Config
    {
        private static Lazy<IConfiguration> _configuration = new Lazy<IConfiguration>(() =>
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

            var configuration = builder.Build();
            return configuration;
        });

        public static T GetValue<T>(string key, T defaultValue)
        {
            var value = _configuration.Value[key];

            if (string.IsNullOrEmpty(value))
                return defaultValue;

            try
            {
                if (IsSimple(typeof(T)))
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                else
                {
                    return JsonConvert.DeserializeObject<T>(value);
                }
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static T GetValue<T>(string key)
        {
            return GetValue<T>(key, default(T));
        }

        private static bool IsSimple(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return IsSimple(type.GetGenericArguments()[0]);

            return type.IsPrimitive
              || type.IsEnum
              || type.Equals(typeof(string))
              || type.Equals(typeof(decimal));
        }
    }
}
