using MathWorks.MATLAB.NET.Arrays;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroundWellDesign
{
    partial class Document
    {
        private ERRORCODE computeCutOffSet(int keycount, int allcount)
        {
            if (keycount == 0)
            {
                return ERRORCODE.没有关键层数据;
            }


            //需要修正的p
            if (pjxsxz == 0)
            {

                return ERRORCODE.没有评价系数修正系数;
            }

            double[] Pxz = new double[keycount];
            double[] H0, H;
            H0 = H = new double[keycount];
            double[] T = new double[keycount];
            double alpha = Mcqj;
            double m = Mchd;
            double Ll0 = HcqZXcd;
            double L0 = HcqQXcd;
            double v = Gzmsd;
            double x = Jswzjl;
            double[] cengHou = new double[allcount];
            int[] keyIndex = new int[keycount];
            for(int i = 0; i < allcount; i++)
            {
                cengHou[i] = layers[i].cengHou;
            }

            MWNumericArray Eta, Wmax, Zx, Qx, Type, D, tgbeta, S, Lp, Llc, Lc, R, C, W0, W, P1, P2, P3, Ux, Uz, Up;

            for (int i = 0; i < keycount; i++)
            {
                Pxz[i] = keyLayers[i].fypjxsxz;
                H0[i] = keyLayers[i].mcms;
                T[i] = keyLayers[i].gzmtjsj;
                keyIndex[i] = keyLayers[i].ycbh;
            }

            try
            {
                Eta = (MWNumericArray)logic.calEta((MWNumericArray)Pxz);

                //计算Wmax
                //需要煤层倾角 煤层厚度
                if (Mchd == 0)
                {
                    return ERRORCODE.没有煤层倾角和煤层厚度;
                }
                Wmax = (MWNumericArray)logic.calWmax(m, Eta, alpha);

                //走向l0/H0  倾向L0/H0
                //需要走向倾向
                if (HcqZXcd == 0 || HcqQXcd == 0)
                {

                    return ERRORCODE.没有回采区长度;
                }
                Zx = (MWNumericArray)logic.caldiv(Ll0, (MWNumericArray)H0);
                Qx = (MWNumericArray)logic.caldiv(L0, (MWNumericArray)H0);

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

                //拐点移动据S
                S = (MWNumericArray)logic.calS(Type, Ll0, (MWNumericArray)H0);

                //倾斜煤层倾向回采空间尺寸L'
                Lp = (MWNumericArray)logic.call((MWNumericArray)Ll0, S);

                //采场充分开采空间距离lc

                MWArray[] temp2 = logic.callc(2, transpose(R), transpose(S));
                Llc = (MWNumericArray)temp2[0];
                Lc = (MWNumericArray)temp2[1];

                //岩层最大下沉值W0
                W0 = (MWNumericArray)logic.calW0(transpose(Wmax), Ll0, L0, transpose(Llc), transpose(Lc));

                //岩层沉降迟滞系数C
                C = (MWNumericArray)logic.calC(R, (MWNumericArray)H0);


                //计算点即时沉降位移1Wt
                //需要推进速度
                if (Gzmsd == 0)
                {

                    return ERRORCODE.没有工作面推进速度;
                }
                MWArray[] temp3 = logic.calW(4, x, transpose(W0), transpose(R), transpose(Lp), transpose(C), v, transpose((MWNumericArray)T));
                W = (MWNumericArray)temp3[0];
                P1 = (MWNumericArray)temp3[1];
                P2 = (MWNumericArray)temp3[2];
                P3 = (MWNumericArray)temp3[3];


                MWArray[] temp5 = logic.calu(2, transpose((MWNumericArray)cengHou), transpose((MWNumericArray)keyIndex), transpose(R), transpose(W), transpose(P1), transpose(P2), transpose(P3));
                Ux = (MWNumericArray)temp5[0];
                Uz = (MWNumericArray)temp5[1];
                Up = (MWNumericArray)logic.calup(Ux, Uz);
                double[] ux = (double[])Ux.ToVector(MWArrayComponent.Real);
                double[] uz = (double[])Uz.ToVector(MWArrayComponent.Real);
                double[] up = (double[])Up.ToVector(MWArrayComponent.Real);
                int newlength = up.Length;
                int oldlength = cutoffLayers.Count;
                if (newlength < oldlength)
                {
                    for (int i = oldlength - 1; i >= newlength; i--)
                    {
                        cutoffLayers.RemoveAt(i);
                    }
                }
                else if (newlength > oldlength)
                {
                    for (int i = oldlength; i < newlength; i++)
                    {
                        cutoffLayers.Add(layers[i]);
                    }
                }
                for (int i = 0; i < up.Length; i++)
                {
                    cutoffLayers[i].QXJQWY = ux[i] * 100;
                    cutoffLayers[i].ZXJQWY = uz[i] * 100;
                    cutoffLayers[i].JQHWY = up[i] * 100;
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
