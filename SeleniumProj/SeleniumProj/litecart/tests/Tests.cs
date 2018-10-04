using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumProj.litecart.tests
{
    public class Tests : BaseTestCase
    {
        [Test]
        public void task11()
        {
            openMainPageLitecart();
            driver.FindElement(By.XPath("//a[contains(@href, 'create_account')]")).Click();

            var emailAddress = "email_" + getUnique() + "@gmail.com";
            var password = "newpassword";


            fillInfoCreateNewAccounts("taxId_" + getUnique(), "company_" + getUnique(), "firstname_" + getUnique(),
                "lastname_" + getUnique(), "address1_" + getUnique(), "address2_" + getUnique(),
                getNumber(5), "city_" + getUnique(), "United States", "Arizona", emailAddress, getNumber(5),
                password);

            logout();
            loginUser(emailAddress, password);
            logout();
        }


        public void fillInfoCreateNewAccounts(string taxId, string company, string firstname, string lastname,
            string address1, string address2, string postcode, string city, string country, string zone, string email,
            string phone,
            string password)
        {
            driver.FindElement(By.XPath("//input[@name='tax_id']")).SendKeys(taxId);
            driver.FindElement(By.XPath("//input[@name='company']")).SendKeys(company);
            driver.FindElement(By.XPath("//input[@name='firstname']")).SendKeys(firstname);
            driver.FindElement(By.XPath("//input[@name='lastname']")).SendKeys(lastname);
            driver.FindElement(By.XPath("//input[@name='address1']")).SendKeys(address1);
            driver.FindElement(By.XPath("//input[@name='address2']")).SendKeys(address2);
            driver.FindElement(By.XPath("//input[@name='postcode']")).SendKeys(postcode);
            driver.FindElement(By.XPath("//input[@name='city']")).SendKeys(city);

            new SelectElement(driver.FindElement(By.XPath("//select[@name='country_code']"))).SelectByText(
                country);

            if (country.Equals("United States"))
            {
                new SelectElement(driver.FindElement(By.XPath("//select[@name='zone_code']"))).SelectByText(
                    zone);
            }

            driver.FindElement(By.XPath("//input[@name='email']")).SendKeys(email);
            driver.FindElement(By.XPath("//input[@name='phone']")).SendKeys(phone);
            driver.FindElement(By.XPath("//input[@name='password']")).SendKeys(password);
            driver.FindElement(By.XPath("//input[@name='confirmed_password']")).SendKeys(password);

            driver.FindElement(By.XPath("//button[@name='create_account']")).Click();
        }
    }
}