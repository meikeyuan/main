using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroundWellDesign
{
    [Serializable]
    public class KeyLayerParams
    {
        public KeyLayerParams()
        {
            this.Kjqd = Document.KjqdOpt[0];
            this.Klqd = Document.KlqdOpt[0];
            this.Tgtxml = Document.TgtxmlOpt[0];
        }

        public KeyLayerParams Clone()
        {
            return this.MemberwiseClone() as KeyLayerParams;
        }

        public int Ycbh { get; set; }
        public double Ycsd { get; set; }
        public double Mcms { get; set; }
        public double Fypjxs { get; set; }
        public double Fypjxsxz { get; set; }
        public double Gzmtjsj { get; set; }

        public double Cfcdcjwy { get; set; }
        public double Gzmtjjl { get; set; }
        public double Zx { get; set; }
        public double Qx { get; set; }
        public double Cdyxbj { get; set; }


        public double Gdydj { get; set; }
        public double Qxhckj { get; set; }
        public double Cfkckjjl { get; set; }
        public double Sjxcxs { get; set; }
        public double Yczdxcz { get; set; }

        public double Jsdjscjwy { get; set; }
        public double Jsdjslcwy { get; set; }

        //套管安全系数部分
        public double Tgwj { get; set; }
        public double Tgbh { get; set; }
        public string Tgtxml { get; set; }

        public double Lsqycd { get; set; }
        public string Klqd { get; set; }
        public double Lsyl { get; set; }
        public double Lsaqxs { get; set; }

        public double Jqqycd { get; set; }
        public string Kjqd { get; set; }
        public double Zdjqyl { get; set; }
        public double Zdjqyb { get; set; }
        public double Jqaqxs { get; set; }

        public bool IsDangerious { get; set; }
    }
}
