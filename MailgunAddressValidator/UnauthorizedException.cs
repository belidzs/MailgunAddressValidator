using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailgunAddressValidator
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() : base("Bad API key. You should use your public key from My Account page")
        {
        }
    }
}
