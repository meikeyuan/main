using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GroundWellDesign
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        private string filepath;
        public LoginWindow(string filepath)
        {
            InitializeComponent();
            this.filepath = filepath;
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            
            if (pwdBox.Password == "")
            {
                enterMainBoard(true);
            }
            else
            {
                MessageBox.Show("密码错误");
            }
        }


        private void anonBtn_Click(object sender, RoutedEventArgs e)
        {
            enterMainBoard(false);

        }
        private void minBtn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;

        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void enterMainBoard(bool blogin)
        {
            ContainerWindow container = new ContainerWindow();
            ContainerWindow.loginInfo.BLogin = blogin;
            container.openFile(filepath);
            container.Show();
            Close();
        }
    }
}
