using MvvmFoundation.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroundWellDesign.ViewModel
{
    public class KeyLayerParamsViewModel : ObservableObject
    {
        private Document mainWindow;
        private KeyLayerParams keyParams;


        public KeyLayerParamsViewModel(Document window = null, KeyLayerParams param = null)
        {
            mainWindow = window;
            if(param == null)
            {
                keyParams = new KeyLayerParams();
            }
            else
            {
                keyParams = param.Clone() as KeyLayerParams;
            }
        }

        public Document Window
        {
            get { return this.mainWindow; }
            set { this.mainWindow = value; }
        }

        public KeyLayerParams Params
        {
            get { return this.keyParams; }
            set { this.keyParams = value; }
        }


        public bool IsDangerious
        {
            get { return this.keyParams.IsDangerious; }
            set
            {
                this.keyParams.IsDangerious = value;
                RaisePropertyChanged("IsDangerious");
            }
        }

        public int Ycbh
        {
            get { return this.keyParams.Ycbh; }
            set
            {
                this.keyParams.Ycbh = value;
                RaisePropertyChanged("Ycbh");
            }
        }


        public double Ycsd
        {
            get { return this.keyParams.Ycsd; }
            set
            {
                this.keyParams.Ycsd = value;
                RaisePropertyChanged("Ycsd");
            }
        }


        public double Mcms
        {
            get { return this.keyParams.Mcms; }
            set
            {
                this.keyParams.Mcms = value;
                RaisePropertyChanged("Mcms");
            }
        }


        public double Fypjxs
        {
            get { return this.keyParams.Fypjxs; }
            set
            {
                this.keyParams.Fypjxs = value;
                RaisePropertyChanged("Fypjxs");
            }
        }


        public double Fypjxsxz
        {
            get { return this.keyParams.Fypjxsxz; }
            set
            {
                this.keyParams.Fypjxsxz = value;
                RaisePropertyChanged("Fypjxsxz");
            }
        }


        public double Gzmtjsj
        {
            get { return this.keyParams.Gzmtjsj; }
            set
            {
                this.keyParams.Gzmtjsj = value;
                Gzmtjjl = Gzmtjsj * mainWindow.gzmsd;

                RaisePropertyChanged("Gzmtjsj");

                mainWindow.computeMidData(mainWindow.keyLayers.Count);
            }
        }


        public double Cfcdcjwy
        {
            get { return this.keyParams.Cfcdcjwy; }
            set
            {
                this.keyParams.Cfcdcjwy = value;
                RaisePropertyChanged("Cfcdcjwy");
            }
        }


        public double Gzmtjjl
        {
            get { return this.keyParams.Gzmtjjl; }
            set
            {
                this.keyParams.Gzmtjjl = value;
                RaisePropertyChanged("Gzmtjjl");
            }
        }


        public double Zx
        {
            get { return this.keyParams.Zx; }
            set
            {
                this.keyParams.Zx = value;
                RaisePropertyChanged("Zx");
            }
        }


        public double Qx
        {
            get { return this.keyParams.Qx; }
            set
            {
                this.keyParams.Qx = value;
                RaisePropertyChanged("Qx");
            }
        }

        public double Cdyxbj
        {
            get { return this.keyParams.Cdyxbj; }
            set
            {
                this.keyParams.Cdyxbj = value;
                RaisePropertyChanged("Cdyxbj");
            }
        }


        public double Gdydj
        {
            get { return this.keyParams.Gdydj; }
            set
            {
                this.keyParams.Gdydj = value;
                RaisePropertyChanged("Gdydj");
            }
        }

        public double Qxhckj
        {
            get { return this.keyParams.Qxhckj; }
            set
            {
                this.keyParams.Qxhckj = value;
                RaisePropertyChanged("Qxhckj");
            }
        }


        public double Cfkckjjl
        {
            get { return this.keyParams.Cfkckjjl; }
            set
            {
                this.keyParams.Cfkckjjl = value;
                RaisePropertyChanged("Cfkckjjl");
            }
        }


        public double Sjxcxs
        {
            get { return this.keyParams.Sjxcxs; }
            set
            {
                this.keyParams.Sjxcxs = value;
                RaisePropertyChanged("Sjxcxs");
            }
        }


        public double Yczdxcz
        {
            get { return this.keyParams.Yczdxcz; }
            set
            {
                this.keyParams.Yczdxcz = value;
                RaisePropertyChanged("Yczdxcz");
            }
        }


        public double Jsdjscjwy
        {
            get { return this.keyParams.Jsdjscjwy; }
            set
            {
                this.keyParams.Jsdjscjwy = value;
                RaisePropertyChanged("Jsdjscjwy");
            }
        }


        public double Jsdjslcwy
        {
            get { return this.keyParams.Jsdjslcwy; }
            set
            {
                this.keyParams.Jsdjslcwy = value;
                RaisePropertyChanged("Jsdjslcwy");
            }
        }

        //套管安全系数部分
        private void InvalidDanger()
        {
            int keycount = mainWindow.keyLayers.Count;
            for (int i = 0; i < keycount; i++)
            {
                mainWindow.keyLayers[i].IsDangerious = false;
            }
        }


        public double Tgwj
        {
            get { return this.keyParams.Tgwj; }
            set
            {
                this.keyParams.Tgwj = value;
                InvalidDanger();
                RaisePropertyChanged("Tgwj");
            }
        }

        public double Tgbh
        {
            get { return this.keyParams.Tgbh; }
            set
            {
                this.keyParams.Tgbh = value;
                InvalidDanger();
                RaisePropertyChanged("Tgbh");
            }
        }

        public string Tgtxml
        {
            get { return this.keyParams.Tgtxml; }
            set
            {
                this.keyParams.Tgtxml = value;
                InvalidDanger();
                RaisePropertyChanged("Tgtxml");
            }
        }

        public double Lsqycd
        {
            get { return this.keyParams.Lsqycd; }
            set
            {
                this.keyParams.Lsqycd = value;
                InvalidDanger();
                RaisePropertyChanged("Lsqycd");
            }
        }
        public string Klqd
        {
            get { return this.keyParams.Klqd; }
            set
            {
                this.keyParams.Klqd = value;
                InvalidDanger();
                RaisePropertyChanged("Klqd");
            }
        }
        public double Lsyl
        {
            get { return this.keyParams.Lsyl; }
            set
            {
                this.keyParams.Lsyl = value;
                RaisePropertyChanged("Lsyl");
            }
        }
        public double Lsaqxs
        {
            get { return this.keyParams.Lsaqxs; }
            set
            {
                this.keyParams.Lsaqxs = value;
                RaisePropertyChanged("Lsaqxs");
            }
        }

        public double Jqqycd
        {
            get { return this.keyParams.Jqqycd; }
            set
            {
                this.keyParams.Jqqycd = value;
                InvalidDanger();
                RaisePropertyChanged("Jqqycd");
            }
        }
        public string Kjqd
        {
            get { return this.keyParams.Kjqd; }
            set
            {
                this.keyParams.Kjqd = value;
                InvalidDanger();
                RaisePropertyChanged("Kjqd");
            }
        }
        public double Zdjqyl
        {
            get { return this.keyParams.Zdjqyl; }
            set
            {
                this.keyParams.Zdjqyl = value;
                RaisePropertyChanged("Zdjqyl");
            }
        }
        public double Zdjqyb
        {
            get { return this.keyParams.Zdjqyb; }
            set
            {
                this.keyParams.Zdjqyb = value;
                RaisePropertyChanged("Zdjqyb");
            }
        }
        public double Jqaqxs
        {
            get { return this.keyParams.Jqaqxs; }
            set
            {
                this.keyParams.Jqaqxs = value;
                RaisePropertyChanged("Jqaqxs");
            }
        }
    }
}
