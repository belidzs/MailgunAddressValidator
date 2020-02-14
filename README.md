# MailgunAddressValidator - an e-mail validation library

**Important:** To use this library or run the included tests you need to sign up at https://www.mailgun.com and get a public API key. ~~The first 100 validation is free each month.~~

*Note:* I'm not in any way affiliated or endorsed by Mailgun.

## Usage

Add NuGet package to your project, then simply start validating: 

```c#
using MailgunAddressValidator;

namespace MailgunAddressValidatorExamples
{
    public class MailgunAddressValidatorExamples
    {
        private string apikey = "pubkey-xxxxxxxxxxxxxxxxxxxxxxx";

        public void ValidateValidAddress()
        {
            ValidationResult result = Validator.Validate("sales@mailgun.com", apikey);
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

You can find the official API documentation for `v3` here: https://documentation.mailgun.com/en/latest/api-email-validation-deprecated.html


## Running tests with NUnit

If you want to run NUnit tests, you need to rename `app.config.example` to `app.config` and replace `PublicApiKey` with your own public API key. It should look like this: `pubkey-xxxxxxxxxxxxxxxxxxxxxxxx`