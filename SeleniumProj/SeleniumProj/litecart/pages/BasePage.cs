using OpenQA.Selenium;
using OpenQA.Selenium.Support.Events;
using OpenQA.Selenium.Support.UI;
using SeleniumProj.litecart.tests;

namespace SeleniumProj.litecart.pages
{
    public class BasePage
    {
        public BasePage clickLogoType()
        {
            getDriver().FindElement(By.XPath("//div[@id='logotype-wrapper']")).Click();
            return this;
        }

        public BasketPage openBasket()
        {
            return new Basket().open();
        }

        public EventFiringWebDriver getDriver()
        {
            return BaseTestCase.driver;
        }
    }
}