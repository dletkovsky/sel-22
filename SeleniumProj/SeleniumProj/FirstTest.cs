using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumProj
{
    [TestFixture]
    [Description("First webdriver test")]
    public class FirstTest
    {

        IWebDriver driver;

        [SetUp]
        public void startBrowser()
        {
            driver = new ChromeDriver("c:\\Users\\d_letkovskiy\\Downloads");
        }

        [Test]
        public void test()
        {
            driver.Url = "http://www.google.co.in";
        }

        [TearDown]
        public void closeBrowser()
        {
            driver.Close();
        }

    }
}
