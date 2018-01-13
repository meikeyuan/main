using GroundWellDesign.Util;
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
        private void taoGuanDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender != taoGuanDataGrid || tabControl.SelectedItem != taoGuanTabItem)
            {
                return;
            }
        }

        private void taoGuanDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count == 0)
                return;
            taoGuanDataGrid.BeginEdit();    //  进入编辑模式
        }

        private void calSafeBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ComputeHelper.computeSafe(this);
                for (int i = 0; i < keyLayers.Count; i++)
                {
                    if (keyLayers[i].Jqaqxs < 1 || keyLayers[i].Lsaqxs < 1)
                    {
                        keyLayers[i].IsDangerious = 1;
                    }
                    else
                    {
                        keyLayers[i].IsDangerious = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


    }
}
