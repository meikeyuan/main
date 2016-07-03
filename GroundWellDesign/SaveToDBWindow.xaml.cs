using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace GroundWellDesign
{
    /// <summary>
    /// SaveToDBWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SaveToDBWindow : Window
    {
        LayerParams layer;
        List<String> items = new List<string>();
        string cus = "其他(自定义)";
        public SaveToDBWindow(LayerParams layer)
        {
            InitializeComponent();
            this.layer = layer;

            DataSaveAndRestore.getAllWellName(items);

            items.Add(cus);
            comboBox.ItemsSource = items;
            if(items.Count > 0)
                comboBox.SelectedIndex = 0;
        }

        private void saveOkBtn_Click(object sender, RoutedEventArgs e)
        {
            string wellName = comboBox.SelectedValue.ToString();
            //如果选择的是其他。。。
            if(wellName.Equals(cus))
            {
                wellName = textBox.Text;
                if(wellName.Equals(""))
                {
                    MessageBox.Show("请输入矿井名称");
                    return;
                }else if(items.Contains(wellName))
                {
                    MessageBox.Show("您输入的矿井名称已存在，请在下拉列表选择");
                    return;
                }
            }


            string uuid = Guid.NewGuid().ToString();
            bool success = DataSaveAndRestore.saveToSqlite(layer, uuid, false, wellName, items.Contains(wellName));

            if (success)
            {
                layer.dataBaseKey = uuid;
                layer.wellNamePK = wellName;
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
            if (comboBox.SelectedValue.Equals(cus))
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
