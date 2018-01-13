using GroundWellDesign.Util;
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
            if(SQLDBHelper.LayerExisted(layer.DataBaseKey))
            {
                MessageBoxResult res = MessageBox.Show(Properties.Resources.LayerExistPrompt, "请注意", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                switch (res)
                {
                    case MessageBoxResult.No:
                        new SaveToDBWindow(layer).ShowDialog();
                        break;
                    case MessageBoxResult.Yes:
                        bool success = SQLDBHelper.SaveLayer(layer, layer.DataBaseKey, true, layer.WellNamePK, true);
                        if (success)
                        {
                            MessageBox.Show(GroundWellDesign.Properties.Resources.OperateOk);
                        }
                        else
                        {
                            MessageBox.Show(GroundWellDesign.Properties.Resources.OperateNotOk);
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                new SaveToDBWindow(layer).ShowDialog();
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


        // 从Excel导入岩层参数
        private void click_importExcel(object sender, RoutedEventArgs e)
        {
            string filePath = FileDialogHelper.getOpenPath("Excel文件(*.xls)|*.xls");
            DataTable dt = null;
            if (filePath != null)
            {
                dt = ExcelHelper.LoadExcelWithAspose(filePath);
                if (dt == null || dt.Columns.Count != 17)
                {
                    App.logger.Error("文件格式可能不正确。列数不对。");
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
                        // 因为层厚是影响前两个字段的字段，通过刷新层厚刷新前两个字段。
                        baseParam.CengHou = baseParam.LayerParams.CengHou;
                    }
                }
                catch (Exception ex)
                {
                    App.logger.Error("文件格式可能不正确。数据格式不对。", ex);
                    MessageBox.Show("读取文件失败,请检查文件第" + rowIndex + "行数据的格式问题。");
                }
            }
        }
         
        
        
        // 计算关键层和竖三带
        private void click_showKeyRow(object sender, RoutedEventArgs e)
        {
            //先撤销之前的变色
            foreach (KeyLayerParamsViewModel nbr in keyLayers)
            {
                layers[nbr.Params.Ycbh].IsKeyLayer = false;
            }

            // 计算关键层
            try
            {
                int[] biaoHaoList = null;
                double[] pjxsList = null;
                ComputeHelper.computeKeyLayer(this, ref biaoHaoList, ref pjxsList);
                if (biaoHaoList == null || pjxsList == null)
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
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
        // 地面井设计需要用到该参数
        int wanQuDaiIndex = 0;


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
