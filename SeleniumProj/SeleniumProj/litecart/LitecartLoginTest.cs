using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumProj.litecart.objects;
using SeleniumProj.litecart.utils;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace SeleniumProj.litecart
{
    public class LitecartLoginTest : BaseTestCase
    {
        private const string USERNAME_INPUT_XPATH = "//input[@name='username']";
        private const string PASSWORD_INPUT_XPATH = "//input[@name='password']";
        private const string LOGIN_BUTTON_XPATH = "//button[@name='login']";
        private const string LOGOUT_BUTTON_XPATH = "//a[@href='http://localhost/litecart/admin/logout.php']";


        const string MAIN_MENU_XPATH_PATTERN = "//span[text()='{0}']";
        const string MAIN_MENU_CHILD_XPATH_PATTERN = "//li[//span[text()='{0}']]//span[text()='{1}']";


        private enum MENU_ENUM
        {
            [Description("Appearence")] APPEARENCE = 1,
            [Description("Template")] TEMPLATE = 2,
            [Description("Logotype")] LOGOTYPE = 3,
            [Description("Catalog")] CATALOG = 4,
            [Description("Product Groups")] PRODUCT_GROUPS = 5,
            [Description("Option Groups")] OPTION_GROUPS = 6,
            [Description("Manufacturers")] MANUFACTURES = 7,
            [Description("Suppliers")] SUPPLIERS = 8,
            [Description("Delivery Statuses")] DELIVERY_STATUSES = 9,
            [Description("Sold Out Statuses")] SOLD_OUT_STATUSES = 10,
            [Description("Quantity Units")] QUANTITY_UNITS = 11,
            [Description("CSV Import/Export")] CSV_IMPORT_EXPORT = 12,
            [Description("Countries")] COUNTRIES = 13,
            [Description("Currencies")] CURRENCIES = 14,
            [Description("Customers")] CUSTOMERS = 15,
            [Description("CSV Import/Export")] CSV_IMPORT_EXPORT_CURRENCIES = 15,
            [Description("Newsletter")] NEWSLETTER = 16,
            [Description("Geo Zones")] GEOZONES = 17,
            [Description("Languages")] LANGUAGES = 18,
            [Description("Storage Encoding")] STORAGE_ENCODING = 19,
            [Description("Modules")] MODULES = 19,
            [Description("Customer")] CUSTOMER = 20,
            [Description("Shipping")] SHIPPING = 21,
            [Description("Payment")] PAYMENT = 22,
            [Description("Order Total")] ORDER_TOTAL = 23,
            [Description("Order Success")] ORDER_SUCCESS = 24,
            [Description("Order Action")] ORDER_ACTION = 25,
            [Description("Orders")] ORDERS = 26,
            [Description("Order Statuses")] ORDERS_STATUSES = 27,
            [Description("Pages")] PAGES = 28,
            [Description("Reports")] REPORTS = 29,
            [Description("Most Sold Products")] MOST_SOLD_PRODUCTS = 30,

            [Description("Most Shopping Customers")]
            MOST_SHOPPING_CUSTOMERS = 31,
            [Description("Settings")] SETTINGS = 32,
            [Description("Defaults")] DEFAULTS = 33,
            [Description("General")] GENERAL = 34,
            [Description("Listings")] LISTINGS = 35,
            [Description("Images")] IMAGES = 36,
            [Description("Checkout")] CHECKOUT = 37,
            [Description("Advanced")] ADVANCED = 38,
            [Description("Security")] SECURITY = 39,
            [Description("Slides")] SLIDES = 40,
            [Description("Tax")] TAX = 41,
            [Description("Tax Rates")] TAX_RATES = 42,
            [Description("Translations")] TRANSLATIONS = 43,
            [Description("Scan Files")] SCAN_FILES = 44,
            [Description("CSV Import/Export")] CSV_IMPORT_EXPORT_TRANSLATIONS = 45,
            [Description("Users")] USERS = 46,
            [Description("vQmods")] QMODS = 47
        }


        public static string ToDescriptionString(Enum val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[]) val
                .GetType()
                .GetField(val.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }


        public void loginAdmin()
        {
            //1) входит в панель администратора http://localhost/litecart/admin
            driver.Url = "http://localhost/litecart/admin/";
            driver.FindElement(By.XPath(USERNAME_INPUT_XPATH)).SendKeys("admin");
            driver.FindElement(By.XPath(PASSWORD_INPUT_XPATH)).SendKeys("admin");
            driver.FindElement(By.XPath(LOGIN_BUTTON_XPATH)).Click();
        }

        public void loginAdmin(string url)
        {
            driver.Url = url;
            driver.FindElement(By.XPath(USERNAME_INPUT_XPATH)).SendKeys("admin");
            driver.FindElement(By.XPath(PASSWORD_INPUT_XPATH)).SendKeys("admin");
            driver.FindElement(By.XPath(LOGIN_BUTTON_XPATH)).Click();
        }

        public void openMainPageLitecart()
        {
            driver.Url = "http://localhost/litecart/";
        }


        [Test]
        public void LoginTest()
        {
            //1) входит в панель администратора http://localhost/litecart/admin
            loginAdmin();


            //2) прокликивает последовательно все пункты меню слева, включая вложенные пункты
            var menuItemsList = new List<Tuple<Enum, Enum>>
            {
                new Tuple<Enum, Enum>(MENU_ENUM.APPEARENCE, null),
                new Tuple<Enum, Enum>(MENU_ENUM.APPEARENCE, MENU_ENUM.TEMPLATE),
                new Tuple<Enum, Enum>(MENU_ENUM.APPEARENCE, MENU_ENUM.LOGOTYPE),
                new Tuple<Enum, Enum>(MENU_ENUM.CATALOG, null),
                new Tuple<Enum, Enum>(MENU_ENUM.CATALOG, MENU_ENUM.PRODUCT_GROUPS),
                new Tuple<Enum, Enum>(MENU_ENUM.CATALOG, MENU_ENUM.OPTION_GROUPS),
                new Tuple<Enum, Enum>(MENU_ENUM.CATALOG, MENU_ENUM.MANUFACTURES),
                new Tuple<Enum, Enum>(MENU_ENUM.CATALOG, MENU_ENUM.SUPPLIERS),
                new Tuple<Enum, Enum>(MENU_ENUM.CATALOG, MENU_ENUM.DELIVERY_STATUSES),
                new Tuple<Enum, Enum>(MENU_ENUM.CATALOG, MENU_ENUM.SOLD_OUT_STATUSES),
                new Tuple<Enum, Enum>(MENU_ENUM.CATALOG, MENU_ENUM.QUANTITY_UNITS),
                new Tuple<Enum, Enum>(MENU_ENUM.CATALOG, MENU_ENUM.CSV_IMPORT_EXPORT),
                new Tuple<Enum, Enum>(MENU_ENUM.COUNTRIES, null),
                new Tuple<Enum, Enum>(MENU_ENUM.CURRENCIES, null),
                new Tuple<Enum, Enum>(MENU_ENUM.CUSTOMERS, null),
                new Tuple<Enum, Enum>(MENU_ENUM.CUSTOMERS, MENU_ENUM.CSV_IMPORT_EXPORT),
                new Tuple<Enum, Enum>(MENU_ENUM.CUSTOMERS, MENU_ENUM.NEWSLETTER),
                new Tuple<Enum, Enum>(MENU_ENUM.GEOZONES, null),
                new Tuple<Enum, Enum>(MENU_ENUM.LANGUAGES, null),
                new Tuple<Enum, Enum>(MENU_ENUM.LANGUAGES, MENU_ENUM.STORAGE_ENCODING),
                new Tuple<Enum, Enum>(MENU_ENUM.MODULES, null),
                new Tuple<Enum, Enum>(MENU_ENUM.MODULES, MENU_ENUM.CUSTOMER),
                new Tuple<Enum, Enum>(MENU_ENUM.MODULES, MENU_ENUM.SHIPPING),
                new Tuple<Enum, Enum>(MENU_ENUM.MODULES, MENU_ENUM.PAYMENT),
                new Tuple<Enum, Enum>(MENU_ENUM.MODULES, MENU_ENUM.ORDER_TOTAL),
                new Tuple<Enum, Enum>(MENU_ENUM.MODULES, MENU_ENUM.ORDER_SUCCESS),
                new Tuple<Enum, Enum>(MENU_ENUM.MODULES, MENU_ENUM.ORDER_ACTION),
                new Tuple<Enum, Enum>(MENU_ENUM.ORDERS, null),
                new Tuple<Enum, Enum>(MENU_ENUM.ORDERS, MENU_ENUM.ORDERS_STATUSES),
                new Tuple<Enum, Enum>(MENU_ENUM.PAGES, null),
                new Tuple<Enum, Enum>(MENU_ENUM.REPORTS, null),
                new Tuple<Enum, Enum>(MENU_ENUM.REPORTS, MENU_ENUM.MOST_SOLD_PRODUCTS),
                new Tuple<Enum, Enum>(MENU_ENUM.REPORTS, MENU_ENUM.MOST_SHOPPING_CUSTOMERS),
                new Tuple<Enum, Enum>(MENU_ENUM.SETTINGS, null),
                new Tuple<Enum, Enum>(MENU_ENUM.SETTINGS, MENU_ENUM.DEFAULTS),
                new Tuple<Enum, Enum>(MENU_ENUM.SETTINGS, MENU_ENUM.GENERAL),
                new Tuple<Enum, Enum>(MENU_ENUM.SETTINGS, MENU_ENUM.LISTINGS),
                new Tuple<Enum, Enum>(MENU_ENUM.SETTINGS, MENU_ENUM.IMAGES),
                new Tuple<Enum, Enum>(MENU_ENUM.SETTINGS, MENU_ENUM.CHECKOUT),
                new Tuple<Enum, Enum>(MENU_ENUM.SETTINGS, MENU_ENUM.ADVANCED),
                new Tuple<Enum, Enum>(MENU_ENUM.SETTINGS, MENU_ENUM.SECURITY),
                new Tuple<Enum, Enum>(MENU_ENUM.SLIDES, null),
                new Tuple<Enum, Enum>(MENU_ENUM.TAX, null),
                new Tuple<Enum, Enum>(MENU_ENUM.TAX, MENU_ENUM.TAX_RATES),
                new Tuple<Enum, Enum>(MENU_ENUM.TRANSLATIONS, null),
                new Tuple<Enum, Enum>(MENU_ENUM.TRANSLATIONS, MENU_ENUM.SCAN_FILES),
                new Tuple<Enum, Enum>(MENU_ENUM.TRANSLATIONS, MENU_ENUM.CSV_IMPORT_EXPORT_TRANSLATIONS),
                new Tuple<Enum, Enum>(MENU_ENUM.USERS, null),
                new Tuple<Enum, Enum>(MENU_ENUM.QMODS, null)
            };


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
                                                                       $"//div[@id='{box}']//ul[@class='listing-wrapper products']/li[{index}]//div[contains(@class, 'sticker ')]"))
                                                                   .Count >
                                                               0;


        [Test]
        public void LoginTest_task8()
        {
            openMainPageLitecart();

            foreach (var box in new List<string> {"box-most-popular", "box-campaigns", "box-latest-products"})
            {
                var countMostPopularProducts = driver
                    .FindElements(By.XPath($"//div[@id='{box}']//ul[@class='listing-wrapper products']/li"))
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
            verifyUtils.verifyTrue(productMainPage.isRGBEqualColorRegPrice(), "Цвет обычной цены на главной странице некорректен!");
            verifyUtils.verifyTrue(product.isRGBEqualColorRegPrice(), "Цвет обычной цены на странице товара некорректен!");
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
                                                                        $"[{index}]//div/strong[@class='campaign-price']")).GetCssValue("fontSize")
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
                    .FindElement(By.XPath(CAMPAIGNS_PRODUCT_PRODUCT_PAGE_XPATH_PATTERN + "//s[@class='regular-price']"))
                        .GetCssValue("fontSize")
                        .Replace("px", ""),
                    CultureInfo.InvariantCulture.NumberFormat)
            };
            return product;
        }
    }
}