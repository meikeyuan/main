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
    /// SaveToDBWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SaveToDBWindow : Window
    {
        public SaveToDBWindow()
        {
            InitializeComponent();
            comboBox.ItemsSource = new List<String> {"#1", "#2", "其他" };
            comboBox.SelectedIndex = 0;
        }

        private void saveOkBtn_Click(object sender, RoutedEventArgs e)
        {
            //save to sqlite...
            bool bSuccess = false;

            if (bSuccess)
            {
                MessageBox.Show("保存成功");
            }
            else
            {
                MessageBox.Show("保存失败");
            }
            
            Close();
        }

        private void saveCancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox.SelectedValue.Equals("其他"))
            {
                textBox.IsEnabled = true;
            }
            else
            {
                textBox.IsEnabled = false;
            }

        }
    }
}
