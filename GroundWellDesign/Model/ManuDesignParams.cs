using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GroundWellDesign
{
    [Serializable]
    public class ManuDesignParams : ICloneable
    {
        public ManuDesignParams()
        {
            jieGou = Document.JieGouOpt[0];
            tggj1 = Document.TggjOpt[0];
            gjfs1 = Document.GjfsOpt[0];
            tggj2 = Document.TggjOpt[0];
            gjfs2 = Document.GjfsOpt[0];
            wjfs2 = Document.WjfsOpt[0];
            tggj3 = Document.TggjOpt[0];
            wjfs3 = Document.WjfsOpt[0];
        }

        public Object Clone()
        {
            return this.MemberwiseClone();
        }


        public string jieGou;

        public double zjsd1;
        public string tggj1;
        public string tgxh1;
        public string gjfs1;
        public string miaoshu1;

        public double zjsd2;
        public string tggj2;
        public string tgxh2;
        public string gjfs2;
        public string wjfs2;
        public string miaoshu2;

        public double zjsd3;
        public string tggj3;
        public string tgxh3;
        public string wjfs3;
        public string miaoshu3;
    }

}
