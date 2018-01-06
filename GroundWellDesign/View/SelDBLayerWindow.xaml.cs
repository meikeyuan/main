using GroundWellDesign.ViewModel;
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


        public ObservableCollection<LayerBaseParamsViewModel> existedLayers = new ObservableCollection<LayerBaseParamsViewModel>();
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


            // 取List平均值
            int count = selectedList.Count;
            if (count <= 0)
            {
                MessageBox.Show("请至少选择一项");
                return;
            }
            
            if(count == 1)
            {
                for (int j = 1; j <= 15; ++j)
                {
                    WantedLayer[j] = existedLayers[selectedList[0]].LayerParams[j];
                }
                this.Close();
                return;
            }

            //遍历编号list
            LayerBaseParams tmpLayer = new LayerBaseParams();
            foreach(int i in selectedList)
            {
                LayerBaseParams layer = existedLayers[i].LayerParams;
                for (int j = 1; j <= 15; ++j)
                {
                    double res = (double)tmpLayer[j] + (double)layer[j];
                    tmpLayer[j] = res;
                }
            }
            for (int j = 1; j <= 15; ++j)
            {
                double res = (double)tmpLayer[j] / count;
                WantedLayer[j] = res;
            }
            WantedLayer.DataBaseKey = null;
            WantedLayer.WellNamePK = null;
            this.Close();
        }


    }
}
