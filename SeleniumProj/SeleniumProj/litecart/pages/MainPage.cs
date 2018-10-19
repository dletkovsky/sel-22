using OpenQA.Selenium;


namespace SeleniumProj.litecart.pages
{
    public class MainPage : BasePage
    {
        public MainPage open()
        {
            getDriver().Url = "http://localhost/litecart/";
            return this;
        }


        public CartPage openCart()
        {
            getDriver().FindElement(By.XPath("//div[@id='box-most-popular']//li[contains(@class, 'product')][1]"))
                .Click();
            return new CartPage();
        }


        public MainPage addCarts(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var cartPage = openCart();
                cartPage.addCartToBasket();
                clickLogoType();
            }
            return this;
        }

    }
}