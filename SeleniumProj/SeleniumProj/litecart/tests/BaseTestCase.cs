using System;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumProj.litecart.tests
{
    public class BaseTestCase
    {
        protected IWebDriver driver;
        protected WebDriverWait wait;
        protected VerifyUtils verifyUtils;

        [SetUp]
        public void startBrowser()
        {
//            driver = new InternetExplorerDriver();
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            verifyUtils = new VerifyUtils();
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


        public void switchToAnotherWindowHandleFromCurrent()
        {
            var windowHandles = driver.WindowHandles;
            var mainWindow = driver.CurrentWindowHandle;
            foreach (var handle in windowHandles.Where(handle => handle != mainWindow))
            {
                driver.SwitchTo().Window(handle);
                break;
            }
        }

        public void closeOtherWindows()
        {
            var windowHandles = driver.WindowHandles;
            var mainWindow = driver.CurrentWindowHandle;
            foreach (var windowHandle in windowHandles.Where(windowHandle => !windowHandle.Equals(mainWindow)))
            {
                driver.SwitchTo().Window(windowHandle).Close();
            }

            driver.SwitchTo().Window(mainWindow);
        }


        private const string ADMIN_USERNAME_INPUT_XPATH = "//input[@name='username']";
        private const string PASSWORD_INPUT_XPATH = "//input[@name='password']";

        private const string USER_USERNAME_INPUT_XPATH = "//input[@name='email']";
        private const string LOGIN_BUTTON_XPATH = "//button[@name='login']";


        public void loginAdmin()
        {
            //1) входит в панель администратора http://localhost/litecart/admin
            driver.Url = "http://localhost/litecart/admin/";
            driver.FindElement(By.XPath(ADMIN_USERNAME_INPUT_XPATH)).SendKeys("admin");
            driver.FindElement(By.XPath(PASSWORD_INPUT_XPATH)).SendKeys("admin");
            driver.FindElement(By.XPath(LOGIN_BUTTON_XPATH)).Click();
        }

        public void loginUser(string username, string password)
        {
            driver.FindElement(By.XPath(USER_USERNAME_INPUT_XPATH)).SendKeys(username);
            driver.FindElement(By.XPath(PASSWORD_INPUT_XPATH)).SendKeys(password);
            driver.FindElement(By.XPath(LOGIN_BUTTON_XPATH)).Click();
        }

        public void logout()
        {
            driver.FindElement(By.XPath("//a[contains(@href, 'logout')]")).Click();
        }

        public void loginAdmin(string url)
        {
            driver.Url = url;
            driver.FindElement(By.XPath(ADMIN_USERNAME_INPUT_XPATH)).SendKeys("admin");
            driver.FindElement(By.XPath(PASSWORD_INPUT_XPATH)).SendKeys("admin");
            driver.FindElement(By.XPath(LOGIN_BUTTON_XPATH)).Click();
        }

        public void openMainPageLitecart()
        {
            driver.Url = "http://localhost/litecart/";
        }


        public static string getUnique()
        {
            var result = string.Empty;
            var random = new Random();
            var chars = new char[10];
            for (var i = 0; i < 10; i++)
            {
                chars[i] = (char) random.Next(65, 90);
            }

            return chars.Aggregate(result, (current, c) => current + c);
        }

        public static string getNumber(int length)
        {
            var randomGenerator = new Random();
            var buf = new StringBuilder();

            for (var i = 0; i < length; i++)
            {
                buf.Append(Convert.ToString(randomGenerator.Next(10)));
            }

            return buf.ToString();
        }
    }
}