using MathWorks.MATLAB.NET.Arrays;
using System;
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
        private void paramGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

            if (e.AddedCells.Count == 0)
                return;
            paramGrid.BeginEdit();    //进入编辑模式
        }


        //保存到数据库
        private void click_saveToDB(object sender, RoutedEventArgs e)
        {
            int selectedIndex = paramGrid.SelectedIndex;
            //未选择行
            if (selectedIndex == -1)
            {
                MessageBox.Show("请选中一行");
                return;
            }

            LayerBaseParams layer = layers[selectedIndex];
/*            string path = DATABASE_PATH + layer.yanXing;
            if (layer.dataBaseNum == 0 || !File.Exists(path + "\\" + layer.dataBaseNum))
            {
                int count = Directory.GetFiles(path).Length;
                layer.dataBaseNum = count + 1;
            }
            else
            {
                MessageBoxResult res = MessageBox.Show("该条记录已存在，覆盖旧的数据吗？选择否则新增一条", "警告", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                switch (res)
                {
                    case MessageBoxResult.Cancel:
                        return;
                    case MessageBoxResult.No:
                        int count = Directory.GetFiles(path).Length;
                        layer.dataBaseNum = count + 1;
                        break;
                    case MessageBoxResult.Yes:
                        break;
                }
            }
            BaseParams baseParam = new BaseParams(layer);
            bool bSuccess = DataSaveAndRestore.saveObj(baseParam, path + "\\" + layer.dataBaseNum   */


            //先检查该岩层是否已经已经存在于数据库中
            if(layer.dataBaseKey != null)
            {
                SQLiteConnection conn = null;
                string dbPath = "Data Source =" + ContainerWindow.DATABASE_PATH;
                conn = new SQLiteConnection(dbPath);//创建数据库实例，指定文件位置
                conn.Open();//打开数据库，若文件不存在会自动创建

                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "select * from yanceng where id = '" + layer.dataBaseKey + "'";//插入几条数据
                var reader = cmd.ExecuteReader();
                bool exist = reader.HasRows;
                reader.Close();
                conn.Close();

                if(exist)
                {
                    MessageBoxResult res = MessageBox.Show("该记录已存在，覆盖旧的数据吗？选择\"否\"新增一条", "警告", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                    switch (res)
                    {
                        case MessageBoxResult.Cancel:
                            break;
                        case MessageBoxResult.No:
                            new SaveToDBWindow(layer).ShowDialog();
                            break;
                        case MessageBoxResult.Yes:
                            bool success = DataSaveAndRestore.saveToSqlite(layer, layer.dataBaseKey, true, layer.wellNamePK, true);
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
                    return;
                }
            }
            new SaveToDBWindow(layer).ShowDialog();
        }



        //下方增加行
        private void click_addRow(object sender, RoutedEventArgs e)
        {
            int selectedIndex = paramGrid.SelectedIndex;
            if (selectedIndex == -1) //未选择行
            {
                layers.Add(new LayerBaseParams(this));
                return;
            }
            if (selectedIndex >= layers.Count)  //选择了空行
            {
                layers.Insert(selectedIndex, new LayerBaseParams(this));
                return;
            }
            layers.Insert(selectedIndex + 1, new LayerBaseParams(this));  //选择了非空行
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
                layers.RemoveAt(selectedIndex);
                paramGrid.SelectedIndex = selectedIndex;
            }

            if (layers.Count == 0)
            {
                LayerBaseParams param = new LayerBaseParams(this);
                param.yanXing = "地表";
                layers.Add(param);
                return;
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
            LayerBaseParams layer = layers[selectedIndex];
            layers[selectedIndex] = layers[selectedIndex - 1];
            layers[selectedIndex - 1] = layer;
            paramGrid.SelectedIndex = selectedIndex - 1;
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

            LayerBaseParams layer = layers[selectedIndex];
            layers[selectedIndex] = layers[selectedIndex + 1];
            layers[selectedIndex + 1] = layer;
            paramGrid.SelectedIndex = selectedIndex + 1;
        }

        //计算关键层和竖三带
        private void click_showKeyRow(object sender, RoutedEventArgs e)
        {
            //先撤销之前的变色
            foreach (KeyLayerParams nbr in keyLayers)
            {
                layers[nbr.ycbh - 1].IsKeyLayer = false;
            }

            //当前关键层变色显示
            int[] biaoHaoList = null;
            double[] pjxsList = null;
            getKeyLayer(ref biaoHaoList, ref pjxsList);

            if (biaoHaoList == null || pjxsList == null)
            {
                MessageBox.Show("未计算出关键层，请检查数据合理性");
                return;
            }
            keyLayers.Clear();
            int count = biaoHaoList.Length;
            for (int i = 0; i < count; i++)
            {
                KeyLayerParams layer = new KeyLayerParams(this);
                layer.ycbh = biaoHaoList[i];
                layer.fypjxs = pjxsList[i];

                layer.ycsd = layers[biaoHaoList[i] - 1].LeiJiShenDu;
                layer.mcms = layers[biaoHaoList[i] - 1].JuLiMeiShenDu;
                layer.fypjxsxz = layer.fypjxs * pjxsxz;
                keyLayers.Add(layer);

                layers[biaoHaoList[i] - 1].IsKeyLayer = true;
            }

            //竖三带计算
            try
            {
                var array1 = (MWNumericArray)logic.calHm(FuYanXCL, CaiGao, SuiZhangXS, Mcqj);
                double maoLuoDai = array1.ToScalarDouble();
                maoLuoDaiTb.Text = maoLuoDai.ToString("f3");
                var array2 = (MWNumericArray)logic.calHl(CaiGao, 1);
                double lieXiDai = array2.ToScalarDouble();
                lieXiDaiTb.Text = lieXiDai.ToString("f3");

                bool bWanquDai = false;
                for (int i = keyLayers.Count; i > 0; i--)
                {
                    if (keyLayers[i - 1].mcms < lieXiDai)
                    {
                        continue;
                    }
                    bWanquDai = true;
                    double wanquDai = keyLayers[i - 1].mcms;
                    wanQuDaiTb.Text = wanquDai.ToString("f3");
                    break;
                }
                if(!bWanquDai)
                {
                    MessageBox.Show("没有找到弯曲带，请检查数据合理性");
                }
            }
            catch (Exception)
            {
                e.ToString();
                MessageBox.Show("竖三带计算出错，请检查数据合理性");
            }

        }



        //模块一获取岩层参数的回调函数
        public double[,] getParams()
        {

            int count = layers.Count;
            for (count--; count > 0 && layers[count].yanXing != "煤"; count--) ;

            //判断是否有煤层 或者 煤层是最后一层 或者第一层不是地表 则失败
            if (count == 0 || count == layers.Count - 1 || !layers[0].yanXing.Equals("地表"))
                return null;


            double[,] res = new double[count + 2, 11];

            for (int i = 0; i <= count + 1; i++)
            {
                LayerBaseParams param = layers[i];
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

            for (int i = 0; i < width; i++)
                for (int j = 0; j < length; j++)
                {
                    output[i * length + j] = input[i, j];
                }

            return output;

        }



        //计算关键层接口
        private void getKeyLayer(ref int[] bianHao, ref double[] pjxs)
        {

            double[,] data = getParams();
            if (data == null)
            {
                MessageBox.Show("数据录入有误，请检查。(第一层应为地表，应该有煤层底板。)");
                return;
            }


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

                return;

            }
            catch (Exception e)
            {
                e.ToString();
                MessageBox.Show("计算出现错误，请检查数据准确性");
                return;

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
