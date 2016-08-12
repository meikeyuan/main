using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroundWellDesign
{


    [Serializable]
    public class BaseZengYiParams
    {
        public BaseZengYiParams()
        {
            ec = Document.TgtxmlOpt[0];
        }

        public BaseZengYiParams(ZengYiParams param)
        {
            time = param.time;

            ec = param.ec;
            vc = param.vc;
            es = param.es;
            vs = param.vs;
            e = param.e;
            v = param.v;

            a0 = param.a0;
            aw = param.aw;
            a1 = param.a1;
            b = param.b;

            kc = param.kc;
            ks = param.ks;
            psi = param.psi;
        }

        public DateTime time;

        public string ec;
        public double vc;
        public double es;
        public double vs;
        public double e;
        public double v;

        public double a0;
        public double aw;
        public double a1;
        public double b;

        public double kc;
        public double ks;
        public double psi;

    }





    public class ZengYiParams : BaseZengYiParams, INotifyPropertyChanged
    {

        public Document mainWindow;
        public ZengYiParams(Document mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        public ZengYiParams(Document mainWindow, BaseZengYiParams param)
        {
            this.mainWindow = mainWindow;
            copyAndEvent(param);
        }

        public void reset()
        {
            copyAndEvent(new BaseZengYiParams());
        }


        public void copyAndEvent(BaseZengYiParams param)
        {
            Time = param.time;

            Ec = param.ec;
            Vc = param.vc;
            Es = param.es;
            Vs = param.vs;
            E = param.e;
            V = param.v;

            A0 = param.a0;
            Aw = param.aw;
            A1 = param.a1;
            B = param.b;

            Kc = param.kc;
            Ks = param.ks;
            Psi = param.psi;
        }

        public DateTime Time
        {
            get { return time; }
            set
            {
                time = value;
                SetUI("Time");
            }
        }


        public string Ec
        {
            get { return ec; }
            set
            {
                ec = value;
                SetUI("Ec");
            }
        }

        public double Vc
        {
            get { return vc; }
            set
            {
                vc = value;
                SetUI("Vc");
            }
        }


        public double Es
        {
            get { return es; }
            set
            {
                es = value;
                SetUI("Es");
            }
        }

        public double Vs
        {
            get { return vs; }
            set
            {
                vs = value;
                SetUI("Vs");
            }
        }

        public double E
        {
            get { return e; }
            set
            {
                e = value;
                SetUI("E");
            }
        }

        public double V
        {
            get { return v; }
            set
            {
                v = value;
                SetUI("V");
            }
        }


        public double A0
        {
            get { return a0; }
            set
            {
                a0 = value;
                SetUI("A0");
            }
        }

        public double Aw
        {
            set
            {
                aw = value;
                SetUI("Aw");
            }
            get
            {
                return aw;
            }
        }

        public double A1
        {
            set
            {
                a1 = value;
                SetUI("A1");
            }
            get
            {
                return a1;
            }
        }


        public double B
        {
            get { return b; }
            set
            {
                b = value;
                SetUI("B");
            }
        }

        public double Kc
        {
            get { return kc; }
            set
            {
                kc = value;
                SetUI("Kc");
            }
        }

        public double Ks
        {
            get { return ks; }
            set
            {
                ks = value;
                SetUI("Ks");
            }
        }

        public double Psi
        {
            get { return psi; }
            set
            {
                psi = value;
                SetUI("Psi");
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
