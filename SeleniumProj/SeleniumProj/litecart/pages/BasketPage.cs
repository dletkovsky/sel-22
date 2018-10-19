using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumProj.litecart.pages
{
    public class BasketPage : BasePage
    {
        public BasketPage deleteAllProductsFromBasket()
        {
            if (getDriver().FindElements(By.XPath("//ul[@class='shortcuts']")).Count > 0)
            {
                getDriver().FindElement(By.XPath("//ul[@class='shortcuts']/li[1]")).Click();


                var count = getDriver().FindElements(By.XPath("//ul[@class='shortcuts']/li")).Count;
                Console.WriteLine("всего элементов в корзине: " + count);

                for (var i = count; i >= 1; i--)
                {
                    var wait = new WebDriverWait(getDriver(), TimeSpan.FromSeconds(10));
                    wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[@name='remove_cart_item']")));


                    getDriver().FindElement(By.XPath(
                        $"//table[@class = 'dataTable rounded-corners']/tbody/tr[{i + 1}]/td[2][@class='item']"));


                    getDriver().FindElement(By.XPath("//button[@name='remove_cart_item']")).Click();
                    Console.WriteLine("удалили элемент номер: " + i);

                    wait = new WebDriverWait(getDriver(), TimeSpan.FromSeconds(10));
                    wait.Until(ExpectedConditions.StalenessOf(getDriver().FindElement(By.XPath(
                        $"//table[@class = 'dataTable rounded-corners']/tbody/tr[{i + 1}]/td[2][@class='item']"))));
                }
            }
            else
            {
                Console.WriteLine("был один продукт в корзине");
                getDriver().FindElement(By.XPath("//button[@name='remove_cart_item']")).Click();
            }

            new WebDriverWait(getDriver(), TimeSpan.FromSeconds(3)).Until(
                ExpectedConditions.ElementIsVisible(By.XPath("//div[@id='checkout-cart-wrapper']//em")));
            return this;
        }

        public bool isCartEmpty()
        {
            return getDriver().FindElements(
                           By.XPath(
                               "//div[@id='checkout-cart-wrapper']//em[text()='There are no items in your cart.']"))
                       .Count > 0;
        }
    }
}