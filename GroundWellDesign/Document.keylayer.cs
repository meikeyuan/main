using GroundWellDesign.Util;
using MathWorks.MATLAB.NET.Arrays;
using MxDrawXLib;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GroundWellDesign
{
    partial class Document : Window
    {
        private void keyLayerDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender != keyLayerDataGrid || tabControl.SelectedItem != keyLayerTabItem)
            {
                return;
            }
            
        }

        private void keyLayerDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

            if (e.AddedCells.Count == 0)
                return;
            keyLayerDataGrid.BeginEdit();
        }

        private void showResBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int upcount = 0;
                ComputeHelper.computeKeyLayerOffset(this, ref upcount);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }

}
