using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailgunAddressValidator
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string Address { get; set; }
        public ValidationResultParts Parts { get; set; }
        public string DidYouMean { get; set; }
    }

    public class ValidationResultParts
    {
        public string DisplayName { get; set; }
        public string LocalPart { get; set; }
        public string Domain { get; set; }
    }
}
