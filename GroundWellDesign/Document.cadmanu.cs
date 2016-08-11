using MxDrawXLib;
using System;
using System.Collections.Generic;
using System.IO;
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

        private void makeManuImgBtn_Click(object sender, RoutedEventArgs e)
        {
            //先进行参数计算
            //每一开深度，三开底部和煤层顶板的距离
            double oneKai = manuDesignParams.zjsd1;
            double twoKai = manuDesignParams.zjsd2;
            double threeKai = manuDesignParams.zjsd3;
            /*int meiIndex = layers.Count - 1;
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
            double threeKai = layers[meiIndex - 1].leiJiShenDu - 10;*/
            double threeToMei = 10;

            //局部固井的深度值
            double jbgjShendu = 0;

            //各级套管外径；水泥环厚度
            double snhHoudu = a1 - aw;
            double tgwj1, tgwj2, tgwj3;
            tgwj1 = tgwj2 = tgwj3 = 0;


            //高位位置数量，破坏主因
            if (keyLayers[0].IsDangerous == null)
            {
                MessageBox.Show("请先计算套管安全系数");
                return;
            }
            int dangerCount = 0;
            int keycount = keyLayers.Count;
            for (int i = 0; i < keycount; i++)
            {
                if (keyLayers[i].IsDangerous == true)
                {
                    dangerCount++;
                }
            }

            //显示cad
            tabControl.SelectedItem = manDesignCadTabItem;
            string gy = "";
            if(manuDesignParams.jieGou.Equals(JieGouOpt[0]))
            {
                gy = "一" + manuDesignParams.gjfs1 + "二" + manuDesignParams.wjfs2;
                cadViewer2.OpenDwgFile("cads/二开-" + gy + ".dwg");
            }
            else
            {
                gy = "一全固" + "二" + manuDesignParams.gjfs2 + "三" + manuDesignParams.wjfs3;
                cadViewer2.OpenDwgFile("cads/三开-" + gy + ".dwg");
            }
            cadViewer2.ZoomCenter(2000, 300);
            cadViewer2.ZoomScale(1);

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
                    if (contents.Contains("地面井名称"))
                    {
                        mText.Contents = contents.Replace("地面井名称", "地面井名称:" + Path.GetFileNameWithoutExtension(FilePath));
                    }
                    //描述
                    else if (contents.Contains("一开结构"))
                    {
                        mText.Contents = contents.Replace("一开结构", "一开结构\\P\\P" + manuDesignParams.miaoshu1);
                    }
                    else if (contents.Contains("二开结构"))
                    {
                        mText.Contents = contents.Replace("二开结构", "二开结构\\P\\P" + manuDesignParams.miaoshu2);
                    }
                    else if (contents.Contains("三开结构"))
                    {
                        mText.Contents = contents.Replace("三开结构", "二开结构\\P\\P" + manuDesignParams.miaoshu3);
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
                        string tgcs = "一开型号：" + manuDesignParams.tgxh1 + "   外径：" + tgwj1 + "\\P" +
                            "二开型号：" + manuDesignParams.tgxh2 + "   外径：" + tgwj2;
                        if(manuDesignParams.jieGou.Equals(JieGouOpt[1]))
                        {
                            tgcs += "\\P三开型号：" + manuDesignParams.tgxh3 + "   外径：" + tgwj3;
                        }
                        mText.Contents = contents.Replace("各级套管型号和参数", "各级套管型号和参数\\P\\P" + tgcs);
                    }
                    //固井工艺、完井工艺、水泥环厚度
                    else if (contents.Contains("固井工艺、完井工艺、水泥环厚度"))
                    {
                        mText.Contents = contents.Replace("固井工艺、完井工艺、水泥环厚度", "固井工艺、完井工艺、水泥环厚度\\P\\P" +
                            gy + "\\P" + "水泥环厚度：" + snhHoudu);
                    }
                    //高危位置
                    else if (contents.Contains("高危位置"))
                    {
                        mText.Contents = contents.Replace("高危位置", "高危位置数量：" + dangerCount);
                    }
                }
            }
            cadViewer2.ReDraw();
        }
    }
}
