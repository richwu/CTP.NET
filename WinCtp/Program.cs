using System;
using System.Threading;
using System.Windows.Forms;
using log4net;

namespace WinCtp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
            Application.ThreadException += ApplicationOnThreadException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }

        private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            if (ex == null)
                return;
            var log = LogManager.GetLogger("CTP");
            while (ex != null)
            {
                log.Error("unhandled exception.", ex);
                ex = ex.InnerException;
            }
        }

        private static void ApplicationOnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            var log = LogManager.GetLogger("CTP");
            var ex = e.Exception;
            while (ex != null)
            {
                log.Error("unhandled exception.", ex);
                ex = ex.InnerException;
            }
        }
    }
}
