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
    public class BaseTestCase
    {
        protected IWebDriver driver;

        [SetUp]
        public void startBrowser()
        {
            driver = new ChromeDriver("c:\\Users\\d_letkovskiy\\Downloads");
        }

        [TearDown]
        public void closeBrowser()
        {
            driver.Close();
        }
    }
}
