using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmailValidation;

namespace WpfApp1
{
    class Validation
    {
        public bool Validate(string email, ref string result)
        {
            EmailValidator emailValidator = new EmailValidator();


            EmailValidationResult res;

            if (!emailValidator.Validate(email, out res))
            {
                result = "Unable to check e-mail";
                return false;
            }

            switch (res)
            {
                case EmailValidationResult.OK:

                    result = "Valid Email";
                    return true;

                case EmailValidationResult.MailboxUnavailable:
                    result = "Email server replied invalid Email";
                    return false;

                case EmailValidationResult.MailboxStorageExceeded:
                    result = "Mailbox overflow";
                    return false;

                case EmailValidationResult.NoMailForDomain:
                    result = "Emails are not configured for domain";
                    return false;
            }
            return false;
        }
    }
}
