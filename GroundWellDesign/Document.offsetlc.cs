using GroundWellDesign.Util;
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
        int lcIndex = 0;
        private void lcOffsetDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender != lcOffsetDataGrid || tabControl.SelectedItem != lcOffsetTabItem)
            {
                return;
            }

            try
            {
                int upcount = 0;
                ComputeHelper.computeKeyLayerOffset(this, ref upcount);
                CreateLcChartSpline(lcIndex);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void LCDestCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lcIndex = LCDestCombo.SelectedIndex;
            CreateLcChartSpline(lcIndex);
        }


        private void CreateLcChartSpline(int index)
        {
            //添加横坐标
            if (lcChart.AxesX.Count == 0)
            {
                Axis xAxis = new Axis();
                xAxis.Title = "岩层编号";
                lcChart.AxesX.Add(xAxis);
            }


            //添加纵坐标
            if (lcChart.AxesY.Count == 0)
            {
                Axis yAxis = new Axis();
                yAxis.Title = LCDestOpt[index];
                yAxis.IntervalType = IntervalTypes.Number;
                yAxis.ValueFormatString = "f3";
                yAxis.Suffix = "m";
                lcChart.AxesY.Add(yAxis);
            }
            else
            {
                lcChart.AxesY[0].Title = LCDestOpt[index];
            }

            //设置数据点
            lcDataSeries.DataPoints.Clear();
            DataPoint dataPoint;
            int drawCount = keyLayers.Count;
            for (int i = 0; i < drawCount; i++)
            {
                //创建一个数据点的实例
                dataPoint = new DataPoint();
                //设置X轴点
                dataPoint.AxisXLabel = keyLayers[i].Ycbh.ToString();
                dataPoint.XValue = i + 1;
                //设置Y轴点
                switch(index)
                {
                    case 0:
                        dataPoint.YValue = keyLayers[i].Yczdxcz;
                        break;
                    case 1:
                        dataPoint.YValue = keyLayers[i].Jsdjscjwy;
                        break;
                    case 2:
                        dataPoint.YValue = keyLayers[i].Jsdjslcwy;
                        break;
                }
                
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
            MessageBox.Show(LCDestOpt[lcIndex] + "：  " + dp.YValue.ToString("f5") + "m");

            int index = lcDataSeries.DataPoints.IndexOf(dp);
            lcOffsetDataGrid.SelectedIndex = index;
            lcOffsetDataGrid.ScrollIntoView(lcOffsetDataGrid.Items[index]);
        }
    }
}
