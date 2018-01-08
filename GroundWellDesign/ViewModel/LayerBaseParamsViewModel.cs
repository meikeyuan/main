using MvvmFoundation.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GroundWellDesign.ViewModel
{
    public class LayerBaseParamsViewModel : ObservableObject
    {
        private LayerBaseParams layerBaseParams;
        private Document mainWindow;

        public LayerBaseParamsViewModel(Document window = null, LayerBaseParams param = null)
        {
            mainWindow = window;
            if(param == null)
            {
                layerBaseParams = new LayerBaseParams();
            }
            else
            {
                layerBaseParams = param.Clone() as LayerBaseParams;
            }
        }

        public void reset()
        {
            this.layerBaseParams = new LayerBaseParams();
            RaiseUI();
        }

        private void RaiseUI()
        {
            LeiJiShenDu = this.layerBaseParams.LeiJiShenDu;
            JuLiMeiShenDu = this.layerBaseParams.JuLiMeiShenDu;
            CengHou = this.layerBaseParams.CengHou;
            ZiRanMiDu = this.layerBaseParams.ZiRanMiDu;
            BianXingMoLiang = this.layerBaseParams.BianXingMoLiang;
            KangLaQiangDu = this.layerBaseParams.KangLaQiangDu;
            KangYaQiangDu = this.layerBaseParams.KangYaQiangDu;
            TanXingMoLiang = this.layerBaseParams.TanXingMoLiang;
            BoSonBi = this.layerBaseParams.BoSonBi;
            NeiMoCaJiao = this.layerBaseParams.NeiMoCaJiao;
            NianJuLi = this.layerBaseParams.NianJuLi;
            F = this.layerBaseParams.F;
            Q0 = this.layerBaseParams.Q0;
            Q1 = this.layerBaseParams.Q1;
            Q2 = this.layerBaseParams.Q2;
            MiaoShu = this.layerBaseParams.MiaoShu;

            QXJQWY = this.layerBaseParams.QxJQWY;
            ZXJQWY = this.layerBaseParams.ZxJQWY;
            JQHWY = this.layerBaseParams.JqHWY;
        }

        public LayerBaseParams LayerParams
        {
            get { return this.layerBaseParams; }
            set { this.layerBaseParams = value; }
        }

        public Document Window
        {
            get { return this.mainWindow; }
            set { this.mainWindow = value; }
        }

        public string YanXing
        {
            get { return this.layerBaseParams.YanXing; }
            set
            {
                this.layerBaseParams.YanXing = value;
                var selectLayerWindow = new SelectLayerWindow(this.layerBaseParams.YanXing, this.layerBaseParams);
                if(selectLayerWindow.existedLayers.Count > 0)
                    selectLayerWindow.ShowDialog();
                RaiseUI();
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

                RaisePropertyChanged("YanXing");
            }
        }

        public string WellNamePK
        {
            get { return this.layerBaseParams.WellNamePK; }
            set {
                this.layerBaseParams.WellNamePK = value;
                RaisePropertyChanged("WellNamePK");
            }
        }

        public double LeiJiShenDu
        {
            get { return this.layerBaseParams.LeiJiShenDu; }
            set
            {
                this.layerBaseParams.LeiJiShenDu = value;
                RaisePropertyChanged("LeiJiShenDu");
            }
        }

        public double JuLiMeiShenDu
        {
            get { return this.layerBaseParams.JuLiMeiShenDu; }
            set
            {
                this.layerBaseParams.JuLiMeiShenDu = value;
                RaisePropertyChanged("JuLiMeiShenDu");
            }
        }

        //******************************************
        private void refreshLeiJiShenDu()
        {
            if (mainWindow == null)
                return;
            int count = mainWindow.layers.Count;
            if(count <= 0)
            {
                return;
            }
            mainWindow.layers[0].LeiJiShenDu = 0;
            for (int i = 1; i < count; i++)
            {
                mainWindow.layers[i].LeiJiShenDu = mainWindow.layers[i].CengHou + mainWindow.layers[i - 1].LeiJiShenDu;
            }

        }

        private void refreshJuLiMeiShenDu()
        {
            if (mainWindow == null)
                return;
            int i = mainWindow.layers.Count;
            for (i--; i > 0 && mainWindow.layers[i].YanXing != "煤"; i--) ;
            if (i == 0)
                return;
            for (i -= 2; i >= 0; i--)
            {
                mainWindow.layers[i].JuLiMeiShenDu = mainWindow.layers[i + 1].CengHou + mainWindow.layers[i + 1].JuLiMeiShenDu;
            }
        }
        //******************************************

        public double CengHou
        {
            get { return this.layerBaseParams.CengHou; }
            set
            {
                this.layerBaseParams.CengHou = value;
                //刷新
                refreshLeiJiShenDu();
                refreshJuLiMeiShenDu();
                RaisePropertyChanged("CengHou");
            }
        }

        public double ZiRanMiDu
        {
            get { return this.layerBaseParams.ZiRanMiDu; }
            set
            {
                this.layerBaseParams.ZiRanMiDu = value;
                RaisePropertyChanged("ZiRanMiDu");
            }
        }

        public double BianXingMoLiang
        {
            get { return this.layerBaseParams.BianXingMoLiang; }
            set
            {
                this.layerBaseParams.BianXingMoLiang = value;
                RaisePropertyChanged("BianXingMoLiang");
            }
        }

        public double KangLaQiangDu
        {
            get { return this.layerBaseParams.KangLaQiangDu; }
            set
            {
                this.layerBaseParams.KangLaQiangDu = value;
                RaisePropertyChanged("KangLaQiangDu");
            }
        }

        public double KangYaQiangDu
        {
            get { return this.layerBaseParams.KangYaQiangDu; }
            set
            {
                this.layerBaseParams.KangYaQiangDu = value;
                RaisePropertyChanged("KangYaQiangDu");
            }
        }

        public double TanXingMoLiang
        {
            get { return this.layerBaseParams.TanXingMoLiang; }
            set
            {
                this.layerBaseParams.TanXingMoLiang = value;
                RaisePropertyChanged("TanXingMoLiang");
            }
        }

        public double BoSonBi
        {
            get { return this.layerBaseParams.BoSonBi; }
            set
            {
                this.layerBaseParams.BoSonBi = value;
                RaisePropertyChanged("BoSonBi");
            }
        }

        public double NeiMoCaJiao
        {
            get { return this.layerBaseParams.NeiMoCaJiao; }
            set
            {
                this.layerBaseParams.NeiMoCaJiao = value;
                RaisePropertyChanged("NeiMoCaJiao");
            }
        }

        public double NianJuLi
        {
            get { return this.layerBaseParams.NianJuLi; }
            set
            {
                this.layerBaseParams.NianJuLi = value;
                RaisePropertyChanged("NianJuLi");
            }
        }

        public double F
        {
            get { return this.layerBaseParams.F; }
            set
            {
                this.layerBaseParams.F = value;
                double f = this.layerBaseParams.F;
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
                RaisePropertyChanged("F");
            }
        }

        public double Q0
        {
            get { return this.layerBaseParams.Q0; }
            set
            {
                this.layerBaseParams.Q0 = value;
                RaisePropertyChanged("Q0");
            }
        }

        public double Q1
        {
            get { return this.layerBaseParams.Q1; }
            set
            {
                this.layerBaseParams.Q1 = value;
                RaisePropertyChanged("Q1");
                
            }
        }

        public double Q2
        {
            get { return this.layerBaseParams.Q2; }
            set
            {
                this.layerBaseParams.Q2 = value;
                RaisePropertyChanged("Q2");
            }
        }

        public string MiaoShu
        {
            get { return this.layerBaseParams.MiaoShu; }
            set
            {
                this.layerBaseParams.MiaoShu = value;
                RaisePropertyChanged("MiaoShu");
            }
        }

        public double QXJQWY
        {
            get { return this.layerBaseParams.QxJQWY; }
            set
            {
                this.layerBaseParams.QxJQWY = value;
                RaisePropertyChanged("QXJQWY");
            }
        }

        public double ZXJQWY
        {
            get { return this.layerBaseParams.ZxJQWY; }
            set
            {
                this.layerBaseParams.ZxJQWY = value;
                RaisePropertyChanged("ZXJQWY");
            }
        }

        public double JQHWY
        {
            get { return this.layerBaseParams.JqHWY; }
            set
            {
                this.layerBaseParams.JqHWY = value;
                RaisePropertyChanged("JQHWY");
            }
        }

        public bool IsKeyLayer
        {
            get { return this.layerBaseParams.IsKeyLayer; }
            set
            {
                this.layerBaseParams.IsKeyLayer = value;
                RaisePropertyChanged("IsKeyLayer");
            }
        }

    }
}
