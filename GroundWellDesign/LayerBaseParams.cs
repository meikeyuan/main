using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace GroundWellDesign
{

    [Serializable]
    public class BaseLayerBaseParams
    {
        public BaseLayerBaseParams()
        {
            yanXing = Document.YanXingOpt[1];
            miaoShu = "岩层描述";
            dataBaseKey = "";
            wellNamePK = "";
        }

        //用于拆箱到本可序列化对象 保存到文件
        public BaseLayerBaseParams(BaseLayerBaseParams layer)
        {
            copy(layer);
        }

        public void copy(BaseLayerBaseParams layer)
        {
            for (int i = 0; i < 22; i++)
            {
                this[i] = layer[i];
            }
        }

        public object this[int index]
        {
            get
            {
                switch(index)
                {
                    case 0:
                        return yanXing;
                    case 1:
                        return leiJiShenDu;
                    case 2:
                        return juLiMeiShenDu;
                    case 3:
                        return cengHou;
                    case 4:
                        return ziRanMiDu;
                    case 5:
                        return bianXingMoLiang;
                    case 6:
                        return kangLaQiangDu;
                    case 7:
                        return kangYaQiangDu;
                    case 8:
                        return tanXingMoLiang;
                    case 9:
                        return boSonBi;
                    case 10:
                        return neiMoCaJiao;
                    case 11:
                        return nianJuLi;
                    case 12:
                        return f;
                    case 13:
                        return q0;
                    case 14:
                        return q1;
                    case 15:
                        return q2;
                    case 16:
                        return miaoShu;
                    case 17:
                        return dataBaseKey;
                    case 18:
                        return wellNamePK;
                    case 19:
                        return qxJQWY;
                    case 20:
                        return zxJQWY;
                    case 21:
                        return jqHWY;
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
                        yanXing = string_value;
                        break;
                    case 1:
                        leiJiShenDu = double_value;
                        break;
                    case 2:
                        juLiMeiShenDu = double_value;
                        break;
                    case 3:
                        cengHou = double_value;
                        break;
                    case 4:
                        ziRanMiDu = double_value;
                        break;
                    case 5:
                        bianXingMoLiang = double_value;
                        break;
                    case 6:
                        kangLaQiangDu = double_value;
                        break;
                    case 7:
                        kangYaQiangDu = double_value;
                        break;
                    case 8:
                        tanXingMoLiang = double_value;
                        break;
                    case 9:
                        boSonBi = double_value;
                        break;
                    case 10:
                        neiMoCaJiao = double_value;
                        break;
                    case 11:
                        nianJuLi = double_value;
                        break;
                    case 12:
                        f = double_value;
                        break;
                    case 13:
                        q0 = double_value;
                        break;
                    case 14:
                        q1 = double_value;
                        break;
                    case 15:
                        q2 = double_value;
                        break;
                    case 16:
                        miaoShu = string_value;
                        break;
                    case 17:
                        dataBaseKey = string_value;
                        break;
                    case 18:
                        wellNamePK = string_value;
                        break;
                    case 19:
                        qxJQWY = double_value;
                        break;
                    case 20:
                        zxJQWY = double_value;
                        break;
                    case 21:
                        jqHWY = double_value;
                        break;
                    default:
                        break;
                }
            }
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
        public double f;
        public double q0;
        public double q1;
        public double q2;
        public string miaoShu;

        public string dataBaseKey;
        public string wellNamePK;


        public double qxJQWY;
        public double zxJQWY;
        public double jqHWY;

    }

    public class LayerBaseParams : BaseLayerBaseParams, INotifyPropertyChanged
    {
        // 增加对窗口的引用
        public Document mainWindow;

        public LayerBaseParams(Document mainWindow, BaseLayerBaseParams layer = null)
        {
            this.mainWindow = mainWindow;
            if(layer != null)
            {
                copy(layer);
            }
        }

        public void reset()
        {
            copyAndEventEcpYanXing(new BaseLayerBaseParams());
        }

        public void copyAndEventEcpYanXing(BaseLayerBaseParams layer)
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
            F = layer.f;
            Q0 = layer.q0;
            Q1 = layer.q1;
            Q2 = layer.q2;
            MiaoShu = layer.miaoShu;

            dataBaseKey = layer.dataBaseKey;
            WellNamePK = layer.wellNamePK;

            QXJQWY = layer.qxJQWY;
            ZXJQWY = layer.zxJQWY;
            JQHWY = layer.jqHWY;
        }


        //增加是否为关键层标志
        private bool isKeyLayer;
        public bool IsKeyLayer
        {
            get { return isKeyLayer; }
            set
            {
                isKeyLayer = value;
                SetUI("IsKeyLayer");

            }
        }

        public string YanXing
        {
            get { return yanXing; }
            set
            {
                yanXing = value;
                var selectLayerWindow = new SelectLayerWindow(yanXing, this);
                if(selectLayerWindow.existedLayers.Count > 0)
                    selectLayerWindow.ShowDialog();
                int index = Document.YanXingOpt.IndexOf(value);
                switch (index)
                {
                    case 0:  //地表
                        break;

                    case 10: //煤  刷新距离煤深度
                        refreshJuLiMeiShenDu();
                        break;

                    case 1:  // "黄土"
                        F = 0.5;
                        break;

                    case 2://"细粒砂岩"
                    case 8://"细砂岩"
                        F = 2;
                        break;

                    case 3://, "泥岩"
                    case 6://, "砂质泥岩"
                        F = 3;
                        break;

                    case 4://, "中粒砂岩"
                    case 9://"中砂岩”
                        F = 6;
                        break;

                    case 5://, "粉砂岩"
                        F = 1;
                        break;

                    case 7://, "粗粒砂岩"
                        F = 7;
                        break;

                }

                SetUI("YanXing");
            }
        }

        public string WellNamePK
        {
            get { return wellNamePK;}
            set {
                wellNamePK = value;
                SetUI("WellNamePK");
            }
        }




        public double LeiJiShenDu
        {
            get { return leiJiShenDu; }
            set
            {
                leiJiShenDu = value;
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


        //******************************************
        private void refreshLeiJiShenDu()
        {
            int count = mainWindow.layers.Count;
            mainWindow.layers[0].LeiJiShenDu = 0;
            for (int i = 1; i < count; i++)
            {
                mainWindow.layers[i].LeiJiShenDu = mainWindow.layers[i].cengHou + mainWindow.layers[i - 1].leiJiShenDu;
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
        //******************************************

        public double CengHou
        {
            get { return cengHou; }
            set
            {
                cengHou = value;
                //刷新
                refreshLeiJiShenDu();
                refreshJuLiMeiShenDu();
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

        public double F
        {
            get { return f; }
            set
            {
                f = value;
                if(f >= 9)
                {
                    Q0 = 0.0; Q1 = 0.0; Q2 = 0.1;
                }
                else if(f >= 8)
                {
                    Q0 = 0.0; Q1 = 0.1; Q2 = 0.4;
                }
                else if (f >= 7)
                {
                    Q0 = 0.1; Q1 = 0.2; Q2 = 0.5;
                }
                else if (f >= 6)
                {
                    Q0 = 0.1; Q1 = 0.3; Q2 = 0.6;
                }
                else if (f >= 5)
                {
                    Q0 = 0.2; Q1 = 0.5; Q2 = 0.7;
                }
                else if (f >= 4)
                {
                    Q0 = 0.4; Q1 = 0.7; Q2 = 1.0;
                }
                else if (f >= 3)
                {
                    Q0 = 0.6; Q1 = 0.8; Q2 = 1.0;
                }
                else if (f >= 2)
                {
                    Q0 = 0.8; Q1 = 0.9; Q2 = 1.0;
                }
                else if (f >= 1)
                {
                    Q0 = 0.9; Q1 = 1.0; Q2 = 1.1;
                }
                else
                {
                    Q0 = 1.0; Q1 = 1.1; Q2 = 1.1;
                }
                SetUI("F");
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
                zxJQWY = value;
                SetUI("ZXJQWY");
            }
        }

        public double JQHWY
        {
            get { return jqHWY; }
            set
            {
                jqHWY = value;
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
