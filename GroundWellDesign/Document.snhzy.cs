using MathWorks.MATLAB.NET.Arrays;
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

        private void resetSnhBtn_Click(object sender, RoutedEventArgs e)
        {
            Ec = TgtxmlOpt[0];
            Vc = 0;
            Es = 0;
            Vs = 0;
            E = 0;
            V = 0;
            A0 = 0;
            Aw = 0;
            A1 = 0;
            B = 0;


            ecCombo.Text = Ec;
            vcTb.Text = 0.000 + "";
            esTb.Text = 0.000 + "";
            vsTb.Text = 0.000 + "";
            eTb.Text = 0.000 + "";
            vTb.Text = 0.000 + "";
            a0Tb.Text = 0.000 + "";
            awTb.Text = 0.000 + "";
            aw2Tb.Text = 0.000 + "";
            a1Tb.Text = 0.000 + "";
            a12Tb.Text = 0.000 + "";
            bTb.Text = 0.000 + "";

            kcTb.Text = 0.000 + "";
            ksTb.Text = 0.000 + "";
            psiTb.Text = 0.000 + "";
        }

        private void saveSnhBtn_Click(object sender, RoutedEventArgs ex)
        {
            try
            {
                double ec;

                if (Ec.Equals(TgtxmlOpt[0]))
                {
                    ec = 2.050e11;
                }
                else
                {
                    ec = 2.070e11;
                }


                MWArray[] array = (MWArray[])logic.calpsi(3, A0, Aw, A1, B, E, V, ec, Vc, Es, Vs);
                double psi = ((MWNumericArray)array[0]).ToScalarDouble();
                double kc = ((MWNumericArray)array[1]).ToScalarDouble();
                double ks = ((MWNumericArray)array[2]).ToScalarDouble();

                kcTb.Text = kc.ToString("f3");
                ksTb.Text = ks.ToString("f3");
                psiTb.Text = psi.ToString("f3");
            }
            catch (Exception)
            {
                MessageBox.Show("计算异常，请检查数据合理性");
            }
        }
    }
}
