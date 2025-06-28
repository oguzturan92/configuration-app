using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using DynamicConfiguration.Core.Models;

namespace DynamicConfiguration.Core.Storage
{
    public class SqlStorageProvider : IStorageProvider
    {
        private readonly string _connectionString;

        public SqlStorageProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<ConfigurationItem>> LoadAsync(string applicationName)
        {
            var result = new List<ConfigurationItem>();
            var query = @"SELECT Id, Name, Type, Value, IsActive, ApplicationName
                        FROM ConfigurationItems
                        WHERE ApplicationName = @appName AND IsActive = 1";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@appName", applicationName);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(new ConfigurationItem
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Type = reader.GetString(2),
                    Value = reader.GetString(3),
                    IsActive = reader.GetBoolean(4),
                    ApplicationName = reader.GetString(5),
                });
            }

            return result;
        }
    }
}