using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Collections;
using System.Windows.Data;

namespace GroundWellDesign
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        //所有已经录入岩层参数
        ObservableCollection<LayerParams> layers = new ObservableCollection<LayerParams>();


        //向导式当前编辑的岩层参数
        LayerParams editLayer = new LayerParams();

        public MainWindow()
        {
            InitializeComponent();
            layers.Add(new LayerParams());
            dataGrid.DataContext = layers;

            dataGrid.LoadingRow += new EventHandler<DataGridRowEventArgs>(dataGrid_LoadingRow);
            dataGrid.UnloadingRow += new EventHandler<DataGridRowEventArgs>(dataGrid_UnloadingRow);

            //向导式binding
           miaoshuTb.SetBinding(TextBox.TextProperty, new Binding("MiaoShu"){Source=editLayer});
           yanXingCB.SetBinding(ComboBox.TextProperty, new Binding("YanXing") { Source = editLayer, UpdateSourceTrigger= UpdateSourceTrigger.PropertyChanged });
           leiJiShenDuTb.SetBinding(TextBox.TextProperty, new Binding("LeiJiShenDu") { Source = editLayer });
           juLiMeiShenDuTb.SetBinding(TextBox.TextProperty, new Binding("JuLiMeiShenDu") { Source = editLayer });
           cengHouTb.SetBinding(TextBox.TextProperty, new Binding("CengHou") { Source = editLayer });
           ziRanMiDuTb.SetBinding(TextBox.TextProperty, new Binding("ZiRanMiDu") { Source = editLayer });
           bianXingMoLiangTb.SetBinding(TextBox.TextProperty, new Binding("BianXingMoLiang") { Source = editLayer });
           kangLaQiangDuTb.SetBinding(TextBox.TextProperty, new Binding("KangLaQiangDu") { Source = editLayer });
           kangYaQiangDuTb.SetBinding(TextBox.TextProperty, new Binding("KangYaQiangDu") { Source = editLayer });
           tanXingMoLiangTb.SetBinding(TextBox.TextProperty, new Binding("TanXingMoLiang") { Source = editLayer});
           boSonBiTb.SetBinding(TextBox.TextProperty, new Binding("BoSonBi") { Source = editLayer });
           neiMoCaJiaoTb.SetBinding(TextBox.TextProperty, new Binding("NeiMoCaJiao") { Source = editLayer});
           nianJuLiTb.SetBinding(TextBox.TextProperty, new Binding("NianJuLi") { Source = editLayer });



        }

        void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        void dataGrid_UnloadingRow(object sender, DataGridRowEventArgs e)
        {
            dataGrid_LoadingRow(sender, e);
            if (dataGrid.Items != null)
            {
                for (int i = 0; i < dataGrid.Items.Count; i++)
                {
                    try
                    {
                        DataGridRow row = dataGrid.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;
                        if (row != null)
                        {
                            row.Header = (i + 1).ToString();
                        }
                    }
                    catch { }
                }
            }

        }


        //删除
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            layers.RemoveAt(dataGrid.SelectedIndex);
            dataGrid.UpdateLayout();
        }

        //下方插入
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            layers.Insert(dataGrid.SelectedIndex + 1, new LayerParams());
        }

        //上移
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            int selectedIndex = dataGrid.SelectedIndex;
            if (selectedIndex == -1 || selectedIndex == 0)
                return;

            LayerParams layer = layers[selectedIndex];
            layers[selectedIndex] = layers[selectedIndex - 1];
            layers[selectedIndex - 1] = layer;
        }

        //下移
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            int selectedIndex = dataGrid.SelectedIndex;
            if (selectedIndex == -1 || selectedIndex == dataGrid.Items.Count - 1)
                return;

            LayerParams layer = layers[selectedIndex];
            layers[selectedIndex] = layers[selectedIndex + 1];
            layers[selectedIndex + 1] = layer;
        }


        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            int layerNum = int.Parse(currLayerTb.Text);
            if(layerNum > layers.Count + 1){
                currLayerTb.Text += "       已经录入" + layers.Count + "层,请修改层号。"; 
                return;
            }

            LayerParams layer = new LayerParams(editLayer);
            if (layerNum == layers.Count + 1)
                layers.Add(layer);
            else
                layers[layerNum - 1] = layer;

            currLayerTb.Text = int.Parse(currLayerTb.Text) + 1 + "";

        }

        private void stopBtn_Click(object sender, RoutedEventArgs e)
        {

            tabControl.SelectedItem = tabControl.Items[0];

        }

    }

    public enum YanXingOpt {黄土,细粒砂岩,泥岩,中粒砂岩,粉砂岩,砂质泥岩,粗粒砂岩,细砂岩,中砂岩, 煤};

    public class LayerParams
    {

        public LayerParams()
        {

        }

        public LayerParams(LayerParams layer)
        {
            YanXing = layer.YanXing;
            LeiJiShenDu = layer.LeiJiShenDu;
            JuLiMeiShenDu = layer.JuLiMeiShenDu;
            CengHou = layer.CengHou;
            ZiRanMiDu = layer.ZiRanMiDu;
            BianXingMoLiang = layer.BianXingMoLiang;
            KangLaQiangDu = layer.KangLaQiangDu;
            KangYaQiangDu = layer.KangYaQiangDu;
            TanXingMoLiang = layer.TanXingMoLiang;
            BoSonBi = layer.BoSonBi;
            NeiMoCaJiao = layer.NeiMoCaJiao;
            NianJuLi = layer.NianJuLi;
            MiaoShu = layer.MiaoShu;
        }

        public YanXingOpt YanXing { get; set; }
        public string LeiJiShenDu { get; set; }
        public string JuLiMeiShenDu { get; set; }
        public string CengHou { get; set; }
        public string ZiRanMiDu { get; set; }
        public string BianXingMoLiang { get; set; }
        public string KangLaQiangDu { get; set; }
        public string KangYaQiangDu { get; set; }
        public string TanXingMoLiang { get; set; }
        public string BoSonBi { get; set; }
        public string NeiMoCaJiao { get; set; }
        public string NianJuLi { get; set; }
        public string MiaoShu { get; set; }
    }

}
