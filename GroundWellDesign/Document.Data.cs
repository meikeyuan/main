﻿using AxMxDrawXLib;
using GroundWellDesign.Util;
using GroundWellDesign.ViewModel;
using Mky;
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
        // 当前文档对应的磁盘文件路径
        public string FilePath { set; get; }

        //cad ActiveX控件和matlab函数类
        AxMxDrawX cadViewer, cadViewer2;

        public ObservableCollection<LayerBaseParamsViewModel> layers = new ObservableCollection<LayerBaseParamsViewModel>();
        LayerBaseParams copyedLayer;
        LayerBaseParamsViewModel editLayer;

        public ObservableCollection<KeyLayerParamsViewModel> keyLayers = new ObservableCollection<KeyLayerParamsViewModel>();

        public ObservableCollection<ZengYiParamsViewModel> zengYis = new ObservableCollection<ZengYiParamsViewModel>();
        public ZengYiParamsViewModel editZengYi;

        public ManuDesignParamsViewModel manuDesignParams;

        public static ObservableCollection<int> LayerNbrs = new ObservableCollection<int>();
        public static List<String> YanXingOpt = new List<string> { "地表", "黄土", "泥岩", "砂质泥岩", "细粒砂岩", "中粒砂岩", "粗粒砂岩", "粉砂岩", "细砂岩", "中砂岩", "煤" };
        public static List<String> CaiDongOpt = new List<string> { "初次采动Q0", "重复采动Q1", "重复采动Q2" };
        
        // 安全系数计算部分
        public static List<String> TgxhOpt = new List<string> { "J55", "N80", "自定义" };
        public static List<double> KjqdOpt = new List<double> { 0.2378, 0.319 };
        public static List<double> TxmlOpt = new List<double> { 2.050e11, 2.070e11 };
        public static List<double> KlqdOpt = new List<double> { 0.41, 0.55 };

        // 增益计算部分
        public static List<string> TgtxmlOpt = new List<string> { "J55(2.050e11)", "N80(2.070e11)" };

        public static List<String> TggjOpt = new List<string> { "J55", "N80" };
        public static List<String> GjfsOpt = new List<string> { "全固", "局固" };
        public static List<String> WjfsOpt = new List<string> { "悬挂", "自由", "裸孔" };
        public static List<String> JieGouOpt = new List<string> { "二开", "三开" };

        public static List<String> JQDestOpt = new List<string> { "倾向剪切位移u(x)", "走向剪切位移u(z)", "剪切合位移u(p)"};
        public static List<String> LCDestOpt = new List<string> { "岩层最大下沉值W0", "计算点即时沉降位移Wt", "计算点即时离层位移△Wt" };


        //以下为横三带竖三带变量
        public double Mcqj
        {
            set;
            get;
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
        public double Mchd
        {
            get;
            set;
        }

        public double pjxsxz;
        public double Pjxsxz
        {
            set
            {
                pjxsxz = value;
                foreach (KeyLayerParamsViewModel layer in keyLayers)
                {
                    layer.Fypjxsxz = layer.Fypjxs * value;
                }
                //int upcount = 0;
                //ComputeHelper.computeKeyLayerOffset(this, ref upcount);
            }
            get
            {
                return pjxsxz;
            }
        }

        public double hcqZxcd;
        public double HcqZXcd
        {
            set
            {
                hcqZxcd = value;
                //int upcount = 0;
                //ComputeHelper.computeKeyLayerOffset(this, ref upcount);
            }
            get
            {
                return hcqZxcd;
            }
        }

        public double hcqQxcd;
        public double HcqQXcd
        {
            set
            {
                hcqQxcd = value;
                //int upcount = 0;
                //ComputeHelper.computeKeyLayerOffset(this, ref upcount);
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
                foreach (KeyLayerParamsViewModel layer in keyLayers)
                {
                    layer.Gzmtjjl = gzmsd * layer.Gzmtjsj;
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
