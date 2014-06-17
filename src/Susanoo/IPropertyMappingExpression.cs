﻿using System;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace Susanoo
{
    public interface IPropertyMappingConfiguration<TRecord>
        where TRecord : IDataRecord
    {
        /// <summary>
        /// Gets the property metadata.
        /// </summary>
        /// <value>The property metadata.</value>
        PropertyInfo PropertyMetadata { get; }

        /// <summary>
        /// Gets or sets the name of the return column.
        /// </summary>
        /// <value>The name of the return.</value>
        string ReturnName { get; }

        /// <summary>
        /// Maps the property conditionally.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>IPropertyMappingConfiguration&lt;TRecord&gt;.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        IPropertyMappingConfiguration<TRecord> MapIf(Expression<Func<TRecord, string, bool>> condition);

        /// <summary>
        /// Uses the specified alias when mapping from the data call.
        /// </summary>
        /// <param name="alias">The alias.</param>
        /// <returns>Susanoo.ICommandResultMappingExpression&lt;TFilter,TResult&gt;.</returns>
        IPropertyMappingConfiguration<TRecord> AliasProperty(string propertyAlias);

        /// <summary>
        /// Processes the value in some form before assignment.
        /// </summary>
        /// <param name="process"></param>
        /// <returns>IPropertyMappingConfiguration&lt;TRecord&gt;.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        IPropertyMappingConfiguration<TRecord> ProcessValue(Expression<Func<Type, object, object, object>> process);

        /// <summary>
        /// Assembles the mapping expression.
        /// </summary>
        /// <returns>Expression&lt;Action&lt;IDataRecord&gt;&gt;.</returns>
        Expression<Action<IDataRecord>> AssembleMappingExpression(MemberExpression property);
    }
}