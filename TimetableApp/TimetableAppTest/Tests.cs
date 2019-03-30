using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace TimetableAppTest
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public void WelcomeTextIsDisplayed()
        {
            AppResult[] results = app.WaitForElement(c => c.Marked("Welcome to Xamarin.Forms!"));
            app.Screenshot("Welcome screen.");
            
            Assert.IsTrue(results.Any());
        }


        //TEST IF WIFI OR DATA TURNED OFF WHEN SEARCH, ERROR MESSAGE "CONNECTIVITY ERROR, NO INTERNET ACCESS"
        [Test]
        public void Test1()     
        {
            AppResult[] results = app.WaitForElement(c => c.Marked("Welcome to Xamarin.Forms!"));
            app.Screenshot("Welcome screen.");

            Assert.IsTrue(results.Any());
        }


        //TEST IF ROOM NUMBER SEARCHED FOR THAT IS NOT IN DB, ERROR MESSSAGE "ROOM NOT FOUND" APPEARS
        [Test]
        public void Test2()     
        {
            AppResult[] results = app.WaitForElement(c => c.Marked("Welcome to Xamarin.Forms!"));
            app.Screenshot("Welcome screen.");

            Assert.IsTrue(results.Any());
        }

    }
}
