using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace GroundWellDesign
{

    [Serializable]
    public class LayerBaseParams : ICloneable
    {
        public LayerBaseParams()
        {
            YanXing = Document.YanXingOpt[1];
            MiaoShu = "";
            DataBaseKey = "";
            WellNamePK = "";
        }

        public Object Clone()
        {
            return this.MemberwiseClone();
        }

        public object this[int index]
        {
            get
            {
                switch(index)
                {
                    case 0:
                        return YanXing;
                    case 1:
                        return LeiJiShenDu;
                    case 2:
                        return JuLiMeiShenDu;
                    case 3:
                        return CengHou;
                    case 4:
                        return ZiRanMiDu;
                    case 5:
                        return BianXingMoLiang;
                    case 6:
                        return KangLaQiangDu;
                    case 7:
                        return KangYaQiangDu;
                    case 8:
                        return TanXingMoLiang;
                    case 9:
                        return BoSonBi;
                    case 10:
                        return NeiMoCaJiao;
                    case 11:
                        return NianJuLi;
                    case 12:
                        return F;
                    case 13:
                        return Q0;
                    case 14:
                        return Q1;
                    case 15:
                        return Q2;
                    case 16:
                        return MiaoShu;
                    case 17:
                        return DataBaseKey;
                    case 18:
                        return WellNamePK;
                    case 19:
                        return QxJQWY;
                    case 20:
                        return ZxJQWY;
                    case 21:
                        return JqHWY;
                    case 22:
                        return IsKeyLayer;
                    default:
                        return null;
                }
            }

            set
            {
                double double_value;
                string string_value;

                if(value == null)
                {
                    double_value = 0.0;
                    string_value = null;
                }
                else
                {
                    double.TryParse(value.ToString(), out double_value);
                    string_value = value.ToString();
                }

                switch (index)
                {
                    case 0:
                        YanXing = string_value;
                        break;
                    case 1:
                        LeiJiShenDu = double_value;
                        break;
                    case 2:
                        JuLiMeiShenDu = double_value;
                        break;
                    case 3:
                        CengHou = double_value;
                        break;
                    case 4:
                        ZiRanMiDu = double_value;
                        break;
                    case 5:
                        BianXingMoLiang = double_value;
                        break;
                    case 6:
                        KangLaQiangDu = double_value;
                        break;
                    case 7:
                        KangYaQiangDu = double_value;
                        break;
                    case 8:
                        TanXingMoLiang = double_value;
                        break;
                    case 9:
                        BoSonBi = double_value;
                        break;
                    case 10:
                        NeiMoCaJiao = double_value;
                        break;
                    case 11:
                        NianJuLi = double_value;
                        break;
                    case 12:
                        F = double_value;
                        break;
                    case 13:
                        Q0 = double_value;
                        break;
                    case 14:
                        Q1 = double_value;
                        break;
                    case 15:
                        Q2 = double_value;
                        break;
                    case 16:
                        MiaoShu = string_value;
                        break;
                    case 17:
                        DataBaseKey = string_value;
                        break;
                    case 18:
                        WellNamePK = string_value;
                        break;
                    case 19:
                        QxJQWY = double_value;
                        break;
                    case 20:
                        ZxJQWY = double_value;
                        break;
                    case 21:
                        JqHWY = double_value;
                        break;
                    case 22:
                        IsKeyLayer = (bool)value;
                        break;
                    default:
                        break;
                }
            }
        }

        public string YanXing { get; set; }
        public double LeiJiShenDu { get; set; }
        public double JuLiMeiShenDu { get; set; }
        public double CengHou { get; set; }
        public double ZiRanMiDu { get; set; }
        public double BianXingMoLiang { get; set; }
        public double KangLaQiangDu { get; set; }
        public double KangYaQiangDu { get; set; }
        public double TanXingMoLiang { get; set; }
        public double BoSonBi { get; set; }
        public double NeiMoCaJiao { get; set; }
        public double NianJuLi { get; set; }
        public double F { get; set; }
        public double Q0 { get; set; }
        public double Q1 { get; set; }
        public double Q2 { get; set; }
        public string MiaoShu { get; set; }

        public string DataBaseKey { get; set; }
        public string WellNamePK { get; set; }


        public double QxJQWY { get; set; }
        public double ZxJQWY { get; set; }
        public double JqHWY { get; set; }

        public bool IsKeyLayer { get; set; }
    }
}
