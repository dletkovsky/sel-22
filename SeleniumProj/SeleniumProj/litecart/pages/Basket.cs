using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumProj.litecart.pages
{
    public class Basket : BasePage
    {
        public int getQuantityItems()
        {
            return int.Parse(getDriver().FindElement(By.XPath("//span[@class='quantity']")).GetAttribute("textContent"));
        }

        public Basket waitQuantityUpdated(int quanitityBefore)
        {
            //подождать, пока счётчик товаров в корзине обновится
            new WebDriverWait(getDriver(), TimeSpan.FromSeconds(5)).Until(
                d => getDriver().FindElement(By.XPath($"//span[@class='quantity' and text()={quanitityBefore + 1}]"))
                    .Displayed);
            return this;
        }

        public BasketPage open()
        {
            getDriver().FindElement(By.XPath("//a[contains(text(), 'Checkout')]")).Click();
            return new BasketPage();
        }
    }
}
