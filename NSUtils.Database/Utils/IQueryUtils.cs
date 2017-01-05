using System;
using System.Collections.Generic;
using System.Reflection;

namespace NSUtils.Database.Utils
{
    public interface IQueryUtils<TDataReader, TParameters>
    {
        string GetTableName(Type tModelType);

        string CreateTableQuery(Type tModelType);

        string ConvertDatabaseTypeTo(Type typeField);
        
        CommonDataReader<TModel> ConvertToCommonDataReader<TModel>(TDataReader dataReader) where TModel : class, new();

        string CreateSelectQuery<TModel>() where TModel : class, new();

        string CreateSelectQuery<TModel>(TModel parameters) where TModel : class, new();

        List<TParameters> CreateParameters<TModel>(TModel parameters) where TModel : class, new();

    }
}
