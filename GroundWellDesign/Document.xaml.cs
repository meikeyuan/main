﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Data;
using AxMxDrawXLib;
using System.Collections.ObjectModel;
using mky;
using System.IO;
using System.Globalization;
using System.Windows.Media;

namespace GroundWellDesign
{



    public partial class Document : Window
    {
        AxMxDrawX cadViewer;
        static MkyLogic logic;

        public ObservableCollection<LayerParams> layers = new ObservableCollection<LayerParams>();
        //向导式当前编辑的岩层参数
        LayerParams editLayer;
        public ObservableCollection<KeyLayerParams> keyLayers = new ObservableCollection<KeyLayerParams>();
        public const string DATABASE_PATH = "c:\\ProgramData\\GroundWellDesign\\";

        public static List<String> YanXingOpt = new List<string> { "地表", "黄土", "泥岩", "砂质泥岩", "细粒砂岩", "中粒砂岩", "粗粒砂岩", "粉砂岩", "细砂岩", "中砂岩", "煤" };
        public static List<String> CaiDongOpt = new List<string> { "初次采动Q0", "重复采动Q1", "重复采动Q2" };


        public string FilePath
        {
            set;
            get;
        }


        public Document(string filepath)
        {
            InitializeComponent();
            if (logic == null)
                logic = new MkyLogic();

            tabControl.Items.Remove(guidinputTabItem);


            //岩层参数录入初始化
            LayerParams dibiao = new LayerParams(this);
            dibiao.yanXing = YanXingOpt[0];
            layers.Add(dibiao);
            caiDongComBox.SelectedIndex = 0;



            //自动更新层号 层号不保存在集合中
            paramGrid.LoadingRow += new EventHandler<DataGridRowEventArgs>(dataGrid_LoadingRow);
            paramGrid.UnloadingRow += new EventHandler<DataGridRowEventArgs>(dataGrid_UnloadingRow);
            keyLayerDataGrid.LoadingRow += new EventHandler<DataGridRowEventArgs>(dataGrid_LoadingRow);
            keyLayerDataGrid.UnloadingRow += new EventHandler<DataGridRowEventArgs>(dataGrid_UnloadingRow);
            cutOffsetDataGrid.LoadingRow += new EventHandler<DataGridRowEventArgs>(dataGrid_LoadingRow);
            cutOffsetDataGrid.UnloadingRow += new EventHandler<DataGridRowEventArgs>(dataGrid_UnloadingRow);

            //向导式录入初始化
            editLayer = new LayerParams(this);
            guideBind(editLayer);


            cadViewer = new AxMxDrawX();
            cadViewer.BeginInit();
            wfHost.Child = cadViewer;
            cadViewer.EndInit();
            cadViewer.OpenDwgFile("示意钻井结构.dwg");
            cadViewer.ZoomCenter(1500, 1000);
            cadViewer.ZoomScale(0.4);

            paramGrid.DataContext = layers;
            keyLayerDataGrid.DataContext = keyLayers;
            yancengListBox.ItemsSource = layers;
            cutOffsetDataGrid.ItemsSource = layers;
            taoGuanDataGrid.ItemsSource = layers;



            //关键层其他参数绑定
            meiCengQingJIaoTb.DataContext = this;
            fuYanXCLTb.DataContext = this;
            caiGaoTb.DataContext = this;
            suiZhangXSTb.DataContext = this;
            maoLuoDaiTb.DataContext = this;
            lieXiDaiTb.DataContext = this;

            meiCengHouDuTb.DataContext = this;
            xiuZhengXishuTb.DataContext = this;
            hcqZXcdTb.DataContext = this;
            hcqQXcdTb.DataContext = this;
            gZMTJSDTb.DataContext = this;
            jswzjlTb.DataContext = this;


            //创建目录
            foreach (string yanxing in YanXingOpt)
            {
                Directory.CreateDirectory(DATABASE_PATH + yanxing);
            }

            FilePath = filepath;
        }

        //向导式绑定
        private void guideBind(LayerParams editLayer)
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
                return false;
            }
            object obj = DataSaveAndRestore.restoreObj(FilePath);
            if (obj == null || !(obj is DataSaveAndRestore.DataToSave))
            {
                return false;
            }
            DataSaveAndRestore.DataToSave data = obj as DataSaveAndRestore.DataToSave;

            //回复基本参数
            layers.Clear();
            foreach (BaseParams baseParam in data.Layers)
            {
                LayerParams layer = new LayerParams(baseParam);
                layer.mainWindow = this;
                layers.Add(layer);
            }


            //回复关键层数据
            keyLayers.Clear();
            foreach (BaseKeyParams baseParam in data.KeyLayers)
            {
                KeyLayerParams layer = new KeyLayerParams(baseParam);
                layer.mainWindow = this;
                keyLayers.Add(layer);

            }

            //回复关键层其他数据
            if (data.KeyLayerData != null && data.KeyLayerData.Count == 10)
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

            return true;

        }

        //保存到文件
        public bool saveFile()
        {
            DataSaveAndRestore.DataToSave data = new DataSaveAndRestore.DataToSave();

            //基本参数
            data.Layers = new ObservableCollection<BaseParams>();
            foreach (LayerParams layerParam in layers)
            {
                data.Layers.Add(new BaseParams(layerParam));
            }

            //关键层参数
            data.KeyLayers = new ObservableCollection<BaseKeyParams>();
            foreach (KeyLayerParams layerParam in keyLayers)
            {
                data.KeyLayers.Add(new BaseKeyParams(layerParam));
            }


            data.KeyLayerData = new List<double>();
            data.KeyLayerData.Add(Mcqj);
            data.KeyLayerData.Add(FuYanXCL);
            data.KeyLayerData.Add(CaiGao);
            data.KeyLayerData.Add(SuiZhangXS);

            data.KeyLayerData.Add(Mchd);
            data.KeyLayerData.Add(Pjxsxz);
            data.KeyLayerData.Add(HcqZXcd);
            data.KeyLayerData.Add(HcqQXcd);
            data.KeyLayerData.Add(Gzmsd);
            data.KeyLayerData.Add(Jswzjl);




            return DataSaveAndRestore.saveObj(data, FilePath);

        }



        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                if (tabControl.SelectedItem == guidinputTabItem)
                {
                    editLayer.copyNoEvent(layers[int.Parse(currLayerTb.Text) - 1]);
                    guideBind(editLayer);
                }
                //else if (tabControl.SelectedItem == cutOffsetTabItem)
                //{
                //    switch (computeCutOffSet(keyLayers.Count, layers.Count))
                //    {
                //        case ERRORCODE.计算成功:
                //            //MessageBox.Show("计算成功");
                //            break;
                //        case ERRORCODE.计算异常:
                //            MessageBox.Show("计算出错，请检查数据合理性");
                //            break;
                //        case ERRORCODE.没有关键层数据:
                //            MessageBox.Show("没有关键层数据");
                //            break;
                //        case ERRORCODE.没有评价系数修正系数:
                //            MessageBox.Show("没有评价系数修正系数，部分参数未计算");
                //            break;
                //        case ERRORCODE.没有煤层倾角和煤层厚度:
                //            MessageBox.Show("没有煤层倾角和煤层厚度，部分参数未计算");
                //            break;
                //        case ERRORCODE.没有回采区长度:
                //            MessageBox.Show("没有回采区长度(走向/倾向)，部分参数未计算");
                //            break;
                //        case ERRORCODE.没有工作面推进速度:
                //            MessageBox.Show("没有工作面推进速度，部分参数未计算");
                //            break;
                //    }
                //}
            }
        }







    }

}
