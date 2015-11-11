using MathWorks.MATLAB.NET.Arrays;
using mky;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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


        //显示关键层
        private void click_showKeyRow(object sender, RoutedEventArgs e)
        {
            //先撤销之前的变色
            foreach (KeyLayerParams nbr in keyLayers)
            {
                var row = paramGrid.ItemContainerGenerator.ContainerFromIndex(nbr.ycbh - 1) as DataGridRow;
                if(row != null)
                row.Background = new SolidColorBrush(Colors.White);
                paramGrid.UpdateLayout();
            }


            //当前关键层变色显示
            int[] biaoHaoList = null;
            double[] pjxsList = null;

            getKeyLayer(ref biaoHaoList, ref pjxsList);


            if (biaoHaoList == null || pjxsList == null)
            {
                //keyLayerNbr.Clear();
                return;
            }

            keyLayers.Clear();
            int count = biaoHaoList.Length;
            for (int i = 0; i < count; i++)
            {
                KeyLayerParams layer = new KeyLayerParams();
                layer.ycbh = biaoHaoList[i];
                layer.fypjxs = pjxsList[i];

                layer.ycsd = layers[biaoHaoList[i] - 1].LeiJiShenDu;
                layer.mcms = layers[biaoHaoList[i] - 1].JuLiMeiShenDu;
                layer.fypjxsxz = layer.fypjxs * pjxsxz;
                keyLayers.Add(layer);


                var row = paramGrid.ItemContainerGenerator.ContainerFromIndex(biaoHaoList[i] - 1) as DataGridRow;
                if(row != null)
                row.Background = new SolidColorBrush(Colors.Yellow);

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

            int count = MainWindow.layers.Count;
            for (count--; count > 0 && MainWindow.layers[count].yanXing != "煤"; count--) ;

            //判断是否有煤层 或者 煤层是最后一层 或者第一层不是地表 则失败
            if (count == 0 || count == MainWindow.layers.Count - 1 || !MainWindow.layers[0].yanXing.Equals("地表"))
                return null;


            double[,] res = new double[count + 2, 11];

            for (int i = 0; i <= count + 1; i++)
            {
                LayerParams param = layers[i];
                if (param.yanXing == "地表")
                {
                    res[i, 0] = 1;
                }
                else if (param.yanXing == "黄土")
                {
                    res[i, 0] = 2;
                }
                else if (param.yanXing == "煤")
                {
                    res[i, 0] = 9;
                }
                else
                {
                    res[i, 0] = 0;
                }

                res[i, 1] = param.LeiJiShenDu;
                res[i, 2] = param.ZiRanMiDu;
                res[i, 3] = param.BianXingMoLiang;
                res[i, 4] = param.KangLaQiangDu;
                res[i, 5] = param.KangYaQiangDu;
                res[i, 6] = param.TanXingMoLiang;
                res[i, 7] = param.BoSonBi;
                res[i, 8] = param.NeiMoCaJiao;
                res[i, 9] = param.NianJuLi;

                if (caiDongComBox.SelectedIndex == 0)
                {
                    res[i, 10] = param.Q0;

                }
                else if (caiDongComBox.SelectedIndex == 1)
                {
                    res[i, 10] = param.Q1;
                }
                else
                {
                    res[i, 10] = param.Q2;
                }
            }

            return res;

        }


        private double[] arrayTrans(double[,] input)
        {
            int width = input.GetLength(0);
            int length = input.GetLength(1);

            double[] output = new double[length * width];

            for(int i = 0; i < width; i++)
                for (int j = 0; j < length; j++)
                {
                    output[i * length + j] = input[i, j];
                }

            return output;

        }


        //需要提供的计算出地标最大沉降位移接口
        private double getGroundOffset()
        {
            logic.calWmax();
            return 2;

        }


        //需要提供的计算出关键层接口
        private void getKeyLayer(ref int[] bianHao, ref double[] pjxs)
        {

            double[,] data = getParams();
            if (data == null)
            {
                MessageBox.Show("数据录入有误，请检查。(第一层应为地表，应该有煤层底板。)");
                return;
            }
               

            double[] rowData = arrayTrans(data);
            MWNumericArray mwdata = new MWNumericArray(89, 11, rowData);

            try{
                MWArray[] result = logic.yancengzuhe(2, mwdata);
                MWNumericArray biaoHaoList = (MWNumericArray)result[0];
                MWNumericArray pjxsList = (MWNumericArray)result[1];

                double[] outBiaoHao = (double[])biaoHaoList.ToVector(MWArrayComponent.Real);
                int length = outBiaoHao.Length;
                bianHao = new int[length];
                for (int i = 0; i < length; i++)
                {
                    bianHao[i] = (int)outBiaoHao[i];
                }


                    pjxs = (double[])pjxsList.ToVector(MWArrayComponent.Real);

                return;

            }catch(Exception e){
                e.ToString();
                MessageBox.Show("计算出现错误，请检查数据准确性");
                return;

            }

        }


        //保存岩层数据
        private void saveYanCengBtn_Click(object sender, RoutedEventArgs e)
        {
            bool bSuccess = true;
            foreach (LayerParams layer in layers)
            {
                string path = FILE_PATH + layer.yanXing;
                if(layer.dataBaseNum == 0)
                {
                    int count = Directory.GetFiles(path).Length;
                    layer.dataBaseNum = count + 1;
                }
                BaseParams baseParam = new BaseParams(layer);
                bSuccess &= DataSaveAndRestore.saveObj(baseParam, path + "\\" + layer.dataBaseNum);
            }
            if(bSuccess)
            {
                MessageBox.Show("全部保存成功");
            }
            else
            {
                MessageBox.Show("部分保存失败");
            }
        }
    }
}
