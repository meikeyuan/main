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
        public LoginWindow()
        {
            
            InitializeComponent();
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            
            if (pwdBox.Password == "123456")
            {
                ContainerWindow container = new ContainerWindow();
                ContainerWindow.loginInfo.BLogin = true;
                container.Show();
                Close();
            }else if(pwdBox.Password == ""){
                ContainerWindow container = new ContainerWindow();
                ContainerWindow.loginInfo.BLogin = false;
                container.Show();
                Close();
            }
            else
            {
                MessageBox.Show("密码错误");
            }
        }

        private void minBtn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;

        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
