using log4net;
using log4net.Config;
using Mky;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace GroundWellDesign
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static ILog logger;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // 加载公式接口，因为是耗时操作。
            Document.logic = new MkyLogic();

            // 初始化日志组件。
            InitLog4Net();
            logger = LogManager.GetLogger(typeof(App));

            string filePath = null;
            if (e.Args.Length != 0)
            {
                filePath = e.Args[0];
            }
            LoginWindow loginWin = new LoginWindow(filePath);
            loginWin.Show();
        }

        private void InitLog4Net()
        {
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.ConfigureAndWatch(logCfg);
        }
    }
}
