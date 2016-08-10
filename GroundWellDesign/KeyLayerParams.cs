using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroundWellDesign
{
    [Serializable]
    public class BaseKeyLayerParams
    {
        public BaseKeyLayerParams()
        {
            this.kjqd = Document.KjqdOpt[0];
            this.klqd = Document.KlqdOpt[0];
            this.tgtxml = Document.TgtxmlOpt[0];
        }

        //用于提取参数 保存到文件
        public BaseKeyLayerParams(KeyLayerParams layer)
        {

            ycbh = layer.ycbh;
            ycsd = layer.ycsd;
            mcms = layer.mcms;
            fypjxs = layer.fypjxs;
            fypjxsxz = layer.fypjxsxz;
            gzmtjsj = layer.gzmtjsj;

            cfcdcjwy = layer.cfcdcjwy;
            gzmtjjl = layer.gzmtjjl;
            zx = layer.zx;
            qx = layer.qx;
            cdyxbj = layer.cdyxbj;


            gdydj = layer.gdydj;
            qxhckj = layer.qxhckj;
            cfkckjjl = layer.cfkckjjl;
            sjxcxs = layer.sjxcxs;
            yczdxcz = layer.yczdxcz;

            jsdjscjwy = layer.jsdjscjwy;
            jsdjslcwy = layer.jsdjscjwy;

            //套管安全系数部分
            tgwj = layer.tgwj;
            tgbh = layer.tgbh;
            tgtxml = layer.tgtxml;

            lsqycd = layer.lsqycd;
            klqd = layer.klqd;
            lsyl = layer.lsyl;
            lsaqxs = layer.lsaqxs;

            jqqycd = layer.jqqycd;
            kjqd = layer.kjqd;
            zdjqyl = layer.zdjqyl;
            zdjqyb = layer.zdjqyb;
            jqaqxs = layer.jqaqxs;

        }

        public int ycbh;
        public double ycsd;
        public double mcms;
        public double fypjxs;
        public double fypjxsxz;
        public double gzmtjsj;

        public double cfcdcjwy;
        public double gzmtjjl;
        public double zx;
        public double qx;
        public double cdyxbj;


        public double gdydj;
        public double qxhckj;
        public double cfkckjjl;
        public double sjxcxs;
        public double yczdxcz;

        public double jsdjscjwy;
        public double jsdjslcwy;

        //套管安全系数部分
        public double tgwj;
        public double tgbh;
        public string tgtxml;

        public double lsqycd;
        public string klqd;
        public double lsyl;
        public double lsaqxs;

        public double jqqycd;
        public string kjqd;
        public double zdjqyl;
        public double zdjqyb;
        public double jqaqxs;
    }


    public class KeyLayerParams : BaseKeyLayerParams, INotifyPropertyChanged
    {
        public Document mainWindow;

        public KeyLayerParams(Document mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        public KeyLayerParams(BaseKeyLayerParams layer)
        {
            copyAndNoEvent(layer);
        }

        public void copyAndNoEvent(BaseKeyLayerParams layer)
        {
            ycbh = layer.ycbh;
            ycsd = layer.ycsd;
            mcms = layer.mcms;
            fypjxs = layer.fypjxs;
            fypjxsxz = layer.fypjxsxz;
            gzmtjsj = layer.gzmtjsj;

            cfcdcjwy = layer.cfcdcjwy;
            gzmtjjl = layer.gzmtjjl;
            zx = layer.zx;
            qx = layer.qx;
            cdyxbj = layer.cdyxbj;


            gdydj = layer.gdydj;
            qxhckj = layer.qxhckj;
            cfkckjjl = layer.cfkckjjl;
            sjxcxs = layer.sjxcxs;
            yczdxcz = layer.yczdxcz;

            jsdjscjwy = layer.jsdjscjwy;
            jsdjslcwy = layer.jsdjscjwy;

            //套管安全系数部分
            tgwj = layer.tgwj;
            tgbh = layer.tgbh;
            tgtxml = layer.tgtxml;

            lsqycd = layer.lsqycd;
            klqd = layer.klqd;
            lsyl = layer.lsyl;
            lsaqxs = layer.lsaqxs;

            jqqycd = layer.jqqycd;
            kjqd = layer.kjqd;
            zdjqyl = layer.zdjqyl;
            zdjqyb = layer.zdjqyb;
            jqaqxs = layer.jqaqxs;
        }

        //增加是否危险
        private bool? isDangerous;
        public bool? IsDangerous
        {
            get { return isDangerous; }
            set
            {
                isDangerous = value;
                SetUI("IsDangerous");
            }
        }

        public int Ycbh
        {
            get { return ycbh; }
            set
            {
                ycbh = value;
                SetUI("Ycbh");
            }
        }


        public double Ycsd
        {
            get { return ycsd; }
            set
            {
                ycsd = value;
                SetUI("Ycsd");
            }
        }


        public double Mcms
        {
            get { return mcms; }
            set
            {
                mcms = value;
                SetUI("Mcms");
            }
        }


        public double Fypjxs
        {
            get { return fypjxs; }
            set
            {
                fypjxs = value;
                SetUI("Fypjxs");
            }
        }


        public double Fypjxsxz
        {
            get { return fypjxsxz; }
            set
            {
                fypjxsxz = value;
                SetUI("Fypjxsxz");
            }
        }


        public double Gzmtjsj
        {
            get { return gzmtjsj; }
            set
            {
                gzmtjsj = value;
                Gzmtjjl = gzmtjsj * mainWindow.gzmsd;

                SetUI("Gzmtjsj");

                mainWindow.computeMidData(mainWindow.keyLayers.Count);
            }
        }


        public double Cfcdcjwy
        {
            get { return cfcdcjwy; }
            set
            {
                cfcdcjwy = value;
                SetUI("Cfcdcjwy");
            }
        }


        public double Gzmtjjl
        {
            get { return gzmtjjl; }
            set
            {
                gzmtjjl = value;
                SetUI("Gzmtjjl");
            }
        }


        public double Zx
        {
            get { return zx; }
            set
            {
                zx = value;
                SetUI("Zx");
            }
        }


        public double Qx
        {
            get { return qx; }
            set
            {
                qx = value;
                SetUI("Qx");
            }
        }

        public double Cdyxbj
        {
            get { return cdyxbj; }
            set
            {
                cdyxbj = value;
                SetUI("Cdyxbj");
            }
        }


        public double Gdydj
        {
            get { return gdydj; }
            set
            {
                gdydj = value;
                SetUI("Gdydj");
            }
        }

        public double Qxhckj
        {
            get { return qxhckj; }
            set
            {
                qxhckj = value;
                SetUI("Qxhckj");
            }
        }


        public double Cfkckjjl
        {
            get { return cfkckjjl; }
            set
            {
                cfkckjjl = value;
                SetUI("Cfkckjjl");
            }
        }


        public double Sjxcxs
        {
            get { return sjxcxs; }
            set
            {
                sjxcxs = value;
                SetUI("Sjxcxs");
            }
        }


        public double Yczdxcz
        {
            get { return yczdxcz; }
            set
            {
                yczdxcz = value;
                SetUI("Yczdxcz");
            }
        }


        public double Jsdjscjwy
        {
            get { return jsdjscjwy; }
            set
            {
                jsdjscjwy = value;
                SetUI("Jsdjscjwy");
            }
        }


        public double Jsdjslcwy
        {
            get { return jsdjslcwy; }
            set
            {
                jsdjslcwy = value;
                SetUI("Jsdjslcwy");
            }
        }

        //套管安全系数部分
        private void InvalidDanger()
        {
            int keycount = mainWindow.keyLayers.Count;
                for (int i = 0; i < keycount; i++)
                {
                    mainWindow.keyLayers[i].IsDangerous = null;
                }
        }


        public double Tgwj
        {
            get { return tgwj; }
            set
            {
                tgwj = value;
                InvalidDanger();
                SetUI("Tgwj");
            }
        }

        public double Tgbh
        {
            get { return tgbh; }
            set
            {
                tgbh = value;
                InvalidDanger();
                SetUI("Tgbh");
            }
        }

        public string Tgtxml
        {
            get { return tgtxml; }
            set
            {
                tgtxml = value;
                InvalidDanger();
                SetUI("Tgtxml");
            }
        }

        public double Lsqycd
        {
            get { return lsqycd; }
            set
            {
                lsqycd = value;
                InvalidDanger();
                SetUI("Lsqycd");
            }
        }
        public string Klqd
        {
            get { return klqd; }
            set
            {
                klqd = value;
                InvalidDanger();
                SetUI("Klqd");
            }
        }
        public double Lsyl
        {
            get { return lsyl; }
            set
            {
                lsyl = value;
                SetUI("Lsyl");
            }
        }
        public double Lsaqxs
        {
            get { return lsaqxs; }
            set
            {
                lsaqxs = value;
                SetUI("Lsaqxs");
            }
        }

        public double Jqqycd
        {
            get { return jqqycd; }
            set
            {
                jqqycd = value;
                InvalidDanger();
                SetUI("Jqqycd");
            }
        }
        public string Kjqd
        {
            get { return kjqd; }
            set
            {
                kjqd = value;
                InvalidDanger();
                SetUI("Kjqd");
            }
        }
        public double Zdjqyl
        {
            get { return zdjqyl; }
            set
            {
                zdjqyl = value;
                SetUI("Zdjqyl");
            }
        }
        public double Zdjqyb
        {
            get { return zdjqyb; }
            set
            {
                zdjqyb = value;
                SetUI("Zdjqyb");
            }
        }
        public double Jqaqxs
        {
            get { return jqaqxs; }
            set
            {
                jqaqxs = value;
                SetUI("Jqaqxs");
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
