using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace GroundWellDesign
{
    public class LayerParams : INotifyPropertyChanged
    {

        public static List<String> YanXingOpt { get; set; }
        public static List<String> CaiDongOpt { get; set; }
        static LayerParams()
        {
            YanXingOpt = new List<string> { "黄土", "细粒砂岩", "泥岩", "中粒砂岩", "粉砂岩", "砂质泥岩", "粗粒砂岩", "细砂岩", "中砂岩", "煤" };
            CaiDongOpt = new List<string> { "初次采动Q0", "重复采动Q1", "重复采动Q2" };
        }

        public LayerParams()
        {
            YanXing = YanXingOpt[0];
            CaiDong = CaiDongOpt[0];

            MiaoShu = "描述";

        }

        public LayerParams(LayerParams layer)
        {
            copy(layer);
        }

        public bool equals(LayerParams layer)
        {
            return YanXing.Equals(layer.YanXing) &&
            CaiDong.Equals(layer.CaiDong) &&
            LeiJiShenDu.Equals(layer.LeiJiShenDu) &&
            JuLiMeiShenDu.Equals(layer.JuLiMeiShenDu) &&
            CengHou.Equals(layer.CengHou) &&
            ZiRanMiDu.Equals(layer.ZiRanMiDu) &&
            BianXingMoLiang.Equals(layer.BianXingMoLiang) &&
            KangLaQiangDu.Equals(layer.KangLaQiangDu) &&
            KangYaQiangDu.Equals(layer.KangYaQiangDu) &&
            TanXingMoLiang.Equals(layer.TanXingMoLiang) &&
            BoSonBi.Equals(layer.BoSonBi) &&
            NeiMoCaJiao.Equals(layer.NeiMoCaJiao) &&
            NianJuLi.Equals(layer.NianJuLi) && MiaoShu.Equals(layer.MiaoShu);

        }

        public void reset()
        {
            copy(new LayerParams());
        }

        public void copy(LayerParams layer)
        {
            YanXing = layer.YanXing;
            CaiDong = layer.CaiDong;
            LeiJiShenDu = layer.LeiJiShenDu;
            JuLiMeiShenDu = layer.JuLiMeiShenDu;
            CengHou = layer.CengHou;
            ZiRanMiDu = layer.ZiRanMiDu;
            BianXingMoLiang = layer.BianXingMoLiang;
            KangLaQiangDu = layer.KangLaQiangDu;
            KangYaQiangDu = layer.KangYaQiangDu;
            TanXingMoLiang = layer.TanXingMoLiang;
            BoSonBi = layer.BoSonBi;
            NeiMoCaJiao = layer.NeiMoCaJiao;
            NianJuLi = layer.NianJuLi;
            MiaoShu = layer.MiaoShu;
        }

        String yanXing;
        public String YanXing
        {
            get { return yanXing; }
            set
            {
                yanXing = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("YanXing"));
                }
            }
        }

        String caiDong;
        public String CaiDong
        {
            get { return caiDong; }
            set
            {
                caiDong = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("CaiDong"));
                }
            }
        }

        double leiJiShenDu;
        public double LeiJiShenDu
        {
            get { return leiJiShenDu; }
            set
            {
                leiJiShenDu = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("LeiJiShenDu"));
                }
            }
        }

        double juLiMeiShenDu;
        public double JuLiMeiShenDu
        {
            get { return juLiMeiShenDu; }
            set
            {
                juLiMeiShenDu = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("JuLiMeiShenDu"));
                }
            }
        }

        double cengHou;
        public double CengHou
        {
            get { return cengHou; }
            set
            {
                cengHou = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("CengHou"));
                }
            }
        }

        double ziRanMiDu;
        public double ZiRanMiDu
        {
            get { return ziRanMiDu; }
            set
            {
                ziRanMiDu = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("ZiRanMiDu"));
                }
            }
        }

        double bianXingMoLiang;
        public double BianXingMoLiang
        {
            get { return bianXingMoLiang; }
            set
            {
                bianXingMoLiang = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("BianXingMoLiang"));
                }
            }
        }

        double kangLaQiangDu;
        public double KangLaQiangDu
        {
            get { return kangLaQiangDu; }
            set
            {
                kangLaQiangDu = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("KangLaQiangDu"));
                }
            }
        }

        double kangYaQiangDu;
        public double KangYaQiangDu
        {
            get { return kangYaQiangDu; }
            set
            {
                kangYaQiangDu = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("KangYaQiangDu"));
                }
            }
        }

        double tanXingMoLiang;
        public double TanXingMoLiang
        {
            get { return tanXingMoLiang; }
            set
            {
                tanXingMoLiang = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("TanXingMoLiang"));
                }
            }
        }

        double boSonBi;
        public double BoSonBi
        {
            get { return boSonBi; }
            set
            {
                boSonBi = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("BoSonBi"));
                }
            }
        }

        double neiMoCaJiao;
        public double NeiMoCaJiao
        {
            get { return neiMoCaJiao; }
            set
            {
                neiMoCaJiao = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("NeiMoCaJiao"));
                }
            }
        }

        double nianJuLi;
        public double NianJuLi
        {
            get { return nianJuLi; }
            set
            {
                nianJuLi = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("NianJuLi"));
                }
            }
        }

        string miaoShu;
        public string MiaoShu
        {
            get { return miaoShu; }
            set
            {
                miaoShu = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("MiaoShu"));
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
