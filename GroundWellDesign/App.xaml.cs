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
            ContainerWindow container = new ContainerWindow();

            string filePath = null;
            if (args.Length != 0)
            {
                filePath = args[0];
            }
            if(!container.openFile(filePath))
            {
                MessageBox.Show("打开文件错误");
                return;
            }
            app.initial();
            s.Close(new TimeSpan(0, 0, 0));
            app.Run(container);
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
