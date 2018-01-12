using GroundWellDesign.Util;
using GroundWellDesign.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GroundWellDesign
{
    class DataSaveAndRestore
    {
        private static DataToSave dump_obj(Document document)
        {
            DataToSave data = new DataToSave();

            //基本参数
            data.Layers = new List<LayerBaseParams>();
            foreach (LayerBaseParamsViewModel layerParam in document.layers)
            {
                data.Layers.Add(layerParam.LayerParams);
            }

            //人工设计数据
            data.ManuDesignParams = document.manuDesignParams.Param;

            //关键层参数
            data.KeyLayers = new List<KeyLayerParams>();
            foreach (KeyLayerParamsViewModel layerParam in document.keyLayers)
            {
                data.KeyLayers.Add(layerParam.Params);
            }


            data.KeyLayerData = new List<double>();
            //保存横三带竖三代数据
            data.KeyLayerData.Add(document.Mcqj);
            data.KeyLayerData.Add(document.FuYanXCL);
            data.KeyLayerData.Add(document.CaiGao);
            data.KeyLayerData.Add(document.SuiZhangXS);
            double maoLuoDai, lieXiDai, wanQuDai;
            double.TryParse(document.maoLuoDaiTb.Text, out maoLuoDai);
            double.TryParse(document.lieXiDaiTb.Text, out lieXiDai);
            double.TryParse(document.wanQuDaiTb.Text, out wanQuDai);
            data.KeyLayerData.Add(maoLuoDai);
            data.KeyLayerData.Add(lieXiDai);
            data.KeyLayerData.Add(wanQuDai);
            //保存关键层计算相关数据
            data.KeyLayerData.Add(document.Mchd);
            data.KeyLayerData.Add(document.Pjxsxz);
            data.KeyLayerData.Add(document.HcqZXcd);
            data.KeyLayerData.Add(document.HcqQXcd);
            data.KeyLayerData.Add(document.Gzmsd);
            data.KeyLayerData.Add(document.Jswzjl);

            //保存水泥环增益计算的数据
            data.ZengYis = new List<ZengYiParams>();
            foreach (ZengYiParamsViewModel param in document.zengYis)
            {
                data.ZengYis.Add(param.Params);
            }

            return data;
        }


        public static bool saveDocument(Document document, string path)
        {
            DataToSave obj = dump_obj(document);
            if(obj == null)
            {
                return false;
            }

            //通过formatter对象以二进制格式将obj对象序列化后到文件中
            Stream stream = null;
            try
            {
                stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                return true;
            }
            catch (Exception e)
            {
                FileHelper.WriteFile("C:\\log.txt", e.Message);
                return false;
            }
            finally
            {
                stream.Close();
            }
            
        }


        public static bool restoreDocument(Document document, string path)
        {
            //重新取回数据 
            Stream stream = null;
            object obj = null;
            try
            {
                IFormatter formatter = new BinaryFormatter();
                stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
                obj = formatter.Deserialize(stream);
                if (obj == null || !(obj is DataSaveAndRestore.DataToSave))
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                FileHelper.WriteFile("C:\\log.txt", e.Message);
                return false;
            }
            finally
            {
                stream.Close();
            }

            DataSaveAndRestore.DataToSave data = obj as DataSaveAndRestore.DataToSave;
            //恢复基本参数
            document.layers.Clear();
            foreach (LayerBaseParams baseParam in data.Layers)
            {
                LayerBaseParamsViewModel layer = new LayerBaseParamsViewModel(document, baseParam);
                document.layers.Add(layer);
            }

            //恢复关键层数据
            document.keyLayers.Clear();
            foreach (KeyLayerParams baseParam in data.KeyLayers)
            {
                KeyLayerParamsViewModel layer = new KeyLayerParamsViewModel(document, baseParam);
                document.keyLayers.Add(layer);

            }

            //恢复关键层其他数据
            if (data.KeyLayerData != null && data.KeyLayerData.Count == 13)
            {
                int i = 0;
                document.mcqj = data.KeyLayerData[i++];
                document.FuYanXCL = data.KeyLayerData[i++];
                document.CaiGao = data.KeyLayerData[i++];
                document.SuiZhangXS = data.KeyLayerData[i++];
                document.maoLuoDaiTb.Text = data.KeyLayerData[i++].ToString("f3");
                document.lieXiDaiTb.Text = data.KeyLayerData[i++].ToString("f3");
                document.wanQuDaiTb.Text = data.KeyLayerData[i++].ToString("f3");

                document.mchd = data.KeyLayerData[i++];
                document.pjxsxz = data.KeyLayerData[i++];
                document.hcqZxcd = data.KeyLayerData[i++];
                document.hcqQxcd = data.KeyLayerData[i++];
                document.gzmsd = data.KeyLayerData[i++];
                document.jswzjl = data.KeyLayerData[i++];


                // 刷新UI
                document.meiCengQingJIaoTb.Text = document.mcqj + "";
                document.fuYanXCLTb.Text = document.FuYanXCL + "";
                document.caiGaoTb.Text = document.CaiGao + "";
                document.suiZhangXSTb.Text = document.SuiZhangXS + "";

                document.meiCengHouDuTb.Text = document.mchd + "";
                document.xiuZhengXishuTb.Text = document.pjxsxz + "";
                document.hcqZXcdTb.Text = document.hcqZxcd + "";
                document.hcqQXcdTb.Text = document.hcqQxcd + "";
                document.gZMTJSDTb.Text = document.gzmsd + "";
                document.jswzjlTb.Text = document.jswzjl + "";
            }

            //恢复水泥环历史数据
            document.zengYis.Clear();
            foreach (ZengYiParams baseParam in data.ZengYis)
            {
                ZengYiParamsViewModel param = new ZengYiParamsViewModel(document, baseParam);
                document.zengYis.Add(param);
            }

            //恢复人工设计数据
            document.manuDesignParams.Param = data.ManuDesignParams;

            document.FilePath = path;
            return true;
        }


        [Serializable]
        public class DataToSave
        {
            public List<LayerBaseParams> Layers
            {
                get;
                set;
            }

            public List<KeyLayerParams> KeyLayers
            {
                get;
                set;
            }
            
            public List<double> KeyLayerData
            {
                get;
                set;
            }

            public List<ZengYiParams> ZengYis
            {
                get;
                set;
            }
            
            public ManuDesignParams ManuDesignParams
            {
                get;
                set;
            }

        }
    }
}
