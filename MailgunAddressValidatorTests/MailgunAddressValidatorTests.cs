using System.Threading.Tasks;
using NUnit.Framework;
using MailgunAddressValidator;

namespace MailgunAddressValidatorTests
{
    [TestFixture]
    public class MailgunAddressValidatorTests
    {
        private string apikey = Properties.Settings.Default.PublicApiKey;

        [Test]
        public void ValidateValidAddress()
        {
            ValidationResult result = Validator.Validate("foo@mailgun.org", apikey);
            Assert.That(result.IsValid, Is.True);
        }

        [Test]
        public void ValidateInvalidAddress()
        {
            ValidationResult result = Validator.Validate("test@example.com", apikey);
            Assert.That(result.IsValid, Is.False);
        }

        [Test]
        public void ValidateAddressWithTypo()
        {
            ValidationResult result = Validator.Validate("test@gmaill.com", apikey);
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.DidYouMean, Is.EqualTo("test@gmail.com"));
        }

        [Test]
        public void ThrowTimeoutException()
        {
            Assert.That(() => Validator.Validate("foo@mailgun.org", apikey, 1), Throws.TypeOf<System.Net.WebException>());
        }

        [Test]
        public void ThrowUnauthorizedExceptionOnBadApiKey()
        {
            Assert.That(() => Validator.Validate("foo@mailgun.org", "blabla"), Throws.TypeOf<UnauthorizedException>());
        }

        [Test]
        public async Task ValidateValidAddressAsync()
        {
            ValidationResult result = await Validator.ValidateAsync("foo@mailgun.org", apikey);
            Assert.That(result.IsValid, Is.True);
        }
    }
}
