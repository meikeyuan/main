using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroundWellDesign
{
    [Serializable]
    public class BaseKeyParams
    {
        public BaseKeyParams()
        {

        }

        public BaseKeyParams(BaseKeyParams layer)
        {

            ycbh = layer.ycbh;
        ycsd = layer.ycsd;
         mcms = layer.mcms;
         fypjxs = layer.fypjxs;
         fypjxsxz = layer.fypjxsxz;
         gzmtjsj = layer.gzmtjsj;

         cfcdcjwy = layer.cfcdcjwy;
         gzmtjjl = layer.gzmtjjl;
         qx = layer.qx;
         cdyxbj = layer.cdyxbj;


         gdydj = layer.gdydj;
         cfkckjjl = layer.cfkckjjl;
         sjxcxs = layer.sjxcxs;
         yczdxcz = layer.yczdxcz;

         jsdjscjwy = layer.jsdjscjwy;
         jsdjslcwy = layer.jsdjscjwy;


        }

        public int ycbh;
        public double ycsd;
        public double mcms;
        public double fypjxs;
        public double fypjxsxz;
        public double gzmtjsj;

        public double cfcdcjwy;
        public double gzmtjjl;
        public double qx;
        public double cdyxbj;


        public double gdydj;
        public double cfkckjjl;
        public double sjxcxs;
        public double yczdxcz;

        public double jsdjscjwy;
        public double jsdjslcwy;
    }


    public class KeyLayerParams : BaseKeyParams, INotifyPropertyChanged
    {


        public KeyLayerParams()
        {
        }

        public KeyLayerParams(BaseKeyParams layer)
        {
            copyAndEvent(layer);
        }


        public void reset()
        {
            copyAndEvent(new KeyLayerParams());
        }


        public void copyAndEvent(BaseKeyParams layer)
        {
            Ycbh = layer.ycbh;
            Ycsd = layer.ycsd;
            Mcms = layer.mcms;
            Fypjxs = layer.fypjxs;
            Fypjxsxz = layer.fypjxsxz;
            Gzmtjsj = layer.gzmtjsj;

            Cfcdcjwy = layer.cfcdcjwy;
            Gzmtjjl = layer.gzmtjjl;
            Qx = layer.qx;
            Cdyxbj = layer.cdyxbj;


            Gdydj = layer.gdydj;
            Cfkckjjl = layer.cfkckjjl;
            Sjxcxs = layer.sjxcxs;
            Yczdxcz = layer.yczdxcz;

            Jsdjscjwy = layer.jsdjscjwy;
            Jsdjslcwy = layer.jsdjscjwy;
        }


        public int Ycbh
        {
            get { return ycbh; }
            set
            {
                ycbh = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Ycbh"));
                }
            }
        }


        public double Ycsd
        {
            get { return ycsd; }
            set
            {
                ycsd = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Ycsd"));
                }
            }
        }


        public double Mcms
        {
            get { return mcms; }
            set
            {
                mcms = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Mcms"));
                }
            }
        }


        public double Fypjxs
        {
            get { return fypjxs; }
            set
            {
                fypjxs = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Fypjxs"));
                }
            }
        }


        public double Fypjxsxz
        {
            get { return fypjxsxz; }
            set
            {
                fypjxsxz = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Fypjxsxz"));
                }
            }
        }


        public double Gzmtjsj
        {
            get { return gzmtjsj; }
            set
            {
                gzmtjsj = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Gzmtjsj"));
                }
            }
        }


        public double Cfcdcjwy
        {
            get { return cfcdcjwy; }
            set
            {
                cfcdcjwy = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Cfcdcjwy"));
                }
            }
        }


        public double Gzmtjjl
        {
            get { return gzmtjjl; }
            set
            {
                gzmtjjl = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Gzmtjjl"));
                }
            }
        }


        public double Qx
        {
            get { return qx; }
            set
            {
                qx = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Qx"));
                }
            }
        }


        public double Cdyxbj
        {
            get { return cdyxbj; }
            set
            {
                cdyxbj = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Cdyxbj"));
                }
            }
        }


        public double Gdydj
        {
            get { return gdydj; }
            set
            {
                gdydj = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Gdydj"));
                }
            }
        }


        public double Cfkckjjl
        {
            get { return cfkckjjl; }
            set
            {
                cfkckjjl = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Cfkckjjl"));
                }
            }
        }


        public double Sjxcxs
        {
            get { return sjxcxs; }
            set
            {
                sjxcxs = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Sjxcxs"));
                }
            }
        }


        public double Yczdxcz
        {
            get { return yczdxcz; }
            set
            {
                yczdxcz = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Yczdxcz"));
                }
            }
        }


        public double Jsdjscjwy
        {
            get { return jsdjscjwy; }
            set
            {
                jsdjscjwy = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Jsdjscjwy"));
                }
            }
        }


        public double Jsdjslcwy
        {
            get { return jsdjslcwy; }
            set
            {
                jsdjslcwy = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Jsdjslcwy"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
