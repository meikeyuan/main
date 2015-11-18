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
    /// ContainerWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ContainerWindow : Window
    {
        public List<MainWindow> windows = new List<MainWindow>();

        public ContainerWindow()
        {
            InitializeComponent();
        }

        private void DeleteSheetMenu_Click(object sender, RoutedEventArgs e)
        {
            int index = tabControl.SelectedIndex;
            tabControl.Items.RemoveAt(index);
            windows.RemoveAt(index);
        }


        //从文件恢复数据
        private void openFileBtn_Click(object sender, RoutedEventArgs e)
        {

            System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.Title = "打开文件";
            fileDialog.Filter = "数据文件(*.data)|*.data";

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filePath = fileDialog.FileName;
                if (!openFile(filePath))
                {
                    MessageBox.Show("打开文件错误");
                }
            }
        }


        public bool openFile(string filePath)
        {
            MainWindow mainwindow = new MainWindow();
            mainwindow.FilePath = filePath;
            //<TabItem Header="表格式岩层录入"  Style="{DynamicResource TabItemStyle}">
            TabItem tabitem = new TabItem();
           // tabitem.SetResourceReference(TabItem.StyleProperty, "TabItemStyle");
            if (filePath == null || filePath.Length == 0)
            {
                tabitem.Header = "新建文档";
            }
            else
            {
                if (!mainwindow.openFile())
                {
                    return false;
                }
                int index1 = filePath.LastIndexOf('\\');
                int index2 = filePath.LastIndexOf('.');
                tabitem.Header = filePath.Substring(index1 + 1, index2 - index1 - 1);
            }
            tabitem.Content = mainwindow.Content;
            windows.Add(mainwindow);
            tabControl.Items.Add(tabitem);
            tabControl.SelectedItem = tabitem;
            
            return true;
        }

        //另存为
        private void saveOthFileBtn_Click(object sender, RoutedEventArgs e)
        {
            int index = tabControl.SelectedIndex;
            if(index == -1)
            {
                return;
            }


            System.Windows.Forms.SaveFileDialog fileDialog = new System.Windows.Forms.SaveFileDialog();
            fileDialog.Title = "保存文件";
            fileDialog.Filter = "数据文件(*.data)|*.data";
            fileDialog.FileName = "岩层数据.data";

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filePath = fileDialog.FileName;
                windows[index].FilePath = filePath;
                if (windows[index].saveFile())
                {
                    MessageBox.Show("保存成功");
                }
                else
                {
                    MessageBox.Show("保存失败");
                }
            }

        }

        //保存数据到文件
        private void saveFileBtn_Click(object sender, RoutedEventArgs e)
        {
            int index = tabControl.SelectedIndex;
            if (index == -1)
            {
                return;
            }
            if (windows[index].FilePath == null)
            {
                MessageBox.Show("该文件不存在，请点击“另存为...”");
                return;
            }
            if (MessageBox.Show("确定覆盖旧文件吗？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
            if (windows[index].saveFile())
            {
                MessageBox.Show("保存成功");
            }
            else
            {
                MessageBox.Show("保存失败");
            }

        }



        private void saveAllFileBtn_Click(object sender, RoutedEventArgs e)
        {
            
            foreach(MainWindow window in windows)
            {
                tabControl.SelectedIndex = windows.IndexOf(window);
                if (window.FilePath == null)
                {
                    MessageBox.Show("该文件不存在，请点击“另存为...”");
                    continue;
                }
                if (MessageBox.Show("确定覆盖旧文件吗？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    continue;
                }
                if (window.saveFile())
                {
                    MessageBox.Show("保存成功");
                }
                else
                {
                    MessageBox.Show("保存失败");
                }
            }
        }




    }
}
