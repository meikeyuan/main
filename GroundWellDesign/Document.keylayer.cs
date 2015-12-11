using MathWorks.MATLAB.NET.Arrays;
using MxDrawXLib;
using System;
using System.Windows;

namespace GroundWellDesign
{
    partial class Document : Window
    {
        private void showCadBtn_Click(object sender, RoutedEventArgs e)
        {
            //操作控件
            MxDrawSelectionSet ss = new MxDrawSelectionSet();
            // 创建过滤对象.
            MxDrawResbuf spFilte = new MxDrawResbuf();
            // 把文字对象，当着过滤条件.
            spFilte.AddStringEx("MTEXT", 5020);
            // 得到图上，所有文字对象.
            ss.Select2(MCAD_McSelect.mcSelectionSetAll, null, null, null, spFilte);
            // 遍历每个文字.
            for (int i = 0; i < ss.Count; i++)
            {
                MxDrawEntity ent = ss.Item(i);
                if (ent == null)
                    continue;
                if (ent.ObjectName == "McDbText" || ent.ObjectName == "McDbMText")
                {
                    // 是文字
                    MxDrawMText text = (MxDrawMText)ent;
                    string sTxt = text.Contents;

                    if (sTxt.StartsWith("地面井编号"))
                    {
                        text.Contents = "地面井编号:  #1";
                    }
                    else if (sTxt.StartsWith("布井位置描述"))
                    {
                        text.Contents = "布井位置描述: \n离回风巷100M处";
                    }
                    else if (sTxt.StartsWith("各级套管型号及参数"))
                    {
                        text.Contents = "各级套管型号及参数: \n型号。。。参数。。。";
                    }
                    else if (sTxt.StartsWith("固井工艺"))
                    {
                        text.Contents = "固井工艺: 工艺。。。";
                    }
                    else if (sTxt.StartsWith("高危位置数量"))
                    {
                        text.Contents = "高危位置数量: 1个";
                    }
                }

            }




            //显示
            tabControl.SelectedItem = showTabItem;

        }

        private void showResBtn_Click(object sender, RoutedEventArgs e)
        {
            switch(compute(keyLayers.Count))
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


        public enum ERRORCODE { 计算成功, 计算异常, 没有关键层数据, 没有评价系数修正系数, 没有煤层倾角和煤层厚度, 没有回采区长度, 没有工作面推进速度 }
        //返回值
        public ERRORCODE compute(int count)
        {
            if (count == 0)
            {
                return ERRORCODE.没有关键层数据;
            }


            //需要修正的p
            if (pjxsxz == 0)
            {
                
                return ERRORCODE.没有评价系数修正系数;
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

                double[] wmax = (double[])Wmax.ToVector(MWArrayComponent.Real);
                for (int i = 0; i < count; i++)
                {
                    keyLayers[i].Cfcdcjwy = wmax[i];
                }




                //走向l0/H0  倾向L0/H0
                //需要走向倾向
                if (HcqZXcd == 0 || HcqQXcd == 0)
                {
                    
                    return ERRORCODE.没有回采区长度;
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
                    
                    return ERRORCODE.没有工作面推进速度;
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

                return ERRORCODE.计算成功;

            }
            catch (Exception)
            {
                
                return ERRORCODE.计算异常;
            }


        }


        private MWNumericArray transpose(MWNumericArray array)
        {
            double[] tmp = (double[])array.ToVector(MWArrayComponent.Real);
            return new MWNumericArray(tmp.Length, 1, tmp);
        }


    }

}
