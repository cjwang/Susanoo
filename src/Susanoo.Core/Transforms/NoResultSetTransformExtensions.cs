﻿using System;
using System.Collections.Generic;
using Susanoo.Processing;
using Susanoo.Transforms;

namespace Susanoo
{
    /// <summary>
    /// Provides extensions to the pipeline at the no result set command processor just prior to execution.
    /// </summary>
    public static class NoResultSetTransformExtensions
    {
        /// <summary>
        /// Applies a set of transforms to future executions.
        /// </summary>
        /// <typeparam name="TFilter">The type of the filter.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="transforms">The transforms.</param>
        /// <returns>ICommandProcessor&lt;TFilter, TResult&gt;.</returns>
        /// <exception cref="System.ArgumentNullException">At least one transform is required.</exception>
        public static ICommandProcessor<TFilter> ApplyTransforms<TFilter>(
            this ICommandProcessor<TFilter> source, Func<ICommandProcessor<TFilter>, IEnumerable<CommandTransform>> transforms)
        {
            if (transforms == null)
                throw new ArgumentNullException(nameof(transforms), "At least one transform is required.");

            return source.InterceptOrProxyWith(s => new NoResultSetTransformProxy<TFilter>(s, transforms(s)));
        }
    }
}
