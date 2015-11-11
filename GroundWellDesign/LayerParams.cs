using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace GroundWellDesign
{

    [Serializable]
    public class BaseParams
    {
        public BaseParams()
        {
        }

        public BaseParams(BaseParams layer)
        {
            
            yanXing = layer.yanXing;
            leiJiShenDu = layer.leiJiShenDu;
            juLiMeiShenDu = layer.juLiMeiShenDu;
            cengHou = layer.cengHou;
            ziRanMiDu = layer.ziRanMiDu;
            bianXingMoLiang = layer.bianXingMoLiang;
            kangLaQiangDu = layer.kangLaQiangDu;
            kangYaQiangDu = layer.kangYaQiangDu;
            tanXingMoLiang = layer.tanXingMoLiang;
            boSonBi = layer.boSonBi;
            neiMoCaJiao = layer.neiMoCaJiao;
            nianJuLi = layer.nianJuLi;
            q0 = layer.q0;
            q1 = layer.q1;
            q2 = layer.q2;
            miaoShu = layer.miaoShu;
            dataBaseNum = layer.dataBaseNum;

        }

        public string yanXing;
        public double leiJiShenDu;
        public double juLiMeiShenDu;
        public double cengHou;
        public double ziRanMiDu;
        public double bianXingMoLiang;
        public double kangLaQiangDu;
        public double kangYaQiangDu;
        public double tanXingMoLiang;
        public double boSonBi;
        public double neiMoCaJiao;
        public double nianJuLi;
        public double q0;
        public double q1;
        public double q2;
        public string miaoShu;
        public int dataBaseNum;

    }


    public class LayerParams : BaseParams, INotifyPropertyChanged
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
            yanXing = YanXingOpt[1];
            miaoShu = "";
        }

        public LayerParams(BaseParams layer)
        {
            copyNoEvent(layer);
        }

        public bool Equals(LayerParams layer)
        {
            return YanXing.Equals(layer.YanXing) &&
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
            Q1 == layer.Q1 && Q2 == layer.Q2 && Q0 == layer.Q0 && dataBaseNum == layer.dataBaseNum;

        }

        public void reset()
        {
            copyAndEvent(new LayerParams());
        }

        public void copyAndEvent(BaseParams layer)
        {
            YanXing = layer.yanXing;
            LeiJiShenDu = layer.leiJiShenDu;
            JuLiMeiShenDu = layer.juLiMeiShenDu;
            CengHou = layer.cengHou;
            ZiRanMiDu = layer.ziRanMiDu;
            BianXingMoLiang = layer.bianXingMoLiang;
            KangLaQiangDu = layer.kangLaQiangDu;
            KangYaQiangDu = layer.kangYaQiangDu;
            TanXingMoLiang = layer.tanXingMoLiang;
            BoSonBi = layer.boSonBi;
            NeiMoCaJiao = layer.neiMoCaJiao;
            NianJuLi = layer.nianJuLi;
            Q0 = layer.q0;
            Q1 = layer.q1;
            Q2 = layer.q2;
            MiaoShu = layer.miaoShu;
            dataBaseNum = layer.dataBaseNum;
        }

        public void copyAndEventEcpYanXing(BaseParams layer)
        {
            LeiJiShenDu = layer.leiJiShenDu;
            JuLiMeiShenDu = layer.juLiMeiShenDu;
            CengHou = layer.cengHou;
            ZiRanMiDu = layer.ziRanMiDu;
            BianXingMoLiang = layer.bianXingMoLiang;
            KangLaQiangDu = layer.kangLaQiangDu;
            KangYaQiangDu = layer.kangYaQiangDu;
            TanXingMoLiang = layer.tanXingMoLiang;
            BoSonBi = layer.boSonBi;
            NeiMoCaJiao = layer.neiMoCaJiao;
            NianJuLi = layer.nianJuLi;
            Q0 = layer.q0;
            Q1 = layer.q1;
            Q2 = layer.q2;
            MiaoShu = layer.miaoShu;
            dataBaseNum = layer.dataBaseNum;
        }

        public void copyNoEvent(BaseParams layer)
        {
            yanXing = layer.yanXing;
            leiJiShenDu = layer.leiJiShenDu;
            juLiMeiShenDu = layer.juLiMeiShenDu;
            cengHou = layer.cengHou;
            ziRanMiDu = layer.ziRanMiDu;
            bianXingMoLiang = layer.bianXingMoLiang;
            kangLaQiangDu = layer.kangLaQiangDu;
            kangYaQiangDu = layer.kangYaQiangDu;
            tanXingMoLiang = layer.tanXingMoLiang;
            boSonBi = layer.boSonBi;
            neiMoCaJiao = layer.neiMoCaJiao;
            nianJuLi = layer.nianJuLi;
            q0 = layer.q0;
            q1 = layer.q1;
            q2 = layer.q2;
            miaoShu = layer.miaoShu;
            dataBaseNum = layer.dataBaseNum;
        }


        private void refreshJuLiMeiShenDu()
        {
            int i = MainWindow.layers.Count;
            for (i--; i > 0 && MainWindow.layers[i].yanXing != "煤"; i--) ;
            if (i == 0)
                return;
            for (i -= 2; i >= 0; i--)
            {
                MainWindow.layers[i].JuLiMeiShenDu = MainWindow.layers[i + 1].cengHou + MainWindow.layers[i + 1].juLiMeiShenDu;
            }

        }

        public string YanXing
        {
            get { return yanXing; }
            set
            {
                yanXing = value;

                new SelectLayerWindow(yanXing, this).ShowDialog();
                int index = YanXingOpt.IndexOf(value);
                switch (index)
                {
                    case 0:  //地表
                        break;

                    case 10: //煤  刷新距离煤深度
                        refreshJuLiMeiShenDu();
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

        private void refreshCengHou()
        {
            int count = MainWindow.layers.Count;
            for (int i = 1; i < count; i++)
            {
                MainWindow.layers[i].CengHou = MainWindow.layers[i].leiJiShenDu - MainWindow.layers[i - 1].leiJiShenDu;
            }

        }

        public double LeiJiShenDu
        {
            get { return leiJiShenDu; }
            set
            {
                leiJiShenDu = value;
                //刷新层厚
                refreshCengHou();
                refreshJuLiMeiShenDu();



                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("LeiJiShenDu"));
                }
            }
        }


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
