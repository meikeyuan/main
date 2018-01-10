using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace GroundWellDesign
{
    #region LoginStatus
    // 登录状态相关
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
    
    // 登录状态相关
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

    #endregion



    public partial class ContainerWindow : Window
    {
        public List<Document> windows = new List<Document>();
        public static LoginInfo loginInfo = new LoginInfo();
        private int newFileCount = 1;

        // 岩层数据库相关
        private static string DATABASE_DIR = "c:\\ProgramData\\GroundWellDesign";
        public static string DATABASE_PATH = DATABASE_DIR + "\\yanceng.db";
        

        public ContainerWindow(bool bLogin, string filepath)
        {
            InitializeComponent();
            loginInfo.BLogin = bLogin;
            if(filepath != null && !filepath.Equals(""))
                openFileHelper(filepath);

            // 创建岩层数据库目录
            Directory.CreateDirectory(DATABASE_DIR);
            string dbPath = "Data Source =" + DATABASE_PATH;
            SQLiteConnection conn = new SQLiteConnection(dbPath);
            conn.Open();

            // 创建矿井表和岩层表  1对多关系
            string sql2 = "CREATE TABLE IF NOT EXISTS yanceng(" +
                         "id char(36) primary key, wellName text references well(wellName), " +
                         "yanXing text, leiJiShenDu double, juLiMeiShenDu double, cengHou double, ziRanMiDu double, " +
                         "bianXingMoLiang double, kangLaQiangDu double, kangYaQiangDu double, tanXingMoLiang double, boSonBi double, " +
                         "neiMoCaJiao double, nianJuLi double, f double, q0 double, q1 double, q2 double, miaoShu Text" +
                         ");";

            string sql1 = "CREATE TABLE IF NOT EXISTS well(" +
                         "wellName text primary key" +
                         ");";

            SQLiteCommand cmdCreateTable = new SQLiteCommand(conn);
            cmdCreateTable.CommandText = sql1;
            cmdCreateTable.ExecuteNonQuery();

            cmdCreateTable.CommandText = sql2;
            cmdCreateTable.ExecuteNonQuery();

            conn.Close();

        }

        protected override void OnClosed(EventArgs e)
        {
            Environment.Exit(0);
            base.OnClosed(e);
        }


        #region FileMenu

        //新建文件菜单
        private void newFileMenu_Click(object sender, RoutedEventArgs e)
        {
            // 打开空文件。
            openFileHelper();
        }

        //打开文件菜单
        private void openFileMenu_Click(object sender, RoutedEventArgs e)
        {
            string filePath = FileDialogHelper.getOpenPath("数据文件(*.data)|*.data");
            if (filePath != null)
            {
                foreach(Document document in windows)
                {
                    if(document.FilePath == filePath)
                    {
                        MessageBox.Show("该文件已经打开。");
                        return;
                    }
                }
                if (!openFileHelper(filePath))
                {
                    MessageBox.Show("打开文件失败。");
                }
            }
        }

        //另存为菜单
        private void saveOthFileMenu_Click(object sender, RoutedEventArgs e)
        {
            int index = tabControl.SelectedIndex;
            if (index == -1)
            {
                return;
            }

            var tabitem = tabControl.Items.GetItemAt(index) as TabItem;
            string defaultName = ((ContentControl)tabitem.Header).Content + ".data";
            string filePath = FileDialogHelper.getSavePath(defaultName, "数据文件(*.data)|*.data");
            if(filePath == null)
                return;

            Document window = windows[index];
            if(!window.saveFile(filePath))
            {
                MessageBox.Show("保存失败。");
                return;
            }
            if(window.FilePath == null)
            {
                window.FilePath = filePath;
                ((ContentControl)tabitem.Header).Content = Path.GetFileNameWithoutExtension(filePath);
            }
        }

        //保存到文件菜单
        private void saveFileMenu_Click(object sender, RoutedEventArgs e)
        {
            int index = tabControl.SelectedIndex;
            if (index == -1)
            {
                return;
            }

            if(!saveFileHelper(index))
            {
                MessageBox.Show("保存失败。");
            }
        }

        //保存所有文件菜单
        private void saveAllFileMenu_Click(object sender, RoutedEventArgs e)
        {

            foreach (Document window in windows)
            {
                int index = windows.IndexOf(window);
                tabControl.SelectedIndex = index;

                if (!saveFileHelper(index))
                {
                    MessageBox.Show("保存失败。");
                }
            }
            MessageBox.Show("操作完成。");
        }



        //打开文件工具方法
        private bool openFileHelper(string filePath = null)
        {
            Document document = new Document();
            if (!document.openFile(filePath))
            {
                return false;
            }


            TabItem tabitem = new TabItem();
            ContentControl contentCtl = new ContentControl();
            contentCtl.ContextMenu = tabControl.Resources["menu"] as ContextMenu;
            if (filePath == null)
            {
                contentCtl.Content = "新建文档" + newFileCount++;
            }
            else
            {
                contentCtl.Content = Path.GetFileNameWithoutExtension(filePath);
            }
            tabitem.Header = contentCtl;
            tabitem.Content = document.Content;

            windows.Add(document);
            tabControl.Items.Add(tabitem);
            tabControl.SelectedItem = tabitem;

            return true;
        }

        private bool saveFileHelper(int index)
        {
            Document window = windows[index];
            if (window.FilePath == null)
            {
                var tabitem = tabControl.Items.GetItemAt(index) as TabItem;
                string defaultName = ((ContentControl)tabitem.Header).Content + ".data";
                string filePath = FileDialogHelper.getSavePath(defaultName, "数据文件(*.data)|*.data");
                if (filePath == null)
                    return false;
                if (!window.saveFile(filePath))
                {
                    return false;
                }
                window.FilePath = filePath;
                ((ContentControl)tabitem.Header).Content = Path.GetFileNameWithoutExtension(filePath);
            }
            else
            {
                if (!window.saveFile(window.FilePath))
                {
                    return false;
                }
            }
            return true;
        }


        //关闭选项卡
        private void closeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var mi = sender as MenuItem;
            var cm = mi.Parent as ContextMenu;
            var contentControl = cm.PlacementTarget as ContentControl;
            var tabItem = contentControl.Parent as TabItem;
            if (tabItem is TabItem)
            {
                int index = tabControl.Items.IndexOf(tabItem);
                if (MessageBox.Show("是否保存？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    if (!saveFileHelper(index))
                    {
                        MessageBox.Show("保存失败。");
                        return;
                    }
                }
                tabControl.Items.RemoveAt(index);
                windows.RemoveAt(index);
            }
        }


        #endregion // FileMenu
        

        private void inputBaseDataMenu_Click(object sender, RoutedEventArgs e)
        {
            int index = tabControl.SelectedIndex;
            if (index == -1)
            {
                return;
            }
            Document curWindow = windows[index];
            showTitle("岩层基本数据录入--表格式");
            curWindow.tabControl.SelectedItem = curWindow.gridinputTabItem;
        }

        private void inputBaseGuidMenu_Click(object sender, RoutedEventArgs e)
        {
            int index = tabControl.SelectedIndex;
            if (index == -1)
            {
                return;
            }
            Document curWindow = windows[index];
            showTitle("岩层基本数据录入--向导式");
            curWindow.tabControl.SelectedItem = curWindow.guidinputTabItem;
        }

        private void inputKeyDataMenu_Click(object sender, RoutedEventArgs e)
        {
            int index = tabControl.SelectedIndex;
            if (index == -1)
            {
                return;
            }
            Document curWindow = windows[index];
            showTitle("关键层数据录入");
            curWindow.tabControl.SelectedItem = curWindow.keyLayerTabItem;
        }


        //*************************************位移计算**************************
        private void jqoffsetMenu_Click(object sender, RoutedEventArgs e)
        {
            int index = tabControl.SelectedIndex;
            if (index == -1)
            {
                return;
            }
            Document curWindow = windows[index];
            showTitle("岩层剪切位移");
            curWindow.tabControl.SelectedItem = curWindow.cutOffsetTabItem;
        }

        private void lcoffsetMenu_Click(object sender, RoutedEventArgs e)
        {
            int index = tabControl.SelectedIndex;
            if (index == -1)
            {
                return;
            }
            Document curWindow = windows[index];
            showTitle("岩层离层位移");
            curWindow.tabControl.SelectedItem = curWindow.lcOffsetTabItem;
        }

        //*******************************参数优化****************************************
        private void secureMenu_Click(object sender, RoutedEventArgs e)
        {
            int index = tabControl.SelectedIndex;
            if (index == -1)
            {
                return;
            }
            Document curWindow = windows[index];
            showTitle("套管安全系数");
            curWindow.tabControl.SelectedItem = curWindow.taoGuanTabItem;

        }

        private void snhComputeMenu_Click(object sender, RoutedEventArgs e)
        {
            int index = tabControl.SelectedIndex;
            if (index == -1)
            {
                return;
            }
            Document curWindow = windows[index];
            showTitle("水泥环增益计算");
            curWindow.tabControl.SelectedItem = curWindow.snhComputeTabItem;
        }


        //******************************地面井设计****************************************
        private void autoDesignMenu_Click(object sender, RoutedEventArgs e)
        {
            int index = tabControl.SelectedIndex;
            if (index == -1)
            {
                return;
            }
            Document curWindow = windows[index];
            showTitle("井型自动设计");
            curWindow.tabControl.SelectedItem = curWindow.autoDesignTabItem;
        }

        private void manDesignMenu_Click(object sender, RoutedEventArgs e)
        {
            int index = tabControl.SelectedIndex;
            if (index == -1)
            {
                return;
            }
            Document curWindow = windows[index];
            showTitle("井型人工设计");
            curWindow.tabControl.SelectedItem = curWindow.manuDesignTabItem;
        }

        private void showTitle(String title)
        {
            titleBlock.Text = title;
        }
        //***********************************菜单end******************


        // 导出表格到Excel
        private void exportExcel_Click(object sender, RoutedEventArgs e)
        {
            int index = tabControl.SelectedIndex;
            if (index == -1)
            {
                MessageBox.Show("当前界面无表格。");
                return;
            }
            Document curWindow = windows[index];
            Object selectedItem = curWindow.tabControl.SelectedItem;
            DataGrid datagrid = null;
            if(selectedItem == curWindow.gridinputTabItem)
            {
                datagrid = curWindow.paramGrid;
            }
            else if(selectedItem == curWindow.keyLayerTabItem)
            {
                datagrid = curWindow.keyLayerDataGrid;
            }
            else if(selectedItem == curWindow.cutOffsetTabItem)
            {
                datagrid = curWindow.cutOffsetDataGrid;
            }
            else if(selectedItem == curWindow.lcOffsetTabItem)
            {
                datagrid = curWindow.lcOffsetDataGrid;
            }
            else if(selectedItem == curWindow.taoGuanTabItem)
            {
                datagrid = curWindow.taoGuanDataGrid;
            }

            if(datagrid == null)
            {
                MessageBox.Show("当前界面无表格。");
                return;
            }

            string defaultName = ((TabItem)selectedItem).Header + ".xls";
            string filePath = FileDialogHelper.getSavePath(defaultName, "Excel文件(*.xls)|*.xls");
            if (filePath != null)
            {
                DataTable dt = ExcelHelper.ExtractDataTable(datagrid);
                if(ExcelHelper.ExportExcelWithAspose(dt, filePath))
                {
                    MessageBox.Show("导出成功。");
                }
                else
                {
                    MessageBox.Show("导出失败。");
                }
            }
        }

        //切换主题
        private void themeCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] themes = { "Colors_Blue.xaml", "Colors_Neutral.xaml", "Colors_Ocean.xaml", "Colors_Violet.xaml" };
            Uri skinDictUri = new Uri("..\\Resource\\Colors\\" + themes[styleComBox.SelectedIndex], UriKind.Relative);
            if (skinDictUri == null)
                return;
            ResourceDictionary skinDict = Application.LoadComponent(skinDictUri) as ResourceDictionary;
            Collection<ResourceDictionary> mergedDicts = Application.Current.Resources.MergedDictionaries;
            if (mergedDicts.Count > 1)
                mergedDicts.RemoveAt(1);
            mergedDicts.Add(skinDict);
        }

    }
}
