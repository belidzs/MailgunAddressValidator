# MailgunAddressValidator - an e-mail validation library

[![Build status](https://ci.appveyor.com/api/projects/status/ltaecen2djcjlxhl?svg=true)](https://ci.appveyor.com/project/belidzs/mailgunaddressvalidator)

**Important:** To use this library, you need to sign up at https://www.mailgun.com and get a public API key. The first 100 validation is free each month.

*Note:* I'm not in any way affiliated or endorsed by Mailgun.

## Running tests with NUnit

If you want to run NUnit tests, you need to rename `app.config.example` to `app.config` and set `PublicApiKey` to your public API key. It should look like this: `pubkey-xxxxxxxxxxxxxxxxxxxxxxxx`

## Usage

Add NuGet package to your project, then simply start validating:

```c#
using MailgunAddressValidator;

namespace MailgunAddressValidatorTests
{
    public class MailgunAddressValidatorTests
    {
        private string apikey = "pubkey-xxxxxxxxxxxxxxxxxxxxxxx";

        public void ValidateValidAddress()
        {
            ValidationResult result = Validator.Validate("foo@mailgun.org", apikey);
            MessageBox.Show(String.Format("Validation result: {0}", result.IsValid));
        }

        public void ValidateAddressWithTypo()
        {
            ValidationResult result = Validator.Validate("test@gmaill.com", apikey);
            MessageBox.Show(String.Format("Validation result: {0}\nDid you mean the following? {1}", result.IsValid, result.DidYouMean);
        }
    }
}
```

## API documentation

You can find the official API documentation here: https://documentation.mailgun.com/en/latest/api-email-validation.html