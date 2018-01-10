using GroundWellDesign.ViewModel;
using MathWorks.MATLAB.NET.Arrays;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GroundWellDesign
{
    public partial class Document : Window
    {
        // 实现Grid点击即进入编辑模式
        private void paramGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count == 0)
                return;
            paramGrid.BeginEdit();
        }


        // 保存到数据库
        private void click_saveToDB(object sender, RoutedEventArgs e)
        {
            int selectedIndex = paramGrid.SelectedIndex;
            if (selectedIndex == -1)
            {
                MessageBox.Show("请选中一行");
                return;
            }

            LayerBaseParams layer = layers[selectedIndex].LayerParams;

            //先检查该岩层是否已经已经存在于数据库中
            bool exist = false;
            if(layer.DataBaseKey != null)
            {
                // 打开数据库,若文件不存在会自动创建
                string dbPath = "Data Source =" + ContainerWindow.DATABASE_PATH;
                SQLiteConnection conn = new SQLiteConnection(dbPath);
                conn.Open();

                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "select * from yanceng where id = '" + layer.DataBaseKey + "'";
                var reader = cmd.ExecuteReader();
                exist = reader.HasRows;
                reader.Close();
                conn.Close();
            }

            if(exist)
            {
                MessageBoxResult res = MessageBox.Show("该岩层记录已存在，是否覆盖旧记录？选择\"否\"可新增一条。", "警告", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                switch (res)
                {
                    case MessageBoxResult.Cancel:
                        break;
                    case MessageBoxResult.No:
                        new SaveToDBWindow(layer).ShowDialog();
                        break;
                    case MessageBoxResult.Yes:
                        bool success = DataSaveAndRestore.saveToSqlite(layer, layer.DataBaseKey, true, layer.WellNamePK, true);
                        if (success)
                        {
                            MessageBox.Show("操作成功");
                        }
                        else
                        {
                            MessageBox.Show("操作失败");
                        }
                        break;
                }
            }
            else
            {
                new SaveToDBWindow(layer).ShowDialog();
            }
            
        }


        // 从Excel导入岩层参数
        private void click_importExcel(object sender, RoutedEventArgs e)
        {
            string filePath = FileDialogHelper.getOpenPath("Excel文件(*.xls)|*.xls");
            DataTable dt = null;
            if(filePath != null)
            {
                dt = ExcelHelper.LoadExcelWithAspose(filePath);
                if(dt == null || dt.Columns.Count != 17)
                {
                    MessageBox.Show("读取文件失败，请检查格式。");
                    return;
                }
                int rowIndex = 0;
                try
                {
                    layers.Clear();
                    for (; rowIndex < dt.Rows.Count; ++rowIndex)
                    {
                        LayerBaseParamsViewModel baseParam = new LayerBaseParamsViewModel(this);
                        for (int col = 0; col < 17; ++col)
                        {
                            baseParam.LayerParams[col] = dt.Rows[rowIndex][col];
                        }
                        layers.Add(baseParam);
                    }
                }
                catch(Exception)
                {
                    MessageBox.Show("读取文件失败,请检查文件第" + rowIndex + "行数据的格式问题。");
                }
            }
        }
         

        // 下方增加行
        private void click_addRow(object sender, RoutedEventArgs e)
        {
            int selectedIndex = paramGrid.SelectedIndex;
            if (selectedIndex == -1) //未选择行
            {
                layers.Add(new LayerBaseParamsViewModel(this));
                return;
            }
            if (selectedIndex >= layers.Count)  //选择了空行
            {
                layers.Insert(selectedIndex, new LayerBaseParamsViewModel(this));
                return;
            }
            layers.Insert(selectedIndex + 1, new LayerBaseParamsViewModel(this));  //选择了非空行
        }


        // 删除选中行
        private void click_delRow(object sender, RoutedEventArgs e)
        {
            
            int selectedIndex = paramGrid.SelectedIndex;
            if (selectedIndex == -1)
            {
                MessageBox.Show("请选中一行...");
                return;
            }

            if (selectedIndex >= layers.Count)  //选择了空行
                return;

            if (MessageBox.Show("确定删除第" + (selectedIndex) + "层数据吗？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                layers.RemoveAt(selectedIndex);
                paramGrid.SelectedIndex = selectedIndex;
            }

            if (layers.Count == 0)
            {
                LayerBaseParamsViewModel param = new LayerBaseParamsViewModel(this);
                param.LayerParams.YanXing = "地表";
                layers.Add(param);
                return;
            }

        }


        // 上移
        private void click_upRow(object sender, RoutedEventArgs e)
        {
            int selectedIndex = paramGrid.SelectedIndex;
            if (selectedIndex == -1)
            {
                MessageBox.Show("请选中一行...");
                return;
            }
            //选择了第一行 或者 下标超出已经录入范围
            if (selectedIndex == 0 || selectedIndex >= layers.Count)
                return;

            //交换
            LayerBaseParamsViewModel layer = layers[selectedIndex];
            layers[selectedIndex] = layers[selectedIndex - 1];
            layers[selectedIndex - 1] = layer;
            paramGrid.SelectedIndex = selectedIndex - 1;
        }

        // 下移
        private void click_downRow(object sender, RoutedEventArgs e)
        {
            int selectedIndex = paramGrid.SelectedIndex;
            if (selectedIndex == -1)
            {
                MessageBox.Show("请选中一行...");
                return;
            }

            //下标超出已经录入范围
            if (selectedIndex >= layers.Count - 1)
                return;

            // 交换
            LayerBaseParamsViewModel layer = layers[selectedIndex];
            layers[selectedIndex] = layers[selectedIndex + 1];
            layers[selectedIndex + 1] = layer;
            paramGrid.SelectedIndex = selectedIndex + 1;
        }

        // 复制行
        private void click_copyRow(object sender, RoutedEventArgs e)
        {
            int selectedIndex = paramGrid.SelectedIndex;
            if (selectedIndex == -1)
            {
                MessageBox.Show("请选中一行...");
                return;
            }

            //下标超出已经录入范围
            if (selectedIndex >= layers.Count)
                return;

            copyedLayer = layers[selectedIndex].LayerParams.Clone() as LayerBaseParams;
        }

        // 粘贴行
        private void click_pasteRow(object sender, RoutedEventArgs e)
        {
            if(copyedLayer == null)
            {
                MessageBox.Show("粘贴板无数据。");
                return;
            }

            LayerBaseParamsViewModel newLine = new LayerBaseParamsViewModel(this, copyedLayer);
            int selectedIndex = paramGrid.SelectedIndex;
            if (selectedIndex == -1) //未选择行
            {
                layers.Add(newLine);
                return;
            }
            if (selectedIndex >= layers.Count)  //选择了空行
            {
                layers.Insert(selectedIndex, newLine);
                return;
            }
            layers.Insert(selectedIndex + 1, newLine);  //选择了非空行
        }
        
        
        // 计算关键层和竖三带
        private void click_showKeyRow(object sender, RoutedEventArgs e)
        {
            //先撤销之前的变色
            foreach (KeyLayerParamsViewModel nbr in keyLayers)
            {
                layers[nbr.Params.Ycbh].IsKeyLayer = false;
            }

            //当前关键层变色显示
            int[] biaoHaoList = null;
            double[] pjxsList = null;
            bool isOk = getKeyLayer(ref biaoHaoList, ref pjxsList);

            if(!isOk)
            {
                return;
            }
            else if (biaoHaoList == null || pjxsList == null)
            {
                MessageBox.Show("未计算出关键层，请检查数据合理性。");
                return;
            }

            keyLayers.Clear();
            int count = biaoHaoList.Length;
            for (int i = 0; i < count; i++)
            {
                KeyLayerParamsViewModel layer = new KeyLayerParamsViewModel(this);
                layer.Ycbh = biaoHaoList[i] - 1;
                layer.Fypjxs = pjxsList[i];

                layer.Ycsd = layers[biaoHaoList[i] - 1].LeiJiShenDu;
                layer.Mcms = layers[biaoHaoList[i] - 1].JuLiMeiShenDu;
                layer.Fypjxsxz = layer.Fypjxs * pjxsxz;
                keyLayers.Add(layer);

                layers[biaoHaoList[i] - 1].IsKeyLayer = true;
            }

            //竖三带计算
            try
            {
                // 冒落带
                var array1 = (MWNumericArray)logic.calHm(FuYanXCL, CaiGao, SuiZhangXS, Mcqj);
                double maoLuoDai = array1.ToScalarDouble();
                maoLuoDaiTb.Text = maoLuoDai.ToString("f3");
                // 裂隙带
                var array2 = (MWNumericArray)logic.calHl(CaiGao, 1);
                double lieXiDai = array2.ToScalarDouble();
                lieXiDaiTb.Text = lieXiDai.ToString("f3");
                // 弯曲下沉带 = 煤层以上深度 - 裂隙带高度
                double wanquDai = layers[0].JuLiMeiShenDu - lieXiDai;
                wanQuDaiTb.Text = wanquDai.ToString("f3");
            }
            catch (Exception)
            {
                e.ToString();
                MessageBox.Show("竖三带计算出错，请检查数据合理性");
            }

        }
        // 地面井设计需要用到该参数
        int wanQuDaiIndex = 0;


        // 获取岩层参数
        private double[,] getParams(int meiIndex)
        {
            double[,] res = new double[meiIndex + 2, 11];

            for (int i = 0; i <= meiIndex + 1; i++)
            {
                LayerBaseParams param = layers[i].LayerParams;
                if (param.YanXing == "地表")
                {
                    res[i, 0] = 1;
                }
                else if (param.YanXing == "黄土")
                {
                    res[i, 0] = 2;
                }
                else if (param.YanXing == "煤")
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


        // 二维转置
        private double[] arrayTrans(double[,] input)
        {
            int width = input.GetLength(0);
            int length = input.GetLength(1);

            double[] output = new double[length * width];

            for (int i = 0; i < width; i++)
                for (int j = 0; j < length; j++)
                {
                    output[i * length + j] = input[i, j];
                }

            return output;

        }



        //计算关键层接口
        private bool getKeyLayer(ref int[] bianHao, ref double[] pjxs)
        {
            //判断是否有煤层 或者 煤层是最后一层 或者 第一层不是地表 则不能计算
            int meiIndex = -1;
            for (int i = layers.Count - 1; i >= 0; --i)
            {
                if(layers[i].YanXing.Equals("煤"))
                {
                    meiIndex = i;
                    break;
                }
            }
            if (meiIndex == -1 || meiIndex == layers.Count - 1 || !layers[0].YanXing.Equals("地表"))
            {
                MessageBox.Show("数据录入有误，请检查。(第一层应为地表，煤层应有底板。)");
                return false;
            }

            double[,] data = getParams(meiIndex);
            double[] rowData = arrayTrans(data);
            MWNumericArray mwdata = new MWNumericArray(rowData.Length / 11, 11, rowData);

            try
            {
                MWArray[] result = logic.yancengzuhe(2, mwdata, FuYanXCL, CaiGao, SuiZhangXS, Mcqj);
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

                return true;

            }
            catch (Exception e)
            {
                MessageBox.Show("关键层计算出现错误，请检查数据合理性。");
                return false;
            }

        }


        private void yancengListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = yancengListBox.SelectedIndex;
            if (index != -1)
            {
                paramGrid.SelectedIndex = index;
                paramGrid.ScrollIntoView(paramGrid.Items[index]);
            }
        }

        private void paramGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = paramGrid.SelectedIndex;
            if (index != -1)
            {
                yancengListBox.SelectedIndex = index;
                yancengListBox.ScrollIntoView(yancengListBox.Items[index]);
            }

        }


    }


    public class RangeValidationRule : ValidationRule
    {
        public double Min
        {
            get;
            set;
        }
        public bool CanEqualMin
        {
            get;
            set;
        }
        public double Max
        {
            get;
            set;
        }
        public bool CanEqualMax
        {
            get;
            set;
        }

        public RangeValidationRule()
        {
            Min = 0;
            CanEqualMin = true;
            Max = double.PositiveInfinity;
            CanEqualMax = true;

        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double d = 0;
            if (double.TryParse(value.ToString(), out d))
            {
                if (CanEqualMin && d >= Min || d > Min)
                    if (CanEqualMax && d <= Max || d < Max)
                    {
                        return new ValidationResult(true, null);
                    }
            }
            string minNode = CanEqualMin ? "[" : "(";
            string maxNode = CanEqualMax ? "]" : ")";
            return new ValidationResult(false, "范围应该为" + minNode + Min + ", " + Max + maxNode);
        }
    }


}
