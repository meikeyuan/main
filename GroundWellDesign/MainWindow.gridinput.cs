using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GroundWellDesign
{
    public partial class MainWindow : Window

    {
        //所有已经录入岩层参数
        ObservableCollection<LayerParams> layers = new ObservableCollection<LayerParams>();

        //关键层
        List<int> keyLayerNbr = new List<int>();
        ObservableCollection<OtherData> keyLayers = new ObservableCollection<OtherData>();


        void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        void dataGrid_UnloadingRow(object sender, DataGridRowEventArgs e)
        {
            dataGrid_LoadingRow(sender, e);
            if (paramGrid.Items != null)
            {
                for (int i = 0; i < paramGrid.Items.Count; i++)
                {
                    try
                    {
                        DataGridRow row = paramGrid.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;
                        if (row != null)
                        {
                            row.Header = (i + 1).ToString();
                        }
                    }
                    catch { }
                }
            }

        }


        private void paramGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

            if (e.AddedCells.Count == 0)
                return;
            var currentCell = e.AddedCells[0];
            if (currentCell.Column == paramGrid.Columns[0] || currentCell.Column == paramGrid.Columns[1])   //Columns[]从0开始  我这的ComboBox在第四列  所以为3  
            {
                paramGrid.BeginEdit();    //  进入编辑模式  这样单击一次就可以选择ComboBox里面的值了  
            }
        }





        //下方增加行
        private void click_addRow(object sender, RoutedEventArgs e)
        {

            int selectedIndex = paramGrid.SelectedIndex;
            if (selectedIndex == -1) //未选择行
            {
                layers.Add(new LayerParams());
                return;
            }
            if (selectedIndex >= layers.Count)  //选择了空行
            {
                layers.Insert(selectedIndex, new LayerParams());
                return;
            }
            layers.Insert(selectedIndex + 1, new LayerParams());  //选择了非空行
        }


        //删除选中行
        private void click_delRow(object sender, RoutedEventArgs e)
        {

            int selectedIndex = paramGrid.SelectedIndex;
            //未选择行
            if (selectedIndex == -1)
            {
                MessageBox.Show("请选中一行...");
                return;
            }

            if (selectedIndex >= layers.Count)  //选择了空行
                return;

            if (MessageBox.Show("确定删除第" + (selectedIndex + 1) + "层数据吗？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                layers.RemoveAt(selectedIndex);//选择了非空行
            }
        }


        //上移
        private void click_upRow(object sender, RoutedEventArgs e)
        {
            int selectedIndex = paramGrid.SelectedIndex;
            //未选择行
            if (selectedIndex == -1)
            {
                MessageBox.Show("请选中一行...");
                return;
            }
            //选择了第一行 或者 下标超出已经录入范围
            if (selectedIndex == 0 || selectedIndex > layers.Count - 1)
                return;

            //交换
            LayerParams layer = layers[selectedIndex];
            layers[selectedIndex] = layers[selectedIndex - 1];
            layers[selectedIndex - 1] = layer;
        }

        //下移
        private void click_downRow(object sender, RoutedEventArgs e)
        {
            int selectedIndex = paramGrid.SelectedIndex;
            //未选择行
            if (selectedIndex == -1)
            {
                MessageBox.Show("请选中一行...");
                return;
            }

            //下标超出已经录入范围
            if (selectedIndex >= layers.Count - 1)
                return;

            LayerParams layer = layers[selectedIndex];
            layers[selectedIndex] = layers[selectedIndex + 1];
            layers[selectedIndex + 1] = layer;
        }


        private void click_Wmax(object sender, RoutedEventArgs e)
        {
            //to do  计算出地表最大沉降位移
            wmaxText.Text = "100m";
            //。。。
        }


        //显示关键层 黄色
        private void click_showKeyRow(object sender, RoutedEventArgs e)
        {
            //to do  计算出关键层  并将关键层数据存入keyLayersNbr
            keyLayerNbr.Clear();
            keyLayerNbr.Add(1);
            keyLayerNbr.Add(3);
            //....

           
            foreach(int nbr in keyLayerNbr)
            {
                var row = paramGrid.ItemContainerGenerator.ContainerFromItem(paramGrid.Items[nbr - 1]) as DataGridRow;
                row.Background = new SolidColorBrush(Colors.Yellow);

                //添加关键层
                OtherData data = new OtherData();
                data.YanCengShenDu = layers[nbr - 1].LeiJiShenDu;
                data.MeiCengMaiShen = layers[nbr - 1].JuLiMeiShenDu;
                keyLayers.Add(new OtherData());
                
            }
            
        }


        //转到输入其他数据
        private void click_inputOtherData(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 3;
        }

        

    }
}
