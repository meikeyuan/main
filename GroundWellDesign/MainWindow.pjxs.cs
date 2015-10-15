using System.Collections.Generic;
using System.Windows;

namespace GroundWellDesign
{
    public partial class MainWindow : Window
    {

        //录入的岩性评价系数
        List<PJXS> pjxss = new List<PJXS>();
    }



    public class PJXS
    {

        public PJXS()
        {

        }

        public PJXS(PJXS pjxs)
        {
            copy(pjxs);
        }


        public void reset()
        {
            copy(new PJXS());
        }

        public void copy(PJXS pjxs)
        {
            YanXing = pjxs.YanXing;
            Q0 = pjxs.Q0;
            Q1 = pjxs.Q1;
            Q2 = pjxs.Q2;

        }

        public string YanXing { get; set; }

        public string Q0 { get; set; }

        public string Q1 { get; set; }
        public string Q2 { get; set; }
    }

}
