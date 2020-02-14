// <copyright file="ValidationResult.cs" company="Balázs Keresztury">
// Copyright (c) Balázs Keresztury. All rights reserved.
// </copyright>

namespace MailgunAddressValidator
{
    /// <summary>
    /// Contains the result of a validation.
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Runs the email segments across a valid known provider rule list. If a violation occurs this value is false.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Email address being validated.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Parsed segments of the provided email address.
        /// </summary>
        public ValidationResultParts Parts { get; set; }

        /// <summary>
        /// Null if nothing, however if a potential typo is made, the closest suggestion is provided.
        /// </summary>
        public string DidYouMean { get; set; }
    }
}
