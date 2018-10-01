using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
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


        [Test]
        public void LoginTest()
        {
            //1) входит в панель администратора http://localhost/litecart/admin
            driver.Url = "http://localhost/litecart/admin/";
            driver.FindElement(By.XPath(USERNAME_INPUT_XPATH)).SendKeys("admin");
            driver.FindElement(By.XPath(PASSWORD_INPUT_XPATH)).SendKeys("admin");
            driver.FindElement(By.XPath(LOGIN_BUTTON_XPATH)).Click();


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
    }
}