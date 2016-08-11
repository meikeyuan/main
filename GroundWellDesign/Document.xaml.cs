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

namespace GroundWellDesign
{
    public partial class Document : Window
    {
        public Document(string filepath)
        {
            InitializeComponent();
            FilePath = filepath;
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

            //水泥环增益初始化
            Ec = TgtxmlOpt[0];
            ecCombo.Text = Ec;

            //人工设计
            manuDesignParams = new ManuDesignParams(this);
            manuDesignParams.JieGou = JieGouOpt[1];

            //井型自动设计初始化
            AutoTgxh1 = TggjOpt[0];
            AutoTgxh2 = TggjOpt[1];
            AutoWjfs3 = WjfsOpt[0];

            //cad初始化
            cadViewer = new AxMxDrawX();
            cadViewer.BeginInit();
            wfHost.Child = cadViewer;
            Thread thread = new Thread(new ThreadStart(closeFuckDlg));
            thread.Start();
            cadViewer.EndInit();


            cadViewer2 = new AxMxDrawX();
            cadViewer2.BeginInit();
            wfHost2.Child = cadViewer2;
            Thread thread2 = new Thread(new ThreadStart(closeFuckDlg));
            thread2.Start();
            cadViewer2.EndInit();

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

            ecCombo.DataContext = this;
            vcTb.DataContext = this;
            esTb.DataContext = this;
            vsTb.DataContext = this;
            eTb.DataContext = this;
            vTb.DataContext = this;
            a0Tb.DataContext = this;
            awTb.DataContext = this;
            aw2Tb.DataContext = this;
            a1Tb.DataContext = this;
            a12Tb.DataContext = this;
            bTb.DataContext = this;            
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
                            row.Header = i + 1;
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
            leiJiShenDuTb.SetBinding(TextBox.TextProperty, new Binding("LeiJiShenDu") { Source = editLayer, StringFormat = "n3" });
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
        public bool openFile()
        {
            if (FilePath == null)
            {
                return true;
            }
            object obj = DataSaveAndRestore.restoreObj(FilePath);
            if (obj == null || !(obj is DataSaveAndRestore.DataToSave))
            {
                return false;
            }
            DataSaveAndRestore.DataToSave data = obj as DataSaveAndRestore.DataToSave;

            //恢复基本参数
            layers.Clear();
            foreach (BaseLayerBaseParams baseParam in data.Layers)
            {
                LayerBaseParams layer = new LayerBaseParams(baseParam);
                layer.mainWindow = this;
                layers.Add(layer);
            }

            //恢复人工设计数据
            manuDesignParams.copyAndEvent(data.ManuDesignParams);



            //恢复关键层数据
            keyLayers.Clear();
            foreach (BaseKeyLayerParams baseParam in data.KeyLayers)
            {
                KeyLayerParams layer = new KeyLayerParams(baseParam);
                layer.mainWindow = this;
                keyLayers.Add(layer);

            }

            //恢复关键层其他数据
            if (data.KeyLayerData != null && data.KeyLayerData.Count == 20)
            {
                mcqj = data.KeyLayerData[0];
                FuYanXCL = data.KeyLayerData[1];
                CaiGao = data.KeyLayerData[2];
                SuiZhangXS = data.KeyLayerData[3];

                mchd = data.KeyLayerData[4];
                pjxsxz = data.KeyLayerData[5];
                hcqZxcd = data.KeyLayerData[6];
                hcqQxcd = data.KeyLayerData[7];
                gzmsd = data.KeyLayerData[8];
                jswzjl = data.KeyLayerData[9];

                Ec = TgtxmlOpt[(int)data.KeyLayerData[10]];
                Vc = data.KeyLayerData[11];
                Es = data.KeyLayerData[12];
                Vs = data.KeyLayerData[13];
                E = data.KeyLayerData[14];
                V = data.KeyLayerData[15];
                A0 = data.KeyLayerData[16];
                Aw = data.KeyLayerData[17];
                A1 = data.KeyLayerData[18];
                B = data.KeyLayerData[19];

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

                ecCombo.Text = Ec;
                vcTb.Text = Vc + "";
                esTb.Text = Es + "";
                vsTb.Text = Vs + "";
                eTb.Text = E + "";
                vTb.Text = V + "";
                a0Tb.Text = A0 + "";
                awTb.Text = Aw + "";
                aw2Tb.Text = Aw + "";
                a1Tb.Text = A1 + "";
                a12Tb.Text = A1 + "";
                bTb.Text = B + "";
            }

            return true;

        }

        //保存到文件
        public bool saveFile()
        {
            DataSaveAndRestore.DataToSave data = new DataSaveAndRestore.DataToSave();

            //基本参数
            data.Layers = new ObservableCollection<BaseLayerBaseParams>();
            foreach (LayerBaseParams layerParam in layers)
            {
                data.Layers.Add(new BaseLayerBaseParams(layerParam));
            }

            //人工设计数据
            data.ManuDesignParams = new BaseManuDesignParams(manuDesignParams);

            //关键层参数
            data.KeyLayers = new ObservableCollection<BaseKeyLayerParams>();
            foreach (KeyLayerParams layerParam in keyLayers)
            {
                data.KeyLayers.Add(new BaseKeyLayerParams(layerParam));
            }


            data.KeyLayerData = new List<double>();
            //保存横三带竖三代数据
            data.KeyLayerData.Add(Mcqj);
            data.KeyLayerData.Add(FuYanXCL);
            data.KeyLayerData.Add(CaiGao);
            data.KeyLayerData.Add(SuiZhangXS);
            //保存关键层计算相关数据
            data.KeyLayerData.Add(Mchd);
            data.KeyLayerData.Add(Pjxsxz);
            data.KeyLayerData.Add(HcqZXcd);
            data.KeyLayerData.Add(HcqQXcd);
            data.KeyLayerData.Add(Gzmsd);
            data.KeyLayerData.Add(Jswzjl);
            //保存水泥环增益计算的数据
            if (Ec.Equals(TgtxmlOpt[0]))
                data.KeyLayerData.Add(0);
            else
                data.KeyLayerData.Add(1);
            data.KeyLayerData.Add(Vc);

            data.KeyLayerData.Add(Es);
            data.KeyLayerData.Add(Vs);

            data.KeyLayerData.Add(E);
            data.KeyLayerData.Add(V);

            data.KeyLayerData.Add(A0);
            data.KeyLayerData.Add(Aw);
            data.KeyLayerData.Add(A1);
            data.KeyLayerData.Add(B);
            
            return DataSaveAndRestore.saveObj(data, FilePath);
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
                    currLayerTb.Text = (index + 1).ToString();
                    editLayer.copyNoEvent(layers[index]);
                    guideBind(editLayer);
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
    }

}
