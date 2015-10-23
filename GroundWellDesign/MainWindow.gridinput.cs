using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GroundWellDesign
{
    public partial class MainWindow : Window

    {

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
            if (currentCell.Column == paramGrid.Columns[0] || currentCell.Column == paramGrid.Columns[12])   //Columns[]从0开始  我这的ComboBox在第四列  所以为3  
            {
                paramGrid.BeginEdit();    //  进入编辑模式  这样单击一次就可以选择ComboBox里面的值了  
            }
        }


        //从文件恢复数据
        private void openFileBtn_Click(object sender, RoutedEventArgs e)
        {

            System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.Title = "打开文件";
            fileDialog.Filter = "bin文件(*.bin)|*.bin";

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filePath = fileDialog.FileName;
                object obj = DataSaveAndRestore.restoreObj(filePath);
                if(obj == null || !(obj is DataSaveAndRestore.DataToSave))
                {
                    MessageBox.Show("打开文件错误");
                    return;
                }
                DataSaveAndRestore.DataToSave data = obj as DataSaveAndRestore.DataToSave;
                Layers.Clear();
                foreach(BaseParams baseParam in data.Layers)
                {
                    LayerParams layer = new LayerParams(baseParam);
                    Layers.Add(layer);

                }

                KeyLayers = data.KeyLayers;
                KeyLayerNbr = data.KeyLayerNbr;
                //对象引用已经改变 需要重新绑定
                initialView();
            }

        }


        //保存数据到文件
        private void saveFileBtn_Click(object sender, RoutedEventArgs e)
        {

            System.Windows.Forms.SaveFileDialog fileDialog = new System.Windows.Forms.SaveFileDialog();
            fileDialog.Title = "保存文件";
            fileDialog.Filter = "bin文件(*.bin)|*.bin";

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filePath = fileDialog.FileName;
                DataSaveAndRestore.DataToSave data = new DataSaveAndRestore.DataToSave();
                data.Layers = new ObservableCollection<BaseParams>();
                foreach (LayerParams layerParam in Layers)
                {
                    data.Layers.Add(new BaseParams(layerParam));
                }
                data.KeyLayerNbr = this.keyLayerNbr;
                data.KeyLayers = this.keyLayers;

                DataSaveAndRestore.saveObj(data, filePath);
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

        //显示沉降位移
        private void click_Wmax(object sender, RoutedEventArgs e)
        {
            wmaxText.Text = getGroundOffset() + "M";
        }



        //显示关键层
        private void click_showKeyRow(object sender, RoutedEventArgs e)
        {
            keyLayerNbr = getKeyLayerNbr();


            foreach (int nbr in keyLayerNbr)
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
            tabControl.SelectedIndex = 2;
        }
        




        //模块一获取岩层参数的回调函数
        public double[,] getParams()
        {

            int layerCount = layers.Count;
            double[,] res = new double[layerCount - 1, 12];

            for (int i = 1; i < layerCount; i++)
            {
                LayerParams param = layers[i];


                res[i - 1, 0] = param.LeiJiShenDu;
                res[i - 1, 1] = param.JuLiMeiShenDu;
                res[i - 1, 2] = param.CengHou;
                res[i - 1, 3] = param.ZiRanMiDu;
                res[i - 1, 4] = param.BianXingMoLiang;
                res[i - 1, 5] = param.KangLaQiangDu;
                res[i - 1, 6] = param.KangYaQiangDu;
                res[i - 1, 7] = param.TanXingMoLiang;
                res[i - 1, 8] = param.BoSonBi;
                res[i - 1, 9] = param.NeiMoCaJiao;
                res[i - 1, 10] = param.NianJuLi;


                if (caiDongComBox.SelectedIndex == 0)
                {
                    res[i - 1, 11] = param.Q0;

                }
                else if (caiDongComBox.SelectedIndex == 1)
                {
                    res[i - 1, 11] = param.Q1;
                }
                else
                {
                    res[i - 1, 11] = param.Q2;
                }
            }

            return res;

        }



        //需要提供的计算出地标最大沉降位移接口
        private double getGroundOffset()
        {
            Random ran = new Random();
            return ran.NextDouble();

        }


        //需要提供的计算出关键层接口
        private List<int> getKeyLayerNbr()
        {
            List<int> list = new List<int>();
            list.Clear();
            list.Add(1);
            list.Add(3);

            return list;
        }

    }
}
