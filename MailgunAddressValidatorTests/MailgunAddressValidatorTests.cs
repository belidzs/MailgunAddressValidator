// <copyright file="MailgunAddressValidatorTests.cs" company="Balázs Keresztury">
// Copyright (c) Balázs Keresztury. All rights reserved.
// </copyright>

using System;
using System.Configuration;
using System.Threading.Tasks;
using MailgunAddressValidator;
using NUnit.Framework;

namespace MailgunAddressValidatorTests
{
    /// <summary>
    /// Contains tests for the MailgunAddressValidator project.
    /// </summary>
    [TestFixture]
    public class MailgunAddressValidatorTests
    {
        private readonly string _apikey = ConfigurationManager.AppSettings["PublicApiKey"];
        private readonly string _validAddress = "sales@mailgun.com";
        private readonly string _invalidAddress = "test@nonexistentdomain.com";

        /// <summary>
        /// Tests if a valid email address returns valid response.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Parallelizable]
        [Test]
        public async Task ValidateValidAddressAsync()
        {
            ValidationResult result = await Validator.ValidateAsync(_validAddress, _apikey).ConfigureAwait(false);
            Assert.That(result.IsValid, Is.True);
        }

        /// <summary>
        /// Tests if a valid email address returns valid response.
        /// </summary>
        [Parallelizable]
        [Test]
        public void ValidateValidAddress()
        {
            ValidationResult result = Validator.Validate(_validAddress, _apikey);
            Assert.That(result.IsValid, Is.True);
        }

        /// <summary>
        /// Tests if an invalid e-mail address returns invalid response.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Parallelizable]
        [Test]
        public async Task ValidateInvalidAddressAsync()
        {
            ValidationResult result = await Validator.ValidateAsync(_invalidAddress, _apikey).ConfigureAwait(false);
            Assert.That(result.IsValid, Is.False);
        }

        /// <summary>
        /// Tests if an invalid e-mail address returns invalid response.
        /// </summary>
        [Parallelizable]
        [Test]
        public void ValidateInvalidAddress()
        {
            ValidationResult result = Validator.Validate(_invalidAddress, _apikey);
            Assert.That(result.IsValid, Is.False);
        }

        /// <summary>
        /// Tests if an e-mail containing a typo gets fixed.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Parallelizable]
        [Test]
        public async Task ValidateAddressWithTypoAsync()
        {
            ValidationResult result = await Validator.ValidateAsync("test@gmaill.com", _apikey).ConfigureAwait(false);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.DidYouMean, Is.EqualTo("test@gmail.com"));
        }

        /// <summary>
        /// Tests if an e-mail containing a typo gets fixed.
        /// </summary>
        [Parallelizable]
        [Test]
        public void ValidateAddressWithTypo()
        {
            ValidationResult result = Validator.Validate("test@gmaill.com", _apikey);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.DidYouMean, Is.EqualTo("test@gmail.com"));
        }

        /// <summary>
        /// Tests if a request with very short timeout actually fails.
        /// </summary>
        [Parallelizable]
        [Test]
        public void ThrowTimeoutExceptionAsync()
        {
            Assert.That(async () => await Validator.ValidateAsync(_validAddress, _apikey, 1).ConfigureAwait(false), Throws.TypeOf<TimeoutException>());
        }

        /// <summary>
        /// Tests if a request with very short timeout actually fails.
        /// </summary>
        [Parallelizable]
        [Test]
        public void ThrowTimeoutException()
        {
            Assert.That(() => Validator.Validate(_validAddress, _apikey, 1), Throws.TypeOf<TimeoutException>());
        }

        /// <summary>
        /// Tests if an exception is thrown when the API key is invalid.
        /// </summary>
        [Parallelizable]
        [Test]
        public void ThrowUnauthorizedExceptionOnBadApiKeyAsync()
        {
            Assert.That(async () => await Validator.ValidateAsync(_validAddress, "blabla").ConfigureAwait(false), Throws.TypeOf<UnauthorizedException>());
        }

        /// <summary>
        /// Tests if an exception is thrown when the API key is invalid.
        /// </summary>
        [Parallelizable]
        [Test]
        public void ThrowUnauthorizedExceptionOnBadApiKey()
        {
            Assert.That(() => Validator.Validate(_validAddress, "blabla"), Throws.TypeOf<UnauthorizedException>());
        }
    }
}
