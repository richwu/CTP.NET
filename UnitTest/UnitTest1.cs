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
            Console.WriteLine((char)CtpOffsetFlagType.Open);
            Console.WriteLine((char)CtpOffsetFlagType.Close);
            Console.WriteLine((char)CtpOffsetFlagType.CloseToday);
            Console.WriteLine((char)CtpOffsetFlagType.CloseYesterday);
            Console.WriteLine((char)CtpOffsetFlagType.ForceClose);
            Console.WriteLine((char)CtpOffsetFlagType.ForceOff);
            Console.WriteLine((char)CtpOffsetFlagType.LocalForceClose);
        }
    }
}
