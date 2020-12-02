using System;
using System.Configuration;

namespace ImageUploadDemo.Common
{
    internal static class DBConfig
    {
        private static string _connString = string.Empty;
        internal static string ConnStr
        {
            get
            {
                if (string.IsNullOrEmpty(_connString))
                {
                    _connString = GetConnectionString("connStr");
                }

                return _connString;
            }
        }

        private static string GetConnectionString(string name)
        {
            ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings[name];
            return connectionString != null 
                ? connectionString.ConnectionString 
                : throw new ArgumentException(nameof (name), $"Name:{(object) name} connection string is not setting.");
        }

    }
}