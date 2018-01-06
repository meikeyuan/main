using MvvmFoundation.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroundWellDesign.ViewModel
{
    public class ZengYiParamsViewModel : ObservableObject
    {
        private ZengYiParams zengYiParams;
        public Document mainWindow;


        public ZengYiParamsViewModel(Document mainWindow = null, ZengYiParams param = null)
        {
            this.mainWindow = mainWindow;
            if(param == null)
            {
                zengYiParams = new ZengYiParams();
            }
            else
            {
                zengYiParams = param.Clone() as ZengYiParams;
            }
            RaiseUI();
        }

        public void reset()
        {
            this.zengYiParams = new ZengYiParams();
            RaiseUI();
        }

        private void RaiseUI()
        {
            Time = this.zengYiParams.Time;

            Ec = this.zengYiParams.Ec;
            Vc = this.zengYiParams.Vc;
            Es = this.zengYiParams.Es;
            Vs = this.zengYiParams.Vs;
            E = this.zengYiParams.E;
            V = this.zengYiParams.V;

            A0 = this.zengYiParams.A0;
            Aw = this.zengYiParams.Aw;
            A1 = this.zengYiParams.A1;
            B = this.zengYiParams.B;

            Kc = this.zengYiParams.Kc;
            Ks = this.zengYiParams.Ks;
            Psi = this.zengYiParams.Psi;
        }

        public Document Window
        {
            get { return mainWindow; }
            set { this.mainWindow = value; }
        }

        public ZengYiParams Params
        {
            get { return zengYiParams; }
            set { zengYiParams = value; }
        }

        public DateTime Time
        {
            get { return this.zengYiParams.Time; }
            set
            {
                this.zengYiParams.Time = value;
                RaisePropertyChanged("Time");
            }
        }


        public string Ec
        {
            get { return this.zengYiParams.Ec; }
            set
            {
                this.zengYiParams.Ec = value;
                RaisePropertyChanged("Ec");
            }
        }

        public double Vc
        {
            get { return this.zengYiParams.Vc; }
            set
            {
                this.zengYiParams.Vc = value;
                RaisePropertyChanged("Vc");
            }
        }


        public double Es
        {
            get { return this.zengYiParams.Es; }
            set
            {
                this.zengYiParams.Es = value;
                RaisePropertyChanged("Es");
            }
        }

        public double Vs
        {
            get { return this.zengYiParams.Vs; }
            set
            {
                this.zengYiParams.Vs = value;
                RaisePropertyChanged("Vs");
            }
        }

        public double E
        {
            get { return this.zengYiParams.E; }
            set
            {
                this.zengYiParams.E = value;
                RaisePropertyChanged("E");
            }
        }

        public double V
        {
            get { return this.zengYiParams.V; }
            set
            {
                this.zengYiParams.V = value;
                RaisePropertyChanged("V");
            }
        }


        public double A0
        {
            get { return this.zengYiParams.A0; }
            set
            {
                this.zengYiParams.A0 = value;
                RaisePropertyChanged("A0");
            }
        }

        public double Aw
        {
            set
            {
                this.zengYiParams.Aw = value;
                RaisePropertyChanged("Aw");
            }
            get
            {
                return this.zengYiParams.Aw;
            }
        }

        public double A1
        {
            set
            {
                this.zengYiParams.A1 = value;
                RaisePropertyChanged("A1");
            }
            get
            {
                return this.zengYiParams.A1;
            }
        }


        public double B
        {
            get { return this.zengYiParams.B; }
            set
            {
                this.zengYiParams.B = value;
                RaisePropertyChanged("B");
            }
        }

        public double Kc
        {
            get { return this.zengYiParams.Kc; }
            set
            {
                this.zengYiParams.Kc = value;
                RaisePropertyChanged("Kc");
            }
        }

        public double Ks
        {
            get { return this.zengYiParams.Ks; }
            set
            {
                this.zengYiParams.Ks = value;
                RaisePropertyChanged("Ks");
            }
        }

        public double Psi
        {
            get { return this.zengYiParams.Psi; }
            set
            {
                this.zengYiParams.Psi = value;
                RaisePropertyChanged("Psi");
            }
        }
    }
}
