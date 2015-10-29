using MathWorks.MATLAB.NET.Arrays;
using System;
using System.Windows;

namespace GroundWellDesign
{
    partial class MainWindow : Window
    {

        private void showCadBtn_Click(object sender, RoutedEventArgs e)
        {
            compute(keyLayers.Count);
            //操作控件


            //显示
            tabControl.SelectedIndex = 3;

        }

        private void showResBtn_Click(object sender, RoutedEventArgs e)
        {
            compute(keyLayers.Count);
            //

        }




        private void compute(int count)
        {
            if (count == 0)
            {
                MessageBox.Show("没有关键层数据");
                return;
            }


            //需要修正的p
            if (pjxsxz == 0)
            {
                MessageBox.Show("请输入评价系数修正系数");
                return;
            }

            double[] Pxz = new double[count];
            double[] H0, H;
            H0 = H = new double[count];
            double[] T = new double[count];
            double alpha = Mcqj;
            double m = Mchd;
            double Ll0 = HcqZXcd;
            double L0 = HcqQXcd;
            double v = Gzmsd;
            double x = Jswzjl;
            MWNumericArray Eta, Wmax, Zx, Qx, Type, D, tgbeta, S, Lp, Llc, Lc, R, Qxxc, Zxxc, C, W0, W, DelW;

            for (int i = 0; i < count; i++)
            {
                Pxz[i] = keyLayers[i].fypjxsxz;
                H0[i] = keyLayers[i].mcms;
                T[i] = keyLayers[i].gzmtjsj;
            }


            Eta = (MWNumericArray)logic.calEta((MWNumericArray)Pxz);

            //计算Wmax
            //需要煤层倾角 煤层厚度
            if (Mchd == 0)
            {
                MessageBox.Show("请输入煤层倾角和煤层厚度");
                return;
            }
            Wmax = (MWNumericArray)logic.calWmax(m, Eta, alpha);
            
            double[] wmax = (double[])Wmax.ToVector(MWArrayComponent.Real);
            for (int i = 0; i < count; i++)
            {
                keyLayers[i].Cfcdcjwy = wmax[i];
            }

            


            //走向l0/H0  倾向L0/H0
            //需要走向倾向
            if (HcqZXcd == 0 || HcqQXcd == 0)
            {
                MessageBox.Show("请输入回采区长度(走向/倾向)");
                return;
            }
            Zx = (MWNumericArray)logic.caldiv(Ll0, (MWNumericArray)H0);
            Qx = (MWNumericArray)logic.caldiv(L0, (MWNumericArray)H0);
            double[] zx = (double[])Zx.ToVector(MWArrayComponent.Real);
            double[] qx = (double[])Qx.ToVector(MWArrayComponent.Real);
            for (int i = 0; i < count; i++)
            {
                keyLayers[i].Zx = zx[i];
                keyLayers[i].Qx = qx[i];
            }

            //岩性影响系数D type
            MWNumericArray p = (MWNumericArray)Pxz;
            MWArray[] temp = logic.table4(2, (MWNumericArray)Pxz);
            D = (MWNumericArray)temp[0];
            Type = (MWNumericArray)temp[1];
            Type = transpose(Type);

            //tgbate
            tgbeta = (MWNumericArray)logic.caltgbeta(alpha, D, (MWNumericArray)H);

            //采动影响半径
            R = (MWNumericArray)logic.calr((MWNumericArray)H, tgbeta);
            double[] r = (double[])R.ToVector(MWArrayComponent.Real);
            for (int i = 0; i < count; i++)
            {
                keyLayers[i].Cdyxbj = r[i];
            }


            //拐点移动据S
            S = (MWNumericArray)logic.calS(Type, Ll0, (MWNumericArray)H0);
            double[] s = (double[])S.ToVector(MWArrayComponent.Real);
            for (int i = 0; i < count; i++)
            {
                keyLayers[i].Gdydj = s[i];
            }


            //倾斜煤层倾向回采空间尺寸L'
            Lp = (MWNumericArray)logic.call((MWNumericArray)Ll0, S);
            double[] lp = (double[])Lp.ToVector(MWArrayComponent.Real);
            for (int i = 0; i < count; i++)
            {
                keyLayers[i].Qxhckj = lp[i];
            }



            //采场充分开采空间距离lc

            MWArray[] temp2 = logic.callc(2, transpose(R), transpose(S));
            Llc = (MWNumericArray)temp2[0];
            Lc = (MWNumericArray)temp2[1];
            double[] lc = (double[])Llc.ToVector(MWArrayComponent.Real);
            for (int i = 0; i < count; i++)
            {
                keyLayers[i].Cfkckjjl = lc[i];
            }

            //倾向下沉系数 走向下沉系数  实际下沉系数η0
            Qxxc = (MWNumericArray)logic.table6(Qx);
            Zxxc = (MWNumericArray)logic.table6(Zx);
            MWNumericArray N0 = (MWNumericArray)logic.calEta0(Eta, Zxxc, Qxxc);

            double[] n0 = (double[])N0.ToVector(MWArrayComponent.Real);
            for (int i = 0; i < count; i++)
            {
                keyLayers[i].Sjxcxs = n0[i];
            }
            

            //岩层最大下沉值W0
            W0 = (MWNumericArray)logic.calW0(transpose(Wmax), Ll0, L0, transpose(Llc), transpose(Lc));
            double[] w0 = (double[])W0.ToVector(MWArrayComponent.Real);
            for (int i = 0; i < count; i++)
            {
                keyLayers[i].Yczdxcz = w0[i];
            }


            //岩层沉降迟滞系数C
            C = (MWNumericArray)logic.calC(R, (MWNumericArray)H0);


            //计算点即时沉降位移1Wt
            //需要推进速度
            if (Gzmsd == 0)
            {
                MessageBox.Show("请输入工作面推进速度");
                return;
            }
            W = (MWNumericArray)logic.calW(x, transpose(W0), transpose(R), transpose(Lp), transpose(C), v, transpose((MWNumericArray)T));
            double[] w = (double[])W.ToVector(MWArrayComponent.Real);
            for (int i = 0; i < count; i++)
            {
                keyLayers[i].Jsdjscjwy = w[i];
            }



            //计算点即时离层位移1△Wt
            DelW = (MWNumericArray)logic.caldeltaW(transpose(W));
            double[] deltaW = (double[])DelW.ToVector(MWArrayComponent.Real);
            for (int i = 0; i < count; i++)
            {
                keyLayers[i].Jsdjslcwy = deltaW[i];
            }


        }


        private MWNumericArray transpose(MWNumericArray array)
        {
            double[] tmp = (double[])array.ToVector(MWArrayComponent.Real);
            return new MWNumericArray(tmp.Length, 1, tmp);
        }


    }

}
