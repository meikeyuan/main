using GroundWellDesign.Properties;
using GroundWellDesign.ViewModel;
using MathWorks.MATLAB.NET.Arrays;
using Mky;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GroundWellDesign.Util
{
    class ComputeHelper
    {
        private static MkyLogic logic = null;
        public static void initLogic()
        {
            if (logic == null)
            {
                logic = new MkyLogic();
            }
        }

        private static MWNumericArray transpose(MWNumericArray array)
        {
            double[] tmp = (double[])array.ToVector(MWArrayComponent.Real);
            return new MWNumericArray(tmp.Length, 1, tmp);
        }


        #region 关键层
        //计算关键层接口
        public static void computeKeyLayer(Document doc, ref int[] bianHao, ref double[] pjxs)
        {
            // 判断是否录入了数据
            if (doc.layers.Count <= 0)
            {
                throw new Exception(ErrorInfo.BaseParamsError4);
            }
            // 判断是否有地表
            if (!doc.layers[0].YanXing.Equals("地表"))
            {
                throw new Exception(ErrorInfo.BaseParamsError1);
            }
            // 判断是否有煤层
            int meiIndex = -1;
            for (int i = doc.layers.Count - 1; i >= 0; --i)
            {
                if (doc.layers[i].YanXing.Equals("煤"))
                {
                    meiIndex = i;
                    break;
                }
            }
            if (meiIndex == -1)
            {
                throw new Exception(ErrorInfo.BaseParamsError2);
            }
            // 判断是否有底板
            if (meiIndex == doc.layers.Count - 1)
            {
                throw new Exception(ErrorInfo.BaseParamsError3);
            }

            // 开始计算
            try
            {
                double[] rowData = arrayTrans(getBaseParams(doc, meiIndex));
                MWNumericArray mwdata = new MWNumericArray(rowData.Length / 11, 11, rowData);

                MWArray[] result = logic.yancengzuhe(2, mwdata, doc.FuYanXCL, doc.CaiGao, doc.SuiZhangXS, doc.Mcqj);
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

                // 冒落带
                var array1 = (MWNumericArray)logic.calHm(doc.FuYanXCL, doc.CaiGao, doc.SuiZhangXS, doc.Mcqj);
                double maoLuoDai = array1.ToScalarDouble();
                doc.maoLuoDaiTb.Text = maoLuoDai.ToString("f3");
                // 裂隙带
                var array2 = (MWNumericArray)logic.calHl(doc.CaiGao, 1);
                double lieXiDai = array2.ToScalarDouble();
                doc.lieXiDaiTb.Text = lieXiDai.ToString("f3");
                // 弯曲下沉带 = 煤层以上深度 - 裂隙带高度
                double wanquDai = doc.layers[0].JuLiMeiShenDu - lieXiDai;
                doc.wanQuDaiTb.Text = wanquDai.ToString("f3");
            }
            catch (Exception ex)
            {
                App.logger.Error(ErrorInfo.ComputeKeyLayerError, ex);
                throw new Exception(ErrorInfo.ComputeKeyLayerError);
            }

        }


        // 二维转置
        private static double[] arrayTrans(double[,] input)
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


        // 获取岩层参数
        private static double[,] getBaseParams(Document doc, int meiIndex)
        {
            double[,] res = new double[meiIndex + 2, 11];

            for (int i = 0; i <= meiIndex + 1; i++)
            {
                LayerBaseParams param = doc.layers[i].LayerParams;
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

                if (doc.caiDongComBox.SelectedIndex == 0)
                {
                    res[i, 10] = param.Q0;

                }
                else if (doc.caiDongComBox.SelectedIndex == 1)
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

        #endregion

        #region 关键层相关计算

        public static void computeKeyLayerOffset(Document doc, ref int upCount)
        {
            if (doc.keyLayers.Count <= 0)
            {
                throw new Exception(ErrorInfo.NoKeyParamsError);
            }
            if (doc.Pjxsxz == 0)
            {
                throw new Exception(ErrorInfo.NoPjxsxzError);
            }
            if (doc.Mchd == 0)
            {
                throw new Exception(ErrorInfo.NoMchdError);
            }
            if (doc.HcqZXcd == 0 || doc.HcqQXcd == 0)
            {
                throw new Exception(ErrorInfo.NoHcqcdError);
            }
            if (doc.Gzmsd == 0)
            {
                throw new Exception(ErrorInfo.NoGzmsd);
            }

            // 参数初始化
            int allcount = doc.layers.Count;
            int keycount = doc.keyLayers.Count;
            double[] Pxz = new double[keycount];
            double[] H0, H;
            H0 = H = new double[keycount];
            double[] T = new double[keycount];
            double alpha = doc.Mcqj;
            double m = doc.Mchd;
            double Ll0 = doc.HcqZXcd;
            double L0 = doc.HcqQXcd;
            double v = doc.Gzmsd;
            double x = doc.Jswzjl;
            double[] cengHou = new double[allcount];
            int[] keyIndex = new int[keycount];
            for (int i = 0; i < allcount; i++)
            {
                cengHou[i] = doc.layers[i].CengHou;
            }

            for (int i = 0; i < keycount; i++)
            {
                Pxz[i] = doc.keyLayers[i].Fypjxsxz;
                H0[i] = doc.keyLayers[i].Mcms;
                T[i] = doc.keyLayers[i].Gzmtjsj;
                keyIndex[i] = doc.keyLayers[i].Ycbh + 1;
            }

            // 开始计算
            try
            {
                MWNumericArray Eta, Wmax, Zx, Qx, Type, D, tgbeta, S, Lp, Llc, Lc, Qxxc, Zxxc, R, C, W0, W, P1, P2, P3, DelW, Ux, Uz, Up;
                Eta = (MWNumericArray)logic.calEta((MWNumericArray)Pxz);
                Wmax = (MWNumericArray)logic.calWmax(m, Eta, alpha);
                double[] wmax = (double[])Wmax.ToVector(MWArrayComponent.Real);
                for (int i = 0; i < keycount; i++)
                {
                    doc.keyLayers[i].Cfcdcjwy = wmax[i];
                }

                // 走向、倾向
                Zx = (MWNumericArray)logic.caldiv(Ll0, (MWNumericArray)H0);
                Qx = (MWNumericArray)logic.caldiv(L0, (MWNumericArray)H0);
                double[] zx = (double[])Zx.ToVector(MWArrayComponent.Real);
                double[] qx = (double[])Qx.ToVector(MWArrayComponent.Real);
                for (int i = 0; i < keycount; i++)
                {
                    doc.keyLayers[i].Zx = zx[i];
                    doc.keyLayers[i].Qx = qx[i];
                }

                // 岩性影响系数D type
                MWNumericArray p = (MWNumericArray)Pxz;
                MWArray[] temp = logic.table4(2, (MWNumericArray)Pxz);
                D = (MWNumericArray)temp[0];
                Type = (MWNumericArray)temp[1];
                Type = transpose(Type);

                // tgbate
                tgbeta = (MWNumericArray)logic.caltgbeta(alpha, D, (MWNumericArray)H);

                // 采动影响半径
                R = (MWNumericArray)logic.calr((MWNumericArray)H, tgbeta);
                double[] r = (double[])R.ToVector(MWArrayComponent.Real);
                for (int i = 0; i < keycount; i++)
                {
                    doc.keyLayers[i].Cdyxbj = r[i];
                }

                // 拐点移动据S
                S = (MWNumericArray)logic.calS(Type, Ll0, (MWNumericArray)H0);
                double[] s = (double[])S.ToVector(MWArrayComponent.Real);
                for (int i = 0; i < keycount; i++)
                {
                    doc.keyLayers[i].Gdydj = s[i];
                }

                // 倾斜煤层倾向回采空间尺寸L'
                Lp = (MWNumericArray)logic.call((MWNumericArray)Ll0, S);
                double[] lp = (double[])Lp.ToVector(MWArrayComponent.Real);
                for (int i = 0; i < keycount; i++)
                {
                    doc.keyLayers[i].Qxhckj = lp[i];
                }

                // 采场充分开采空间距离lc
                MWArray[] temp2 = logic.callc(2, transpose(R), transpose(S));
                Llc = (MWNumericArray)temp2[0];
                Lc = (MWNumericArray)temp2[1];
                double[] lc = (double[])Llc.ToVector(MWArrayComponent.Real);
                for (int i = 0; i < keycount; i++)
                {
                    doc.keyLayers[i].Cfkckjjl = lc[i];
                }

                // 倾向下沉系数 走向下沉系数  实际下沉系数η0
                Qxxc = (MWNumericArray)logic.table6(Qx);
                Zxxc = (MWNumericArray)logic.table6(Zx);
                MWNumericArray N0 = (MWNumericArray)logic.calEta0(Eta, Zxxc, Qxxc);
                double[] n0 = (double[])N0.ToVector(MWArrayComponent.Real);
                for (int i = 0; i < keycount; i++)
                {
                    doc.keyLayers[i].Sjxcxs = n0[i];
                }

                // 岩层最大下沉值W0
                W0 = (MWNumericArray)logic.calW0(transpose(Wmax), Ll0, L0, transpose(Llc), transpose(Lc));
                double[] w0 = (double[])W0.ToVector(MWArrayComponent.Real);
                for (int i = 0; i < keycount; i++)
                {
                    doc.keyLayers[i].Yczdxcz = w0[i];
                }

                // 岩层沉降迟滞系数C
                C = (MWNumericArray)logic.calC(R, (MWNumericArray)H0);


                // 计算点即时沉降位移1Wt
                MWArray[] temp3 = logic.calW(4, x, transpose(W0), transpose(R), transpose(Lp), transpose(C), v, transpose((MWNumericArray)T));
                W = (MWNumericArray)temp3[0];
                double[] w = (double[])W.ToVector(MWArrayComponent.Real);
                for (int i = 0; i < keycount; i++)
                {
                    doc.keyLayers[i].Jsdjscjwy = w[i];
                }
                P1 = (MWNumericArray)temp3[1];
                P2 = (MWNumericArray)temp3[2];
                P3 = (MWNumericArray)temp3[3];

                // 计算点即时离层位移1△Wt
                DelW = (MWNumericArray)logic.caldeltaW(transpose(W));
                double[] deltaW = (double[])DelW.ToVector(MWArrayComponent.Real);
                for (int i = 0; i < keycount; i++)
                {
                    doc.keyLayers[i].Jsdjslcwy = deltaW[i];
                }

                // 计算倾向剪切位移 走向剪切位移 剪切和位移
                MWArray[] temp5 = logic.calu(2, transpose((MWNumericArray)cengHou), transpose((MWNumericArray)keyIndex), transpose(R), transpose(W), transpose(P1), transpose(P2), transpose(P3));
                Ux = (MWNumericArray)temp5[0];
                Uz = (MWNumericArray)temp5[1];
                Up = (MWNumericArray)logic.calup(Ux, Uz);
                double[] ux = (double[])Ux.ToVector(MWArrayComponent.Real);
                double[] uz = (double[])Uz.ToVector(MWArrayComponent.Real);
                double[] up = (double[])Up.ToVector(MWArrayComponent.Real);

                upCount = up.Length;
                for (int i = 0; i < upCount; i++)
                {
                    doc.layers[i].QXJQWY = ux[i] * 100;
                    doc.layers[i].ZXJQWY = uz[i] * 100;
                    doc.layers[i].JQHWY = up[i] * 100;
                    var row = doc.cutOffsetDataGrid.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;
                    if (row != null)
                        row.Visibility = System.Windows.Visibility.Visible;
                }

                // 其他岩层隐藏
                for (int i = upCount; i < doc.layers.Count; i++)
                {
                    var row = doc.cutOffsetDataGrid.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;
                    if (row != null)
                        row.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                App.logger.Error(ErrorInfo.ComputeKeyLayerOffsetError, ex);
                throw new Exception(ErrorInfo.ComputeKeyLayerOffsetError);
            }

        }


        public static void computeSafe(Document doc)
        {
            if (doc.keyLayers.Count <= 0)
            {
                throw new Exception(ErrorInfo.NoKeyParamsError);
            }

            // 获取填写的参数
            int keycount = doc.keyLayers.Count;
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
                w0[i] = doc.keyLayers[i].Yczdxcz;
                e[i] = doc.keyLayers[i].Tgtxml;
                tlim[i] = doc.keyLayers[i].Kjqd;
                sigmaLim[i] = doc.keyLayers[i].Klqd;
                r1[i] = doc.keyLayers[i].Tgwj / 1000;  // 毫米转米
                t[i] = doc.keyLayers[i].Tgbh / 1000;   // 毫米转米
                a[i] = doc.keyLayers[i].Jqqycd;
                up[i] = doc.layers[doc.keyLayers[i].Ycbh].JQHWY;
            }

            // 开始计算
            try
            {
                // 计算Tmax：最大剪切应力
                MWArray Tmax = logic.calTaoMax((MWNumericArray)up, (MWNumericArray)e, (MWNumericArray)r1, (MWNumericArray)t, (MWNumericArray)a);
                double[] tmax = (double[])((MWNumericArray)Tmax).ToVector(MWArrayComponent.Real);

                // 计算fs：剪切安全系数
                MWArray Fs = logic.calfs((MWNumericArray)tlim, Tmax);
                double[] fs = (double[])((MWNumericArray)Fs).ToVector(MWArrayComponent.Real);

                // 计算epsilon：最大剪切应变
                MWArray Epsilon = logic.calepsilon((MWNumericArray)up, (MWNumericArray)a);
                double[] epsilon = (double[])((MWNumericArray)Epsilon).ToVector(MWArrayComponent.Real);

                // 计算sigma:拉伸应力
                MWArray Sigma = logic.calsigmat((MWNumericArray)e, Epsilon, (MWNumericArray)w0, (MWNumericArray)a);
                double[] sigma = (double[])((MWNumericArray)Sigma).ToVector(MWArrayComponent.Real);

                // 计算ft：拉伸安全系数
                MWArray Ft = logic.calfs((MWNumericArray)sigmaLim, Sigma);
                double[] ft = (double[])((MWNumericArray)Ft).ToVector(MWArrayComponent.Real);

                // 显示
                for (int i = 0; i < keycount; i++)
                {
                    doc.keyLayers[i].Zdjqyl = tmax[i];
                    doc.keyLayers[i].Jqaqxs = fs[i];
                    doc.keyLayers[i].Zdjqyb = epsilon[i];
                    doc.keyLayers[i].Lsyl = sigma[i];
                    doc.keyLayers[i].Lsaqxs = ft[i];
                }
            }
            catch (Exception ex)
            {
                App.logger.Error(ErrorInfo.ComputeSafeError, ex);
                throw new Exception(ErrorInfo.ComputeSafeError);
            }

        }

        #endregion


        #region 水泥环增益

        public static void computeZengYi(Document doc)
        {

            double ec;
            if (doc.editZengYi.Ec.Equals(Document.TgtxmlOpt[0]))
            {
                ec = 2.050e11;
            }
            else if (doc.editZengYi.Ec.Equals(Document.TgtxmlOpt[1]))
            {
                ec = 2.070e11;
            }
            else
            {
                bool valid = double.TryParse(doc.editZengYi.Ec, out ec);
                if (!valid)
                {
                    throw new Exception(ErrorInfo.TxmlSyntaxError);
                }
            }

            try
            {
                MWArray[] array = (MWArray[])logic.calpsi(3, doc.editZengYi.A0, doc.editZengYi.Aw, doc.editZengYi.A1, doc.editZengYi.B,
                    doc.editZengYi.E, doc.editZengYi.V, ec, doc.editZengYi.Vc, doc.editZengYi.Es, doc.editZengYi.Vs);
                doc.editZengYi.Psi = ((MWNumericArray)array[0]).ToScalarDouble();
                doc.editZengYi.Kc = ((MWNumericArray)array[1]).ToScalarDouble();
                doc.editZengYi.Ks = ((MWNumericArray)array[2]).ToScalarDouble();
                doc.editZengYi.Time = DateTime.Now;

                if (doc.zengYis.Count < 10)
                {
                    doc.zengYis.Add(new ZengYiParamsViewModel(doc, doc.editZengYi.Params));
                }
                else
                {
                    // 往前移动 只保留10条记录
                    for (int i = 0; i < 9; i++)
                    {
                        doc.zengYis[i] = doc.zengYis[i + 1];
                    }
                    doc.zengYis[9] = new ZengYiParamsViewModel(doc, doc.editZengYi.Params);
                }
            }
            catch (Exception ex)
            {
                App.logger.Error(ErrorInfo.ComputeZengYiError, ex);
                throw new Exception(ErrorInfo.ComputeZengYiError);
            }
        }

        #endregion


    }
}
