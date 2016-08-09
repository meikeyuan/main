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

        private void resetManuBtn_Click(object sender, RoutedEventArgs e)
        {
            manuDesignParams.reset();
        }

        private void makeImgBtn_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedItem = manCadTabItem;
            cadViewer2.OpenDwgFile("cads/二开-一局固二裸孔.dwg");
            cadViewer2.ZoomCenter(1500, 300);
            cadViewer2.ZoomScale(1);
        }
    }
}
