using NSUtils.Database.Utils;
using NSUtils.Interfaces;
using System;
using System.Collections.Generic;


namespace NSUtils.Database
{
    public abstract class CommonDatabase<TCommand, TParameter, TReader>
    {        
        public virtual bool IsConnected { get; protected set; }
        protected ILogger Logger {get;set;}
        protected IQueryUtils<TReader, TParameter> Utils { get; set; }
        protected void LogInfo(string message)
        {
            if(Logger != null)
            {
                Logger.Info(message);
            }

        }
        protected void LogError(Exception e)
        {
            if(Logger != null)
            {
                Logger.Error(e);
            }

        }        
        protected abstract void Connect(string connectionString);
        protected abstract void Close();
        protected abstract void CreateTable(string query);
        protected abstract void CreateTable<TModel>() where TModel : class, new();

        protected abstract void DropTable<TModel>() where TModel : class, new();
        protected abstract void DropTable(string tableName);
        protected abstract void BeginTransaction();
        protected abstract void CommitTransaction();
        protected abstract void RollbackTransaction();
        public abstract List<TModel> Select<TModel>(string query, Func<TReader, List<TModel>> serializeAction) where TModel : class, new();

        public abstract List<TModel> Select<TModel>(string sql, List<TParameter> parameters, Func<TReader, List<TModel>> serializeAction) where TModel : class, new();

        public abstract List<TModel> Select<TModel>(string query, List<TParameter> parameters) where TModel : class, new();

        public abstract List<TModel> Select<TModel>(TModel parameters) where TModel : class, new();

        public abstract List<TModel> Select<TModel>() where TModel : class, new();

        public abstract TElement SelectFirst<TElement>(string sql, List<TParameter> parameters, Func<TReader, TElement> serializeAction);
        
        public abstract TransactionInfo Insert(string sql, List<TParameter> parameters);

        public abstract TransactionInfo Update(string sql, List<TParameter> parameters);

        public abstract TransactionInfo Delete(string sql, List<TParameter> parameters);

        public abstract TransactionInfo InsertAll(string sql, List<List<TParameter>> parameters);

        public abstract TransactionInfo InsertAllWOTransaction(string sql, List<List<TParameter>> parameters);

        protected abstract void LaunchSQL(string sql);

        protected abstract TransactionInfo DoTransaction(string sql, List<TParameter> parameters);

        protected abstract TCommand PrepareCommand(string sql);

        protected abstract TCommand PrepareCommand(string sql, List<TParameter> parameters);
        
            
        








     }
}
