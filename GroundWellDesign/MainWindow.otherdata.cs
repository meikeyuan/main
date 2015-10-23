using System;
using System.Windows;

namespace GroundWellDesign
{
    partial class MainWindow : Window
    {

        private void showResBtn_Click(object sender, RoutedEventArgs e)
        {

            tabControl.SelectedIndex = 3;

        }

    }

    [Serializable]
    public class OtherData
    {

        public OtherData()
        {

        }

        public OtherData(OtherData od)
        {
            copy(od);
        }


        public void reset()
        {
            copy(new OtherData());
        }

        public void copy(OtherData od)
        {
            YanCengShenDu = od.YanCengShenDu;
            MeiCengMaiShen = od.MeiCengMaiShen;
            MeiCengQingJiao = od.MeiCengQingJiao;
            MeiCengHouDu = od.MeiCengHouDu;

            GZMChangDu = od.GZMChangDu;
            GZMTJSuDu = od.GZMTJSuDu;
            JSWZJuLi = od.JSWZJuLi;
        }

        public double YanCengShenDu { get; set; }
        public double MeiCengMaiShen { get; set; }
        public double MeiCengQingJiao { get; set; }
        public double MeiCengHouDu { get; set; }
        public double GZMChangDu { get; set; }
        public double GZMTJSuDu { get; set; }

        public double TJSJ1 { get; set; }
        public double TJSJ2 { get; set; }
        public double TJSJ3 { get; set; }
        public double TJSJ4 { get; set; }

        public double JSWZJuLi { get; set; }



    }
}
