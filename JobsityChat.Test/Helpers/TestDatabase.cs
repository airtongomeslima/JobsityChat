using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JobsityChat.Test.Helpers
{
    public class TestDatabase
    {
        private readonly SqlConnection _connection;
        private string connectionString;

        public TestDatabase(string connectionString)
        {
            this.connectionString = connectionString;
            CreateSqlDatabase();
            _connection = new SqlConnection(connectionString);
        }

        public void CreateSqlDatabase()
        {
            var dbname = "testdb";
            var file = Path.Combine(Directory.GetCurrentDirectory(), $"{dbname}.mdf");
            var logfile = Path.Combine(Directory.GetCurrentDirectory(), $"{dbname}_log.ldf");

            if (File.Exists(file))
            {
                File.Delete(file);
                File.Delete(logfile);
            }

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        String.Format("CREATE DATABASE {0} ON PRIMARY (NAME={0}, FILENAME='{1}')", dbname, file);
                    command.ExecuteNonQuery();

                    command.CommandText =
                        String.Format("EXEC sp_detach_db '{0}', 'true'", dbname);
                    command.ExecuteNonQuery();
                }
            }
        }

        public SqlConnection OpenConnection()
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();
            return _connection;
        }


        public async Task<object> ExecuteScalarAsync<T>(string tableName, string query) where T : class
        {
            var createTableScript = GenerateTableScriptIfNotExist<T>(tableName);
            using SqlCommand createCommand = new SqlCommand(createTableScript, OpenConnection());
            await createCommand.ExecuteNonQueryAsync();

            using SqlCommand InsertCommand = new SqlCommand(query, OpenConnection());
            return await InsertCommand.ExecuteScalarAsync();
        }


        #region Helpers
     
        private static string FindIdentificationPropertie<T>()
        {
            var properties = typeof(T).GetProperties();
            return (from prop in properties
                    let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    where attributes.Length > 0 && (attributes[0] as DescriptionAttribute)?.Description == "id"
                    select prop.Name).FirstOrDefault() ?? $"id";
        }


        public static string GenerateTableScriptIfNotExist<T>(string tableName)
        {
            var identificationField = FindIdentificationPropertie<T>();
            var fields = new List<KeyValuePair<String, Type>>();

            foreach (PropertyInfo p in typeof(T).GetProperties())
            {
                var field = new KeyValuePair<String, Type>(p.Name, p.PropertyType);

                fields.Add(field);
            }

            System.Text.StringBuilder script = new StringBuilder();

            script.AppendLine($"if not exists(select * from sysobjects where name = '{tableName}' and xtype = 'U')");
            script.AppendLine("\tCREATE TABLE [" + tableName + "]");
            script.AppendLine("\t(");
            script.AppendLine("\t\t ID BIGINT,");
            for (int i = 0; i < fields.Count; i++)
            {
                KeyValuePair<String, Type> field = fields[i];


                if(field.Key == identificationField)
                {
                    script.Append("\t\t" + field.Key + " int  IDENTITY(1,1) PRIMARY KEY");
                } 
                else if (dataMapper.ContainsKey(field.Value))
                {
                    script.Append("\t\t " + field.Key + " " + dataMapper[field.Value]);
                }
                else
                {
                    script.Append("\t\t " + field.Key + " NVARCHAR(500)");
                }

                if (i != fields.Count - 1)
                {
                    script.Append(",");
                }

                script.Append(Environment.NewLine);
            }

            script.AppendLine("\t)");

            return script.ToString();
        }

        private static Dictionary<Type, String> dataMapper
        {
            get
            {
                Dictionary<Type, String> dataMapper = new Dictionary<Type, string>();
                dataMapper.Add(typeof(int), "INT");
                dataMapper.Add(typeof(string), "NVARCHAR(500)");
                dataMapper.Add(typeof(bool), "BIT");
                dataMapper.Add(typeof(DateTime), "DATETIME");
                dataMapper.Add(typeof(float), "FLOAT");
                dataMapper.Add(typeof(decimal), "DECIMAL(18,0)");
                dataMapper.Add(typeof(Guid), "UNIQUEIDENTIFIER");

                return dataMapper;
            }
        }

        #endregion

    }
}
