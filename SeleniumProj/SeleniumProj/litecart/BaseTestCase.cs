using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumProj.litecart
{
    public class BaseTestCase
    {
        protected IWebDriver driver;
        protected WebDriverWait wait;

        [SetUp]
        public void startBrowser()
        {
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [TearDown]
        public void closeBrowser()
        {
            driver.Close();
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
            }
        }
    }
}