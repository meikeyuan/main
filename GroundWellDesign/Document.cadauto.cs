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

            //先进行参数计算
            //第一点是：每一开深度，三开底部和煤层顶板的距离
            double oneKai = 30;

            if (wanQuDaiTb.Text.Trim().Equals(""))
            {
                MessageBox.Show("部分计算未完成，请修正再尝试自动设计");
                return;
            }
            double twoKai = double.Parse(wanQuDaiTb.Text) + 20;

            int meiIndex = layers.Count - 1;
            for (; meiIndex >= 0; meiIndex--)
            {
                if (layers[meiIndex].yanXing.Equals("煤"))
                {
                    break;
                }
            }
            if (meiIndex <= 0)
            {
                MessageBox.Show("岩层数据有误，请修正再尝试自动设计");
                tabControl.SelectedItem = gridinputTabItem;
                return;
            }
            double threeKai = layers[meiIndex - 1].leiJiShenDu - 10;
            double threeToMei = 10;

            //第二点是：各级套管外径；水泥环厚度
            double snhHoudu = a1 - aw;
            //double tgwj1, tgwj2, tgwj3;

            //第三点是：局部固井的深度值



            //第四点是：高位位置数量，破坏主因
            if(keyLayers[0].IsDangerous == null)
            {
                MessageBox.Show("请先计算套管安全系数");
                return;
            }
            int dangerCount = 0;
            int keycount = keyLayers.Count;
            for (int i = 0; i < keycount; i++ )
            {
                if(keyLayers[i].IsDangerous == true)
                {
                    dangerCount++;
                }
            }


            //显示cad
            tabControl.SelectedItem = autoDesignCadTabItem;
            cadViewer.OpenDwgFile("cads/三开-一全固二局固三" + AutoWjfs3.Substring(0, 2) + ".dwg");
            cadViewer.ZoomCenter(3000, 300);
            cadViewer.ZoomScale(1);

            //参数标注
            //各级套管直径、长度、水泥环厚度、三开底部和煤层顶板的距离
            MxDrawSelectionSet ss = new MxDrawSelectionSet();
            IMxDrawResbuf spFilter = new MxDrawResbuf();
            ss.Select2(MCAD_McSelect.mcSelectionSetAll, null, null, null, null);

            for (int j = 0; j < ss.Count; j++)
            {
                MxDrawEntity ent = ss.Item(j);

                if (ent is MxDrawMText)
                {
                    MxDrawMText mText = (MxDrawMText)ent;
                    string contents = mText.Contents;
                    if (contents.Contains("50m"))
                    {
                        mText.Contents = contents.Replace("50", oneKai.ToString("f3"));
                    }
                    else if (contents.Contains("250m"))
                    {
                        mText.Contents = contents.Replace("250", twoKai.ToString("f3"));
                    }
                    else if (contents.Contains("310m"))
                    {
                        mText.Contents = contents.Replace("310", threeKai.ToString("f3"));
                    }
                }
                /*else if (ent is MxDrawDimRotated)
                {
                    MxDrawDimRotated dim = (MxDrawDimRotated)ent;
                    MessageBox.Show(dim.DimensionText);
                    dim.DimensionText = "20m";
                    dim.RecomputeDimBlock(true);
                }*/
            }
            //旁边标出各级套管型号，固井工艺、完井工艺、高危位置等





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
