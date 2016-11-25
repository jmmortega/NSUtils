using NSUtils.Interfaces;
using System;
using System.Collections.Generic;


namespace NSUtils.Database
{
    public abstract class CommonDatabase
    {
        protected ILogger Logger {get;set;}
        protected abstract void CreateTable(string query);
        protected abstract void DropTable(string tableName);
        public abstract List<TModel> Select<TModel>(string query, Func<object, List<TModel>> serializeAction) where TModel : class;

        public abstract List<TModel> Select<TModel>(string sql, List<object> parameters, Func<object, List<TModel>> serializeAction) where TModel : class;

        public abstract TElement SelectFirst<TElement>(string sql, List<object> parameters, Func<object, TElement> serializeAction);

        public abstract void Insert(string sql, List<object> parameters);

        public abstract void Update(string sql, List<object> parameters);

        public abstract void Delete(string sql, List<object> parameters);

        public abstract void InsertAll(string sql, List<List<object>> parameters);

        public abstract void InsertAllWOTransaction(string sql, List<List<object>> parameters);

        protected abstract void LaunchSQL(string sql);

        protected abstract void DoTransaction(string sql, List<object> parameters);

        protected abstract TCommand PrepareCommand<TCommand>(string sql);

        protected abstract TCommand PrepareCommand<TCommand>(string sql, List<object> parameters);
        
            
        








     }
}
