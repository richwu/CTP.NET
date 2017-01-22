using System;
using System.Globalization;
using System.Management;
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
        public void GetDiskVolumeSerialNumberTest()
        {
            var managementObject = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
            managementObject.Get();
            var s = managementObject.GetPropertyValue("VolumeSerialNumber").ToString();
            Console.WriteLine(s);
        }

        [TestMethod]
        public void PublicChangePswTest()
        {
            var alg = new AlgClass();
            Console.WriteLine(alg.Public_Change_Psw("123a"));
        }

        [TestMethod]
        public void PswChangePublicTest()
        {
            var alg = new AlgClass();
            Console.WriteLine(alg.Psw_Change_Public("·----··---···----···-····-····-······---"));
        }
    }
}
