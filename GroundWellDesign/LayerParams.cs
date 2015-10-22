﻿using System;
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
            YanXingOpt = new List<string> { "地表", "黄土", "细粒砂岩", "泥岩", "中粒砂岩", "粉砂岩", "砂质泥岩", "粗粒砂岩", "细砂岩", "中砂岩", "煤" };
            CaiDongOpt = new List<string> { "初次采动Q0", "重复采动Q1", "重复采动Q2" };
        }

        public LayerParams()
        {
            YanXing = YanXingOpt[0];
            CaiDong = CaiDongOpt[0];




            miaoShu = "";

        }

        public LayerParams(LayerParams layer)
        {
            copy(layer);
        }

        public bool equals(LayerParams layer)
        {
            return YanXing.Equals(layer.YanXing) &&
            CaiDong.Equals(layer.CaiDong) &&

            LeiJiShenDu == layer.LeiJiShenDu &&
            JuLiMeiShenDu.Equals(layer.JuLiMeiShenDu) &&
            CengHou.Equals(layer.CengHou) &&
            ZiRanMiDu.Equals(layer.ZiRanMiDu) &&
            BianXingMoLiang.Equals(layer.BianXingMoLiang) &&
            KangLaQiangDu.Equals(layer.KangLaQiangDu) &&
            KangYaQiangDu.Equals(layer.KangYaQiangDu) &&
            TanXingMoLiang.Equals(layer.TanXingMoLiang) &&
            BoSonBi.Equals(layer.BoSonBi) &&
            NeiMoCaJiao.Equals(layer.NeiMoCaJiao) &&
            NianJuLi.Equals(layer.NianJuLi) && MiaoShu.Equals(layer.MiaoShu) &&
            Q1 == layer.Q1 && Q2 == layer.Q2 && Q0 == layer.Q0;


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
            Q0 = layer.Q0;
            Q1 = layer.Q1;
            Q2 = layer.Q2;

            MiaoShu = layer.MiaoShu;
        }

        String yanXing;
        public String YanXing
        {
            get { return yanXing; }
            set
            {
                yanXing = value;
                int index = YanXingOpt.IndexOf(value);
                switch (index)
                {
                    case 0:  //地表
                        break;
                    case 10: //煤  刷新距离没深度
                        int i = MainWindow.layers.Count;
                        for(i -= 3; i >= 0; i--)
                        {
                            MainWindow.layers[i].JuLiMeiShenDu = MainWindow.layers[i + 1].cengHou + MainWindow.layers[i + 1].juLiMeiShenDu;
                        }
                        break;

                    case 1:  // "黄土"
                        Q0 = 1.0;
                        Q1 = 1.1;
                        Q2 = 1.1;
                        break;

                    case 2://"细粒砂岩"
                    case 8://"细砂岩"
                        Q0 = 0.8;
                        Q1 = 0.9;
                        Q2 = 1.0;
                        break;

                    case 3://, "泥岩"
                    case 6://, "砂质泥岩"
                        Q0 = 0.6;
                        Q1 = 0.8;
                        Q2 = 1.0;
                        break;

                    case 4://, "中粒砂岩"
                    case 9://"中砂岩”
                        Q0 = 0.1;
                        Q1 = 0.3;
                        Q2 = 0.6;
                        break;

                    case 5://, "粉砂岩"
                        Q0 = 0.9;
                        Q1 = 1.0;
                        Q2 = 1.1;
                        break;
                        
                    case 7://, "粗粒砂岩"
                        Q0 = 0.1;
                        Q1 = 0.2;
                        Q2 = 0.5;
                        break;

                }

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
                //刷新累计深度
                int count = MainWindow.layers.Count;
                for (int i = 1; i < count; i++)
                {
                    MainWindow.layers[i].LeiJiShenDu = MainWindow.layers[i - 1].leiJiShenDu + MainWindow.layers[i].cengHou;
                }


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

        double q0;
        public double Q0
        {
            get { return q0; }
            set
            {
                q0 = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Q0"));
                }
            }
        }

        double q1;
        public double Q1
        {
            get { return q1; }
            set
            {
                q1 = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Q1"));
                }
            }
        }

        double q2;
        public double Q2
        {
            get { return q2; }
            set
            {
                q2 = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Q2"));
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