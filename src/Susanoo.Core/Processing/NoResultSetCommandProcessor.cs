﻿#region

using System;
using System.Data.Common;
using System.Numerics;
#if !NETFX40
using System.Threading;
using System.Threading.Tasks;
#endif
using Susanoo.Command;
using Susanoo.Exceptions;

#endregion

namespace Susanoo.Processing
{
    /// <summary>
    ///     A fully built and ready to be executed CommandBuilder expression with a filter parameter.
    /// </summary>
    /// <typeparam name="TFilter">The type of the filter.</typeparam>
    public partial class NoResultSetCommandProcessor<TFilter> :
        ICommandProcessor<TFilter>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="NoResultSetCommandProcessor{TFilter}" /> class.
        /// </summary>
        /// <param name="command">The CommandBuilder.</param>
        public NoResultSetCommandProcessor(ICommandBuilderInfo<TFilter> command)
        {
            CommandBuilderInfo = command;
        }

        /// <summary>
        ///     Gets the CommandBuilder expression.
        /// </summary>
        /// <value>The CommandBuilder expression.</value>
        public ICommandBuilderInfo<TFilter> CommandBuilderInfo { get; }

        /// <summary>
        /// Gets or sets the timeout of a command execution.
        /// </summary>
        /// <value>The timeout.</value>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);

        /// <summary>
        ///     Gets the hash code used for caching result mapping compilations.
        /// </summary>
        /// <value>The cache hash.</value>
        public BigInteger CacheHash =>
            CommandBuilderInfo.CacheHash;

        /// <summary>
        ///     Executes the non query.
        /// </summary>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="executableCommandInfo">The executable command information.</param>
        /// <returns>System.Int32.</returns>
        public int ExecuteNonQuery(IDatabaseManager databaseManager, IExecutableCommandInfo executableCommandInfo)
        {
            var result = 0;

            try
            {
                result = databaseManager.ExecuteNonQuery(
                    executableCommandInfo.CommandText,
                    executableCommandInfo.DbCommandType,
                    Timeout,
                    executableCommandInfo.Parameters);
            }
            catch (Exception ex)
            {
                throw new SusanooExecutionException(ex, executableCommandInfo, Timeout, executableCommandInfo.Parameters);
            }

            return result;
        }

        /// <summary>
        ///     Executes the scalar.
        /// </summary>
        /// <typeparam name="TReturn">The type of the return.</typeparam>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="executableCommandInfo">The executable command information.</param>
        /// <returns>TReturn.</returns>
        public TReturn ExecuteScalar<TReturn>(IDatabaseManager databaseManager,
            IExecutableCommandInfo executableCommandInfo)
        {
            var result = default(TReturn);

            try
            {
                result = databaseManager.ExecuteScalar<TReturn>(
                    executableCommandInfo.CommandText,
                    executableCommandInfo.DbCommandType,
                    Timeout,
                    executableCommandInfo.Parameters);
            }
            catch (Exception ex)
            {
                throw new SusanooExecutionException(ex, executableCommandInfo, Timeout, executableCommandInfo.Parameters);
            }

            return result;
        }

        /// <summary>
        ///     Allows a hook in an instance of a processor
        /// </summary>
        /// <param name="interceptOrProxy">The intercept or proxy.</param>
        /// <returns>ICommandProcessor&lt;TFilter&gt;.</returns>
        public ICommandProcessor<TFilter> InterceptOrProxyWith(
            Func<ICommandProcessor<TFilter>, ICommandProcessor<TFilter>> interceptOrProxy)
        {
            return interceptOrProxy(this);
        }

        /// <summary>
        ///     Executes the scalar.
        /// </summary>
        /// <typeparam name="TReturn">The type of the return.</typeparam>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="parameterObject">The parameter object.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>TReturn.</returns>
        public TReturn ExecuteScalar<TReturn>(IDatabaseManager databaseManager, TFilter filter,
            object parameterObject, params DbParameter[] explicitParameters)
        {
            var executableCommandInfo = new ExecutableCommandInfo
            {
                CommandText = CommandBuilderInfo.CommandText,
                DbCommandType = CommandBuilderInfo.DbCommandType,
                Parameters = CommandBuilderInfo
                    .BuildParameters(databaseManager, filter, parameterObject, explicitParameters)
            };

            return ExecuteScalar<TReturn>(databaseManager, executableCommandInfo);
        }

        /// <summary>
        ///     Executes the scalar.
        /// </summary>
        /// <typeparam name="TReturn">The type of the return.</typeparam>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>TReturn.</returns>
        public TReturn ExecuteScalar<TReturn>(IDatabaseManager databaseManager, TFilter filter,
            params DbParameter[] explicitParameters)
        {
            return ExecuteScalar<TReturn>(databaseManager, filter, null, explicitParameters);
        }

        /// <summary>
        ///     Executes the scalar.
        /// </summary>
        /// <typeparam name="TReturn">The type of the return.</typeparam>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>TReturn.</returns>
        public TReturn ExecuteScalar<TReturn>(IDatabaseManager databaseManager, params DbParameter[] explicitParameters)
        {
            return ExecuteScalar<TReturn>(databaseManager, default(TFilter), explicitParameters);
        }

        /// <summary>
        ///     Executes the non query.
        /// </summary>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="parameterObject">The parameter object.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>System.Int32.</returns>
        public int ExecuteNonQuery(IDatabaseManager databaseManager, TFilter filter, object parameterObject,
            params DbParameter[] explicitParameters)
        {
            var executableCommandInfo = new ExecutableCommandInfo
            {
                CommandText = CommandBuilderInfo.CommandText,
                DbCommandType = CommandBuilderInfo.DbCommandType,
                Parameters = CommandBuilderInfo
                    .BuildParameters(databaseManager, filter, parameterObject, explicitParameters)
            };

            return ExecuteNonQuery(databaseManager, executableCommandInfo);
        }

        /// <summary>
        ///     Executes the non query.
        /// </summary>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>System.Int32.</returns>
        public int ExecuteNonQuery(IDatabaseManager databaseManager, TFilter filter,
            params DbParameter[] explicitParameters)
        {
            return ExecuteNonQuery(databaseManager, filter, null, explicitParameters);
        }

        /// <summary>
        ///     Executes the non query.
        /// </summary>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>System.Int32.</returns>
        public int ExecuteNonQuery(IDatabaseManager databaseManager, params DbParameter[] explicitParameters)
        {
            return ExecuteNonQuery(databaseManager, default(TFilter), explicitParameters);
        }
    }

#if !NETFX40

    /// <summary>
    ///     A fully built and ready to be executed CommandBuilder expression with a filter parameter.
    /// </summary>
    public partial class NoResultSetCommandProcessor<TFilter>
    {
        /// <summary>
        ///     Executes the non query asynchronously.
        /// </summary>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="executableCommandInfo">The executable command information.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>System.Threading.Tasks.Task&lt;System.Int32&gt;.</returns>
        public async Task<int> ExecuteNonQueryAsync(IDatabaseManager databaseManager,
            IExecutableCommandInfo executableCommandInfo, CancellationToken cancellationToken)
        {
            var result = 0;

            try
            {
                result = await databaseManager.ExecuteNonQueryAsync(
                    executableCommandInfo.CommandText,
                    executableCommandInfo.DbCommandType,
                    Timeout,
                    cancellationToken,
                    executableCommandInfo.Parameters).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new SusanooExecutionException(ex, executableCommandInfo, Timeout, executableCommandInfo.Parameters);
            }

            return result;
        }

        /// <summary>
        ///     Executes the scalar asynchronously.
        /// </summary>
        /// <typeparam name="TReturn">The type of the return.</typeparam>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="executableCommandInfo">The executable command information.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>System.Threading.Tasks.Task&lt;TReturn&gt;.</returns>
        public async Task<TReturn> ExecuteScalarAsync<TReturn>(IDatabaseManager databaseManager,
            IExecutableCommandInfo executableCommandInfo, CancellationToken cancellationToken)
        {
            var result = default(TReturn);

            try
            {
                result = await databaseManager.ExecuteScalarAsync<TReturn>(
                    executableCommandInfo.CommandText,
                    executableCommandInfo.DbCommandType,
                    Timeout,
                    cancellationToken,
                    executableCommandInfo.Parameters).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new SusanooExecutionException(ex, executableCommandInfo, Timeout, executableCommandInfo.Parameters);
            }

            return result;
        }

        /// <summary>
        ///     Execute scalar as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TReturn">The type of the return.</typeparam>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="parameterObject">The parameter object.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>Task&lt;TReturn&gt;.</returns>
        public async Task<TReturn> ExecuteScalarAsync<TReturn>(IDatabaseManager databaseManager,
            TFilter filter, object parameterObject, CancellationToken cancellationToken,
            params DbParameter[] explicitParameters)
        {
            var executableCommandInfo = new ExecutableCommandInfo
            {
                CommandText = CommandBuilderInfo.CommandText,
                DbCommandType = CommandBuilderInfo.DbCommandType,
                Parameters = CommandBuilderInfo
                    .BuildParameters(databaseManager, filter, parameterObject, explicitParameters)
            };

            return await ExecuteScalarAsync<TReturn>(databaseManager, executableCommandInfo, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        ///     Executes the non query asynchronous.
        /// </summary>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="parameterObject">The parameter object.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>System.Threading.Tasks.Task&lt;System.Int32&gt;.</returns>
        public async Task<int> ExecuteNonQueryAsync(IDatabaseManager databaseManager, TFilter filter,
            object parameterObject,
            CancellationToken cancellationToken, params DbParameter[] explicitParameters)
        {
            var executableCommandInfo = new ExecutableCommandInfo
            {
                CommandText = CommandBuilderInfo.CommandText,
                DbCommandType = CommandBuilderInfo.DbCommandType,
                Parameters = CommandBuilderInfo
                    .BuildParameters(databaseManager, filter, parameterObject, explicitParameters)
            };

            return await ExecuteNonQueryAsync(databaseManager, executableCommandInfo, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        ///     Execute scalar as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TReturn">The type of the return.</typeparam>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>Task&lt;TReturn&gt;.</returns>
        public async Task<TReturn> ExecuteScalarAsync<TReturn>(IDatabaseManager databaseManager,
            TFilter filter, CancellationToken cancellationToken, params DbParameter[] explicitParameters)
        {
            return
                await ExecuteScalarAsync<TReturn>(databaseManager, filter, null, cancellationToken, explicitParameters)
                    .ConfigureAwait(false);
        }

        /// <summary>
        ///     Execute scalar as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TReturn">The type of the return.</typeparam>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>Task&lt;TReturn&gt;.</returns>
        public async Task<TReturn> ExecuteScalarAsync<TReturn>(IDatabaseManager databaseManager,
            CancellationToken cancellationToken, params DbParameter[] explicitParameters)
        {
            return
                await
                    ExecuteScalarAsync<TReturn>(databaseManager, default(TFilter), null, CancellationToken.None,
                        explicitParameters)
                        .ConfigureAwait(false);
        }

        /// <summary>
        ///     Execute scalar as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TReturn">The type of the return.</typeparam>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>Task&lt;TReturn&gt;.</returns>
        public async Task<TReturn> ExecuteScalarAsync<TReturn>(IDatabaseManager databaseManager,
            TFilter filter, params DbParameter[] explicitParameters)
        {
            return
                await
                    ExecuteScalarAsync<TReturn>(databaseManager, filter, null, CancellationToken.None,
                        explicitParameters)
                        .ConfigureAwait(false);
        }

        /// <summary>
        ///     Execute scalar as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TReturn">The type of the return.</typeparam>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>Task&lt;TReturn&gt;.</returns>
        public async Task<TReturn> ExecuteScalarAsync<TReturn>(IDatabaseManager databaseManager,
            params DbParameter[] explicitParameters)
        {
            return
                await
                    ExecuteScalarAsync<TReturn>(databaseManager, default(TFilter), null, CancellationToken.None,
                        explicitParameters)
                        .ConfigureAwait(false);
        }

        /// <summary>
        ///     Execute scalar as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TReturn">The type of the return.</typeparam>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="parameterObject">The parameter object.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>Task&lt;TReturn&gt;.</returns>
        public async Task<TReturn> ExecuteScalarAsync<TReturn>(IDatabaseManager databaseManager,
            TFilter filter, object parameterObject, params DbParameter[] explicitParameters)
        {
            return
                await
                    ExecuteScalarAsync<TReturn>(databaseManager, filter, parameterObject, CancellationToken.None,
                        explicitParameters)
                        .ConfigureAwait(false);
        }


        /// <summary>
        ///     Executes the non query async.
        /// </summary>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        public async Task<int> ExecuteNonQueryAsync(IDatabaseManager databaseManager, TFilter filter,
            CancellationToken cancellationToken, params DbParameter[] explicitParameters)
        {
            return
                await ExecuteNonQueryAsync(databaseManager, filter, null, cancellationToken, explicitParameters)
                    .ConfigureAwait(false);
        }

        /// <summary>
        ///     Executes the non query async.
        /// </summary>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        public async Task<int> ExecuteNonQueryAsync(IDatabaseManager databaseManager, TFilter filter,
            params DbParameter[] explicitParameters)
        {
            return await ExecuteNonQueryAsync(databaseManager, filter, default(CancellationToken), explicitParameters)
                .ConfigureAwait(false);
        }

        /// <summary>
        ///     Executes the non query async.
        /// </summary>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        public async Task<int> ExecuteNonQueryAsync(IDatabaseManager databaseManager,
            CancellationToken cancellationToken, params DbParameter[] explicitParameters)
        {
            return
                await ExecuteNonQueryAsync(databaseManager, default(TFilter), cancellationToken, explicitParameters)
                    .ConfigureAwait(false);
        }

        /// <summary>
        ///     Executes the non query async.
        /// </summary>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        public async Task<int> ExecuteNonQueryAsync(IDatabaseManager databaseManager,
            params DbParameter[] explicitParameters)
        {
            return
                await
                    ExecuteNonQueryAsync(databaseManager, default(TFilter), default(CancellationToken),
                        explicitParameters)
                        .ConfigureAwait(false);
        }
    }

#endif
}