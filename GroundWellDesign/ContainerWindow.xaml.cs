using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
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



    public partial class ContainerWindow : Window
    {
        public List<Document> windows = new List<Document>();

        public static LoginInfo loginInfo = new LoginInfo();
        

        public ContainerWindow(bool bLogin, string filepath)
        {
            InitializeComponent();
            loginInfo.BLogin = bLogin;
            openFile(filepath);
        }

        protected override void OnClosed(EventArgs e)
        {
            Application.Current.Shutdown();
            base.OnClosed(e);
        }

        //新建文件
        private void newFileBtn_Click(object sender, RoutedEventArgs e)
        {
            openFile(null);
        }


        //打开文件
        private void openFileBtn_Click(object sender, RoutedEventArgs e)
        {

            System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.Title = "打开文件";
            fileDialog.Filter = "数据文件(*.data)|*.data";

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filePath = fileDialog.FileName;
                foreach(Document document in windows)
                {
                    if(document.FilePath == filePath)
                    {
                        MessageBox.Show("该文件已经打开！");
                        return;
                    }
                }
                if (!openFile(filePath))
                {
                    MessageBox.Show("打开文件失败！");
                }
            }
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
                string oldPath = windows[index].FilePath;
                windows[index].FilePath = filePath;
                if (windows[index].saveFile())
                {
                    MessageBox.Show("保存成功");
                    var tabitem = tabControl.Items.GetItemAt(index) as TabItem;
                    tabitem.Header = Path.GetFileNameWithoutExtension(filePath);

                }
                else
                {
                    MessageBox.Show("保存失败");
                    windows[index].FilePath = oldPath;
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
            saveFile(windows[index]);

        }

        //保存所有文件
        private void saveAllFileBtn_Click(object sender, RoutedEventArgs e)
        {

            foreach (Document window in windows)
            {
                tabControl.SelectedIndex = windows.IndexOf(window);
                saveFile(window);
            }
        }

        //打开文件
        private bool openFile(string filePath)
        {
            Document document = new Document(filePath);
            if (!document.openFile())
            {
                return false;
            }


            TabItem tabitem = new TabItem();
            tabitem.ContextMenu = tabControl.Resources["menu"] as ContextMenu;
            if (filePath == null)
            {
                tabitem.Header = "新建文档";
            }
            else
            {
                tabitem.Header = Path.GetFileNameWithoutExtension(filePath);
            }
            tabitem.Content = document.Content;
            windows.Add(document);
            tabControl.Items.Add(tabitem);
            tabControl.SelectedItem = tabitem;

            return true;
        }

        //保存文件
        private void saveFile(Document window)
        {
            if (window.FilePath == null)
            {
                MessageBox.Show("该文件不存在，请点击“另存为”");
                return;
            }
            if (MessageBox.Show("确定覆盖旧文件吗？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
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



        //关闭选项卡
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

        //切换主题
        private void themeCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] themes = { "Colors_Blue.xaml", "Colors_Neutral.xaml", "Colors_Ocean.xaml", "Colors_Violet.xaml" };
            Uri skinDictUri = new Uri(".\\Colors\\" + themes[styleComBox.SelectedIndex], UriKind.Relative);
            if (skinDictUri == null)
                return;
            ResourceDictionary skinDict = Application.LoadComponent(skinDictUri) as ResourceDictionary;
            Collection<ResourceDictionary> mergedDicts = Application.Current.Resources.MergedDictionaries;
            if (mergedDicts.Count > 1)
                mergedDicts.RemoveAt(1);
            mergedDicts.Add(skinDict);
        }

        //切换录入方式
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
