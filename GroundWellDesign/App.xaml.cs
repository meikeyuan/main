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
           
           
            string filePath = null;
            if (args.Length != 0)
            {
                filePath = args[0];
            }

            LoginWindow loginWin = new LoginWindow(filePath);

            app.initial();
            app.Run(loginWin);
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
