using GroundWellDesign.Util;
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
                ComputeHelper.computeZengYi(this);
            }
            catch (Exception exx)
            {
                MessageBox.Show(exx.Message);
            }
        }
    }
}
