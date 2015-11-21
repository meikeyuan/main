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
        ContainerWindow con;
        public LoginWindow(ContainerWindow container)
        {
            InitializeComponent();
            con = container;
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();

        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (pwdBox.Password == "123456")
            {
                ContainerWindow.loginInfo.BLogin = true;
                Close();
            }
            else
            {
                MessageBox.Show("密码错误");
            }
        }
    }
}
