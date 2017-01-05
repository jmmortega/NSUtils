using Mono.Data.Sqlite;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using NSUtils.Database.Utils;
using NSUtils.Database.SQLite.Droid.Utils;

namespace NSUtils.Database.SQLite.Droid
{
    public class BaseDatabase : CommonDatabase<SqliteCommand, SqliteParameter, SqliteDataReader>
    {
        [DllImport("libsqlite.so")]
        internal static extern int sqlite3_shutdown();

        [DllImport("libsqlite.so")]
        internal static extern int sqlite3_initialize();

        private void ApplySerialized()
        {
            sqlite3_shutdown();
            SqliteConnection.SetConfig(SQLiteConfig.Serialized);
            sqlite3_initialize();
        }
        
        private SqliteConnection _conn;
        private SqliteTransaction _transaction;

        private bool _connected;
        public override bool IsConnected
        {
            get
            {
                return _connected;
            }
            protected set
            {
                _connected = value;
            }
        }

        protected override void Connect(string connectionString)
        {
            Utils = new QueryUtilsSQLite();
            try
            {
                if(!File.Exists(connectionString))
                {
                    SqliteConnection.CreateFile(connectionString);
                }

                ApplySerialized();
                _conn = new SqliteConnection(string.Format("Data Source={0};Version=3;PRAGMA journal_mode=WAL;PRAGMA cache_size=1;PRAGMA synchronous=1;PRAGMA locking_mode=EXCLUSIVE", connectionString));
            }
            catch(Exception e)
            {
                LogError(e);
            }
        }

        protected override void Close()
        {
            if(_conn != null && _conn.State == System.Data.ConnectionState.Open)
            {
                if(_transaction != null)
                {
                    _transaction.Dispose();
                }

                _conn.Close();
                _conn.Dispose();
                GC.Collect();
                _conn = null;
            }
        }

        protected override void BeginTransaction()
        {
            _transaction = _conn.BeginTransaction();
        }

        protected override void CommitTransaction()
        {
            if(_transaction != null)
            {
                _transaction.Commit();
            }
        }

        protected override void RollbackTransaction()
        {
            if(_transaction != null)
            {
                _transaction.Rollback();
            }
        }

        protected override void CreateTable(string query)
        {
            LaunchSQL(query);
        }

        protected override void CreateTable<TModel>()
        {
            CreateTable(Utils.CreateTableQuery(typeof(TModel)));
        }

        protected override void DropTable<TModel>()
        {
            DropTable(typeof(TModel).Name);
        }

        protected override void DropTable(string tableName)
        {
            LaunchSQL(string.Format("DROP TABLE IF EXISTS {0}", tableName));
        }

        public override List<TModel> Select<TModel>(string query, Func<SqliteDataReader, List<TModel>> serializeAction)
        {
            var com = PrepareCommand(query);
            var reader = com.ExecuteReader();
            com.Dispose();

            var list = serializeAction.Invoke(reader);
            reader.Close();
            reader = null;

            return list;
        }

        public override List<TModel> Select<TModel>(string sql, List<SqliteParameter> parameters, Func<SqliteDataReader, List<TModel>> serializeAction)
        {
            var com = PrepareCommand(sql, parameters);
            var reader = com.ExecuteReader();
            com.Dispose();

            var list = serializeAction.Invoke(reader);
            reader.Close();
            reader = null;

            return list;
        }

        public override List<TModel> Select<TModel>(string query, List<SqliteParameter> parameters)
        {
            var com = PrepareCommand(query, parameters);
            var reader = com.ExecuteReader();
            com.Dispose();

            var commonDataReader = Utils.ConvertToCommonDataReader<TModel>(reader);

            reader.Close();
            reader = null;

            return commonDataReader.Rows;
        }

        public override List<TModel> Select<TModel>(TModel parameters)
        {
            var com = PrepareCommand(Utils.CreateSelectQuery<TModel>(parameters), Utils.CreateParameters(parameters));
            var reader = com.ExecuteReader();
            com.Dispose();

            var commonDataReader = Utils.ConvertToCommonDataReader<TModel>(reader);

            reader.Close();
            reader = null;

            return commonDataReader.Rows;
        }

        public override List<TModel> Select<TModel>()
        {
            var com = PrepareCommand(Utils.CreateSelectQuery<TModel>());
            var reader = com.ExecuteReader();
            com.Dispose();

            var commonDataReader = Utils.ConvertToCommonDataReader<TModel>(reader);

            reader.Close();
            reader = null;

            return commonDataReader.Rows;
        }

        public override TElement SelectFirst<TElement>(string sql, List<SqliteParameter> parameters, Func<SqliteDataReader, TElement> serializeAction)
        {
            var com = PrepareCommand(sql, parameters);
            var reader = com.ExecuteReader();
            com.Dispose();

            var list = serializeAction.Invoke(reader);
            reader.Close();
            reader = null;
            return list;
        }
                
        public override TransactionInfo Insert(string sql, List<SqliteParameter> parameters)
        {
            LogInfo(string.Format("Execute insert {0} , with parameters {1}", sql, parameters));
            return DoTransaction(sql, parameters);
        }

        public override TransactionInfo Update(string sql, List<SqliteParameter> parameters)
        {
            LogInfo(string.Format("Execute update {0} , with parameters {1}", sql, parameters));
            return DoTransaction(sql, parameters);
        }

        public override TransactionInfo Delete(string sql, List<SqliteParameter> parameters)
        {
            LogInfo(string.Format("Execute delete {0} , with parameters {1}", sql, parameters));
            return DoTransaction(sql, parameters);
        }

        public override TransactionInfo InsertAll(string sql, List<List<SqliteParameter>> parameters)
        {
            int rowAffected = 0;
            var transactionInfo = new TransactionInfo();
            var command = _conn.CreateCommand();
            BeginTransaction();
            command.CommandText = sql;

            try
            {
                foreach(var queryParameter in parameters)
                {
                    command.Parameters.Clear();
                    command.Parameters.AddRange(queryParameter.ToArray());
                    command.ExecuteNonQuery();
                    rowAffected++;
                }
                CommitTransaction();
            }
            catch(Exception e)
            {
                LogInfo(string.Format("Error in transaction {0}", e.Message));
                RollbackTransaction();
                transactionInfo = new TransactionInfo(e);
            }
            finally
            {
                command.Dispose();
            }
            transactionInfo = new TransactionInfo(rowAffected);

            return transactionInfo;            
        }

        public override TransactionInfo InsertAllWOTransaction(string sql, List<List<SqliteParameter>> parameters)
        {
            var transactionInfo = new TransactionInfo();
            int rowAffected = 0;
            var command = _conn.CreateCommand();
            command.CommandText = sql;

            try
            {
                foreach(var queryParameter in parameters)
                {
                    command.Parameters.Clear();
                    command.Parameters.AddRange(queryParameter.ToArray());
                    command.ExecuteNonQuery();
                }
                transactionInfo = new TransactionInfo(rowAffected);
            }
            catch(Exception e)
            {
                LogInfo(string.Format("Error in transaction {0}", e.Message));                
                transactionInfo = new TransactionInfo(e);
            }
            finally
            {
                command.Dispose();
            }

            return transactionInfo;
        }


        protected override void LaunchSQL(string sql)
        {
            DoTransaction(sql, new List<SqliteParameter>());
        }

        protected override TransactionInfo DoTransaction(string sql, List<SqliteParameter> parameters)
        {
            TransactionInfo transactionInfo = new TransactionInfo();
            var command = PrepareCommand(sql, parameters);

            try
            {
                BeginTransaction();
                transactionInfo = new TransactionInfo(command.ExecuteNonQuery());
                CommitTransaction();
            }
            catch (Exception e)
            {
                LogError(e);
                transactionInfo = new TransactionInfo(e);
                RollbackTransaction();
            }
            finally
            {
                command.Dispose();
            }

            return transactionInfo;
        }

        protected override SqliteCommand PrepareCommand(string sql)
        {
            return PrepareCommand(sql, new List<SqliteParameter>());
        }

        protected override SqliteCommand PrepareCommand(string sql, List<SqliteParameter> parameters)
        {
            var com = _conn.CreateCommand();
            com.CommandText = sql;
            com.Parameters.AddRange(parameters.Select(x => (SqliteParameter)x).ToArray());

            return com;
        }

        
    }

}
