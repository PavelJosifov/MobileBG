namespace MobileBG.Web.Tests
{
    using System;
    using System.Linq;
    using System.Security.Policy;
    using System.Threading;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
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
            this.browser.Manage().Window.Size = new System.Drawing.Size(1629, 850);
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
            var actualText = this.browser.FindElements(By.CssSelector("body > div > main > div > div > div > a > div > h5")).First();
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
            var actualText = this.browser.FindElements(By.CssSelector("body > div > main > div > div > div > a > div > h5")).First();
            Assert.NotEqual("Audi A3", actualText.Text);
        }

        [Fact]
        public void RegisterTest()
        {
            var random = new Random().Next(0,100000);
            string TestEmail = $"{random}test@test.test";
            this.browser.Navigate().GoToUrl(SiteUrl);
            Thread.Sleep(2000);
            this.browser.FindElement(By.CssSelector("#navbarsExample07 > ul.navbar-nav.me-auto > li:nth-child(1) > a")).Click();
            Thread.Sleep(500);
            this.browser.FindElement(By.CssSelector("#Input_Email")).Click();
            Thread.Sleep(500);
            this.browser.FindElement(By.CssSelector("#Input_Email")).SendKeys(TestEmail);
            Thread.Sleep(500);
            this.browser.FindElement(By.CssSelector("#Input_Password")).Click();
            Thread.Sleep(500);
            this.browser.FindElement(By.CssSelector("#Input_Password")).SendKeys("123456");
            Thread.Sleep(500);
            this.browser.FindElement(By.CssSelector("#Input_ConfirmPassword")).Click();
            Thread.Sleep(500);
            this.browser.FindElement(By.CssSelector("#Input_ConfirmPassword")).SendKeys("123456");
            Thread.Sleep(500);
            this.browser.FindElement(By.CssSelector("#registerSubmit")).Click();
            Thread.Sleep(500);
            var actualText = this.browser.FindElements(By.CssSelector("#navbarsExample07 > ul.navbar-nav.me-auto > li:nth-child(1) > a")).First();
            Assert.Equal($"Hello {TestEmail.Split("@")[0]}", actualText.Text);
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
