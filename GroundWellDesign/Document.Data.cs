using AxMxDrawXLib;
using mky;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace GroundWellDesign
{
    partial class Document
    {
        //cad ActiveX控件和matlab函数类
        AxMxDrawX cadViewer;
        static MkyLogic logic;

        
        public ObservableCollection<LayerParams> layers = new ObservableCollection<LayerParams>();
        LayerParams editLayer;
        public ObservableCollection<KeyLayerParams> keyLayers = new ObservableCollection<KeyLayerParams>();
        

        public static List<String> YanXingOpt = new List<string> { "地表", "黄土", "泥岩", "砂质泥岩", "细粒砂岩", "中粒砂岩", "粗粒砂岩", "粉砂岩", "细砂岩", "中砂岩", "煤" };
        public static List<String> CaiDongOpt = new List<string> { "初次采动Q0", "重复采动Q1", "重复采动Q2" };
        public static List<String> KjqdOpt = new List<string> { "J55(0.2378GPa)", "N80(0.319GPa)" };
        public static List<String> TgtxmlOpt = new List<string> { "J55(2.050e11)", "N80(2.070e11)" };
        public static List<String> KlqdOpt = new List<string> { "J55(0.41GPa)", "N80(0.55GPa)" };



        public string FilePath
        {
            set;
            get;
        }

        private double mcqj;
        public double Mcqj
        {
            set
            {
                mcqj = value;
                computeMidData(keyLayers.Count);
            }
            get
            {
                return mcqj;
            }
        }

        public double FuYanXCL
        {
            set;
            get;
        }

        public double CaiGao
        {
            get;
            set;
        }

        public double SuiZhangXS
        {
            get;
            set;
        }


        private double mchd;
        public double Mchd
        {
            set
            {
                mchd = value;
                computeMidData(keyLayers.Count);
            }
            get
            {
                return mchd;
            }
        }

        private double pjxsxz;
        public double Pjxsxz
        {
            set
            {
                pjxsxz = value;
                foreach (KeyLayerParams layer in keyLayers)
                {
                    layer.Fypjxsxz = layer.fypjxs * value;
                }
                computeMidData(keyLayers.Count);

            }
            get
            {
                return pjxsxz;
            }
        }

        private double hcqZxcd;
        public double HcqZXcd
        {
            set
            {
                hcqZxcd = value;
                computeMidData(keyLayers.Count);
            }
            get
            {
                return hcqZxcd;
            }
        }

        private double hcqQxcd;
        public double HcqQXcd
        {
            set
            {
                hcqQxcd = value;
                computeMidData(keyLayers.Count);
            }
            get
            {
                return hcqQxcd;
            }
        }

        public double gzmsd;
        public double Gzmsd
        {
            set
            {
                gzmsd = value;
                foreach (KeyLayerParams layer in keyLayers)
                {
                    layer.Gzmtjjl = gzmsd * layer.gzmtjsj;
                }
            }
            get
            {
                return gzmsd;
            }
        }

        public double jswzjl;
        public double Jswzjl
        {
            set
            {
                if (jswzjl != value)
                {
                    MessageBoxResult result = MessageBox.Show("地面井是否位于工作面内部?", "提示", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        jswzjl = 0 - value;
                    }
                    else
                    {
                        jswzjl = value;
                    }
                }
            }
            get
            {
                return jswzjl;
            }
        }
    }
}
