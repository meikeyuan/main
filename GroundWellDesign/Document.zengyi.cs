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

        private void resetSnhBtn_Click(object sender, RoutedEventArgs e)
        {
            editZengYi.reset();
        }

        private void computeZYBtn_Click(object sender, RoutedEventArgs ex)
        {
            try
            {
                double ec;

                if (editZengYi.Ec.Equals(TgtxmlOpt[0]))
                {
                    ec = 2.050e11;
                }
                else
                {
                    ec = 2.070e11;
                }


                MWArray[] array = (MWArray[])logic.calpsi(3, editZengYi.A0, editZengYi.Aw, editZengYi.A1, editZengYi.B, 
                    editZengYi.E, editZengYi.V, ec, editZengYi.Vc, editZengYi.Es, editZengYi.Vs);
                editZengYi.Psi = ((MWNumericArray)array[0]).ToScalarDouble();
                editZengYi.Kc = ((MWNumericArray)array[1]).ToScalarDouble();
                editZengYi.Ks = ((MWNumericArray)array[2]).ToScalarDouble();
                editZengYi.time = DateTime.Now;

                if(zengYis.Count < 10)
                {
                    zengYis.Add(new ZengYiParams(this, editZengYi));
                }
                else
                {
                    //往前移动 只保留10条记录
                    for(int i = 0; i < 9; i++)
                    {
                        zengYis[i] = zengYis[i + 1];
                    }
                    zengYis[9] = new ZengYiParams(this, editZengYi);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("计算异常，请检查数据合理性");
            }
        }
    }
}