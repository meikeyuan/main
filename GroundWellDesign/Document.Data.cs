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
                case "bool":
                    bool b = (bool)value;
                    if (targetType.Name == "Visibility")
                    {
                        return b ? Visibility.Visible : Visibility.Collapsed;
                    }
                    else if (targetType.Name == "Brush")
                    {
                        return b ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.White);
                    }
                    break;
                case "keyLayer":
                    bool isKeyLayer = (bool)value;
                    if (targetType.Name == "Brush")
                    {
                        return isKeyLayer ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Black);
                    }
                    break;
                case "yanXing":
                    string s = value as string;
                    return Application.Current.FindResource(s);
                case "width":
                    double i = (double)value;
                    return i;
                case "cengHou":
                    double h = (double)value;
                    return 50;

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
