using System;
using System.Globalization;
using System.IO;
using System.Management;
using System.Security.Cryptography;
using System.Text;
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
            var cpuid = "BFEBFBFF000306A9";
            byte[] data = Encoding.ASCII.GetBytes("BFEBFBFF000306A9 20180901");
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = Encoding.ASCII.GetBytes("BFEBFBFF");
            DES.IV = Encoding.ASCII.GetBytes("000306A9");
            MemoryStream ms = new MemoryStream();  //创建其支持存储区为内存的流。
            
            CryptoStream cs = new CryptoStream(ms, DES.CreateEncryptor(), CryptoStreamMode.Write);//将数据流连接到加密转换流
            cs.Write(data, 0, data.Length);
            cs.FlushFinalBlock();  //用缓冲区的当前状态更新基础数据源或储存库，随后清除缓

            var buf = ms.ToArray();
            File.WriteAllBytes(@"E:\github\CTP.NET\BFEBFBFF000306A9", buf);
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
