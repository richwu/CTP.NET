using System.Management;

namespace WinCtp
{
    public class HardwareInfo
    {
        private static string cpuid;

        public static string CupId
        {
            get { return cpuid ?? (cpuid = GetCpuId()); }
        }

        //取CPU编号
        private static string GetCpuId()
        {
            try
            {
                var mc = new ManagementClass("Win32_Processor");
                var moc = mc.GetInstances();
                string strCpuID = null;
                foreach (var mo in moc)
                {
                    strCpuID = mo.Properties["ProcessorId"].Value.ToString();
                    break;
                }
                return strCpuID;
            }
            catch
            {
                return string.Empty;
            }

        }
    }
}