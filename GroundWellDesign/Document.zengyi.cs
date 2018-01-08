using GroundWellDesign.ViewModel;
using MathWorks.MATLAB.NET.Arrays;
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

        private void ycbhCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox combo = sender as ComboBox;
            int index = combo.SelectedIndex;
            if (index == -1)
            {
                return;
            }
            yanxingText.Text = layers[index].YanXing;
            editZengYi.Es = layers[index].TanXingMoLiang;
            editZengYi.Vs = layers[index].BoSonBi;
        }


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
                else if(editZengYi.Ec.Equals(TgtxmlOpt[1]))
                {
                    ec = 2.070e11;
                }
                else
                {
                    bool valid = double.TryParse(editZengYi.Ec, out ec);
                    if(!valid)
                    {
                        MessageBox.Show("套管弹性模量Ec格式不正确，请重新输入。");
                        return;
                    }
                }


                MWArray[] array = (MWArray[])logic.calpsi(3, editZengYi.A0, editZengYi.Aw, editZengYi.A1, editZengYi.B, 
                    editZengYi.E, editZengYi.V, ec, editZengYi.Vc, editZengYi.Es, editZengYi.Vs);
                editZengYi.Psi = ((MWNumericArray)array[0]).ToScalarDouble();
                editZengYi.Kc = ((MWNumericArray)array[1]).ToScalarDouble();
                editZengYi.Ks = ((MWNumericArray)array[2]).ToScalarDouble();
                editZengYi.Time = DateTime.Now;

                if(zengYis.Count < 10)
                {
                    zengYis.Add(new ZengYiParamsViewModel(this, editZengYi.Params));
                }
                else
                {
                    //往前移动 只保留10条记录
                    for(int i = 0; i < 9; i++)
                    {
                        zengYis[i] = zengYis[i + 1];
                    }
                    zengYis[9] = new ZengYiParamsViewModel(this, editZengYi.Params);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("计算异常，请检查数据合理性");
            }
        }
    }
}
