using System;
using System.Reflection;
using System.Linq;
using System.Text;
using NSUtils.Database.Attributes;
using NSUtils.Database.Utils;
using Mono.Data.Sqlite;
using System.Collections.Generic;

namespace NSUtils.Database.SQLite.Droid.Utils
{
    public class QueryUtilsSQLite : IQueryUtils<SqliteDataReader, SqliteParameter>
    {                
        public string GetTableName(Type tModelType)
        {
            var databaseAttribute = tModelType.GetTypeInfo().GetCustomAttribute<DatabaseTableAttribute>();

            string name = databaseAttribute.Name;

            if(string.IsNullOrWhiteSpace(name))
            {
                name = tModelType.Name;
            }

            return name;
        }

        public string CreateTableQuery(Type tModelType)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(string.Format("CREATE TABLE IF NOT EXISTS ([{0}]", GetTableName(tModelType)));

            stringBuilder.Append(GetFieldsToCreateTable(tModelType));
            stringBuilder.Append(GetPrimaryKeys(tModelType));

            stringBuilder.Append(");");
            return stringBuilder.ToString();
        }

        private string GetFieldsToCreateTable(Type tModelType)
        {
            var stringBuilder = new StringBuilder();

            foreach (var field in GetPropertiesThatIncludeInTable(tModelType))
            {
                var fieldAttribute = field.GetCustomAttribute<DatabaseFieldAttribute>();
                string name = fieldAttribute.Name;
                int size = fieldAttribute.Size;

                if (string.IsNullOrWhiteSpace(name))
                {
                    name = field.Name;
                }

                var type = ConvertDatabaseTypeTo(field.DeclaringType);

                stringBuilder.Append(GetFieldToCreateTable(name, size, type, fieldAttribute));
                stringBuilder.Append(", ");
            }

            stringBuilder.Remove(stringBuilder.Length - 2, 2);
            return stringBuilder.ToString();
        }

        private IEnumerable<PropertyInfo> GetPropertiesThatIncludeInTable(Type tModel)
        {
            return tModel.GetRuntimeProperties().Where(x => x.GetCustomAttributes().Any(y => y.GetType() == typeof(DatabaseFieldAttribute)));
        }

        private string GetPrimaryKeys(Type tModelType)
        {            
            var primaryKeys = tModelType.GetRuntimeProperties()
                .Where(x => x.GetCustomAttributes()
                .Any(y => y.GetType() == typeof(DatabaseFieldAttribute)) &&
                x.GetCustomAttribute<DatabaseFieldAttribute>().IsPrimaryKey == true);

            if(primaryKeys.Any())
            {
                var builder = new StringBuilder();
                foreach (var primaryKey in primaryKeys)
                {
                    var fieldAttribute = primaryKey.GetCustomAttribute<DatabaseFieldAttribute>();
                    string name = fieldAttribute.Name;

                    if(string.IsNullOrWhiteSpace(name))
                    {
                        name = primaryKey.Name;
                    }

                    builder.Append(string.Format("[{0}],", name));
                }

                builder.Remove(builder.Length, 1);
                return string.Format(",PRIMARY KEY({0})", builder.ToString());
            }

            return string.Empty;         
        }

        private string GetFieldToCreateTable(string name, int size, string type, DatabaseFieldAttribute fieldAttribute)
        {                    
            if(type == "NVARCHAR")
            {
                type = string.Format("{0}({1})", type, size);
            }

            return string.Format("[{0}] {1} {2}", name, type, GetExtraParameters(fieldAttribute));
        }

        private string GetExtraParameters(DatabaseFieldAttribute fieldAttribute)
        {
            string extraParameters = string.Empty;

            if(fieldAttribute.IsNull)
            {
                extraParameters += " NULL";
            }

            if(fieldAttribute.IsUnique)
            {
                extraParameters += " UNIQUE";
            }

            return extraParameters;
        }

        public string ConvertDatabaseTypeTo(Type typeField)
        {
            if(typeField == typeof(int))
            {
                return "INTEGER";
            }
            else if(typeField == typeof(string))
            {
                return "NVARCHAR";
            }
            else if(typeField == typeof(DateTime))
            {
                return "DATETIME";
            }
            else if(typeField == typeof(float))
            {
                return "FLOAT";
            }
            else if(typeField == typeof(double))
            {
                return "FLOAT";
            }
            else if(typeField == typeof(bool))
            {
                return "BOOLEAN";
            }
            else
            {
                throw new ArgumentException("Type not recognized");
            }
        }

        public CommonDataReader<TModel> ConvertToCommonDataReader<TModel>(SqliteDataReader dataReader) where TModel : class, new()
        {
            var commonDataReader = new CommonDataReader<TModel>();         
            if(dataReader.HasRows)
            {
                dataReader.Read();
                var rawRow = new object[dataReader.FieldCount];
                dataReader.GetValues(rawRow);

                commonDataReader = new CommonDataReader<TModel>(rawRow, GetColumnNames(dataReader));
                
                while (dataReader.Read())
                {
                    dataReader.GetValues(rawRow);
                    commonDataReader.Add(rawRow);
                }                                
            }
            return commonDataReader;
        }

        private List<string> GetColumnNames(SqliteDataReader dataReader)
        {
            var names = new List<string>();

            int fieldCount = dataReader.FieldCount;

            for(int i = 0; i < fieldCount; i++)
            {
                names.Add(dataReader.GetName(i));
            }

            return names;
        }

        public string CreateSelectQuery<TModel>() where TModel : class, new()
        {
            var fields = string.Join(" , ", GetPropertiesThatIncludeInTable(typeof(TModel)).Select(x => x.Name));
            var tableName = GetTableName(typeof(TModel));

            return string.Format("SELECT {0} FROM {1}", fields, tableName);
        }

        public string CreateSelectQuery<TModel>(TModel parameters) where TModel : class, new()
        {
            var query = CreateSelectQuery<TModel>();

            var whereProperties = string.Join(" AND ", GetPropertiesThatIncludeInTable(typeof(TModel))
                .Where(x => x.GetValue(parameters) != null).Select(x => string.Format("{0} = @{1}", x.Name, x.Name)));
                        
            return string.Format("{0} WHERE {1}", query, whereProperties);            
        }

        public List<SqliteParameter> CreateParameters<TModel>(TModel parameters) where TModel : class, new()
        {
            return GetPropertiesThatIncludeInTable(typeof(TModel))
                .Where(x => x.GetValue(parameters) != null)
                .Select(x => new SqliteParameter()
                {
                    Value = x.GetValue(parameters),
                    ParameterName = string.Format("@{0}", x.Name)
                }).ToList();            
        }
    }
}
