namespace MobileBG.Web.Tests
{
    using System;
    using System.Linq;
    using System.Threading;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using Xunit;

    public class SeleniumTests : IClassFixture<SeleniumServerFactory<Startup>>, IDisposable
    {
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
            this.browser.Navigate().GoToUrl("https://localhost:44319/");
            var numberOMakes = this.browser.FindElements(By.XPath("/html/body/div/main/div[2]/div/div[1]/div/span[1]")).First();
            Assert.Equal("71", numberOMakes.Text);
        }

        [Fact]
        public void Number_of_Cars_Test()
        {
            this.browser.Navigate().GoToUrl("https://localhost:44319/");
            var numberOfCars = this.browser.FindElements(By.CssSelector("body > div > main > div.container.align-items-center.mb-5 > div > div:nth-child(2) > div > span.count-numbers")).First();
            Assert.Equal("10", numberOfCars.Text);
        }

        [Fact]
        public void User_Number_Test()
        {
            this.browser.Navigate().GoToUrl("https://localhost:44319/");
            var numberOfUsers = this.browser.FindElements(By.CssSelector("body > div > main > div.container.align-items-center.mb-5 > div > div:nth-child(3) > div > span.count-numbers")).First();
            Assert.Equal("2", numberOfUsers.Text);
        }

        [Fact]
        public void All_Cars_Field_Click_Test()
        {
            this.browser.Navigate().GoToUrl("https://localhost:44319/");
            Thread.Sleep(5000);
            this.browser.FindElement(By.CssSelector("#navbarsExample07 > ul.navbar-nav.mr-auto > li:nth-child(1) > a")).Click();
            Thread.Sleep(2000);
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
