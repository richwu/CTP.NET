using System;
using System.Globalization;
using GalaxyFutures.Sfit.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Console.WriteLine(Convert.ToInt32(DateTime.Now.ToString("ddHHmmssff")));
        }
    }
}
