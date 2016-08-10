using MathWorks.MATLAB.NET.Arrays;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Visifire.Charts;

namespace GroundWellDesign
{
    partial class Document
    {
        private void lcOffsetDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender != lcOffsetDataGrid || tabControl.SelectedItem != lcOffsetTabItem)
            {
                return;
            }


            if (keyLayers.Count == 0)
            {
                MessageBox.Show("请先计算关键层");
                tabControl.SelectedItem = gridinputTabItem;
                return;
            }


            switch (computeMidData(keyLayers.Count))
            {
                case ERRORCODE.计算成功:
                    //MessageBox.Show("计算成功");
                    CreateLcChartSpline(keyLayers, keyLayers.Count);
                    break;
                case ERRORCODE.计算异常:
                    MessageBox.Show("计算出错，请检查数据合理性");
                    break;
                case ERRORCODE.没有关键层数据:
                    MessageBox.Show("没有关键层数据");
                    break;
                case ERRORCODE.没有评价系数修正系数:
                    MessageBox.Show("没有评价系数修正系数，部分参数未计算");
                    break;
                case ERRORCODE.没有煤层倾角和煤层厚度:
                    MessageBox.Show("没有煤层倾角和煤层厚度，部分参数未计算");
                    break;
                case ERRORCODE.没有回采区长度:
                    MessageBox.Show("没有回采区长度(走向/倾向)，部分参数未计算");
                    break;
                case ERRORCODE.没有工作面推进速度:
                    MessageBox.Show("没有工作面推进速度，部分参数未计算");
                    break;
            }
        }


        private void CreateLcChartSpline(ObservableCollection<KeyLayerParams> layers, int drawCount)
        {
            lcChart.Watermark = false;
            //添加横坐标
            if (lcChart.AxesX.Count == 1)
            {
                Axis xAxis = new Axis();
                xAxis.Title = "岩层编号";
                xAxis.IntervalType = IntervalTypes.Number;
                xAxis.Interval = 1;
                lcChart.AxesX.Add(xAxis);
            }


            //添加纵坐标
            if (lcChart.AxesY.Count == 1)
            {
                Axis yAxis = new Axis();
                yAxis.Title = "剪切合位移";
                yAxis.AxisMinimum = 0;
                yAxis.Suffix = "米";
                lcChart.AxesY.Add(yAxis);
            }

            //设置数据点
            lcDataSeries.DataPoints.Clear();
            DataPoint dataPoint;
            for (int i = 0; i < drawCount; i++)
            {
                //创建一个数据点的实例
                dataPoint = new DataPoint();
                //设置X轴点
                dataPoint.XValue = i + 1;
                //设置Y轴点
                dataPoint.YValue = layers[i].yczdxcz;
                dataPoint.MarkerSize = 8;
                dataPoint.MouseLeftButtonDown += new MouseButtonEventHandler(lcdataPoint_MouseLeftButtonDown);
                //添加数据点
                lcDataSeries.DataPoints.Add(dataPoint);
            }
        }

        //点击事件
        void lcdataPoint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataPoint dp = sender as DataPoint;
            MessageBox.Show("岩层最大下沉值W0：" + dp.YValue.ToString());
        }
    }
}
