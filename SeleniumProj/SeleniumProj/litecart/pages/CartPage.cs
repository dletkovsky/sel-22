using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumProj.litecart.pages
{
    public class CartPage : BasePage
    {
        private const string ADD_CART_PRODUCT_XPATH = "//button[@name='add_cart_product']";
        private const string SELECT_SIZE_XPATH = "//select[@name='options[Size]']";


        public CartPage addCartToBasket()
        {
            var basket = new Basket();
            var quanitityBefore = basket.getQuantityItems();
            
            //добавить товар в корзину
            addCartProduct();
            basket.waitQuantityUpdated(quanitityBefore);
            return this;
        }



        public CartPage addCartProduct()
        {
            selectSmallSize();
            getDriver().FindElement(By.XPath(ADD_CART_PRODUCT_XPATH)).Click();
            return this;
        }

        public CartPage selectSmallSize()
        {
            getDriver().Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
            if (getDriver().FindElements(By.XPath(SELECT_SIZE_XPATH)).Count > 0)
            {
                new SelectElement(getDriver().FindElement(By.XPath(SELECT_SIZE_XPATH)))
                    .SelectByValue("Small");
            }

            getDriver().Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            return this;
        }
    }
}
