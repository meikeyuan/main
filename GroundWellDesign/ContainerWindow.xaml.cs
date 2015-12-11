using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace GroundWellDesign
{


    public class LoginInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _BLogin = true;
        public bool BLogin
        {
            get
            {
                return _BLogin;
            }
            set
            {
                _BLogin = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("BLogin"));
                }

            }
        }
    }

    public class BoolToTextConvert : IValueConverter
    {

        public object Convert(object value, Type targetType, object param, CultureInfo c)
        {
            bool b = (bool)value;
            if (b)
            {
                return "已登录";
            }
            else
            {
                return "未登录";
            }
        }

        public object ConvertBack(object value, Type targetType, object param, CultureInfo c)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// ContainerWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ContainerWindow : Window
    {
        public List<Document> windows = new List<Document>();

        public static LoginInfo loginInfo = new LoginInfo();
        

        public ContainerWindow()
        {
            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            Application.Current.Shutdown();
            base.OnClosed(e);
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
            Document document = new Document(filePath);
            TabItem tabitem = new TabItem();
            tabitem.ContextMenu= tabControl.Resources["menu"] as ContextMenu;
            if (filePath == null)
            {
                tabitem.Header = "新建文档";
            }
            else
            {
                if (!document.openFile())
                {
                    return false;
                }
                int index1 = filePath.LastIndexOf('\\');
                int index2 = filePath.LastIndexOf('.');
                tabitem.Header = filePath.Substring(index1 + 1, index2 - index1 - 1);
            }
            tabitem.Content = document.Content;
            windows.Add(document);
            tabControl.Items.Add(tabitem);
            tabControl.SelectedItem = tabitem;

            return true;
        }

        //另存为
        private void saveOthFileBtn_Click(object sender, RoutedEventArgs e)
        {
            int index = tabControl.SelectedIndex;
            if (index == -1)
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

            foreach (Document window in windows)
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


        private void closeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            UIElement ui = ContextMenuService.GetPlacementTarget(LogicalTreeHelper.GetParent(sender as MenuItem));
            if (ui is TabItem)
            {
                int index = tabControl.Items.IndexOf(ui);
                tabControl.Items.RemoveAt(index);
                windows.RemoveAt(index);
            }
        }


        private void themeCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Uri skinDictUri = null;
            switch (themeCB.SelectedIndex)
            {
                case 0:
                    skinDictUri = new Uri("Styles/ExpressionDark.xaml", UriKind.Relative);
                    break;
                case 1:
                    skinDictUri = new Uri("Styles/ExpressionLight.xaml", UriKind.Relative);
                    break;
                case 2:
                    skinDictUri = new Uri("Styles/ShinyBlue.xaml", UriKind.Relative);
                    break;
                case 3:
                    skinDictUri = new Uri("Styles/ShinyRed.xaml", UriKind.Relative);
                    break;
                default:
                    break;

            }
            if (skinDictUri == null)
                return;
            ResourceDictionary skinDict = Application.LoadComponent(skinDictUri) as ResourceDictionary;
            Collection<ResourceDictionary> mergedDicts = Application.Current.Resources.MergedDictionaries;
            if (mergedDicts.Count > 0)
                mergedDicts.Clear();
            mergedDicts.Add(skinDict);
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
                foreach (Document window in windows)
                {
                    
                    if(window.tabControl.Items.Contains(window.guidinputTabItem)){
                        window.tabControl.Items.RemoveAt(0);
                        window.tabControl.Items.Insert(0, window.gridinputTabItem);
                        window.tabControl.SelectedItem = window.gridinputTabItem;
                    }
                    else{
                        window.tabControl.Items.RemoveAt(0);
                        window.tabControl.Items.Insert(0, window.guidinputTabItem);
                        window.tabControl.SelectedItem = window.guidinputTabItem;
                    }
                }
        }
    }
}
