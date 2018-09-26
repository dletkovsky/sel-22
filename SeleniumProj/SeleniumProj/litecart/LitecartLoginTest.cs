using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumProj.litecart
{
    public class LitecartLoginTest : BaseTestCase
    {
        private const string USERNAME_INPUT_XPATH = "//input[@name='username']";
        private const string PASSWORD_INPUT_XPATH = "//input[@name='password']";
        private const string LOGIN_BUTTON_XPATH = "//button[@name='login']";
        private const string LOGOUT_BUTTON_XPATH = "//a[@href='http://localhost/litecart/admin/logout.php']";


        [Test]
        public void LoginTest()
        {
            driver.Url = "http://localhost/litecart/admin/";
            driver.FindElement(By.XPath(USERNAME_INPUT_XPATH)).SendKeys("admin");
            driver.FindElement(By.XPath(PASSWORD_INPUT_XPATH)).SendKeys("admin");
            driver.FindElement(By.XPath(LOGIN_BUTTON_XPATH)).Click();
            Assert.True(driver.FindElements(By.XPath(LOGOUT_BUTTON_XPATH)).Count > 0);
        }
    }
}