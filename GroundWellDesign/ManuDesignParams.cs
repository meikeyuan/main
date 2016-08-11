using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GroundWellDesign
{
    [Serializable]
    public class BaseManuDesignParams
    {
        public BaseManuDesignParams()
        {
            jieGou = Document.JieGouOpt[0];
            tggj1 = Document.TggjOpt[0];
            gjfs1 = Document.GjfsOpt[0];
            tggj2 = Document.TggjOpt[0];
            gjfs2 = Document.GjfsOpt[0];
            wjfs2 = Document.WjfsOpt[0];
            tggj3 = Document.TggjOpt[0];
            wjfs3 = Document.WjfsOpt[0];
        }



        public BaseManuDesignParams(ManuDesignParams param)
        {
            jieGou = param.jieGou;

            zjsd1 = param.zjsd1;
            tggj1 = param.tggj1;
            tgxh1 = param.tgxh1;
            gjfs1 = param.gjfs1;
            miaoshu1 = param.miaoshu1;

            zjsd2 = param.zjsd2;
            tggj2 = param.tggj2;
            tgxh2 = param.tgxh2;
            gjfs2 = param.gjfs2;
            wjfs2 = param.wjfs2;
            miaoshu2 = param.miaoshu2;

            zjsd3 = param.zjsd3;
            tggj3 = param.tggj3;
            tgxh3 = param.tgxh3;
            wjfs3 = param.wjfs3;
            miaoshu3 = param.miaoshu3;
        }

        public string jieGou;

        public double zjsd1;
        public string tggj1;
        public string tgxh1;
        public string gjfs1;
        public string miaoshu1;

        public double zjsd2;
        public string tggj2;
        public string tgxh2;
        public string gjfs2;
        public string wjfs2;
        public string miaoshu2;

        public double zjsd3;
        public string tggj3;
        public string tgxh3;
        public string wjfs3;
        public string miaoshu3;
    }





    public class ManuDesignParams : BaseManuDesignParams, INotifyPropertyChanged
    {
        public Document mainWindow;
        public ManuDesignParams(Document mainWindow)
        {
            this.mainWindow = mainWindow;
        }
        public void reset()
        {
            copyAndEvent(new BaseManuDesignParams());       
        }


        public void copyAndEvent(BaseManuDesignParams param)
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

        public string JieGou
        {
            set
            {
                if (jieGou != value)
                {
                    jieGou = value;
                    if (jieGou.Equals(Document.JieGouOpt[0]))
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
                return jieGou;
            }
        }

        public double Zjsd1
        {
            get { return zjsd1; }
            set
            {
                zjsd1 = value;
                SetUI("Zjsd1");
            }
        }
        public string Tggj1
        {
            get { return tggj1; }
            set
            {
                tggj1 = value;
                SetUI("Tggj1");
            }
        }
        public string Tgxh1
        {
            get { return tgxh1; }
            set
            {
                tgxh1 = value;
                SetUI("Tgxh1");
            }
        }
        public string Gjfs1
        {
            get { return gjfs1; }
            set
            {
                gjfs1 = value;
                SetUI("Gjfs1");
            }
        }
        public string Miaoshu1
        {
            get { return miaoshu1; }
            set
            {
                miaoshu1 = value;
                SetUI("Miaoshu1");
            }
        }

        public double Zjsd2
        {
            get { return zjsd2; }
            set
            {
                zjsd2 = value;
                SetUI("Zjsd2");
            }
        }
        public string Tggj2
        {
            get { return tggj2; }
            set
            {
                tggj2 = value;
                SetUI("Tggj2");
            }
        }
        public string Tgxh2
        {
            get { return tgxh2; }
            set
            {
                tgxh2 = value;
                SetUI("Tgxh2");
            }
        }
        public string Gjfs2
        {
            get { return gjfs2; }
            set
            {
                gjfs2 = value;
                SetUI("Gjfs2");
            }
        }
        public string Wjfs2
        {
            get { return wjfs2; }
            set
            {
                wjfs2 = value;
                SetUI("Wjfs2");
            }
        }
        public string Miaoshu2
        {
            get { return miaoshu2; }
            set
            {
                miaoshu2 = value;
                SetUI("Miaoshu2");
            }
        }

        public double Zjsd3
        {
            get { return zjsd3; }
            set
            {
                zjsd3 = value;
                SetUI("Zjsd3");
            }
        }
        public string Tggj3
        {
            get { return tggj3; }
            set
            {
                tggj3 = value;
                SetUI("Tggj3");
            }
        }
        public string Tgxh3
        {
            get { return tgxh3; }
            set
            {
                tgxh3 = value;
                SetUI("Tgxh3");
            }
        }
        public string Wjfs3
        {
            get { return wjfs3; }
            set
            {
                wjfs3 = value;
                SetUI("Wjfs3");
            }
        }
        public string Miaoshu3
        {
            get { return miaoshu3; }
            set
            {
                miaoshu3 = value;
                SetUI("Miaoshu3");
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
