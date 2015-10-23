using MxDrawXLib;
using System.Windows;

namespace GroundWellDesign
{
    partial class MainWindow :Window
    {


        //编辑dwg
        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            MxDrawSelectionSet ss = new MxDrawSelectionSet();

            IMxDrawResbuf spFilter = new MxDrawResbuf();


            ss.Select2(MCAD_McSelect.mcSelectionSetUserSelect, null, null, null, null);

            for (int i = 0; i < ss.Count; i++)
            {
                MxDrawEntity ent = ss.Item(i);

                if (ent is MxDrawMText)
                {
                    MxDrawMText mText = (MxDrawMText)ent;
                    if (mText.Contents.EndsWith("m"))
                    {
                        mText.Contents = "长度标注";
                    }
                    else
                    {
                        mText.Contents = "其他标注";
                    }

                }
                else if (ent is MxDrawDimRotated)
                {
                    MxDrawDimRotated dim = (MxDrawDimRotated)ent;
                    MessageBox.Show(dim.DimensionText);
                    dim.DimensionText = "20m";
                    dim.RecomputeDimBlock(true);

                }

            }
            cadViewer.ReDraw();
          }
    }
}
