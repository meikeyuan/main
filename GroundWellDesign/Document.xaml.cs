using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Data;
using AxMxDrawXLib;
using System.Collections.ObjectModel;
using mky;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using MxDrawXLib;
using System.Drawing;

namespace GroundWellDesign
{
    public partial class Document : Window
    {
        public Document()
        {
            InitializeComponent();
            if (logic == null)
                logic = new MkyLogic();
            
            //岩层参数录入初始化
            LayerBaseParams dibiao = new LayerBaseParams(this);
            dibiao.yanXing = YanXingOpt[0];
            layers.Add(dibiao);
            caiDongComBox.SelectedIndex = 0;

            //向导式录入初始化
            editLayer = new LayerBaseParams(this);
            guideBind(editLayer);

            //位移计算初始化
            JQDestCombo.Text = JQDestOpt[0];
            LCDestCombo.Text = LCDestOpt[0];

            //水泥环增益初始化
            editZengYi = new ZengYiParams(this);

            //人工设计
            manuDesignParams = new ManuDesignParams(this);
            manuDesignParams.JieGou = JieGouOpt[1];

            //井型自动设计初始化
            AutoTgxh1 = TggjOpt[0];
            AutoTgxh2 = TggjOpt[0];
            AutoTgxh3 = TggjOpt[0];
            AutoWjfs3 = WjfsOpt[0];

            //cad初始化
            cadViewer = new AxMxDrawX();
            cadViewer.BeginInit();
            wfHost.Child = cadViewer;
            Thread thread = new Thread(new ThreadStart(closeFuckDlg));
            thread.Start();
            cadViewer.EndInit();
            cadViewer.ViewColor = Color.FromArgb(255, 255, 255);


            cadViewer2 = new AxMxDrawX();
            cadViewer2.BeginInit();
            wfHost2.Child = cadViewer2;
            Thread thread2 = new Thread(new ThreadStart(closeFuckDlg));
            thread2.Start();
            cadViewer2.EndInit();
            cadViewer2.ViewColor = Color.FromArgb(255, 255, 255);

            //自动更新层号 层号不保存在集合中
            paramGrid.LoadingRow += new EventHandler<DataGridRowEventArgs>(dataGrid_LoadingRow);
            paramGrid.UnloadingRow += new EventHandler<DataGridRowEventArgs>(dataGrid_UnloadingRow);
            keyLayerDataGrid.LoadingRow += new EventHandler<DataGridRowEventArgs>(dataGrid_LoadingRow);
            keyLayerDataGrid.UnloadingRow += new EventHandler<DataGridRowEventArgs>(dataGrid_UnloadingRow);
            cutOffsetDataGrid.LoadingRow += new EventHandler<DataGridRowEventArgs>(dataGrid_LoadingRow);
            cutOffsetDataGrid.UnloadingRow += new EventHandler<DataGridRowEventArgs>(dataGrid_UnloadingRow);
            lcOffsetDataGrid.LoadingRow += new EventHandler<DataGridRowEventArgs>(dataGrid_LoadingRow);
            lcOffsetDataGrid.UnloadingRow += new EventHandler<DataGridRowEventArgs>(dataGrid_UnloadingRow);
            taoGuanDataGrid.LoadingRow += new EventHandler<DataGridRowEventArgs>(dataGrid_LoadingRow);
            taoGuanDataGrid.UnloadingRow += new EventHandler<DataGridRowEventArgs>(dataGrid_UnloadingRow);

            //表格的数据绑定
            paramGrid.DataContext = layers;
            keyLayerDataGrid.DataContext = keyLayers;
            yancengListBox.ItemsSource = layers;
            cutOffsetDataGrid.ItemsSource = layers;
            lcOffsetDataGrid.ItemsSource = keyLayers;
            taoGuanDataGrid.ItemsSource = keyLayers;
            zengYiDataGrid.ItemsSource = zengYis;
            zengYiGrid.DataContext = editZengYi;
            manuDesignGrid.DataContext = manuDesignParams;
            autoDesignGrid.DataContext = this;
            

            //关键层计算相关其他参数绑定
            meiCengQingJIaoTb.DataContext = this;
            fuYanXCLTb.DataContext = this;
            caiGaoTb.DataContext = this;
            suiZhangXSTb.DataContext = this;

            meiCengHouDuTb.DataContext = this;
            xiuZhengXishuTb.DataContext = this;
            hcqZXcdTb.DataContext = this;
            hcqQXcdTb.DataContext = this;
            gZMTJSDTb.DataContext = this;
            jswzjlTb.DataContext = this;

            

            //initialData();
        }


        void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (!(sender is DataGrid))
            {
                return;
            }
            DataGrid grid = sender as DataGrid;
            if (grid.Items != null)
            {
                for (int i = e.Row.GetIndex(); i < grid.Items.Count; i++)
                {
                    try
                    {
                        DataGridRow row = grid.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;
                        if (row != null)
                        {
                            row.Header = i;
                        }
                    }
                    catch { }
                }
            }
        }

        void dataGrid_UnloadingRow(object sender, DataGridRowEventArgs e)
        {
            dataGrid_LoadingRow(sender, e);
        }

        

        //一下代码用于自动消除cad弹出框
        [StructLayout(LayoutKind.Sequential)]
        public struct CopyDataStruct
        {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, ref  CopyDataStruct lParam);

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern int FindWindow(string lpClassName, string lpWindowName); 

        void closeFuckDlg()
        {
            IntPtr hwnd = IntPtr.Zero;
            CopyDataStruct cds = new CopyDataStruct();
            int fromWindowHandler = 0;

            while (hwnd == IntPtr.Zero)
            {
                hwnd = (IntPtr)FindWindow(null, "MxDrawX");
                if(hwnd != IntPtr.Zero)
                {
                    SendMessage(hwnd, 16, fromWindowHandler, ref  cds);
                    break;
                }
            }
        }

        //向导式绑定
        private void guideBind(LayerBaseParams editLayer)
        {
            miaoshuTb.SetBinding(TextBox.TextProperty, new Binding("MiaoShu") { Source = editLayer });
            yanXingCB.SetBinding(ComboBox.TextProperty, new Binding("YanXing") { Source = editLayer });
            //leiJiShenDuTb.SetBinding(TextBox.TextProperty, new Binding("LeiJiShenDu") { Source = editLayer, StringFormat = "n3" });
            //juLiMeiShenDuTb.SetBinding(TextBox.TextProperty, new Binding("JuLiMeiShenDu") { Source = editLayer, StringFormat = "n3" });
            cengHouTb.SetBinding(TextBox.TextProperty, new Binding("CengHou") { Source = editLayer, StringFormat = "n3" });
            ziRanMiDuTb.SetBinding(TextBox.TextProperty, new Binding("ZiRanMiDu") { Source = editLayer, StringFormat = "n3" });
            bianXingMoLiangTb.SetBinding(TextBox.TextProperty, new Binding("BianXingMoLiang") { Source = editLayer, StringFormat = "n3" });
            kangLaQiangDuTb.SetBinding(TextBox.TextProperty, new Binding("KangLaQiangDu") { Source = editLayer, StringFormat = "n3" });
            kangYaQiangDuTb.SetBinding(TextBox.TextProperty, new Binding("KangYaQiangDu") { Source = editLayer, StringFormat = "n3" });
            tanXingMoLiangTb.SetBinding(TextBox.TextProperty, new Binding("TanXingMoLiang") { Source = editLayer, StringFormat = "n3" });
            boSonBiTb.SetBinding(TextBox.TextProperty, new Binding("BoSonBi") { Source = editLayer, StringFormat = "n3" });
            neiMoCaJiaoTb.SetBinding(TextBox.TextProperty, new Binding("NeiMoCaJiao") { Source = editLayer, StringFormat = "n3" });
            nianJuLiTb.SetBinding(TextBox.TextProperty, new Binding("NianJuLi") { Source = editLayer, StringFormat = "n3" });
            q0Tb.SetBinding(TextBox.TextProperty, new Binding("Q0") { Source = editLayer, StringFormat = "n2" });
            q1Tb.SetBinding(TextBox.TextProperty, new Binding("Q1") { Source = editLayer, StringFormat = "n2" });
            q2Tb.SetBinding(TextBox.TextProperty, new Binding("Q2") { Source = editLayer, StringFormat = "n2" });
        }

        //打开文件
        public bool openFile(string filePath)
        {
            if (filePath == null)
            {
                FilePath = filePath;
                return true;
            }

            object obj = DataSaveAndRestore.restoreObj(filePath);
            if (obj == null || !(obj is DataSaveAndRestore.DataToSave))
            {
                return false;
            }
            DataSaveAndRestore.DataToSave data = obj as DataSaveAndRestore.DataToSave;

            //恢复基本参数
            layers.Clear();
            foreach (BaseLayerBaseParams baseParam in data.Layers)
            {
                LayerBaseParams layer = new LayerBaseParams(this, baseParam);
                layers.Add(layer);
            }

            //恢复关键层数据
            keyLayers.Clear();
            foreach (BaseKeyLayerParams baseParam in data.KeyLayers)
            {
                KeyLayerParams layer = new KeyLayerParams(this, baseParam);
                keyLayers.Add(layer);

            }

            //恢复关键层其他数据
            if (data.KeyLayerData != null && data.KeyLayerData.Count == 13)
            {
                int i = 0;
                mcqj = data.KeyLayerData[i++];
                FuYanXCL = data.KeyLayerData[i++];
                CaiGao = data.KeyLayerData[i++];
                SuiZhangXS = data.KeyLayerData[i++];
                maoLuoDaiTb.Text = data.KeyLayerData[i++].ToString("f3");
                lieXiDaiTb.Text = data.KeyLayerData[i++].ToString("f3");
                wanQuDaiTb.Text = data.KeyLayerData[i++].ToString("f3");

                mchd = data.KeyLayerData[i++];
                pjxsxz = data.KeyLayerData[i++];
                hcqZxcd = data.KeyLayerData[i++];
                hcqQxcd = data.KeyLayerData[i++];
                gzmsd = data.KeyLayerData[i++];
                jswzjl = data.KeyLayerData[i++];


                //没有刷新ui
                meiCengQingJIaoTb.Text = mcqj + "";
                fuYanXCLTb.Text = FuYanXCL + "";
                caiGaoTb.Text = CaiGao + "";
                suiZhangXSTb.Text = SuiZhangXS + "";

                meiCengHouDuTb.Text = mchd + "";
                xiuZhengXishuTb.Text = pjxsxz + "";
                hcqZXcdTb.Text = hcqZxcd + "";
                hcqQXcdTb.Text = hcqQxcd + "";
                gZMTJSDTb.Text = gzmsd + "";
                jswzjlTb.Text = jswzjl + "";
            }

            //恢复水泥环历史数据
            zengYis.Clear();
            foreach (BaseZengYiParams baseParam in data.ZengYis)
            {
                ZengYiParams param = new ZengYiParams(this, baseParam);
                zengYis.Add(param);
            }

            //恢复人工设计数据
            manuDesignParams.copyAndEvent(data.ManuDesignParams);

            FilePath = filePath;
            return true;

        }

        //保存到文件
        public bool saveFile(string filePath)
        {
            Object data = DataSaveAndRestore.getObject(this);
            
            return DataSaveAndRestore.saveObj(data, filePath);
        }


        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                object selectedItem = tabControl.SelectedItem;
                //选择了向导式录入
                if (selectedItem == guidinputTabItem)
                {
                    int index = paramGrid.SelectedIndex;
                    if(index < 0)
                    {
                        index = 0;
                    }
                    LayerNbrs.Clear();
                    for(int i = 0; i < layers.Count; i++)
                    {
                        LayerNbrs.Add(i+1);
                    }
                    currLayerCombo.SelectedIndex = index;
                }else if(selectedItem == snhComputeTabItem)
                {
                    LayerNbrs.Clear();
                    for (int i = 0; i < layers.Count; i++)
                    {
                        LayerNbrs.Add(i + 1);
                    }
                    if (layers.Count > 0)
                        snhtcbhCombo.SelectedIndex = 0;
                }
                else if(selectedItem == autoDesignTabItem || selectedItem == manuDesignTabItem || selectedItem == taoGuanTabItem
                    || selectedItem == keyLayerTabItem || selectedItem == cutOffsetTabItem || selectedItem == lcOffsetTabItem)
                {
                    if(keyLayers.Count == 0)
                    {
                        MessageBox.Show("请先计算关键层");
                        tabControl.SelectedItem = gridinputTabItem;
                        return;
                    }
                }
            }
        }




        private void initialData()
        {
            double[] data2 = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 8.5, 0, 0, 0, 0, 0, 0, 0, 0, 0.5, 15.9, 0, 0, 0, 0, 0, 0, 0, 0, 0.4, 3.3, 2309.41, 1304.7015, 2.225, 14.87, 2653.915, 0.2195, 47.868, 4.67, 0.7, 3.15, 2267.47, 605.92, 1.65, 5.13, 1140.6, 0.235, 47.868, 4.67, 1, 9.95, 2465.27, 2003.483, 2.8, 24.61, 4167.23, 0.204, 47.868, 4.67, 0.5, 0.58, 2567.91, 4121.43, 10.6, 43.53, 8656.4, 0.191, 52.275, 9.717, 0.32, 0.82, 2020.56, 5178.27, 0.42, 58.57, 11476.13, 0.186, 48.507, 11.487, 0.11, 1.9, 2407.39, 417.93, 1.58, 6.82, 1062, 0.235, 56.545, 0.573, 1, 2.28, 2474.88, 1593.11, 5.26, 30.38, 3203.87, 0.213, 54.316, 4.442, 0.6, 3.73, 2553.48, 4284.26, 12.43, 65.1, 9785.73, 0.191, 52.575, 4.899, 0.1, 10.63, 2480.9, 3045.245, 3.18, 65.1, 9785.73, 0.191, 52.575, 4.899, 0.1, 4.27, 2543.38, 5178.27, 5.39, 58.57, 11476.13, 0.186, 48.507, 11.487, 0.11, 13.65, 2516.41, 1806.23, 3.8, 25.43, 3724.1, 0.215, 56.545, 0.573, 0.7, 1.8, 2553.9, 5068.8, 6.37, 57.27, 10198.7, 0.195, 52.275, 9.717, 0.13, 13.88, 2541.91, 1135.78, 5.01, 12.63, 2420.5, 0.223, 52.575, 4.899, 0.87, 0.96, 2342, 5236.667, 2.59, 60.64, 9615.2, 0.194, 51.169, 3.039, 0.1, 4.9, 2567.65, 3372.88, 7.42, 49.27, 7560.29, 0.2, 49.72, 7.66, 0.22, 1.54, 2541.91, 1135.78, 5.01, 12.63, 2420.5, 0.223, 52.575, 4.899, 0.87, 3.16, 2561.12, 3128.032, 8.37, 47.93, 6646.83, 0.196, 48.507, 11.487, 0.24, 5.7, 2397.51, 438.344, 1.22, 8.7, 1100.73, 0.233, 57.883, 1.446, 1, 18.18, 2570.25, 908.2475, 8.57, 10.77, 1871.75, 0.2245, 52.575, 4.899, 0.89, 0.87, 2590.99, 3406.154, 6.17, 48.68, 9106.8, 0.189, 15.235, 18.479, 0.22, 1.55, 2574.1, 5372.11, 7.08, 71.23, 12044.37, 0.185, 62.307, 8.668, 0.1, 1.68, 2561.12, 3128.032, 8.37, 47.93, 6646.83, 0.196, 48.507, 11.487, 0.24, 1.52, 2597.98, 3446.33, 8.62, 40.98, 8005.27, 0.192, 51.169, 3.039, 0.38, 3.3, 2625.095, 5253.91, 11.79, 75.78, 11021.6, 0.188, 64.179, 9.056, 0.1, 6, 2545.73, 2114.49, 7.22, 28.92, 4904.75, 0.206, 52.575, 4.899, 0.62, 4.43, 2556.85, 5323.2, 10.92, 63.96, 11680.27, 0.185, 48.507, 11.487, 0.1, 4.97, 2608.41, 2295.97, 8.97, 25.36, 5860.6, 0.206, 51.169, 3.039, 0.7, 10.8, 2573.3, 3677.29, 8.91, 46.54, 9405.7, 0.2, 51.76, 8.356, 0.26, 21.86, 2606, 2688, 6.26, 32.81, 6943.523333, 0.199333333, 46.7205, 7.54, 0.54, 8.3, 2629.59, 3310.54, 9.7, 45.27, 8684.4, 0.191, 40.866, 10.181, 0.3, 4.24, 2583.51, 2652.52, 2.88, 27.99, 6509.1, 0.204, 51.169, 3.039, 0.64, 1.8, 2583.53, 2739.06, 6.31, 41.87, 6485.63, 0.205, 40.731, 9.608, 0.36, 6.7, 2589.42, 2456.38, 5.513, 31.97, 5825.46, 0.2, 54.09, 5.1533, 0.56, 0.8, 2649.2, 4929.86, 19.01, 71.28, 12562.2, 0.187, 40.731, 9.608, 0.1, 4.4, 2563.29, 1649.01, 5.57, 23.63, 4103.77, 0.214, 49.039, 3.771, 0.72, 1.4, 2598.33, 3142.58, 7.69, 54.38, 8115.95, 0.197, 40.866, 10.181, 0.16, 4.9, 2613.99, 5393.26, 9.54, 61.14, 12665.1, 0.188, 61.858, 8.191, 0.1, 4.1, 2584.415, 2723.815, 5.43, 36.965, 6508.325, 0.198, 54.729, 7.234, 0.46, 0.6, 2710.27, 4104.984, 6.31, 52.57, 11084, 0.187, 40.731, 9.608, 0.17, 12.01, 2584.185, 1545.353175, 4.905, 19.855, 4036.185, 0.2155, 60.198, 3.315, 0.8, 5.49, 2594.745, 1567.32, 5.815, 24.82, 5219.965, 0.203, 51.169, 3.039, 0.7, 6.9, 2666.75, 3101.99, 5.35, 30.1, 7431.57, 0.197, 51.929, 5.601, 0.6, 5.1, 2644.73, 2559.62, 6.63, 48.73, 9785.65, 0.19, 58.558, 5.719, 0.22, 3.9, 2534.95, 2223.8, 7.13, 33.33, 6203.4, 0.203, 49.885, 5.339, 0.54, 1.2, 2493.69, 908.17, 3.79, 21.9, 4437.6, 0.208, 58.558, 5.719, 0.76, 6, 2532.38, 2419.71, 2.86, 34.4, 7067.6, 0.199, 56.399, 3.323, 0.52, 2.1, 2539.85, 2361.67, 5.13, 33.92, 5602.53, 0.212, 53.246, 8.26, 0.52, 7.5, 2615.14, 2190.8, 7.72, 23.17, 5633.83, 0.206, 61.606, 2.871, 0.74, 2.2, 2720.81, 4307.07, 11.2, 52.32, 10321.45, 0.19, 53.246, 8.26, 0.18, 3.9, 2635.7, 1929.96, 5.34, 22.45, 5069.47, 0.203, 56.399, 3.323, 0.78, 0.8, 2585.83, 1981.59, 3.38, 21.94, 5722.95, 0.202, 52.119, 5.144, 0.78, 16.3, 2591.65, 2779.93, 6.15, 36.09, 6197.78, 0.208, 56.857, 4.64, 0.48, 3.5, 2629.99, 3201.12, 6.93, 41.36, 7280.08, 0.2, 58.558, 5.719, 0.38, 8.5, 2608.85, 2171.04, 6.17, 28.52, 5500.46, 0.211, 42.222, 5.881, 0.62, 1.7, 2595.41, 2004.1, 8.35, 28.51, 5068.27, 0.205, 56.503, 4.371, 0.62, 4.5, 2550.66, 1462.16, 3.77, 15.98, 4684.4, 0.207, 56.399, 3.323, 0.84, 3.9, 2634.12, 2027.42, 4.6, 27.11, 5558.35, 0.206, 56.857, 4.64, 0.66, 3.1, 2528.46, 1332.85, 10.09, 19.72, 3375.85, 0.217, 56.399, 3.323, 0.8, 1.5, 2573.3, 2912.99, 8.75, 34.33, 7011.7, 0.197, 54.81, 4.583, 0.52, 7.9, 2542.91, 1974.76, 5.26, 23.18, 3948.3, 0.216, 56.399, 3.323, 0.74, 2.5, 2458.21, 4146.04, 9.05, 54.24, 10958.83, 0.191, 53.417, 9.414, 0.16, 23.82, 2605.4, 1757.66, 4.06, 17.24, 3828.2, 0.213, 50.474, 3.68, 0.83, 3.4, 2609.9, 1874.64, 6.635, 20.135, 4538.85, 0.207, 52.119, 5.144, 0.8, 3.98, 2614.4, 1991.62, 9.21, 23.03, 5249.5, 0.201, 56.857, 4.64, 0.74, 8.98, 2582.56, 8830.83, 11.98, 92.18, 16691.53, 0.171, 50.474, 3.68, 0.8, 4.64, 2643.82, 3953.19, 8.7, 48.45, 8898.97, 0.193, 54.24, 7.01, 0.24, 1.28, 2683.07, 1968.12, 6.23, 28.94, 4585, 0.212, 58.613, 3.391, 0.62, 6.4, 2599.26, 5586.99, 14.77, 64.05, 12670.47, 0.184, 35.327, 16.747, 0.1, 6.3, 2709.57, 4254.52, 14, 53.69, 7315.47, 0.196, 57.078, 7.246, 0.16, 1.2, 2720.88, 4467.85, 11.28, 66.7, 12257.7, 0.187, 54.24, 7.01, 0.1, 2.2, 2582.43, 3329.5, 10.64, 43.02, 6997.57, 0.2, 49.679, 11.248, 0.34, 12.85, 2524.14, 1504.91, 6.41, 20.26, 3951.27, 0.218, 50.8, 4.64, 0.8, 0.5, 2642.76, 2166.88, 9.46, 27.02, 5172.15, 0.21, 52.119, 5.144, 0.66, 10.05, 2658.57, 2444.97, 5.9, 24.99, 5583.92, 0.21, 58.61, 3.39, 0.7, 3.6, 2691.16, 2492.56, 8.51, 34.41, 6925, 0.204, 57.078, 7.246, 0.52, 1.55, 2570.92, 1327.92, 5.56, 22.64, 3601.95, 0.215, 50.8, 4.64, 0.74, 2.95, 2753.23, 5223.21, 19.89, 58.37, 12899.8, 0.186, 49.679, 11.248, 0.12, 2.8, 2596.11, 4293.2, 9.9, 41.69, 9201.9, 0.194, 54.24, 7.01, 0.36, 4.4, 2547.78, 3791.91, 7.74, 40.5, 8777.65, 0.192, 49.679, 11.248, 0.38, 3, 2552.54, 2891.64, 4.91, 33.4, 6340.23, 0.201, 58.613, 3.391, 0.54, 5.2, 2711.56, 6132.78, 11.14, 64.63, 12501.3, 0.183, 35.327, 16.747, 0.1, 6.45, 2689.44, 2657.425, 6.04, 31.995, 6289.3, 0.204, 50.8, 4.64, 0.56, 4.4, 2685.56, 2424.06, 8.47, 24.79, 4705.17, 0.214, 55.227, 3.758, 0.7, 8.99, 2675.185, 1981.43, 7.035, 24.115, 4809.025, 0.21, 0, 0, 0.72, 6.2, 1408.21, 278.12, 1.37, 6.99, 855, 0.24, 0, 0, 0, 10.8, 2581.82, 1637.51, 5.59, 26.46, 5368.67, 0.207, 0, 0, 0 };
            string[] yanxings = {"地表","黄土","细粒砂岩","泥岩","泥岩","泥岩","细粒砂岩","中粒砂岩","泥岩","粉砂岩","泥岩","泥岩","中粒砂岩","泥岩","细粒砂岩","泥岩","砂质泥岩","粗粒砂岩","泥岩","中粒砂岩","粗粒砂岩","泥岩","粉砂岩","细粒砂岩","中粒砂岩","砂质泥岩","中粒砂岩","泥岩","中粒砂岩","砂质泥岩","中粒砂岩","泥岩","粉砂岩","砂质泥岩","细粒砂岩","泥岩","细粒砂岩","泥岩","粉砂岩","中粒砂岩","泥岩","细粒砂岩","泥岩","砂质泥岩","泥岩","粉砂岩","砂质泥岩","粉砂岩","泥岩","中粒砂岩","泥岩","中粒砂岩","泥岩","细粒砂岩","砂质泥岩","粉砂岩","泥岩","中粒砂岩","泥岩","砂质泥岩","泥岩","粉砂岩","泥岩","中粒砂岩","泥岩","细砂岩","砂质泥岩","泥岩","中粒砂岩","泥岩","中粒砂岩","粉砂岩","中粒砂岩","粗粒砂岩","砂质泥岩","细粒砂岩","泥岩","粉砂岩","砂质泥岩","细粒砂岩","中粒砂岩","粗粒砂岩","泥岩","中粒砂岩","砂质泥岩","中砂岩","砂质泥岩","煤","泥岩"};
            
            
            for (int i = 0; i < 89; i++)
            {
                LayerBaseParams param = new LayerBaseParams(this);
                layers.Add(param);
                param.yanXing = yanxings[i];
                param.CengHou = data2[i * 10];
                param.ziRanMiDu = data2[i * 10 + 1];
                param.bianXingMoLiang = data2[i * 10 + 2];;
                param.kangLaQiangDu = data2[i * 10 + 3];
                param.kangYaQiangDu = data2[i * 10 + 4];
                param.tanXingMoLiang = data2[i * 10 + 5];
                param.boSonBi = data2[i * 10 + 6];
                param.neiMoCaJiao = data2[i * 10 + 7];
                param.nianJuLi = data2[i * 10 + 8];
                param.q0 = data2[i * 10 + 9];
            }
        }







 

    }

}
