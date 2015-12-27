using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace GroundWellDesign
{
    public class MyConvert : IValueConverter
    {
        public string sourceType
        {
            get;
            set;
        }
        public object Convert(object value, Type targetType, object param, CultureInfo c)
        {
            switch (sourceType)
            {
                case "login":
                    bool b1 = (bool)value;
                    return b1 ? Visibility.Visible : Visibility.Collapsed;

                case "key1":
                    bool b2 = (bool)value;
                    return b2 ? new SolidColorBrush(Colors.Coral) : new SolidColorBrush(Colors.White);
                case "key2":
                    bool b3 = (bool)value;
                    return b3 ? new SolidColorBrush(Colors.Coral) : new SolidColorBrush(Colors.Black);
                case "yanXing":
                    string s = value as string;
                    return Application.Current.FindResource(s);
                case "windowH":
                    double h = (double)value;
                    return h*3;

            }
            return null;

        }

        public object ConvertBack(object value, Type targetType, object param, CultureInfo c)
        {
            throw new NotImplementedException();
        }
    }
    partial class Document
    {
        private double mcqj;
        public double Mcqj
        {
            set
            {
                mcqj = value;
                compute(keyLayers.Count);
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

        public double MaoLuoDai
        {
            set;
            get;
        }

        public double LieXiDai
        {
            set;
            get;
        }


        private double mchd;
        public double Mchd
        {
            set
            {
                mchd = value;
                compute(keyLayers.Count);
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
                compute(keyLayers.Count);

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
                compute(keyLayers.Count);
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
                compute(keyLayers.Count);
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
