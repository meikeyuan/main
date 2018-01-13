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

        int upCount = 0;
        int jqIndex = 0;
        private void cutOffsetDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender != cutOffsetDataGrid || tabControl.SelectedItem != cutOffsetTabItem)
            {
                return;
            }

            try
            {
                ComputeHelper.computeKeyLayerOffset(this, ref upCount);
                CreateJqChartSpline(0, upCount);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }




        private void JQDestCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            jqIndex = JQDestCombo.SelectedIndex;
            CreateJqChartSpline(jqIndex, upCount);
        }




        private void CreateJqChartSpline(int index, int drawCount)
        {
            //添加横坐标
            if (jqChart.AxesX.Count == 0)
            {
                Axis xAxis = new Axis();
                xAxis.Title = "岩层编号";
                xAxis.IntervalType = IntervalTypes.Number;
                xAxis.Interval = 1;
                jqChart.AxesX.Add(xAxis);
            }


            //添加纵坐标
            if (jqChart.AxesY.Count == 0)
            {
                Axis yAxis = new Axis();
                yAxis.Title = JQDestOpt[index];
                yAxis.IntervalType = IntervalTypes.Number;
                yAxis.ValueFormatString = "f4";
                yAxis.Suffix = "cm";
                jqChart.AxesY.Add(yAxis);
            }
            else  //更改Y坐标标题
            {
                jqChart.AxesY[0].Title = JQDestOpt[index];
            }

            // 设置数据点
            jqDataSeries.DataPoints.Clear();
            DataPoint dataPoint;
            for (int i = 0; i < drawCount; i++)
            {
                // 创建一个数据点的实例。                   
                dataPoint = new DataPoint();
                // 设置X轴点                    
                dataPoint.XValue = i + 1;
                //设置Y轴点
                switch(index)
                {
                    case 0:
                        dataPoint.YValue = layers[i].ZXJQWY;
                        break;
                    case 1:
                        dataPoint.YValue = layers[i].QXJQWY;
                        break;
                    case 2:
                        dataPoint.YValue = layers[i].JQHWY;
                        break;
                }
                
                dataPoint.MarkerSize = 8;                 
                dataPoint.MouseLeftButtonDown += new MouseButtonEventHandler(jqdataPoint_MouseLeftButtonDown);
                //添加数据点
                jqDataSeries.DataPoints.Add(dataPoint);
            }
        }

        //点击事件
        void jqdataPoint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataPoint dp = sender as DataPoint;
            MessageBox.Show(JQDestOpt[jqIndex] + "：  " +  dp.YValue.ToString("f5") + "cm");

            int index = jqDataSeries.DataPoints.IndexOf(dp);
            cutOffsetDataGrid.SelectedIndex = index;
            cutOffsetDataGrid.ScrollIntoView(cutOffsetDataGrid.Items[index]);


        }
    }
}
