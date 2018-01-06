using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroundWellDesign
{


    [Serializable]
    public class ZengYiParams : ICloneable
    {
        public ZengYiParams()
        {
            Ec = Document.TgtxmlOpt[0];
        }

        public Object Clone()
        {
            return this.MemberwiseClone();
        }

        public DateTime Time { get; set; }

        public string Ec { get; set; }
        public double Vc { get; set; }
        public double Es { get; set; }
        public double Vs { get; set; }
        public double E { get; set; }
        public double V { get; set; }

        public double A0 { get; set; }
        public double Aw { get; set; }
        public double A1 { get; set; }
        public double B { get; set; }

        public double Kc { get; set; }
        public double Ks { get; set; }
        public double Psi { get; set; }

    }
}
