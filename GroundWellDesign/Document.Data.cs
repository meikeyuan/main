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
        AxMxDrawX cadViewer, cadViewer2;
        static MkyLogic logic;


        public ObservableCollection<LayerBaseParams> layers = new ObservableCollection<LayerBaseParams>();
        LayerBaseParams editLayer;

        public ObservableCollection<KeyLayerParams> keyLayers = new ObservableCollection<KeyLayerParams>();

        public ObservableCollection<ZengYiParams> zengYis = new ObservableCollection<ZengYiParams>();
        ZengYiParams editZengYi;

        public ManuDesignParams manuDesignParams;
        

        public static List<String> YanXingOpt = new List<string> { "地表", "黄土", "泥岩", "砂质泥岩", "细粒砂岩", "中粒砂岩", "粗粒砂岩", "粉砂岩", "细砂岩", "中砂岩", "煤" };
        public static List<String> CaiDongOpt = new List<string> { "初次采动Q0", "重复采动Q1", "重复采动Q2" };
        public static List<String> KjqdOpt = new List<string> { "J55(0.2378GPa)", "N80(0.319GPa)" };
        public static List<String> TgtxmlOpt = new List<string> { "J55(2.050e11)", "N80(2.070e11)" };
        public static List<String> KlqdOpt = new List<string> { "J55(0.41GPa)", "N80(0.55GPa)" };
        public static List<String> TggjOpt = new List<string> { "J55", "N80" };
        public static List<String> GjfsOpt = new List<string> { "全固", "局固" };
        public static List<String> WjfsOpt = new List<string> { "悬挂", "自由", "裸孔" };
        public static List<String> JieGouOpt = new List<string> { "二开", "三开" };

        public static List<String> JQDestOpt = new List<string> { "倾向剪切位移u(x)", "走向剪切位移u(z)", "剪切合位移u(p)"};
        public static List<String> LCDestOpt = new List<string> { "岩层最大下沉值W0", "计算点即时沉降位移Wt", "计算点即时离层位移△Wt" };

        public string FilePath
        {
            set;
            get;
        }


        //以下为横三带竖三代变量
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


        //以下为计算关键层中间数据需要的变量
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


        //以下为自动设计需要相关变量
        public string AutoTgxh1
        {
            set;
            get;
        }
        public string AutoMiaoshu1
        {
            set;
            get;
        }

        public string AutoTgxh2
        {
            set;
            get;
        }
        public string AutoMiaoshu2
        {
            set;
            get;
        }

        public string AutoTgxh3
        {
            set;
            get;
        }
        public string AutoWjfs3
        {
            set;
            get;
        }
        public string AutoMiaoshu3
        {
            set;
            get;
        }
    }
}
