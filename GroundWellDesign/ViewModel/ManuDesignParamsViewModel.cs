using MvvmFoundation.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GroundWellDesign.ViewModel
{
    public class ManuDesignParamsViewModel : ObservableObject
    {
        private ManuDesignParams param;
        private Document mainWindow;

        public ManuDesignParamsViewModel(Document mainWindow = null, ManuDesignParams paramsss = null)
        {
            this.mainWindow = mainWindow;
            if(paramsss == null)
            {
                this.param = new ManuDesignParams();
            }
            else
            {
                this.param = paramsss.Clone() as ManuDesignParams;
            }
        }

        public void reset()
        {
            this.param = new ManuDesignParams();
            RaiseUI();   
        }

        private void RaiseUI()
        {
            JieGou = param.jieGou;

            Zjsd1 = param.zjsd1;
            Tggj1 = param.tggj1;
            Tgxh1 = param.tgxh1;
            Gjfs1 = param.gjfs1;
            Miaoshu1 = param.miaoshu1;

            Zjsd2 = param.zjsd2;
            Tggj2 = param.tggj2;
            Tgxh2 = param.tgxh2;
            Gjfs2 = param.gjfs2;
            Wjfs2 = param.wjfs2;
            Miaoshu2 = param.miaoshu2;

            Zjsd3 = param.zjsd3;
            Tggj3 = param.tggj3;
            Tgxh3 = param.tgxh3;
            Wjfs3 = param.wjfs3;
            Miaoshu3 = param.miaoshu3;
        }

        public Document Window
        {
            get { return mainWindow; }
            set { this.mainWindow = value; }
        }

        public ManuDesignParams Param
        {
            get { return this.param; }
            set { this.param = value; }
        }

        public string JieGou
        {
            set
            {
                if (param.jieGou != value)
                {
                    param.jieGou = value;
                    if (param.jieGou.Equals(Document.JieGouOpt[0]))
                    {
                        //二开
                        mainWindow.gjfs1Combo.IsEnabled = true;
                        mainWindow.threeGrid.Visibility = Visibility.Hidden;
                        mainWindow.threeTb.Visibility = Visibility.Hidden;
                        mainWindow.gjTB2.Visibility = Visibility.Hidden;
                        mainWindow.gjCombo.Visibility = Visibility.Hidden;
                        mainWindow.wjTB2.Visibility = Visibility.Visible;
                        mainWindow.wjCombo.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        //三开
                        Gjfs1 = Document.GjfsOpt[0];
                        mainWindow.gjfs1Combo.IsEnabled = false;
                        mainWindow.threeGrid.Visibility = Visibility.Visible;
                        mainWindow.threeTb.Visibility = Visibility.Visible;
                        mainWindow.gjTB2.Visibility = Visibility.Visible;
                        mainWindow.gjCombo.Visibility = Visibility.Visible;
                        mainWindow.wjTB2.Visibility = Visibility.Hidden;
                        mainWindow.wjCombo.Visibility = Visibility.Hidden;
                    }
                }
            }
            get
            {
                return param.jieGou;
            }
        }

        public double Zjsd1
        {
            get { return param.zjsd1; }
            set
            {
                param.zjsd1 = value;
                RaisePropertyChanged("Zjsd1");
            }
        }
        public string Tggj1
        {
            get { return param.tggj1; }
            set
            {
                param.tggj1 = value;
                RaisePropertyChanged("Tggj1");
            }
        }
        public string Tgxh1
        {
            get { return param.tgxh1; }
            set
            {
                param.tgxh1 = value;
                RaisePropertyChanged("Tgxh1");
            }
        }
        public string Gjfs1
        {
            get { return param.gjfs1; }
            set
            {
                param.gjfs1 = value;
                RaisePropertyChanged("Gjfs1");
            }
        }
        public string Miaoshu1
        {
            get { return param.miaoshu1; }
            set
            {
                param.miaoshu1 = value;
                RaisePropertyChanged("Miaoshu1");
            }
        }

        public double Zjsd2
        {
            get { return param.zjsd2; }
            set
            {
                param.zjsd2 = value;
                RaisePropertyChanged("Zjsd2");
            }
        }
        public string Tggj2
        {
            get { return param.tggj2; }
            set
            {
                param.tggj2 = value;
                RaisePropertyChanged("Tggj2");
            }
        }
        public string Tgxh2
        {
            get { return param.tgxh2; }
            set
            {
                param.tgxh2 = value;
                RaisePropertyChanged("Tgxh2");
            }
        }
        public string Gjfs2
        {
            get { return param.gjfs2; }
            set
            {
                param.gjfs2 = value;
                RaisePropertyChanged("Gjfs2");
            }
        }
        public string Wjfs2
        {
            get { return param.wjfs2; }
            set
            {
                param.wjfs2 = value;
                RaisePropertyChanged("Wjfs2");
            }
        }
        public string Miaoshu2
        {
            get { return param.miaoshu2; }
            set
            {
                param.miaoshu2 = value;
                RaisePropertyChanged("Miaoshu2");
            }
        }

        public double Zjsd3
        {
            get { return param.zjsd3; }
            set
            {
                param.zjsd3 = value;
                RaisePropertyChanged("Zjsd3");
            }
        }
        public string Tggj3
        {
            get { return param.tggj3; }
            set
            {
                param.tggj3 = value;
                RaisePropertyChanged("Tggj3");
            }
        }
        public string Tgxh3
        {
            get { return param.tgxh3; }
            set
            {
                param.tgxh3 = value;
                RaisePropertyChanged("Tgxh3");
            }
        }
        public string Wjfs3
        {
            get { return param.wjfs3; }
            set
            {
                param.wjfs3 = value;
                RaisePropertyChanged("Wjfs3");
            }
        }
        public string Miaoshu3
        {
            get { return param.miaoshu3; }
            set
            {
                param.miaoshu3 = value;
                RaisePropertyChanged("Miaoshu3");
            }
        }
    }
}
