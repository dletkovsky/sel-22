using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SeleniumProj.litecart
{
    public class VerifyUtils
    {
        private StringBuilder verificationErrors = new StringBuilder();

        public void verifyTrue(bool condition, string message)
        {
            try
            {
                Assert.True(condition, message);
            }
            catch (Exception ae)
            {
                verificationErrors.Append(ae.Message).Append("\n");
            }
        }

        public void checkForVerifications()
        {
            var verificationErrorstring = getVerificationErrors();
            clearVerificationErrors();
            if (!"".Equals(verificationErrorstring))
            {
                throw new AssertionException(verificationErrorstring);
            }
        }

        public void clearVerificationErrors()
        {
            verificationErrors = new StringBuilder();
        }

        public string getVerificationErrors()
        {
            return verificationErrors.ToString();
        }
    }
}