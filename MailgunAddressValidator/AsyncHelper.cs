// <copyright file="AsyncHelper.cs" company="Balázs Keresztury">
// Copyright (c) Balázs Keresztury. All rights reserved.
// </copyright>
// Source: https://cpratt.co/async-tips-tricks/

using System;
using System.Threading;
using System.Threading.Tasks;

namespace MailgunAddressValidator
{
    /// <summary>
    /// Helper class to wrap an async delegate into a syncronous method.
    /// </summary>
    public static class AsyncHelper
    {
        private static readonly TaskFactory _taskFactory = new
            TaskFactory(
                CancellationToken.None,
                TaskCreationOptions.None,
                TaskContinuationOptions.None,
                TaskScheduler.Default);

        /// <summary>
        /// Wraps a generic async delegate into a syncronous method.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The asyncronous delegate.</param>
        /// <returns>The result.</returns>
        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
            => _taskFactory
                .StartNew(func)
                .Unwrap()
                .GetAwaiter()
                .GetResult();

        /// <summary>
        /// Wraps an async delegate into a syncronous method.
        /// </summary>
        /// <param name="func">The asyncronous delegate.</param>
        public static void RunSync(Func<Task> func)
            => _taskFactory
                .StartNew(func)
                .Unwrap()
                .GetAwaiter()
                .GetResult();
    }
}