using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Data;
using AxMxDrawXLib;
using System.Collections.ObjectModel;
using MathWorks.MATLAB.NET.Arrays;
using mky;

namespace GroundWellDesign
{

    public partial class MainWindow : Window
    {

        AxMxDrawX cadViewer;
        //所有已经录入岩层参数
        public static ObservableCollection<LayerParams> layers = new ObservableCollection<LayerParams>();
        public ObservableCollection<LayerParams> Layers
        {
            get { return layers; }
            set { layers = value; }
        }

        private ObservableCollection<KeyLayerParams> keyLayers = new ObservableCollection<KeyLayerParams>();
        public ObservableCollection<KeyLayerParams> KeyLayers
        {
            get { return keyLayers; }
            set { keyLayers = value; }
        }


        public MainWindow()
        {
            InitializeComponent();

            MWArray array = new MWNumericArray(1, 1);
            MkyLogic logic = new MkyLogic();
            logic.ToString();


            //岩层参数录入初始化
           // LayerParams dibiao = new LayerParams();
         //   dibiao.YanXing = LayerParams.YanXingOpt[0];
         //   layers.Add(dibiao);
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

            initialView();

        }

        public void initialView()
        {
            paramGrid.DataContext = layers;
            keyLayerDataGrid.DataContext = keyLayers;

            //向导式binding
            miaoshuTb.SetBinding(TextBox.TextProperty, new Binding("MiaoShu") { Source = editLayer });
            yanXingCB.SetBinding(ComboBox.TextProperty, new Binding("YanXing") { Source = editLayer, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            leiJiShenDuTb.SetBinding(TextBox.TextProperty, new Binding("LeiJiShenDu") { Source = editLayer });
            cengHouTb.SetBinding(TextBox.TextProperty, new Binding("CengHou") { Source = editLayer });
            ziRanMiDuTb.SetBinding(TextBox.TextProperty, new Binding("ZiRanMiDu") { Source = editLayer });
            bianXingMoLiangTb.SetBinding(TextBox.TextProperty, new Binding("BianXingMoLiang") { Source = editLayer });
            kangLaQiangDuTb.SetBinding(TextBox.TextProperty, new Binding("KangLaQiangDu") { Source = editLayer });
            kangYaQiangDuTb.SetBinding(TextBox.TextProperty, new Binding("KangYaQiangDu") { Source = editLayer });
            tanXingMoLiangTb.SetBinding(TextBox.TextProperty, new Binding("TanXingMoLiang") { Source = editLayer });
            boSonBiTb.SetBinding(TextBox.TextProperty, new Binding("BoSonBi") { Source = editLayer });
            neiMoCaJiaoTb.SetBinding(TextBox.TextProperty, new Binding("NeiMoCaJiao") { Source = editLayer });
            nianJuLiTb.SetBinding(TextBox.TextProperty, new Binding("NianJuLi") { Source = editLayer });
            q0Tb.SetBinding(TextBox.TextProperty, new Binding("Q0") { Source = editLayer });
            q1Tb.SetBinding(TextBox.TextProperty, new Binding("Q1") { Source = editLayer });
            q2Tb.SetBinding(TextBox.TextProperty, new Binding("Q2") { Source = editLayer });
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
                object obj = DataSaveAndRestore.restoreObj(filePath);
                if (obj == null || !(obj is DataSaveAndRestore.DataToSave))
                {
                    MessageBox.Show("打开文件错误");
                    return;
                }
                DataSaveAndRestore.DataToSave data = obj as DataSaveAndRestore.DataToSave;

                //回复基本参数
                Layers.Clear();
                foreach (BaseParams baseParam in data.Layers)
                {
                    LayerParams layer = new LayerParams(baseParam);
                    Layers.Add(layer);

                }

                //回复关键层数据
                KeyLayers.Clear();
                foreach (BaseKeyParams baseParam in data.KeyLayers)
                {
                    KeyLayerParams layer = new KeyLayerParams(baseParam);
                    KeyLayers.Add(layer);

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
                foreach (LayerParams layerParam in Layers)
                {
                    data.Layers.Add(new BaseParams(layerParam));
                }

                //关键层参数
                data.KeyLayers = new ObservableCollection<BaseKeyParams>();
                foreach (KeyLayerParams layerParam in KeyLayers)
                {
                    data.KeyLayers.Add(new BaseKeyParams(layerParam));
                }

                DataSaveAndRestore.saveObj(data, filePath);
            }

        }

    }

}
