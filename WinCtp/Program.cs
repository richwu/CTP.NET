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
            log.Error("current domain unhandled exception.", ex);
            //MsgBox.Error(ex.Message);
        }

        private static void ApplicationOnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            var log = LogManager.GetLogger("CTP");
            log.Error("application thread exception.", e.Exception);
            //MsgBox.Error(e.Exception.Message);
        }
    }
}
