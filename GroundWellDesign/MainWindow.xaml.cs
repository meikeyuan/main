using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Data;
using AxMxDrawXLib;
using System.Collections.ObjectModel;
using MathWorks.MATLAB.NET.Arrays;
using mky;
using System.IO;

namespace GroundWellDesign
{

    public partial class MainWindow : Window
    {

        AxMxDrawX cadViewer;
        MkyLogic logic;

        public static ObservableCollection<LayerParams> layers = new ObservableCollection<LayerParams>();
        public static ObservableCollection<KeyLayerParams> keyLayers = new ObservableCollection<KeyLayerParams>();
        public const string FILE_PATH = ".\\data\\";
        public string FileName
        {
            set;
            get;
        }


        public double Mcqj
        {
            set;
            get;
        }
        public double Mchd
        {
            set;
            get;
        }

        private double pjxsxz;
        public double Pjxsxz
        {
            set
            {
                pjxsxz = value;
                foreach (KeyLayerParams layer in keyLayers)
                {
                    layer.Fypjxsxz = layer.fypjxs * value;
                }


            }
            get
            {
                return pjxsxz;
            }
        }
        public double HcqZXcd
        {
            set;
            get;
        }

        public double HcqQXcd
        {
            set;
            get;
        }

        public static double gzmsd;
        public double Gzmsd
        {
            set
            {
                gzmsd = value;
                foreach (KeyLayerParams layer in keyLayers)
                {
                    layer.Gzmtjjl = gzmsd * layer.gzmtjsj;
                }
            }
            get
            {
                return gzmsd;
            }
        }
        public double Jswzjl
        {
            set;
            get;
        }


        public MainWindow()
        {
            InitializeComponent();
            logic = new MkyLogic();


            //岩层参数录入初始化
            LayerParams dibiao = new LayerParams();
            dibiao.yanXing = LayerParams.YanXingOpt[0];
            layers.Add(dibiao);
            caiDongComBox.SelectedIndex = 0;



            //自动更新层号 层号不保存在集合中
            paramGrid.LoadingRow += new EventHandler<DataGridRowEventArgs>(dataGrid_LoadingRow);
            keyLayerDataGrid.LoadingRow += new EventHandler<DataGridRowEventArgs>(dataGrid_LoadingRow);
            paramGrid.UnloadingRow += new EventHandler<DataGridRowEventArgs>(dataGrid_UnloadingRow);


            cadViewer = new AxMxDrawX();
            cadViewer.BeginInit();
            wfHost.Child = cadViewer;
            cadViewer.EndInit();
            cadViewer.OpenDwgFile("示意钻井结构.dwg");
            cadViewer.ZoomCenter(1500, 1000);
            cadViewer.ZoomScale(0.4);

            paramGrid.DataContext = layers;
            keyLayerDataGrid.DataContext = keyLayers;

            //向导式绑定
            miaoshuTb.SetBinding(TextBox.TextProperty, new Binding("MiaoShu") { Source = editLayer });
            yanXingCB.SetBinding(ComboBox.TextProperty, new Binding("YanXing") { Source = editLayer, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
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


            //关键层其他参数绑定
            meiCengQingJIaoTb.DataContext = this;
            meiCengHouDuTb.DataContext = this;
            xiuZhengXishuTb.DataContext = this;
            hcqZXcdTb.DataContext = this;
            hcqQXcdTb.DataContext = this;
            gZMTJSDTb.DataContext = this;
            jswzjlTb.DataContext = this;


            //创建目录
            foreach (string yanxing in LayerParams.YanXingOpt)
            {
                Directory.CreateDirectory(FILE_PATH + yanxing);
            }



        }


        //打开文件
        public bool openFile(string filePath)
        {
            object obj = DataSaveAndRestore.restoreObj(filePath);
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
                layers.Add(layer);
            }


            //回复关键层数据
            keyLayers.Clear();
            foreach (BaseKeyParams baseParam in data.KeyLayers)
            {
                KeyLayerParams layer = new KeyLayerParams(baseParam);
                keyLayers.Add(layer);

            }

            //回复关键层其他数据
            if (data.KeyLayerData != null && data.KeyLayerData.Count == 7)
            {
                Mcqj = data.KeyLayerData[0];
                Mchd = data.KeyLayerData[1];
                Pjxsxz = data.KeyLayerData[2];
                HcqZXcd = data.KeyLayerData[3];
                HcqQXcd = data.KeyLayerData[4];
                Gzmsd = data.KeyLayerData[5];
                Jswzjl = data.KeyLayerData[6];

                //没有刷新ui
                meiCengQingJIaoTb.Text = data.KeyLayerData[0] + "";
                meiCengHouDuTb.Text = data.KeyLayerData[1] + "";
                xiuZhengXishuTb.Text = data.KeyLayerData[2] + "";
                hcqZXcdTb.Text = data.KeyLayerData[3] + "";
                hcqQXcdTb.Text = data.KeyLayerData[4] + "";
                gZMTJSDTb.Text = data.KeyLayerData[5] + "";
                jswzjlTb.Text = data.KeyLayerData[6] + "";
            }

            return true;

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


        //保存数据到文件
        private void saveFileBtn_Click(object sender, RoutedEventArgs e)
        {

            System.Windows.Forms.SaveFileDialog fileDialog = new System.Windows.Forms.SaveFileDialog();
            fileDialog.Title = "保存文件";
            fileDialog.Filter = "数据文件(*.data)|*.data";
            fileDialog.FileName = "岩层数据.data";

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filePath = fileDialog.FileName;
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
                data.KeyLayerData.Add(Mchd);
                data.KeyLayerData.Add(Pjxsxz);
                data.KeyLayerData.Add(HcqZXcd);
                data.KeyLayerData.Add(HcqQXcd);
                data.KeyLayerData.Add(Gzmsd);
                data.KeyLayerData.Add(Jswzjl);



                DataSaveAndRestore.saveObj(data, filePath);
            }

        }

        

    }



}
