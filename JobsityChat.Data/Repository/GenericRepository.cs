using Dapper;
using JobsityChat.Domain.Mappers;
using JobsityChat.Domain.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JobsityChat.Data.Repository
{
    public class GenericRepository<T> : IGenericRepository<T>
        where T : class
    {
        private readonly string connectionString;
        private readonly string tableName;
        private IEnumerable<PropertyInfo> GetProperties => typeof(T).GetProperties();


        public GenericRepository(string tableName, string connectionString)
        {
            this.tableName = tableName;
            this.connectionString = connectionString;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            using (var connection = CreateConnection())
            {
                var result = await connection.QueryAsync<T>($"SELECT * FROM [{tableName}]");
                var resultList = result.Select(r => r).ToList();
                return resultList;
            }
        }

        public async Task<T> GetAsync(int id)
        {
            var identification = FindIdentificationPropertie(GetProperties);

            using (var connection = CreateConnection())
            {
                var result = await connection.QuerySingleOrDefaultAsync<T>($"SELECT * FROM [{tableName}] WHERE {identification}=@Id", new { Id = id });
                if (result == null)
                    throw new KeyNotFoundException($"{tableName} with id [{id}] could not be found.");

                return result;
            }
        }

        public async Task<int> SaveRangeAsync(IEnumerable<T> list)
        {
            var inserted = 0;
            var query = GenerateInsertQuery();
            using (IDbConnection connection = CreateConnection())
            {
                inserted += await connection.ExecuteAsync(query, list);
            }

            return inserted;
        }

        public async Task<int> InsertAsync(T t)
        {
            var insertQuery = GenerateInsertQuery();

            using (var connection = CreateConnection())
            {

                var result = await connection.QueryAsync<int>(insertQuery, t);
                return result.SingleOrDefault();
            }
        }

        public async Task UpdateAsync(T t)
        {
            var updateQuery = GenerateUpdateQuery();

            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync(updateQuery, t);
            }
        }

        public async Task DeleteRowAsync(int id)
        {
            var identification = FindIdentificationPropertie(GetProperties);
            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync($"DELETE FROM [{tableName}] WHERE {identification}=@id", new { id });
            }
        }

        public async Task ExecScript(string script)
        {
            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync(script);
            }
        }

        #region Helpers

        private SqlConnection SqlConnection()
        {
            return new SqlConnection(connectionString);
        }

        private IDbConnection CreateConnection()
        {
            var conn = SqlConnection();
            conn.Open();
            return conn;
        }
        private string GenerateInsertQuery()
        {
            var insertQuery = new StringBuilder($"INSERT INTO [{tableName}] ");
            var identification = FindIdentificationPropertie(GetProperties);
            var properties = GenerateListOfProperties(GetProperties);

            insertQuery.Append("(");

            var props = String.Join(", ", properties
                                    .Where(p => p != identification)
                                    .Select(p => $"[{p}]")
                                    .ToList()
                        );

            insertQuery.Append(props);

            insertQuery.Append(") VALUES (");

            var values = String.Join(", ", properties
                                    .Where(p => p != identification)
                                    .Select(p => $"@{p}")
                                    .ToList()
                        );

            insertQuery.Append(values);

            insertQuery.Append("); SELECT CAST(SCOPE_IDENTITY() as int);");

            return insertQuery.ToString();
        }

        private string GenerateUpdateQuery()
        {
            var updateQuery = new StringBuilder($"UPDATE [{tableName}] SET ");
            var properties = GenerateListOfProperties(GetProperties);
            var identification = FindIdentificationPropertie(GetProperties);

            var props = String.Join(", ", properties.Where(p => p != identification).Select(p => $"[{p}]=@{p}").ToList());

            updateQuery.Append(props);

            updateQuery.Append($" WHERE {identification}=@{identification}");

            return updateQuery.ToString();
        }


        private static List<string> GenerateListOfProperties(IEnumerable<PropertyInfo> listOfProperties)
        {
            return (from prop in listOfProperties
                    let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    where attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore"
                    select prop.Name).ToList();
        }

        private static string FindIdentificationPropertie(IEnumerable<PropertyInfo> listOfProperties)
        {
            return (from prop in listOfProperties
                    let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    where attributes.Length > 0 && (attributes[0] as DescriptionAttribute)?.Description == "id"
                    select prop.Name).FirstOrDefault() ?? $"id";
        }

        #endregion
    }
}
