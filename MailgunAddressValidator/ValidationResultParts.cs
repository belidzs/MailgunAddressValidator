// <copyright file="ValidationResultParts.cs" company="Balázs Keresztury">
// Copyright (c) Balázs Keresztury. All rights reserved.
// </copyright>

namespace MailgunAddressValidator
{
    /// <summary>
    /// Contains parsed segments of the provided email address.
    /// </summary>
    public class ValidationResultParts
    {
        /// <summary>
        /// Display name for the address.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The local part of the e-mail address.
        /// </summary>
        public string LocalPart { get; set; }

        /// <summary>
        /// The domain part of the e-mail address.
        /// </summary>
        public string Domain { get; set; }
    }
}
