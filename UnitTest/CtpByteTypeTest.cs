using System;
using GalaxyFutures.Sfit.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class CtpByteTypeTest
    {
        [TestMethod]
        public void CtpDirectionTypeTest()
        {
            Console.WriteLine("CtpDirectionType");
            ShowValue(CtpDirectionType.Buy, "Buy");
            ShowValue(CtpDirectionType.Sell, "Sell");
        }

        [TestMethod]
        public void CtpOffsetFlagTypeTest()
        {
            Console.WriteLine("CtpOffsetFlagType");
            ShowValue(CtpOffsetFlagType.Open, "Open");
            ShowValue(CtpOffsetFlagType.Close, "Close");
            ShowValue(CtpOffsetFlagType.ForceClose, "ForceClose");
            ShowValue(CtpOffsetFlagType.CloseToday, "CloseToday");
            ShowValue(CtpOffsetFlagType.CloseYesterday, "CloseYesterday");
            ShowValue(CtpOffsetFlagType.ForceOff, "ForceOff");
            ShowValue(CtpOffsetFlagType.LocalForceClose, "LocalForceClose");
        }

        [TestMethod]
        public void CtpHedgeFlagTypeTest()
        {
            Console.WriteLine("CtpHedgeFlagType");
            ShowValue(CtpHedgeFlagType.Speculation, "Speculation");//投机买卖
            ShowValue(CtpHedgeFlagType.Arbitrage, "Arbitrage");//套利
            ShowValue(CtpHedgeFlagType.Hedge, "Hedge");//防止损失
        }

        [TestMethod]
        public void CtpOrderStatusTypeTest()
        {
            Console.WriteLine("CtpOrderStatusType");
            ShowValue(CtpOrderStatusType.AllTraded, "AllTraded");
            ShowValue(CtpOrderStatusType.PartTradedQueueing, "PartTradedQueueing");
            ShowValue(CtpOrderStatusType.PartTradedNotQueueing, "PartTradedNotQueueing");
            ShowValue(CtpOrderStatusType.NoTradeQueueing, "NoTradeQueueing");
            ShowValue(CtpOrderStatusType.NoTradeNotQueueing, "NoTradeNotQueueing");
            ShowValue(CtpOrderStatusType.Canceled, "Canceled");
            ShowValue(CtpOrderStatusType.Unknown, "Unknown");
            ShowValue(CtpOrderStatusType.NotTouched, "NotTouched");
            ShowValue(CtpOrderStatusType.Touched, "Touched");
        }

        [TestMethod]
        public void CtpGenderTypeTest()
        {
            Console.WriteLine("CtpGenderType");
            ShowValue(CtpGenderType.Unknown, "Unknown");
            ShowValue(CtpGenderType.Male, "Male");
            ShowValue(CtpGenderType.Female, "Female");
        }

        [TestMethod]
        public void CtpActionFlagTypeTest()
        {
            Console.WriteLine("CtpActionFlagType");
            ShowValue(CtpActionFlagType.Delete, "Delete");
            ShowValue(CtpActionFlagType.Modify, "Modify");
        }

        private static void ShowValue(byte bt, string typeName)
        {
            Console.WriteLine($"{typeName} -> [{bt}][{(char)bt}]");
        }
    }
}