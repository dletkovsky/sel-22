using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumProj.litecart.objects;
using SeleniumProj.litecart.utils;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace SeleniumProj.litecart.tests
{
    public class LitecartLoginTest : BaseTestCase
    {

        private const string LOGOUT_BUTTON_XPATH = "//a[@href='http://localhost/litecart/admin/logout.php']";


        

        


        [Test]
        public void LoginTest()
        {
            //1) входит в панель администратора http://localhost/litecart/admin
            loginAdmin();


            //2) прокликивает последовательно все пункты меню слева, включая вложенные пункты
           


            int count = driver.FindElements(By.XPath("//ul[@class='list-vertical']/li")).Count;
            for (int i = 1; i <= count; i++)
            {
                driver.FindElement(By.XPath($"//ul[@class='list-vertical']/li[{i}]")).Click();
                if (driver.FindElements(By.XPath($"//li[{i}]/ul")).Count > 0)
                {
                    int countChild = driver.FindElements(By.XPath($"//li[{i}]/ul/li")).Count;
                    for (int j = 1; j <= countChild; j++)
                    {
                        driver.FindElement(By.XPath($"//li[{i}]/ul/li[{j}]")).Click();
                    }
                }
            }

            /*

            foreach (var menuItem in menuItemsList)
            {
                if (menuItem.Item2 != null)
                {
                    driver.FindElement(By.XPath(string.Format(MAIN_MENU_CHILD_XPATH_PATTERN,
                        ToDescriptionString(menuItem.Item1), ToDescriptionString(menuItem.Item2)))).Click();
                }
                else
                {
                    driver.FindElement(By.XPath(string.Format(MAIN_MENU_XPATH_PATTERN,
                        ToDescriptionString(menuItem.Item1)))).Click();
                }

                //3) для каждой страницы проверяет наличие заголовка(то есть элемента с тегом h1)
                Assert.True(driver.FindElements(By.CssSelector("h1")).Count > 0);
            }*/


            Assert.True(driver.FindElements(By.XPath(LOGOUT_BUTTON_XPATH)).Count > 0);
        }


        public bool isStickerPresent(string box, int index) => driver.FindElements(By.XPath(
                                                                       $"//div[@id='{box}']//li[contains(@class, 'product')][{index}]//div[contains(@class, 'sticker ')]"))
                                                                   .Count == 1;


        [Test]
        public void LoginTest_task8()
        {
            openMainPageLitecart();

            foreach (var box in new List<string> {"box-most-popular", "box-campaigns", "box-latest-products"})
            {
                var countMostPopularProducts = driver
                    .FindElements(By.XPath($"//div[@id='{box}']//li[contains(@class, 'product')]"))
                    .Count;
                for (var i = 1; i <= countMostPopularProducts; i++)
                {
                    verifyUtils.verifyTrue(isStickerPresent(box, i),
                        $"Стикер отсутствует у товара в разделе {box} c индексом {i}");
                }
            }

            verifyUtils.checkForVerifications();
        }


        [Test]
        public void LoginTest_task9_1()
        {
            //1) открываем http://localhost/litecart/admin/?app=countries&doc=countries
            loginAdmin("http://localhost/litecart/admin/?app=countries&doc=countries");


            var countCountries = driver.FindElements(By.XPath("//form[@name='countries_form']//tbody/tr")).Count;


            var listCountriesWithZones = new List<int>();


            var listCountries = new List<string>();
            for (var i = 2; i < countCountries; i++)
            {
                listCountries.Add(driver.FindElement(By.XPath($"//form[@name='countries_form']//tbody/tr[{i}]/td[5]"))
                    .Text);
                if (driver.FindElement(By.XPath($"//form[@name='countries_form']//tbody/tr[{i}]/td[6]")).Text != "0")
                {
                    listCountriesWithZones.Add(i);
                }
            }

            var expectedList = listCountries.OrderBy(s => s);
            verifyUtils.verifyTrue(listCountries.SequenceEqual(expectedList), "Некорретная сортировка стран!");


            if (listCountriesWithZones.Count != 0)
            {
                foreach (var index in listCountriesWithZones)
                {
                    driver.FindElement(By.XPath($"//form[@name='countries_form']//tbody/tr[{index}]/td[5]/a")).Click();
                    var count = driver.FindElements(By.XPath("//table[@id='table-zones']/tbody/tr")).Count;
                    var zonesList = new List<string>();
                    for (var i = 2; i < count; i++)
                    {
                        zonesList.Add(driver.FindElement(By.XPath($"//table[@id='table-zones']/tbody/tr[{i}]/td[3]"))
                            .Text);
                    }

                    var expectedZonesList = zonesList.OrderBy(s => s);
                    verifyUtils.verifyTrue(zonesList.SequenceEqual(expectedZonesList), "Некорретная сортировка zones!");
                    driver.Url = "http://localhost/litecart/admin/?app=countries&doc=countries";
                }
            }

            verifyUtils.checkForVerifications();
        }


        [Test]
        public void LoginTest_task9_2()
        {
            //1) открываем http://localhost/litecart/admin/?app=geo_zones&doc=geo_zones
            loginAdmin("http://localhost/litecart/admin/?app=geo_zones&doc=geo_zones");


            var countCountries = driver.FindElements(By.XPath("//form[@name='geo_zones_form']//tbody/tr")).Count;


            var listCountries = new List<string>();
            for (var i = 2; i < countCountries; i++)
            {
                listCountries.Add(driver.FindElement(By.XPath($"//form[@name='geo_zones_form']//tbody/tr[{i}]/td[3]"))
                    .Text);
            }

            var expectedList = listCountries.OrderBy(s => s);
            verifyUtils.verifyTrue(listCountries.SequenceEqual(expectedList), "Некорретная сортировка стран!");


            for (int j = 2; j < countCountries; j++)
            {
                driver.FindElement(By.XPath($"//form[@name='geo_zones_form']//tbody/tr[{j}]/td[3]/a")).Click();

                var count = driver.FindElements(By.XPath("//table[@id='table-zones']/tbody/tr")).Count;
                var zonesList = new List<string>();
                for (var i = 2; i < count; i++)
                {
                    zonesList.Add(driver
                        .FindElement(
                            By.XPath($"//table[@id='table-zones']/tbody/tr[{i}]/td[3]/select/option[@selected]")).Text);
                }

                var expectedZonesList = zonesList.OrderBy(s => s);
                verifyUtils.verifyTrue(zonesList.SequenceEqual(expectedZonesList), "Некорретная сортировка zones!");
                driver.Url = "http://localhost/litecart/admin/?app=geo_zones&doc=geo_zones";
            }

            verifyUtils.checkForVerifications();
        }


        [Test]
        public void LoginTest_task10()
        {
            openMainPageLitecart();
            var productMainPage = getCompaignsProductOnMainPage(1);

            driver.FindElement(By.XPath(CAMPAIGNS_PRODUCT_MAIN_PAGE_XPATH_PATTERN)).Click();
            var product = getCompaignsProductPageProduct();


            //проверка основных значений продукта
            verifyUtils.verifyTrue(productMainPage.Equals(product),
                "Значения полей товара на главной странице и странице товара различны!");

            //проверка размеров цен
            verifyUtils.verifyTrue(productMainPage.sizeCampPrice < product.sizeCampPrice,
                "Размер шрифта акционной цены не соответствует требованиям!");
            verifyUtils.verifyTrue(productMainPage.sizeRegPrice < product.sizeRegPrice,
                "Размер шрифта обычной цены не соответствует требованиям!");

            //проверка цвета акционной цены
            verifyUtils.verifyTrue(productMainPage.colorCampPrice.Equals(product.colorCampPrice),
                "Цвет акционной цены на главной странице и странице товара различны!");

            //проверка цветов обычной цены
            verifyUtils.verifyTrue(productMainPage.isRGBEqualColorRegPrice(),
                "Цвет обычной цены на главной странице некорректен!");
            verifyUtils.verifyTrue(product.isRGBEqualColorRegPrice(),
                "Цвет обычной цены на странице товара некорректен!");
            verifyUtils.checkForVerifications();
        }


        
        public const string CAMPAIGNS_PRODUCT_MAIN_PAGE_XPATH_PATTERN =
            "//div[@id='box-campaigns']//ul[@class='listing-wrapper products']/li";

        public const string CAMPAIGNS_PRODUCT_PRODUCT_PAGE_XPATH_PATTERN =
            "//div[@id='box-product']";


        public Product getCompaignsProductOnMainPage(int index)
        {
            var product = new Product
            {
                name = driver
                    .FindElement(By.XPath(CAMPAIGNS_PRODUCT_MAIN_PAGE_XPATH_PATTERN + $"[{index}]//div[@class='name']"))
                    .Text,
                reg_price = driver.FindElement(By.XPath(
                    CAMPAIGNS_PRODUCT_MAIN_PAGE_XPATH_PATTERN +
                    $"[{index}]//div/s[@class='regular-price']")).Text,
                campaign_price = driver.FindElement(By.XPath(
                    CAMPAIGNS_PRODUCT_MAIN_PAGE_XPATH_PATTERN +
                    $"[{index}]//div/strong[@class='campaign-price']")).Text,
                colorCampPrice = Utils.ParseColor(driver
                    .FindElement(By.XPath(CAMPAIGNS_PRODUCT_MAIN_PAGE_XPATH_PATTERN +
                                          $"[{index}]//div/strong[@class='campaign-price']"))
                    .GetCssValue("color")),
                colorRegPrice = Utils.ParseColor(driver
                    .FindElement(By.XPath(CAMPAIGNS_PRODUCT_MAIN_PAGE_XPATH_PATTERN +
                                          $"[{index}]//div/s[@class='regular-price']"))
                    .GetCssValue("color")),
                sizeCampPrice = float.Parse(driver.FindElement(By.XPath(CAMPAIGNS_PRODUCT_MAIN_PAGE_XPATH_PATTERN +
                                                                        $"[{index}]//div/strong[@class='campaign-price']"))
                    .GetCssValue("fontSize")
                    .Replace("px", ""), CultureInfo.InvariantCulture.NumberFormat),

                sizeRegPrice = float.Parse(driver
                    .FindElement(By.XPath(CAMPAIGNS_PRODUCT_MAIN_PAGE_XPATH_PATTERN +
                                          $"[{index}]//div/s[@class='regular-price']"))
                    .GetCssValue("fontSize")
                    .Replace("px", ""), CultureInfo.InvariantCulture.NumberFormat)
            };
            return product;
        }


        public Product getCompaignsProductPageProduct()
        {
            var product = new Product
            {
                name = driver
                    .FindElement(
                        By.XPath(CAMPAIGNS_PRODUCT_PRODUCT_PAGE_XPATH_PATTERN + "//h1[@class='title']"))
                    .Text,
                reg_price = driver
                    .FindElement(By.XPath(CAMPAIGNS_PRODUCT_PRODUCT_PAGE_XPATH_PATTERN +
                                          "//s[@class='regular-price']"))
                    .Text,
                campaign_price = driver
                    .FindElement(By.XPath(CAMPAIGNS_PRODUCT_PRODUCT_PAGE_XPATH_PATTERN +
                                          "//strong[@class='campaign-price']")).Text,

                colorCampPrice = Utils.ParseColor(driver
                    .FindElement(By.XPath(CAMPAIGNS_PRODUCT_PRODUCT_PAGE_XPATH_PATTERN +
                                          "//strong[@class='campaign-price']"))
                    .GetCssValue("color")),

                colorRegPrice = Utils.ParseColor(driver
                    .FindElement(By.XPath(CAMPAIGNS_PRODUCT_PRODUCT_PAGE_XPATH_PATTERN +
                                          "//s[@class='regular-price']"))
                    .GetCssValue("color")),

                sizeCampPrice = float.Parse(driver
                        .FindElement(By.XPath(CAMPAIGNS_PRODUCT_PRODUCT_PAGE_XPATH_PATTERN +
                                              "//strong[@class='campaign-price']"))
                        .GetCssValue("fontSize")
                        .Replace("px", ""),
                    CultureInfo.InvariantCulture.NumberFormat),

                sizeRegPrice = float.Parse(driver
                        .FindElement(By.XPath(CAMPAIGNS_PRODUCT_PRODUCT_PAGE_XPATH_PATTERN +
                                              "//s[@class='regular-price']"))
                        .GetCssValue("fontSize")
                        .Replace("px", ""),
                    CultureInfo.InvariantCulture.NumberFormat)
            };
            return product;
        }
    }
}