using MxDrawXLib;
using System.Windows;

namespace GroundWellDesign
{
    partial class Document :Window
    {
        private void resetAutoBtn_Click(object sender, RoutedEventArgs e)
        {
            manuDesignParams.reset();
        }

        private void makeAutoImgBtn_Click(object sender, RoutedEventArgs e)
        {
            //先进行参数计算  应该在选择菜单的时候就计算？
            double oneKai = 20;
            int i = layers.Count - 1;
            for (; i >= 0; i--)
            {
                if (layers[i].yanXing.Equals("煤"))
                {
                    break;
                }
            }
            if (i <= 0)
            {
                MessageBox.Show("岩层数据有误，请修正再尝试自动设计");
                tabControl.SelectedItem = gridinputTabItem;
                return;
            }

            if (wanQuDaiTb.Text.Trim().Equals(""))
            {
                MessageBox.Show("部分计算未完成，请修正再尝试自动设计");
                return;
            }

            double twoKai = double.Parse(wanQuDaiTb.Text) + 20;
            double threeKai = layers[i - 1].leiJiShenDu - 10;

            //显示cad 参数标注
            tabControl.SelectedItem = autoDesignCadTabItem;
            cadViewer.OpenDwgFile("cads/三开-一全固二局固三悬挂-默认.dwg");
            cadViewer.ZoomCenter(1500, 300);
            cadViewer.ZoomScale(1);

            MxDrawSelectionSet ss = new MxDrawSelectionSet();
            IMxDrawResbuf spFilter = new MxDrawResbuf();
            ss.Select2(MCAD_McSelect.mcSelectionSetUserSelect, null, null, null, null);

            for (int j = 0; j < ss.Count; j++)
            {
                MxDrawEntity ent = ss.Item(j);

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
            tabControl.SelectedItem = autoDesignCadTabItem;

        }


    }
}
