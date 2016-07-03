using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
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


        private int newFileCount = 1;

        public static string DATABASE_PATH = "c:\\ProgramData\\GroundWellDesign\\yanceng.db";
        

        public ContainerWindow(bool bLogin, string filepath)
        {
            InitializeComponent();
            loginInfo.BLogin = bLogin;
            openFileHelper(filepath);


            //创建数据库目录
            //foreach (string yanxing in YanXingOpt)
            //{
            //Directory.CreateDirectory(DATABASE_PATH /*+ yanxing */);
            //}
            SQLiteConnection conn = null;

            string dbPath = "Data Source =" + DATABASE_PATH;
            conn = new SQLiteConnection(dbPath);//创建数据库实例，指定文件位置
            conn.Open();//打开数据库，若文件不存在会自动创建
            //conn.SetPassword("123456");

            //建表语句
            string sql2 = "CREATE TABLE IF NOT EXISTS yanceng(" +
                         "id char(36) primary key, wellName text references well(wellName), " +
                         "yanXing text, leiJiShenDu double, juLiMeiShenDu double, cengHou double, ziRanMiDu double, " +
                         "bianXingMoLiang double, kangLaQiangDu double, kangYaQiangDu double, tanXingMoLiang double, boSonBi double, " +
                         "neiMoCaJiao double, nianJuLi double, q0 double, q1 double, q2 double, miaoShu Text" +
                         ");";

            string sql1 = "CREATE TABLE IF NOT EXISTS well(" +
                         "wellName text primary key" +
                         ");";

            //如果表不存在，创建数据表 
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


        //**********************文件*******************************


        //新建文件菜单
        private void newFileMenu_Click(object sender, RoutedEventArgs e)
        {
            openFileHelper(null);
        }


        //打开文件菜单
        private void openFileMenu_Click(object sender, RoutedEventArgs e)
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
                if (!openFileHelper(filePath))
                {
                    MessageBox.Show("打开文件失败！");
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
            savetoFileHelper(windows[index]);
        }

        //保存到文件菜单
        private void saveFileMenu_Click(object sender, RoutedEventArgs e)
        {
            int index = tabControl.SelectedIndex;
            if (index == -1)
            {
                return;
            }
            saveFileHelper(windows[index]);

        }

        //保存所有文件菜单
        private void saveAllFileMenu_Click(object sender, RoutedEventArgs e)
        {

            foreach (Document window in windows)
            {
                tabControl.SelectedIndex = windows.IndexOf(window);
                saveFileHelper(window);
            }
        }

        //打开文件
        private bool openFileHelper(string filePath)
        {
            Document document = new Document(filePath);
            if (!document.openFile())
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


        //保存文件
        private void saveFileHelper(Document window)
        {
            if (window.FilePath == null)
            {
                savetoFileHelper(window);
                return;
            }
            if (MessageBox.Show(Path.GetFileNameWithoutExtension(window.FilePath) + ":确定覆盖此文件吗？", 
                "提示", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
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


        private void savetoFileHelper(Document window)
        {
            System.Windows.Forms.SaveFileDialog fileDialog = new System.Windows.Forms.SaveFileDialog();
            var tabitem = tabControl.Items.GetItemAt(windows.IndexOf(window)) as TabItem;
            fileDialog.Title = "存为";
            fileDialog.Filter = "数据文件(*.data)|*.data";
            fileDialog.FileName = tabitem.Header + ".data";

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filePath = fileDialog.FileName;
                string oldPath = window.FilePath;
                window.FilePath = filePath;
                if (window.saveFile())
                {
                    MessageBox.Show("保存成功");
                    tabitem.Header = Path.GetFileNameWithoutExtension(filePath);
                }
                else
                {
                    MessageBox.Show("保存失败");
                    window.FilePath = oldPath;
                }
            }
        }

        //**********************录入数据*****************************

        private void inputBaseDataMenu_Click(object sender, RoutedEventArgs e)
        {
            int index = tabControl.SelectedIndex;
            if (index == -1)
            {
                return;
            }
            Document curWindow = windows[index];
            if (curWindow.tabControl.Items.Contains(curWindow.guidinputTabItem))
            {
                curWindow.tabControl.Items.RemoveAt(0);
                curWindow.tabControl.Items.Insert(0, curWindow.gridinputTabItem);
                
            }
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
            if (curWindow.tabControl.Items.Contains(curWindow.gridinputTabItem))
            {
                curWindow.tabControl.Items.RemoveAt(0);
                curWindow.tabControl.Items.Insert(0, curWindow.guidinputTabItem);
                
            }
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

        private void manDesignsecureMenu_Click(object sender, RoutedEventArgs e)
        {
            int index = tabControl.SelectedIndex;
            if (index == -1)
            {
                return;
            }
            Document curWindow = windows[index];
            showTitle("井型人工设计");
            curWindow.tabControl.SelectedItem = curWindow.manDesignTabItem;

        }

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


        private void showTitle(String title)
        {
            titleBlock.Text = title;
        }


        //***********************************菜单end******************


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
                    ToggleInputType(window);
                    
                }
        }


        private void ToggleInputType(Document window)
        {
            if (window.tabControl.Items.Contains(window.guidinputTabItem))
            {
                window.tabControl.Items.RemoveAt(0);
                window.tabControl.Items.Insert(0, window.gridinputTabItem);
                window.tabControl.SelectedItem = window.gridinputTabItem;
            }
            else
            {
                window.tabControl.Items.RemoveAt(0);
                window.tabControl.Items.Insert(0, window.guidinputTabItem);
                window.tabControl.SelectedItem = window.guidinputTabItem;
            }
        }




    }
}
