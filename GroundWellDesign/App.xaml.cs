using mky;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GroundWellDesign
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // 加载公式接口，因为是耗时操作。
            Document.logic = new MkyLogic();

            string filePath = null;
            if (e.Args.Length != 0)
            {
                filePath = e.Args[0];
            }
            LoginWindow loginWin = new LoginWindow(filePath);
            loginWin.Show();
        }
    }
}
