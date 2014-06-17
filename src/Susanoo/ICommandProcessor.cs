﻿using System.Collections.Generic;
using System.Data;

namespace Susanoo
{
    /// <summary>
    /// Represents a fully built and ready to be executed command expression with a filter parameter.
    /// </summary>
    /// <typeparam name="TFilter">The type of the filter.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <remarks>Appropriate mapping expressions are compiled by the point this interface becomes available.</remarks>
    public interface ICommandProcessor<TFilter, TResult>
        where TResult : new()
    {
        /// <summary>
        /// Assembles a data command for an ADO.NET provider, executes the command and uses pre-compiled mappings to assign the resultant data to the result object type.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>IEnumerable&lt;TResult&gt;.</returns>
        IEnumerable<TResult> Execute(TFilter filter, params IDbDataParameter[] explicitParameters);
    }

    /// <summary>
    /// Represents a fully built and ready to be executed command expression with appropriate mapping expressions compiled.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <remarks>Appropriate mapping expressions are compiled by the point this interface becomes available.</remarks>
    public interface ICommandProcessor<TResult>
        where TResult : new()
    {
        /// <summary>
        /// Assembles a data command for an ADO.NET provider, executes the command and uses pre-compiled mappings to assign the resultant data to the result object type.
        /// </summary>
        /// <param name="explicitParameters">The explicit parameters.</param>
        /// <returns>IEnumerable&lt;TResult&gt;.</returns>
        IEnumerable<TResult> Execute(params IDbDataParameter[] explicitParameters);
    }
}