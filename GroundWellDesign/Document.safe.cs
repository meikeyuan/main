﻿using MathWorks.MATLAB.NET.Arrays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GroundWellDesign
{
    partial class Document
    {
        private void taoGuanDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender != taoGuanDataGrid || tabControl.SelectedItem != taoGuanTabItem)
            {
                return;
            }
        }

        

        private void taoGuanDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count == 0)
                return;
            taoGuanDataGrid.BeginEdit();    //  进入编辑模式
        }


        //计算安全系数
        private void calSafeBtn_Click(object sender, RoutedEventArgs e)
        {
            int keycount = keyLayers.Count;

            ERRORCODE errcode = computeSafe(keycount);
            switch (errcode)
            {
                case ERRORCODE.计算成功:
                    for (int i = 0; i < keycount; i++)
                    {
                        if (keyLayers[i].Jqaqxs < 1 || keyLayers[i].Lsaqxs < 1)
                        {
                            keyLayers[i].IsDangerious = 1;
                        }
                        else
                        {
                            keyLayers[i].IsDangerious = -1;
                        }
                    }
                    break;
                case ERRORCODE.计算异常:
                    MessageBox.Show("计算出错，请检查数据合理性");
                    break;
                case ERRORCODE.没有关键层数据:
                    MessageBox.Show("没有关键层数据");
                    break;
            }
        }

        ERRORCODE computeSafe(int keycount)
        {
            if (keycount <= 0)
            {
                return ERRORCODE.没有关键层数据;
            }

            // 获取填写的参数
            double[] w0 = new double[keycount];
            double[] up = new double[keycount];
            double[] e = new double[keycount];
            double[] r1 = new double[keycount];
            double[] t = new double[keycount];
            double[] a = new double[keycount];
            double[] tlim = new double[keycount];
            double[] sigmaLim = new double[keycount];
            for (int i = 0; i < keycount; i++)
            {
                w0[i] = keyLayers[i].Yczdxcz;
                //if (keyLayers[i].Tgxh.Equals(TgxhOpt[0]))
                //{
                //    e[i] = TgtxmlOpt[0];
                //    tlim[i] = KjqdOpt[0];
                //    sigmaLim[i] = KlqdOpt[0];
                //}
                //else if (keyLayers[i].Tgxh.Equals(TgxhOpt[1]))
                //{
                //    e[i] = TgtxmlOpt[1];
                //    tlim[i] = KjqdOpt[1];
                //    sigmaLim[i] = KlqdOpt[1];
                //}
                //else  // 用户自定义
                {
                    e[i] = keyLayers[i].Tgtxml;
                    tlim[i] = keyLayers[i].Kjqd;
                    sigmaLim[i] = keyLayers[i].Klqd;
                }
                r1[i] = keyLayers[i].Tgwj / 1000; //毫米转米
                t[i] = keyLayers[i].Tgbh / 1000;
                a[i] = keyLayers[i].Jqqycd;
            }
            for (int i = 0, j = 0; i < layers.Count; i++)
            {
                if (layers[i].IsKeyLayer)
                {
                    up[j++] = layers[i].JQHWY;
                }
            }


            try
            {
                //计算Tmax：最大剪切应力
                MWArray Tmax = logic.calTaoMax((MWNumericArray)up, (MWNumericArray)e, (MWNumericArray)r1, (MWNumericArray)t, (MWNumericArray)a);
                double[] tmax = (double[])((MWNumericArray)Tmax).ToVector(MWArrayComponent.Real);

                //计算fs：剪切安全系数
                MWArray Fs = logic.calfs((MWNumericArray)tlim, Tmax);
                double[] fs = (double[])((MWNumericArray)Fs).ToVector(MWArrayComponent.Real);

                //计算epsilon：最大剪切应变
                MWArray Epsilon = logic.calepsilon((MWNumericArray)up, (MWNumericArray)a);
                double[] epsilon = (double[])((MWNumericArray)Epsilon).ToVector(MWArrayComponent.Real);

                //计算sigma:拉伸应力
                MWArray Sigma = logic.calsigmat((MWNumericArray)e, Epsilon, (MWNumericArray)w0, (MWNumericArray)a);
                double[] sigma = (double[])((MWNumericArray)Sigma).ToVector(MWArrayComponent.Real);

                //计算ft：拉伸安全系数
                MWArray Ft = logic.calfs((MWNumericArray)sigmaLim, Sigma);
                double[] ft = (double[])((MWNumericArray)Ft).ToVector(MWArrayComponent.Real);

                //显示
                for (int i = 0; i < keycount; i++)
                {
                    keyLayers[i].Zdjqyl = tmax[i];
                    keyLayers[i].Jqaqxs = fs[i];

                    keyLayers[i].Zdjqyb = epsilon[i];
                    keyLayers[i].Lsyl = sigma[i];
                    keyLayers[i].Lsaqxs = ft[i];
                }

                return ERRORCODE.计算成功;
            }
            catch (Exception)
            {
                return ERRORCODE.计算异常;
            }

        }

    }
}
