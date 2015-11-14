using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace GroundWellDesign
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Application Entry Point.
        /// </summary>
        /// 
        [System.STAThreadAttribute()]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public static void Main(string[] args)
        {
            GroundWellDesign.App app = new GroundWellDesign.App();
            SplashScreen s = new SplashScreen("img/splash.jpg");
            s.Show(false);
            s.Close(new TimeSpan(0, 0, 3));
            
            MainWindow mainwindow = new MainWindow();
            if (args.Length != 0)
              mainwindow.openFile(args[0]);
            app.initial();
            app.Run(mainwindow);
        }

        private void initial()
        {
            if (_contentLoaded)
            {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/GroundWellDesign;component/app.xaml", System.UriKind.Relative);
            System.Windows.Application.LoadComponent(this, resourceLocater);
        }

    }
}
