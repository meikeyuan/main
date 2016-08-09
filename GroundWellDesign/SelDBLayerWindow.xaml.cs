using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GroundWellDesign
{
    /// <summary>
    /// SelectLayerWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SelectLayerWindow : Window
    {


        public ObservableCollection<LayerBaseParams> existedLayers = new ObservableCollection<LayerBaseParams>();
        private List<String> items = new List<string>();
        private string cus = "所有矿井";

        private string yanXing;

        private bool allSelcted;
        public LayerBaseParams WantedLayer
        {
            set;
            get;
        }

        public SelectLayerWindow(string yanXing, LayerBaseParams wantedLayer)
        {
            InitializeComponent();


            //加载矿井下拉选择列表
            DataSaveAndRestore.getAllWellName(items);
            items.Insert(0, cus);
            wellNameCombo.ItemsSource = items;
            wellNameCombo.SelectedIndex = 0;


            //加载所有数据库岩层
            DataSaveAndRestore.getDBLayers(existedLayers, yanXing, null);
            if(existedLayers.Count == 0)
            {
                Close();
            }

            existedLayerGrid.DataContext = existedLayers;
            WantedLayer = wantedLayer;
            this.yanXing = yanXing;
        }


        private void grid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

            if (e.AddedCells.Count == 0)
                return;
             //进入编辑模式  这样单击一次就可以打勾了
             existedLayerGrid.BeginEdit();
        }

        private void wellNameCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //加载数据库岩层
            if(wellNameCombo.SelectedIndex == 0)
                DataSaveAndRestore.getDBLayers(existedLayers, yanXing, null);
            else
                DataSaveAndRestore.getDBLayers(existedLayers, yanXing, wellNameCombo.SelectedValue.ToString());
            existedLayerGrid.DataContext = existedLayers;
        }

        private void selectAllBtn_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < existedLayerGrid.Items.Count; i++)
            {
                var row = existedLayerGrid.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;
                if (row != null)
                {
                    var box = existedLayerGrid.Columns[0].GetCellContent(row) as CheckBox;
                    if (box != null)
                    {
                        if (allSelcted)
                        {
                            box.IsChecked = false;
                        }
                        else
                        {
                            box.IsChecked = true;
                        }
                    }
                }
            }
            allSelcted = !allSelcted;
        }


        private void selectAvgBtn_Click(object sender, RoutedEventArgs e)
        {
            LayerBaseParams tmpLayer = new LayerBaseParams(WantedLayer.mainWindow);
            tmpLayer.yanXing = yanXing;

            //将选择的序号存入list
            List<int> selectedList = new List<int>();
            for (int i = 0; i < existedLayerGrid.Items.Count; i++)
            {
                var row = existedLayerGrid.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;
                if (row != null)
                {
                    var box = existedLayerGrid.Columns[0].GetCellContent(row) as CheckBox;
                    if (box != null)
                    {
                        if (box.IsChecked == true)
                        {
                            selectedList.Add(i);
                        }
                    }
                }
            }


            //首先判断有没有选中
            int count = selectedList.Count;
            if (count <= 0)
            {
                MessageBox.Show("请至少选择一项");
                return;
            }else if(count == 1)
            {
                WantedLayer.copyAndEventEcpYanXing(existedLayers[selectedList[0]]);
                this.Close();
                return;
            }

            //遍历编号list
            foreach(int i in selectedList)
            {
                LayerBaseParams layer = existedLayers[i];
                tmpLayer.leiJiShenDu += layer.leiJiShenDu;
                tmpLayer.juLiMeiShenDu += layer.juLiMeiShenDu;
                tmpLayer.cengHou += layer.cengHou;
                tmpLayer.ziRanMiDu += layer.ziRanMiDu;
                tmpLayer.bianXingMoLiang += layer.bianXingMoLiang;
                tmpLayer.kangLaQiangDu += layer.kangLaQiangDu;
                tmpLayer.kangYaQiangDu += layer.kangYaQiangDu;
                tmpLayer.tanXingMoLiang += layer.tanXingMoLiang;
                tmpLayer.boSonBi += layer.boSonBi;
                tmpLayer.neiMoCaJiao += layer.neiMoCaJiao;
                tmpLayer.nianJuLi += layer.nianJuLi;
                tmpLayer.q0 += layer.q0;
                tmpLayer.q1 += layer.q1;
                tmpLayer.q2 += layer.q2;
                //tmpLayer.miaoShu = layer.miaoShu;
                //tmpLayer.dataBaseKey = null;
            }
            WantedLayer.LeiJiShenDu = tmpLayer.leiJiShenDu / count;
            WantedLayer.JuLiMeiShenDu = tmpLayer.juLiMeiShenDu / count;
            WantedLayer.CengHou = tmpLayer.cengHou / count;
            WantedLayer.ZiRanMiDu = tmpLayer.ziRanMiDu / count;
            WantedLayer.BianXingMoLiang = tmpLayer.bianXingMoLiang / count;
            WantedLayer.KangLaQiangDu = tmpLayer.kangLaQiangDu / count;
            WantedLayer.KangYaQiangDu = tmpLayer.kangYaQiangDu / count;
            WantedLayer.TanXingMoLiang = tmpLayer.tanXingMoLiang / count;
            WantedLayer.BoSonBi = tmpLayer.boSonBi / count;
            WantedLayer.NeiMoCaJiao = tmpLayer.neiMoCaJiao / count;
            WantedLayer.NianJuLi = tmpLayer.nianJuLi / count;
            WantedLayer.Q0 = tmpLayer.q0 / count;
            WantedLayer.Q1 = tmpLayer.q1 / count;
            WantedLayer.Q2 = tmpLayer.q2 / count;
            WantedLayer.MiaoShu = "取数据库平均值";
            WantedLayer.dataBaseKey = null;
            this.Close();
        }


    }
}
