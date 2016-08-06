﻿using MathWorks.MATLAB.NET.Arrays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GroundWellDesign
{
    partial class Document
    {

        //计算安全系数
        private void calSafeBtn_Click(object sender, RoutedEventArgs e)
        {
            int keycount = keyLayers.Count;
            for (int i = 0; i < keycount; i++)
            {
                keyLayers[i].IsDangerous = null;
            }


            ERRORCODE errcode = computeSafe(keycount);
            switch (errcode)
            {
                case ERRORCODE.计算成功:
                    //MessageBox.Show("计算成功");
                    break;
                case ERRORCODE.计算异常:
                    MessageBox.Show("计算出错，请检查数据合理性");
                    break;
                case ERRORCODE.没有关键层数据:
                    MessageBox.Show("没有关键层数据");
                    break;
            }
        }

        private void dangerBtn_Click(object sender, RoutedEventArgs e)
        {
            int keycount = keyLayers.Count;
            for (int i = 0; i < keycount; i++)
            {
                if (keyLayers[i].jqaqxs < 1 || keyLayers[i].lsaqxs < 1)
                {
                    keyLayers[i].IsDangerous = true;
                }
                else
                {
                    keyLayers[i].IsDangerous = false;
                }
            }
        }

        ERRORCODE computeSafe(int keycount)
        {
            if (keycount <= 0)
            {
                return ERRORCODE.没有关键层数据;
            }

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
                w0[i] = keyLayers[i].yczdxcz;
                e[i] = keyLayers[i].tgtxml;
                r1[i] = keyLayers[i].tgwj;
                t[i] = keyLayers[i].tgbh;
                a[i] = keyLayers[i].jqqycd;
                tlim[i] = keyLayers[i].kjqd;
                sigmaLim[i] = keyLayers[i].klqd;
            }
            for (int i = 0, j = 0; i < layers.Count; i++)
            {
                if (layers[i].IsKeyLayer)
                {
                    up[j++] = layers[i].jqHWY;
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
