// <copyright file="UnauthorizedException.cs" company="Balázs Keresztury">
// Copyright (c) Balázs Keresztury. All rights reserved.
// </copyright>

using System;

namespace MailgunAddressValidator
{
    /// <summary>
    /// It is thrown when the client is unable to authorize with the Mailgun API.
    /// </summary>
    public class UnauthorizedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedException"/> class.
        /// </summary>
        public UnauthorizedException()
            : base("Bad API key. You should use your public key from My Account page")
        {
        }
    }
}
