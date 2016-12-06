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

        [TestMethod]
        public void TestMethod2()
        {
            Console.WriteLine((char)CtpOffsetFlagType.Open);
        }

        [TestMethod]
        public void TestMethod3()
        {
            //"ConfirmDate":"20161205","ConfirmTime":"15:22:09"
            var dt = "2016120515:22:09";
            DateTime d;
            if (!DateTime.TryParseExact(dt, "yyyyMMddHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
                Console.WriteLine("null");
            else Console.WriteLine(d);
        }
    }
}
