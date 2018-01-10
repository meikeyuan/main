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

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);

            //通过formatter对象以二进制格式将obj对象序列化后到文件中
            try
            {
                formatter.Serialize(stream, obj);
                stream.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }


        public static bool restoreDocument(Document document, string path)
        {
            //重新取回数据 
            IFormatter formatter = new BinaryFormatter();
            Stream stream2 = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);

            object obj = null;
            try
            {
                obj = formatter.Deserialize(stream2);
                stream2.Close();
                if (obj == null || !(obj is DataSaveAndRestore.DataToSave))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
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


        public static bool saveToSqlite(LayerBaseParams layer, string uuid, bool cover, string wellName, bool wellExist)
        {
            string dbPath = "Data Source =" + ContainerWindow.DATABASE_PATH;
            SQLiteConnection conn = new SQLiteConnection(dbPath);
            conn.Open();
            var trans = conn.BeginTransaction();

            //1well表新增一项
            SQLiteCommand cmd1 = null;
            if(!wellExist)
            {
                cmd1 = new SQLiteCommand(conn);
                cmd1.Transaction = trans;
                cmd1.CommandText = "insert into well values('" + wellName + "')";
            }
            

            //2删除旧的数据
            SQLiteCommand cmd2 = null;
            if(cover)
            {
                cmd2 = new SQLiteCommand(conn);
                cmd2.Transaction = trans;
                cmd2.CommandText = "delete from yanceng where id = '" + uuid + "'";
            }
            
            //3新增岩层
            SQLiteCommand cmd3 = new SQLiteCommand(conn);
            cmd3.Transaction = trans;
            cmd3.CommandText = "insert into yanceng values(@id, @wellName, @yanXing, @leiJiShenDu, @juLiMeiShenDu, @cengHou, @ziRanMiDu, " +
            "@bianXingMoLiang, @kangLaQiangDu, @kangYaQiangDu, @tanXingMoLiang, @boSonBi, @neiMoCaJiao, @nianJuLi, @f, @q0, @q1, @q2, @miaoShu)";

            cmd3.Parameters.AddRange(new[]{
                new SQLiteParameter("@id", uuid),
                new SQLiteParameter("@wellName", wellName),
                new SQLiteParameter("@yanXing", layer.YanXing),
                new SQLiteParameter("@leiJiShenDu", layer.LeiJiShenDu),
                new SQLiteParameter("@juLiMeiShenDu", layer.JuLiMeiShenDu),
                new SQLiteParameter("@cengHou", layer.CengHou),
                new SQLiteParameter("@ziRanMiDu", layer.ZiRanMiDu),
                new SQLiteParameter("@bianXingMoLiang", layer.BianXingMoLiang),
                new SQLiteParameter("@kangLaQiangDu", layer.KangLaQiangDu),
                new SQLiteParameter("@kangYaQiangDu", layer.KangYaQiangDu),
                new SQLiteParameter("@tanXingMoLiang", layer.TanXingMoLiang),
                new SQLiteParameter("@boSonBi", layer.BoSonBi),
                new SQLiteParameter("@neiMoCaJiao", layer.NeiMoCaJiao),
                new SQLiteParameter("@nianJuLi", layer.NianJuLi),
                new SQLiteParameter("@f", layer.F),
                new SQLiteParameter("@q0", layer.Q0),
                new SQLiteParameter("@q1", layer.Q1),
                new SQLiteParameter("@q2", layer.Q2),
                new SQLiteParameter("@miaoShu", layer.MiaoShu)
            });

            bool success = false;
            try
            {
                //矿井不存在则新建
                if(!wellExist)
                    cmd1.ExecuteNonQuery();
                if(cover)
                    cmd2.ExecuteNonQuery();
                cmd3.ExecuteNonQuery();
                trans.Commit();
                success = true;
            }
            catch (Exception)
            {
                trans.Rollback();
                success = false;
            }
            conn.Close();
            return success;

        }


        public static void getAllWellName(List<String> wells)
        {
            if (wells == null)
                return;
            wells.Clear();

            //查询well表
            string dbPath = "Data Source =" + ContainerWindow.DATABASE_PATH;
            SQLiteConnection conn = new SQLiteConnection(dbPath);
            conn.Open();

            string sql = "select wellName from well";
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                wells.Add(reader.GetString(0));
            }
            reader.Close();
            conn.Close();
        }


        public static void getDBLayers(ObservableCollection<LayerBaseParamsViewModel> existedLayers, string yanXing, string wellName)
        {

            if (existedLayers == null)
                return;
            existedLayers.Clear();


            //查询well表
            string dbPath = "Data Source =" + ContainerWindow.DATABASE_PATH;
            SQLiteConnection conn = new SQLiteConnection(dbPath);
            conn.Open();

            string sql = "select * from yanCeng where yanXing = '" + yanXing + "'";
            if(wellName != null)
                sql += " and wellName = '" + wellName + "'";

            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                LayerBaseParamsViewModel param = new LayerBaseParamsViewModel();
                param.LayerParams.WellNamePK = (string)reader["wellName"];
                param.LayerParams.YanXing = (string)reader["yanXing"];
                param.LayerParams.LeiJiShenDu = (double)reader["leiJiShenDu"];
                param.LayerParams.JuLiMeiShenDu = (double)reader["juLiMeiShenDu"];
                param.LayerParams.CengHou = (double)reader["cengHou"];
                param.LayerParams.ZiRanMiDu = (double)reader["ziRanMiDu"];
                param.LayerParams.BianXingMoLiang = (double)reader["bianXingMoLiang"];
                param.LayerParams.KangLaQiangDu = (double)reader["kangYaQiangDu"];
                param.LayerParams.KangYaQiangDu = (double)reader["kangYaQiangDu"];
                param.LayerParams.TanXingMoLiang = (double)reader["tanXingMoLiang"];
                param.LayerParams.BoSonBi = (double)reader["boSonBi"];
                param.LayerParams.NeiMoCaJiao = (double)reader["neiMoCaJiao"];
                param.LayerParams.NianJuLi = (double)reader["nianJuLi"];
                param.LayerParams.F = (double)reader["f"];
                param.LayerParams.Q0 = (double)reader["q0"];
                param.LayerParams.Q1 = (double)reader["q1"];
                param.LayerParams.Q2 = (double)reader["q2"];
                param.LayerParams.MiaoShu = (string)reader["miaoShu"];

                existedLayers.Add(param);
            }
            reader.Close();
            conn.Close();
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
