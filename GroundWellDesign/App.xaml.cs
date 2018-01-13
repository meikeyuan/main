using AxMxDrawXLib;
using GroundWellDesign.Util;
using log4net;
using log4net.Config;
using Mky;
using System;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace GroundWellDesign
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static ILog logger;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // 初始化日志组件。
            InitLog4Net();
            logger = LogManager.GetLogger(typeof(App));

            // 初始化数据库。
            bool success = InitDataBase();
            if(!success)
            {
                Shutdown(-1);
            }

            // 加载公式接口，耗时操作。
            try
            {
                ComputeHelper.initLogic();
            }
            catch(Exception ex)
            {
                App.logger.Fatal(GroundWellDesign.Properties.Resources.InitLogicError, ex);
                Shutdown(-1);
            }

            string filePath = null;
            if (e.Args.Length != 0)
            {
                filePath = e.Args[0];
            }
            LoginWindow loginWin = new LoginWindow(filePath);
            loginWin.Show();
        }

        private void InitLog4Net()
        {
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.ConfigureAndWatch(logCfg);
        }

        private bool InitDataBase()
        {
            // 创建岩层数据库目录
            SQLiteConnection conn =  SQLDBHelper.GetSQLiteConnection();
            if (conn == null)
            {
                App.logger.Error(GroundWellDesign.Properties.Resources.OpenDataBaseError);
                return false;
            }

            // 创建well表和yanceng表
            bool success = false;
            SQLiteCommand sqlCmd = new SQLiteCommand(conn);
            success =
            SQLDBHelper.CreateTable(sqlCmd, "well", "wellName text primary key") &&
            SQLDBHelper.CreateTable(sqlCmd, "yanceng", "id char(36) primary key") &&
            SQLDBHelper.AddColumn(sqlCmd, "yanceng", "wellName", "text references well(wellName)") &&
            SQLDBHelper.AddColumn(sqlCmd, "yanceng", "yanXing", "text") &&
            SQLDBHelper.AddColumn(sqlCmd, "yanceng", "leiJiShenDu", "double") &&
            SQLDBHelper.AddColumn(sqlCmd, "yanceng", "juLiMeiShenDu", "double") &&
            SQLDBHelper.AddColumn(sqlCmd, "yanceng", "cengHou", "double") &&
            SQLDBHelper.AddColumn(sqlCmd, "yanceng", "ziRanMiDu", "double") &&
            SQLDBHelper.AddColumn(sqlCmd, "yanceng", "bianXingMoLiang", "double") &&
            SQLDBHelper.AddColumn(sqlCmd, "yanceng", "kangLaQiangDu", "double") &&
            SQLDBHelper.AddColumn(sqlCmd, "yanceng", "kangYaQiangDu", "double") &&
            SQLDBHelper.AddColumn(sqlCmd, "yanceng", "tanXingMoLiang", "double") &&
            SQLDBHelper.AddColumn(sqlCmd, "yanceng", "boSonBi", "double") &&
            SQLDBHelper.AddColumn(sqlCmd, "yanceng", "neiMoCaJiao", "double") &&
            SQLDBHelper.AddColumn(sqlCmd, "yanceng", "nianJuLi", "double") &&
            SQLDBHelper.AddColumn(sqlCmd, "yanceng", "f", "double") &&
            SQLDBHelper.AddColumn(sqlCmd, "yanceng", "q0", "double") &&
            SQLDBHelper.AddColumn(sqlCmd, "yanceng", "q1", "double") &&
            SQLDBHelper.AddColumn(sqlCmd, "yanceng", "q2", "double") &&
            SQLDBHelper.AddColumn(sqlCmd, "yanceng", "miaoShu", "Text");
            conn.Close();

            if(!success)
            {
                App.logger.Error(GroundWellDesign.Properties.Resources.InitDataBaseTablesError);
            }

            return success;
        }
    }
}
