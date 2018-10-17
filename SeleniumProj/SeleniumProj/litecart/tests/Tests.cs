using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumProj.litecart.objects;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace SeleniumProj.litecart.tests
{
    public class Tests : BaseTestCase
    {
        const string MAIN_MENU_XPATH_PATTERN = "//span[text()='{0}']";
        const string MAIN_MENU_CHILD_XPATH_PATTERN = "//li[//span[text()='{0}']]//span[text()='{1}']";


        [Test]
        public void task11()
        {
            openMainPageLitecart();
            driver.FindElement(By.XPath("//a[contains(@href, 'create_account')]")).Click();

            var emailAddress = "email_" + getUnique() + "@gmail.com";
            var password = "newpassword";


            fillInfoCreateNewAccounts("taxId_" + getUnique(), "company_" + getUnique(), "firstname_" + getUnique(),
                "lastname_" + getUnique(), "address1_" + getUnique(), "address2_" + getUnique(),
                getNumber(5), "city_" + getUnique(), "United States", "Arizona", emailAddress, getNumber(5),
                password);

            logout();
            loginUser(emailAddress, password);
            logout();
        }


        [Test]
        public void task12()
        {
            loginAdmin();


            NewProduct newProduct = new NewProduct();
            newProduct.status = "1";
            newProduct.name = "newName_" + getUnique();
            newProduct.code = "newCode_" + getUnique();
            newProduct.product_group = "1";
            newProduct.quantity = "20";
            newProduct.filename = "flag.png";
            newProduct.date_from = DateTime.Now.ToString("MM/dd/yyyy");
            newProduct.date_to = DateTime.Now.AddDays(1).ToString("MM/dd/yyyy");
            newProduct.manufacturer_id = "1";
            newProduct.keywords = "keyword_" + getUnique();
            newProduct.short_description = "short_description_" + getUnique();
            newProduct.description = "description_" + getUnique();
            newProduct.head_title = "head_title_" + getUnique();
            newProduct.meta_description = "meta_description_" + getUnique();
            newProduct.purchase_price = "123";
            newProduct.purchase_price_currency_code = "USD";
            newProduct.gross_prices_USD = "22";
            newProduct.gross_prices_EUR = "22";


            createNewProduct(newProduct);


            var createdProduct = getNewProductByName(newProduct.name);
            Assert.True(newProduct.Equals(createdProduct), "Созданный продукт некорректен!");
        }


        [Test]
        public void task13()
        {
            //1 открыть главную страницу
            openMainPageLitecart();


            for (var i = 0; i < 3; i++)
            {
                var quanitityBefore = getQuantityItems();


                //2 открыть первый товар из списка
                driver.FindElement(By.XPath("//div[@id='box-most-popular']//li[contains(@class, 'product')][1]"))
                    .Click();
                //3 добавить его в корзину
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
                if (driver.FindElements(By.XPath("//select[@name='options[Size]']")).Count > 0)
                {
                    new SelectElement(driver.FindElement(By.XPath("//select[@name='options[Size]']")))
                        .SelectByValue("Small");
                }

                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                driver.FindElement(By.XPath("//button[@name='add_cart_product']")).Click();


                //4 подождать, пока счётчик товаров в корзине обновится
                new WebDriverWait(driver, TimeSpan.FromSeconds(5)).Until(
                    d => driver.FindElement(By.XPath($"//span[@class='quantity' and text()={quanitityBefore + 1}]"))
                        .Displayed);

                //5 вернуться на главную страницу, повторить предыдущие шаги ещё два раза,
                //чтобы в общей сложности в корзине было 3 единицы товара
                openMainPage();
            }


            //6 открыть корзину(в правом верхнем углу кликнуть по ссылке Checkout)
            driver.FindElement(By.XPath("//a[contains(text(), 'Checkout')]")).Click();


            //7 удалить все товары из корзины один за другим, после каждого удаления подождать, пока внизу обновится таблица
            if (driver.FindElements(By.XPath("//ul[@class='shortcuts']")).Count > 0)
            {
                driver.FindElement(By.XPath("//ul[@class='shortcuts']/li[1]")).Click();


                var count = driver.FindElements(By.XPath("//ul[@class='shortcuts']/li")).Count;
                Console.WriteLine("всего элементов в корзине: " + count);

//                var countInTable = getCountItemsInTable();


                for (var i = count; i >= 1; i--)
                {
                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[@name='remove_cart_item']")));


                    driver.FindElement(By.XPath(
                        $"//table[@class = 'dataTable rounded-corners']/tbody/tr[{i + 1}]/td[2][@class='item']"));


                    driver.FindElement(By.XPath("//button[@name='remove_cart_item']")).Click();
                    Console.WriteLine("удалили элемент номер: " + i);

                    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    wait.Until(ExpectedConditions.StalenessOf(driver.FindElement(By.XPath(
                        $"//table[@class = 'dataTable rounded-corners']/tbody/tr[{i + 1}]/td[2][@class='item']"))));


//                    new WebDriverWait(driver, TimeSpan.FromSeconds(10)).Until(
//                        d => getCountItemsInTable() == countInTable - i);
                }
            }
            else
            {
                Console.WriteLine("был один продукт в корзине");
                driver.FindElement(By.XPath("//button[@name='remove_cart_item']")).Click();
            }

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@id='checkout-cart-wrapper']//em")));
            Assert.True(isCartEmpty(), "Корзина не пустая!");
        }


        [Test]
        public void task14()
        {
            //1 зайти в админку
            loginAdmin();


            //2) открыть пункт меню Countries(или страницу http://localhost/litecart/admin/?app=countries&doc=countries)
            openMenuItem(new Tuple<Enum, Enum>(MENU_ENUM.COUNTRIES, null));


            //3) открыть на редактирование какую-нибудь страну или начать создание новой
            openEditCountry("Australia");


            //4)нажимаем на ссылки с иконкой, они открываются в новом окне.
            foreach (var index in new[] {2, 3, 6, 7, 8, 9, 10})
            {
                driver.FindElement(
                        By.XPath($"//tbody/tr[{index}]//a[@target='_blank']/i"))
                    .Click();

                switchToAnotherWindowHandleFromCurrent();

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
                wait.Until(ExpectedConditions.UrlContains("http"));

                switchToAnotherWindowHandleFromCurrent();
                closeOtherWindows();
            }
        }


        public void openEditCountry(string country)
        {
            driver.FindElement(By.XPath($"//form[@name='countries_form']//tbody/tr/td[5]/a[text()='{country}']"))
                .Click();
        }


        public int getCountItemsInTable()
        {
            int count = driver.FindElements(By.XPath("//table[@class = 'dataTable rounded-corners']/tbody/tr")).Count;
            int counter = 0;
            if (count > 0)
            {
                for (int i = 2; i <= count; i++)
                {
                    if (driver.FindElements(By.XPath(
                                $"//table[@class = 'dataTable rounded-corners']/tbody/tr[{i}]/td[2][@class='item']"))
                            .Count > 0)
                    {
                        counter++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return counter;
        }


        public bool isCartEmpty()
        {
            return driver.FindElements(
                           By.XPath(
                               "//div[@id='checkout-cart-wrapper']//em[text()='There are no items in your cart.']"))
                       .Count > 0;
        }


        public int getQuantityItems()
        {
            return int.Parse(driver.FindElement(By.XPath("//span[@class='quantity']")).GetAttribute("textContent"));
        }


        public void openMainPage()
        {
            driver.FindElement(By.XPath("//div[@id='logotype-wrapper']")).Click();
        }


        public NewProduct getNewProductByName(string nameProduct)
        {
            NewProduct newProduct = new NewProduct();

            newProduct.name = nameProduct;

            //general
            driver.FindElement(By.XPath($"//a[text()='{newProduct.name}']")).Click();

            newProduct.status = driver.FindElement(By.XPath("//input[@name='status' and @checked]"))
                .GetAttribute("value");
            newProduct.code = driver.FindElement(By.XPath("//input[@name='code']")).GetAttribute("value");
            newProduct.product_group = driver.FindElement(By.XPath("//input[@name='product_groups[]' and @checked]"))
                .GetAttribute("value").Replace("1-", "");
            newProduct.quantity = driver.FindElement(By.XPath("//input[@name='quantity']")).GetAttribute("value")
                .Replace("0.00", "");
            var df = driver.FindElement(By.XPath("//input[@name='date_valid_from']")).GetAttribute("value");
            newProduct.date_from = Convert.ToDateTime(df).ToString("MM.dd.yyyy", CultureInfo.InvariantCulture);
            var dt = driver.FindElement(By.XPath("//input[@name='date_valid_to']")).GetAttribute("value");
            newProduct.date_to = Convert.ToDateTime(dt).ToString("MM.dd.yyyy", CultureInfo.InvariantCulture);


            //information
            driver.FindElement(By.XPath("//a[contains(@href, 'tab-information')]")).Click();


            newProduct.manufacturer_id =
                new SelectElement(driver.FindElement(By.XPath("//select[@name='manufacturer_id']"))).SelectedOption
                    .GetAttribute("value");
            newProduct.keywords = driver.FindElement(By.XPath("//input[@name='keywords']")).GetAttribute("value");
            newProduct.short_description = driver.FindElement(By.XPath("//input[@name='short_description[en]']"))
                .GetAttribute("value");
            newProduct.description = driver.FindElement(By.XPath("//textarea[@name='description[en]']"))
                .GetAttribute("value");
            newProduct.head_title =
                driver.FindElement(By.XPath("//input[@name='head_title[en]']")).GetAttribute("value");
            newProduct.meta_description = driver.FindElement(By.XPath("//input[@name='meta_description[en]']"))
                .GetAttribute("value");


            //tab Prices
            driver.FindElement(By.XPath("//a[contains(@href, 'tab-prices')]")).Click();
            newProduct.purchase_price = driver.FindElement(By.XPath("//input[@name='purchase_price']"))
                .GetAttribute("value").Replace("0.00", "");
            newProduct.purchase_price_currency_code =
                new SelectElement(driver.FindElement(By.XPath("//select[@name='purchase_price_currency_code']")))
                    .SelectedOption.GetAttribute("value");
            newProduct.gross_prices_USD = driver.FindElement(By.XPath("//input[@name='prices[USD]']"))
                .GetAttribute("value").Replace("0.0000", "");
            newProduct.gross_prices_EUR = driver.FindElement(By.XPath("//input[@name='prices[EUR]']"))
                .GetAttribute("value").Replace("0.0000", "");
            return newProduct;
        }


        public void createNewProduct(NewProduct newProduct)
        {
            openMenuItem(new Tuple<Enum, Enum>(MENU_ENUM.CATALOG, null));
            driver.FindElement(By.XPath("//a[@class='button' and contains(@href, 'doc=edit_produc')]")).Click();


            //status
            driver.FindElement(By.XPath($"//input[@name='status' and @value='{newProduct.status}']")).Click();
            driver.FindElement(By.XPath("//input[@name='name[en]']")).SendKeys(newProduct.name);
            driver.FindElement(By.XPath("//input[@name='code']")).SendKeys(newProduct.code);
            driver.FindElement(By.XPath($"//input[@name='product_groups[]' and @value='1-{newProduct.product_group}']"))
                .Click();
            driver.FindElement(By.XPath("//input[@name='quantity']")).SendKeys(newProduct.quantity);


            string subfoldername = "..\\..\\litecart\\resources";
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, subfoldername, newProduct.filename);
            driver.FindElement(By.XPath("//input[@type='file' and @name='new_images[]']"))
                .SendKeys(Path.GetFullPath(path));
            driver.FindElement(By.XPath("//input[@name='date_valid_from']")).SendKeys(newProduct.date_from);
            driver.FindElement(By.XPath("//input[@name='date_valid_to']")).SendKeys(newProduct.date_to);


            //tab information
            driver.FindElement(By.XPath("//a[contains(@href, 'tab-information')]")).Click();

            new SelectElement(driver.FindElement(By.XPath("//select[@name='manufacturer_id']"))).SelectByValue(
                newProduct.manufacturer_id);
            driver.FindElement(By.XPath("//input[@name='keywords']")).SendKeys(newProduct.keywords);
            driver.FindElement(By.XPath("//input[@name='short_description[en]']"))
                .SendKeys(newProduct.short_description);
            driver.FindElement(By.XPath("//textarea[@name='description[en]']")).SendKeys(newProduct.description);
            driver.FindElement(By.XPath("//input[@name='head_title[en]']")).SendKeys(newProduct.head_title);
            driver.FindElement(By.XPath("//input[@name='meta_description[en]']")).SendKeys(newProduct.meta_description);


            //tab Prices
            driver.FindElement(By.XPath("//a[contains(@href, 'tab-prices')]")).Click();
            driver.FindElement(By.XPath("//input[@name='purchase_price']")).SendKeys(newProduct.purchase_price);
            new SelectElement(driver.FindElement(By.XPath("//select[@name='purchase_price_currency_code']")))
                .SelectByValue(newProduct.purchase_price_currency_code);
            driver.FindElement(By.XPath("//input[@name='gross_prices[USD]']")).SendKeys(newProduct.gross_prices_USD);
            driver.FindElement(By.XPath("//input[@name='gross_prices[EUR]']")).SendKeys(newProduct.gross_prices_EUR);


            //сохраняем товар
            driver.FindElement(By.XPath("//button[@type='submit' and @name='save']")).Click();
        }


        public void openMenuItem(Tuple<Enum, Enum> menuItem)
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
        }


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


        public void fillInfoCreateNewAccounts(string taxId, string company, string firstname, string lastname,
            string address1, string address2, string postcode, string city, string country, string zone, string email,
            string phone,
            string password)
        {
            driver.FindElement(By.XPath("//input[@name='tax_id']")).SendKeys(taxId);
            driver.FindElement(By.XPath("//input[@name='company']")).SendKeys(company);
            driver.FindElement(By.XPath("//input[@name='firstname']")).SendKeys(firstname);
            driver.FindElement(By.XPath("//input[@name='lastname']")).SendKeys(lastname);
            driver.FindElement(By.XPath("//input[@name='address1']")).SendKeys(address1);
            driver.FindElement(By.XPath("//input[@name='address2']")).SendKeys(address2);
            driver.FindElement(By.XPath("//input[@name='postcode']")).SendKeys(postcode);
            driver.FindElement(By.XPath("//input[@name='city']")).SendKeys(city);

            new SelectElement(driver.FindElement(By.XPath("//select[@name='country_code']"))).SelectByText(
                country);

            if (country.Equals("United States"))
            {
                new SelectElement(driver.FindElement(By.XPath("//select[@name='zone_code']"))).SelectByText(
                    zone);
            }

            driver.FindElement(By.XPath("//input[@name='email']")).SendKeys(email);
            driver.FindElement(By.XPath("//input[@name='phone']")).SendKeys(phone);
            driver.FindElement(By.XPath("//input[@name='password']")).SendKeys(password);
            driver.FindElement(By.XPath("//input[@name='confirmed_password']")).SendKeys(password);

            driver.FindElement(By.XPath("//button[@name='create_account']")).Click();
        }
    }
}