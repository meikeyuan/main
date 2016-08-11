using MxDrawXLib;
using System.IO;
using System.Windows;

namespace GroundWellDesign
{
    partial class Document :Window
    {
        private void resetAutoBtn_Click(object sender, RoutedEventArgs e)
        {
            autoTgxh1Combo.Text = AutoTgxh1 = TggjOpt[0];

            autoTgxh2Combo.Text = AutoTgxh2 = TggjOpt[0];
            autoTgxh3Combo.Text = AutoTgxh3 = TggjOpt[0];
            autoWjfs3Combo.Text = AutoWjfs3 = WjfsOpt[0];

            autoMiaoshu1Tb.Text = AutoMiaoshu1 = "";
            autoMiaoshu2Tb.Text = AutoMiaoshu2 = "";
            autoMiaoshu3Tb.Text = AutoMiaoshu3 = "";
        }

        private void makeAutoImgBtn_Click(object sender, RoutedEventArgs e)
        {

            //先进行参数计算
            //每一开深度，三开底部和煤层顶板的距离
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

            //局部固井的深度值
            double jbgjShendu = 0;

            //各级套管外径；水泥环厚度
            double snhHoudu = a1 - aw;
            double tgwj1, tgwj2, tgwj3;
            tgwj1 = tgwj2 = tgwj3 = 0;

           
            //高位位置数量，破坏主因
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
            cadViewer.OpenDwgFile("cads/三开-一全固二局固三" + AutoWjfs3 + ".dwg");
            cadViewer.ZoomCenter(2500, 300);
            cadViewer.ZoomScale(1);

            //参数标注
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
                    //MessageBox.Show(contents);
                    //地面井名称
                    if(contents.Contains("地面井名称"))
                    {
                        mText.Contents = contents.Replace("地面井名称", "地面井名称:" + Path.GetFileNameWithoutExtension(FilePath));
                    }
                    //描述
                    else if(contents.Contains("一开结构"))
                    {
                        mText.Contents = contents.Replace("一开结构", "一开结构\\P\\P" + AutoMiaoshu1);
                    }
                    else if(contents.Contains("二开结构")){
                        mText.Contents = contents.Replace("二开结构", "二开结构\\P\\P" + AutoMiaoshu2);
                    }
                    else if(contents.Contains("三开结构")){
                        mText.Contents = contents.Replace("三开结构", "三开结构\\P\\P" + AutoMiaoshu3);
                    }
                    //各级套管深度、局部固井深度、三开底部和煤层顶板的距离
                    else if (contents.Contains("一开深度"))
                    {
                        mText.Contents = contents.Replace("一开深度", oneKai.ToString("f3") + "m");
                    }
                    else if (contents.Contains("二开深度"))
                    {
                        mText.Contents = contents.Replace("二开深度", twoKai.ToString("f3") + "m");
                    }
                    else if (contents.Contains("三开深度"))
                    {
                        mText.Contents = contents.Replace("三开深度", threeKai.ToString("f3") + "m");
                    }
                    else if (contents.Contains("局部固井深度"))
                    {
                        mText.Contents = contents.Replace("局部固井深度", jbgjShendu.ToString("f3") + "m");
                    }
                    else if (contents.Contains("底部到顶板距离"))
                    {
                        mText.Contents = contents.Replace("底部到顶板距离", threeToMei.ToString("f3") + "m");
                    }
                    //各级套管型号和参数
                    else if (contents.Contains("各级套管型号和参数"))
                    {
                        mText.Contents = contents.Replace("各级套管型号和参数", "各级套管型号和参数\\P\\P" +
                            "一开型号：" + AutoTgxh1 + "   外径：" + tgwj1 + "\\P" +
                            "二开型号：" + AutoTgxh2 + "   外径：" + tgwj2 + "\\P" + 
                            "三开型号：" + AutoTgxh3 + "   外径：" + tgwj3 );
                    }
                    //固井工艺、完井工艺、水泥环厚度
                    else if (contents.Contains("固井工艺、完井工艺、水泥环厚度"))
                    {
                        mText.Contents = contents.Replace("固井工艺、完井工艺、水泥环厚度", "固井工艺、完井工艺、水泥环厚度\\P\\P" +
                            "一全固二局固三" + AutoWjfs3 + "\\P" +
                            "水泥环厚度：" + snhHoudu);
                    }
                    //高危位置
                    else if (contents.Contains("高危位置"))
                    {
                        mText.Contents = contents.Replace("高危位置", "高危位置数量：" + dangerCount);
                    }
                }
            }
            cadViewer.ReDraw();
        }

    }
}
