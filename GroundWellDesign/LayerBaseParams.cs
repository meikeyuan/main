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
            yanXing = Document.YanXingOpt[1];
            miaoShu = "岩层";
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

            qxJQWY = layer.qxJQWY;
            zxJQWY = layer.zxJQWY;
            jqHWY = layer.jqHWY;

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


        public double qxJQWY;
        public double zxJQWY;
        public double jqHWY;

    }

    public class LayerParams : BaseParams, INotifyPropertyChanged
    {

        public Document mainWindow;

        public LayerParams(Document mainWindow)
        {
            this.mainWindow = mainWindow;
            yanXing = Document.YanXingOpt[1];
            miaoShu = "岩层";
        }

        public LayerParams(BaseParams layer)
        {
            copyNoEvent(layer);
        }

        public bool Equals(BaseParams layer)
        {
            return yanXing.Equals(layer.yanXing) &&
            LeiJiShenDu == layer.leiJiShenDu &&
            JuLiMeiShenDu == layer.juLiMeiShenDu &&
            CengHou == layer.cengHou &&
            ZiRanMiDu == layer.ziRanMiDu &&
            BianXingMoLiang == layer.bianXingMoLiang &&
            KangLaQiangDu == layer.kangLaQiangDu &&
            KangYaQiangDu == layer.kangYaQiangDu &&
            TanXingMoLiang == layer.tanXingMoLiang &&
            BoSonBi == layer.boSonBi &&
            NeiMoCaJiao == layer.neiMoCaJiao &&
            NianJuLi == layer.nianJuLi && MiaoShu.Equals(layer.miaoShu) &&
            Q1 == layer.q1 && Q2 == layer.q2 && Q0 == layer.q0 && dataBaseNum == layer.dataBaseNum;

        }

        public void reset()
        {
            copyNoEvent(new BaseParams());
        }


        public void copyAndEventEcpYanXing(BaseParams layer)
        {
            yanXing = layer.yanXing;
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

            QXJQWY = layer.qxJQWY;
            ZXJQWY = layer.zxJQWY;
            JQHWY = layer.jqHWY;

            
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

            qxJQWY = layer.qxJQWY;
            zxJQWY = layer.zxJQWY;
            jqHWY = layer.jqHWY;
        }


        //增加是否为关键层标志
        private bool isKeyLayer;
        public bool IsKeyLayer
        {
            get { return isKeyLayer; }
            set
            {
                isKeyLayer = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("IsKeyLayer"));
                }

            }
        }


        private void refreshJuLiMeiShenDu()
        {
            int i = mainWindow.layers.Count;
            for (i--; i > 0 && mainWindow.layers[i].yanXing != "煤"; i--) ;
            if (i == 0)
                return;
            for (i -= 2; i >= 0; i--)
            {
                mainWindow.layers[i].JuLiMeiShenDu = mainWindow.layers[i + 1].cengHou + mainWindow.layers[i + 1].juLiMeiShenDu;
            }

        }

        public string YanXing
        {
            get { return yanXing; }
            set
            {
                yanXing = value;

                new SelectLayerWindow(yanXing, this).ShowDialog();
                int index = Document.YanXingOpt.IndexOf(value);
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

                SetUI("YanXing");
            }
        }

        private void refreshCengHou()
        {
            int count = mainWindow.layers.Count;
            for (int i = 1; i < count; i++)
            {
                mainWindow.layers[i].CengHou = mainWindow.layers[i].leiJiShenDu - mainWindow.layers[i - 1].leiJiShenDu;
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

                SetUI("LeiJiShenDu");
            }
        }


        public double JuLiMeiShenDu
        {
            get { return juLiMeiShenDu; }
            set
            {
                juLiMeiShenDu = value;
                SetUI("JuLiMeiShenDu");
            }
        }


        public double CengHou
        {
            get { return cengHou; }
            set
            {
                cengHou = value;
                SetUI("CengHou");
            }
        }


        public double ZiRanMiDu
        {
            get { return ziRanMiDu; }
            set
            {
                ziRanMiDu = value;
                SetUI("ZiRanMiDu");
            }
        }


        public double BianXingMoLiang
        {
            get { return bianXingMoLiang; }
            set
            {
                bianXingMoLiang = value;
                SetUI("BianXingMoLiang");
            }
        }


        public double KangLaQiangDu
        {
            get { return kangLaQiangDu; }
            set
            {
                kangLaQiangDu = value;
                SetUI("KangLaQiangDu");
            }
        }


        public double KangYaQiangDu
        {
            get { return kangYaQiangDu; }
            set
            {
                kangYaQiangDu = value;
                SetUI("KangYaQiangDu");
            }
        }


        public double TanXingMoLiang
        {
            get { return tanXingMoLiang; }
            set
            {
                tanXingMoLiang = value;
                SetUI("TanXingMoLiang");
            }
        }


        public double BoSonBi
        {
            get { return boSonBi; }
            set
            {
                boSonBi = value;
                SetUI("BoSonBi");
            }
        }


        public double NeiMoCaJiao
        {
            get { return neiMoCaJiao; }
            set
            {
                neiMoCaJiao = value;
                SetUI("NeiMoCaJiao");
            }
        }


        public double NianJuLi
        {
            get { return nianJuLi; }
            set
            {
                nianJuLi = value;
                SetUI("NianJuLi");
            }
        }


        public double Q0
        {
            get { return q0; }
            set
            {
                q0 = value;
                SetUI("Q0");
            }
        }


        public double Q1
        {
            get { return q1; }
            set
            {
                q1 = value;
                SetUI("Q1");
                
            }
        }


        public double Q2
        {
            get { return q2; }
            set
            {
                q2 = value;
                SetUI("Q2");
            }
        }


        public string MiaoShu
        {
            get { return miaoShu; }
            set
            {
                miaoShu = value;
                SetUI("MiaoShu");
            }
        }

        public double QXJQWY
        {
            get { return qxJQWY; }
            set
            {
                qxJQWY = value;
                SetUI("QXJQWY");
            }
        }

        public double ZXJQWY
        {
            get { return zxJQWY; }
            set
            {
                qxJQWY = value;
                SetUI("ZXJQWY");
            }
        }

        public double JQHWY
        {
            get { return jqHWY; }
            set
            {
                qxJQWY = value;
                SetUI("JQHWY");
            }
        }


        private void SetUI(string name)
        {
            if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
                }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
