using Dapper;
using MySqlConnector;
using SharedKernel.Core;
using SharedKernel.Log;
using System.Data;
using static Dapper.SqlMapper;

namespace SharedKernel.MySQL
{
    public class DbConnection : IDbConnection
    {
        #region Declare + Constructor
        private readonly MySqlConnection _connection;
        private readonly string _dbConfig;
        private MySqlTransaction _transaction;
        private ConnectionState _currentState = ConnectionState.Connecting;
        private int _numberOfConnection = 0;

        public DbConnection(string dbConfig = "MasterDb")
        {
            _connection = new MySqlConnection(CoreSettings.ConnectionStrings[dbConfig]);
            _dbConfig = dbConfig;

            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
                _currentState = ConnectionState.Open;
                _numberOfConnection = 1;
            }

        }
        #endregion

        #region Implementation
        public MySqlConnection Connection => _connection;

        public MySqlTransaction CurrentTransaction
        {
            get
            {
                if (_transaction == null || _transaction.Connection == null)
                {
                    _transaction = _connection.BeginTransaction();
                }
                return _transaction;
            }
        }
        

        #region Query
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = CommandType.Text)
        {
            try
            {
                if (_currentState == ConnectionState.Fetching || _currentState == ConnectionState.Executing)
                {
                    using (var newConnection = new MySqlConnection(CoreSettings.ConnectionStrings[_dbConfig]))
                    {
                        try
                        {
                            _numberOfConnection++;
                            return await newConnection.QueryAsync<T>(sql, param, commandTimeout: commandTimeout, commandType: commandType);
                        }
                        finally
                        {
                            _numberOfConnection--;
                        }
                    }
                }

                _currentState = ConnectionState.Fetching;
                return await _connection.QueryAsync<T>(sql, param, CurrentTransaction, commandTimeout: commandTimeout, commandType: commandType);
            }
            finally
            {
                if (_numberOfConnection <= 1)
                {
                    _currentState = ConnectionState.Open;
                }
            }
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = CommandType.Text)
        {
            try
            {
                if (_currentState == ConnectionState.Fetching || _currentState == ConnectionState.Executing)
                {
                    using (var newConnection = new MySqlConnection(CoreSettings.ConnectionStrings[_dbConfig]))
                    {
                        try
                        {
                            _numberOfConnection++;
                            return await newConnection.QueryFirstOrDefaultAsync<T>(sql, param, commandTimeout: commandTimeout, commandType: commandType);
                        }
                        finally
                        {
                            _numberOfConnection--;
                        }
                    }
                }

                _currentState = ConnectionState.Fetching;
                return await _connection.QueryFirstOrDefaultAsync<T>(sql, param, CurrentTransaction, commandTimeout: commandTimeout, commandType: commandType);
            }
            finally
            {
                if (_numberOfConnection <= 1)
                {
                    _currentState = ConnectionState.Open;
                }
            }
        }

        public async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = CommandType.Text)
        {
            try
            {
                if (_currentState == ConnectionState.Fetching || _currentState == ConnectionState.Executing)
                {
                    using (var newConnection = new MySqlConnection(CoreSettings.ConnectionStrings[_dbConfig]))
                    {
                        try
                        {
                            _numberOfConnection++;
                            return await newConnection.QuerySingleOrDefaultAsync<T>(sql, param, commandTimeout: commandTimeout, commandType: commandType);
                        }
                        finally
                        {
                            _numberOfConnection--;
                        }
                    }
                }

                _currentState = ConnectionState.Fetching;
                return await _connection.QuerySingleOrDefaultAsync<T>(sql, param, CurrentTransaction, commandTimeout: commandTimeout, commandType: commandType);
            }
            finally
            {
                if (_numberOfConnection <= 1)
                {
                    _currentState = ConnectionState.Open;
                }
            }
        }

        public async Task<GridReader> QueryMultipleAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = CommandType.Text)
        {
            try
            {
                if (_currentState == ConnectionState.Fetching || _currentState == ConnectionState.Executing)
                {
                    using (var newConnection = new MySqlConnection(CoreSettings.ConnectionStrings[_dbConfig]))
                    {
                        try
                        {
                            _numberOfConnection++;
                            return await newConnection.QueryMultipleAsync(sql, param, CurrentTransaction, commandTimeout: commandTimeout, commandType: commandType);
                        }
                        finally
                        {
                            _numberOfConnection--;
                        }
                    }
                }

                _currentState = ConnectionState.Fetching;
                return await _connection.QueryMultipleAsync(sql, param, CurrentTransaction, commandTimeout: commandTimeout, commandType: commandType);
            }
            finally
            {
                if (_numberOfConnection <= 1)
                {
                    _currentState = ConnectionState.Open;
                }
            }
        }
        #endregion

        #region Command
        public async Task<int> ExecuteAsync(string sql, object param, int? commandTimeout = null, CommandType? commandType = CommandType.Text, bool autoCommit = false)
        {
            try
            {
                _currentState = ConnectionState.Executing;

                var result = await _connection.ExecuteAsync(sql, param, CurrentTransaction, commandTimeout, commandType);
                if (autoCommit)
                {
                    await CommitAsync();
                }

                return result;
            }
            finally
            {
                if (_numberOfConnection <= 1)
                {
                    _currentState = ConnectionState.Open;
                }
            }
        }

        public async Task<object> ExecuteScalarAsync(string sql, object param, int? commandTimeout = null, CommandType? commandType = CommandType.Text, bool autoCommit = false)
        {
            try
            {
                _currentState = ConnectionState.Executing;

                var result = await _connection.ExecuteScalarAsync(sql, param, CurrentTransaction, commandTimeout, commandType);
                if (autoCommit)
                {
                    await CommitAsync();
                }

                return result;
            }
            finally
            {
                if (_numberOfConnection <= 1)
                {
                    _currentState = ConnectionState.Open;
                }
            }
        }

        public async Task<IEnumerable<T>> ExecuteAndGetResultAsync<T>(string sql, object param, int? commandTimeout = null, CommandType? commandType = CommandType.Text, bool autoCommit = false)
        {
            try
            {
                _currentState = ConnectionState.Executing;
               
                return await _connection.QueryAsync<T>(sql, param, CurrentTransaction, commandTimeout: commandTimeout, commandType: commandType);
            }
            finally
            {
                if (_numberOfConnection <= 1)
                {
                    _currentState = ConnectionState.Open;
                }
            }
        }

        public async ValueTask<MySqlBulkCopyResult> WriteToServerAsync<T>(DataTable dataTable, IList<T> entites, CancellationToken cancellationToken, bool autoCommit = false)
        {
            try
            {
                _currentState = ConnectionState.Executing;

                // Create object of MySqlBulkCopy which help to insert  
                var bulk = new MySqlBulkCopy(_connection, CurrentTransaction);

                // Assign Destination table name  
                bulk.DestinationTableName = dataTable.TableName;

                // Mapping column
                int index = 0;
                foreach (DataColumn col in dataTable.Columns)
                {
                    bulk.ColumnMappings.Add(new MySqlBulkCopyColumnMapping(index++, col.ColumnName));
                }

                await _connection.ExecuteAsync("SET GLOBAL local_infile=1", null, CurrentTransaction);

                // Insert bulk Records into DataBase.
                _connection.InfoMessage += (s, e) =>
                {
                    Logging.Error(string.Join(" ----> ", e.Errors.Select(e => e.Message)));
                };

                var result = await bulk.WriteToServerAsync(dataTable, cancellationToken);
                if (autoCommit)
                {
                    await CommitAsync();
                }

                return result;
            }
            finally
            {
                if (_numberOfConnection <= 1)
                {
                    _currentState = ConnectionState.Open;
                }
            }
        }
        #endregion

        #region UnitOfWork
        
        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            await CurrentTransaction.RollbackAsync(cancellationToken);
        }
        
        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await CurrentTransaction.CommitAsync(cancellationToken);
        }
        
        public void BeginTransaction()
        {
           
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            //GC.SuppressFinalize(this);
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
        #endregion

        #endregion
    }
}
