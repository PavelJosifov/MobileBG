namespace MobileBG.Web.Tests
{
    using System;
    using System.Linq;
    using System.Security.Policy;
    using System.Threading;
    using CloudinaryDotNet.Actions;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.DevTools.V104.Browser;
    using Xunit;

    public class SeleniumTests : IClassFixture<SeleniumServerFactory<Startup>>, IDisposable
    {
        private const string SiteUrl = "https://localhost:44319/";

        private readonly SeleniumServerFactory<Startup> server;
        private readonly IWebDriver browser;

        public SeleniumTests(SeleniumServerFactory<Startup> server)
        {
            this.server = server;
            server.CreateClient();
            var opts = new ChromeOptions();
            this.browser = new ChromeDriver(opts);
            this.browser.Manage().Window.Maximize();
            
        }

        [Fact]
        public void Number_of_Makes_Test()
        {
            this.browser.Navigate().GoToUrl(SiteUrl);
            var numberOMakes = this.browser.FindElements(By.XPath("/html/body/div/main/div[2]/div/div[1]/div/span[1]")).First();
            Assert.Equal("71", numberOMakes.Text);
        }

        [Fact]
        public void Number_of_Cars_Test()
        {
            this.browser.Navigate().GoToUrl(SiteUrl);
            var numberOfCars = this.browser.FindElements(By.CssSelector("body > div > main > div.container.align-items-center.mb-5 > div > div:nth-child(2) > div > span.count-numbers")).First();
            Assert.Equal("10", numberOfCars.Text);
        }

        [Fact]
        public void User_Number_Test()
        {
            this.browser.Navigate().GoToUrl(SiteUrl);
            var numberOfUsers = this.browser.FindElements(By.CssSelector("body > div > main > div.container.align-items-center.mb-5 > div > div:nth-child(3) > div > span.count-numbers")).First();
            Assert.Equal("2", numberOfUsers.Text);
        }

        [Fact]
        public void All_Cars_Field_Click_Test()
        {
            this.browser.Navigate().GoToUrl(SiteUrl);
            Thread.Sleep(3000);
            this.browser.FindElement(By.CssSelector("#navbarsExample07 > ul.navbar-nav.mr-auto > li:nth-child(1) > a")).Click();
            Thread.Sleep(2000);
        }

        [Fact]

        public void Search_Cars_Valid_Test()
        {
            this.browser.Navigate().GoToUrl(SiteUrl);
            Thread.Sleep(2000);
            this.browser.FindElement(By.CssSelector("#navbarsExample07 > ul.navbar-nav.mr-auto > li:nth-child(1) > a")).Click();
            Thread.Sleep(500);
            this.browser.FindElement(By.CssSelector("#navbarsExample07 > ul.navbar-nav.mr-auto > li:nth-child(1) > a")).Click();
            Thread.Sleep(500);
            this.browser.FindElement(By.CssSelector("#makes > option:nth-child(6)")).Click();
            Thread.Sleep(500);
            this.browser.FindElement(By.CssSelector("#models > option:nth-child(7)")).Click();
            Thread.Sleep(500);
            this.browser.FindElement(By.CssSelector("#search > div.btn-group > button")).Click();
            Thread.Sleep(500);
            var actualText = this.browser.FindElement(By.CssSelector("body > div > main > div > div > div > a > div > h5"));
            Assert.Equal("Audi A3", actualText.Text);
        }

        [Fact]
        public void Search_Cars_Invalid_Test()
        {
            this.browser.Navigate().GoToUrl(SiteUrl);
            Thread.Sleep(2000);
            this.browser.FindElement(By.CssSelector("#navbarsExample07 > ul.navbar-nav.mr-auto > li:nth-child(1) > a")).Click();
            Thread.Sleep(500);
            this.browser.FindElement(By.CssSelector("#navbarsExample07 > ul.navbar-nav.mr-auto > li:nth-child(1) > a")).Click();
            Thread.Sleep(500);
            this.browser.FindElement(By.CssSelector("#makes > option:nth-child(6)")).Click();
            Thread.Sleep(500);
            this.browser.FindElement(By.CssSelector("#models > option:nth-child(12)")).Click();
            Thread.Sleep(500);
            this.browser.FindElement(By.CssSelector("#search > div.btn-group > button")).Click();
            Thread.Sleep(500);
            var actualText = this.browser.FindElement(By.CssSelector("body > div > main > div > div > div > a > div > h5"));
            Assert.NotEqual("Audi A3", actualText.Text);
        }

        [Fact]
        public void Register_Test()
        {
            var random = new Random().Next(0,100000);
            string TestEmail = $"{random}test@test.test";
            this.browser.Navigate().GoToUrl(SiteUrl);
            Thread.Sleep(2000);
            this.browser.FindElement(By.CssSelector("#navbarsExample07 > ul.navbar-nav.me-auto > li:nth-child(1) > a")).Click();
            Thread.Sleep(500);
            this.browser.FindElement(By.CssSelector("#Input_Email")).SendKeys(TestEmail);
            Thread.Sleep(500);
            this.browser.FindElement(By.CssSelector("#Input_Password")).SendKeys("123456");
            Thread.Sleep(500);
            this.browser.FindElement(By.CssSelector("#Input_ConfirmPassword")).SendKeys("123456");
            Thread.Sleep(500);
            this.browser.FindElement(By.CssSelector("#registerSubmit")).Click();
            Thread.Sleep(500);
            var actualText = this.browser.FindElement(By.CssSelector("#navbarsExample07 > ul.navbar-nav.me-auto > li:nth-child(1) > a"));
            Assert.Equal($"Hello {TestEmail.Split("@")[0]}", actualText.Text);
        }

        [Fact]
        public void Login_Test()
        {
            this.browser.Navigate().GoToUrl(SiteUrl);
            Thread.Sleep(2000);
            this.browser.FindElement(By.CssSelector("#navbarsExample07 > ul.navbar-nav.me-auto > li:nth-child(2) > a")).Click();
            Thread.Sleep(500);
            this.browser.FindElement(By.CssSelector("#Input_Email")).SendKeys("simeon99@abv.bg");
            Thread.Sleep(500);
            this.browser.FindElement(By.CssSelector("#Input_Password")).SendKeys("123456");
            Thread.Sleep(500);
            this.browser.FindElement(By.CssSelector("#login-submit")).Click();
            var actualText = this.browser.FindElement(By.CssSelector("#navbarsExample07 > ul.navbar-nav.me-auto > li:nth-child(1) > a"));
            Assert.Equal("Hello simeon99", actualText.Text);
        }

        [Fact]

        public void Car_Creation_Test()
        {
            this.UserLogin();
            Thread.Sleep(500);
            this.CarCreation();
            Thread.Sleep(500);
            this.LogOut();
            Thread.Sleep(500);
            this.AdminLogin();
            Thread.Sleep(500);
            this.CarApprove();
            Thread.Sleep(500);
            this.LogOut();
            Thread.Sleep(500);
            this.UserLogin();
            Thread.Sleep(500);
            this.browser.FindElement(By.CssSelector("#navbarsExample07 > ul.navbar-nav.mr-auto > li:nth-child(3) > a")).Click();
            this.browser.FindElement(By.CssSelector("body > div > main > nav > ul > li:nth-child(4) > a")).Click();
            this.browser.FindElement(By.CssSelector("body > div > main > div > div > div:nth-child(2) > a > img")).Click();
            var actualprice = this.browser.FindElement(By.XPath("/html/body/div/main/div/div/div[2]/ul/li[1]/span"));
            var actualhorsepower = this.browser.FindElement(By.CssSelector("body > div > main > div > div > div:nth-child(5) > ul > li:nth-child(2)"));
            var actualyear = this.browser.FindElement(By.CssSelector("body > div > main > div > div > div:nth-child(5) > ul > li:nth-child(5)"));
            Assert.Equal("21000.00 Lv.", actualprice.Text);
            Assert.Equal("Horse power: 205", actualhorsepower.Text);
            Assert.Equal("Year made: 2005", actualyear.Text);
            this.CarDelete();
            Thread.Sleep(500);
         

        }

        private void CarDelete()
        {
            this.browser.FindElement(By.CssSelector("#navbarsExample07 > ul.navbar-nav.mr-auto > li:nth-child(3) > a")).Click();
            this.browser.FindElement(By.CssSelector("body > div > main > nav > ul > li:nth-child(4) > a")).Click();
            this.browser.FindElement(By.CssSelector("body > div > main > div > div > div:nth-child(2) > a > img")).Click();
            this.browser.FindElement(By.CssSelector("body > div > main > div > div > div:nth-child(5) > div > button.btn.btn-danger.btn-sm")).Click();
        }

        private void CarApprove()
        {
            this.browser.FindElement(By.CssSelector("#dropdown07")).Click();
            this.browser.FindElement(By.CssSelector("#navbarsExample07 > ul.navbar-nav.mr-auto > li.nav-item.dropdown.show > div > a:nth-child(1)")).Click();
            this.browser.FindElement(By.CssSelector("body > div > main > div.container > div > div > div.card-footer > button.btn.btn-success.btn-sm.m-1")).Click();
        }

        private void LogOut()
        {
            this.browser.FindElement(By.CssSelector("#navbarsExample07 > ul.navbar-nav.me-auto > li:nth-child(2) > form > button")).Click();
        }

        private void CarCreation()
        {
            this.browser.FindElement(By.CssSelector("#navbarsExample07 > ul.navbar-nav.mr-auto > li:nth-child(2) > a")).Click();
            this.browser.FindElement(By.CssSelector("#makes")).Click();
            this.browser.FindElement(By.CssSelector("#makes > option:nth-child(9)")).Click();
            this.browser.FindElement(By.CssSelector("#models")).Click();
            this.browser.FindElement(By.CssSelector("#models > option:nth-child(2)")).Click();
            this.browser.FindElement(By.CssSelector("#PetrolTypeId")).Click();
            this.browser.FindElement(By.CssSelector("#PetrolTypeId > option:nth-child(3)")).Click();
            this.browser.FindElement(By.CssSelector("#CityId")).Click();
            this.browser.FindElement(By.CssSelector("#CityId > option:nth-child(11)")).Click();
            this.browser.FindElement(By.CssSelector("#YearMade")).Clear();
            this.browser.FindElement(By.CssSelector("#YearMade")).SendKeys("2005");
            this.browser.FindElement(By.CssSelector("#Km")).Clear();
            this.browser.FindElement(By.CssSelector("#Km")).SendKeys("20005");
            this.browser.FindElement(By.CssSelector("#HorsePower")).Clear();
            this.browser.FindElement(By.CssSelector("#HorsePower")).SendKeys("205");
            this.browser.FindElement(By.CssSelector("#Price")).Clear();
            this.browser.FindElement(By.CssSelector("#Price")).SendKeys("21000");
            this.browser.FindElement(By.CssSelector("body > div > main > div > div > div > form > div:nth-child(11) > input")).SendKeys(@"C:\Users\pavel\Desktop\1.jpg");
            this.browser.FindElement(By.XPath("/html/body/div/main/div/div/div/form/button")).Click();
        }

        private void UserLogin()
        {
            this.browser.Navigate().GoToUrl(SiteUrl);
            Thread.Sleep(2000);
            this.browser.FindElement(By.CssSelector("#navbarsExample07 > ul.navbar-nav.me-auto > li:nth-child(2) > a")).Click();
            Thread.Sleep(500);
            this.browser.FindElement(By.CssSelector("#Input_Email")).SendKeys("simeon99@abv.bg");
            Thread.Sleep(500);
            this.browser.FindElement(By.CssSelector("#Input_Password")).SendKeys("123456");
            Thread.Sleep(500);
            this.browser.FindElement(By.CssSelector("#login-submit")).Click();
        }

        private void AdminLogin()
        {
            this.browser.Navigate().GoToUrl(SiteUrl);
            Thread.Sleep(2000);
            this.browser.FindElement(By.CssSelector("#navbarsExample07 > ul.navbar-nav.me-auto > li:nth-child(2) > a")).Click();
            Thread.Sleep(500);
            this.browser.FindElement(By.CssSelector("#Input_Email")).SendKeys("admin@admin.com");
            Thread.Sleep(500);
            this.browser.FindElement(By.CssSelector("#Input_Password")).SendKeys("123456");
            Thread.Sleep(500);
            this.browser.FindElement(By.CssSelector("#login-submit")).Click();
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.server?.Dispose();
                this.browser?.Dispose();
            }
        }
    }
}
