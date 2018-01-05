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


        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            
            //if (userBox.Text == "mky" && pwdBox.Password == "888888")
            if(true)
            {
                enterMainBoard(true);
            }
            else
            {
                MessageBox.Show("用户名或密码错误!");
                pwdBox.Clear();
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
            ContainerWindow container = new ContainerWindow(blogin, filepath);
            container.Show();
            Close();
        }
    }
}
