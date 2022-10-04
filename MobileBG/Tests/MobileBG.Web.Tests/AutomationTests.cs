namespace MobileBG.Web.Tests
{
    using NUnit.Framework;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;

    public class AutomationTests
    {
        IWebDriver driver;

        [SetUp]
        public void Setup()
        {
         driver = new ChromeDriver();

        }

        [Test]
        public void TestForLogin()
        { 
        
            //driver .Navigate() .GoToUrl: ("https://localhost:44319/");

            //driver.FindElement(By ID)
        }

        [TearDown]

        public void ShutDown()

        { 
        
        }


    }
}
