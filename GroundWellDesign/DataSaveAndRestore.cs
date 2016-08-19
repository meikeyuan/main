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
        public static DataToSave getObject(Document document)
        {
            DataToSave data = new DataToSave();

            //基本参数
            data.Layers = new ObservableCollection<BaseLayerBaseParams>();
            foreach (LayerBaseParams layerParam in document.layers)
            {
                data.Layers.Add(new BaseLayerBaseParams(layerParam));
            }

            //人工设计数据
            data.ManuDesignParams = new BaseManuDesignParams(document.manuDesignParams);

            //关键层参数
            data.KeyLayers = new ObservableCollection<BaseKeyLayerParams>();
            foreach (KeyLayerParams layerParam in document.keyLayers)
            {
                data.KeyLayers.Add(new BaseKeyLayerParams(layerParam));
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
            data.ZengYis = new ObservableCollection<BaseZengYiParams>();
            foreach (ZengYiParams param in document.zengYis)
            {
                data.ZengYis.Add(new BaseZengYiParams(param));
            }

            return data;
        }


        public static bool saveObj(object obj, string path)
        {
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


        public static object restoreObj(string path)
        {
            //重新取回数据 
            IFormatter formatter = new BinaryFormatter();
            Stream stream2 = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);

            try
            {
                object obj = formatter.Deserialize(stream2);
                stream2.Close();
                return obj;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public static bool saveToSqlite(LayerBaseParams layer, string uuid, bool cover, string wellName, bool wellExist)
        {
            SQLiteConnection conn = null;
            string dbPath = "Data Source =" + ContainerWindow.DATABASE_PATH;
            conn = new SQLiteConnection(dbPath);
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
            "@bianXingMoLiang, @kangLaQiangDu, @kangYaQiangDu, @tanXingMoLiang, @boSonBi, @neiMoCaJiao, @nianJuLi, @q0, @q1, @q2, @miaoShu)";

            cmd3.Parameters.AddRange(new[]{
                new SQLiteParameter("@id", uuid),
                new SQLiteParameter("@wellName", wellName),
                new SQLiteParameter("@yanXing", layer.yanXing),
                new SQLiteParameter("@leiJiShenDu", layer.leiJiShenDu),
                new SQLiteParameter("@juLiMeiShenDu", layer.juLiMeiShenDu),
                new SQLiteParameter("@cengHou", layer.cengHou),
                new SQLiteParameter("@ziRanMiDu", layer.ziRanMiDu),
                new SQLiteParameter("@bianXingMoLiang", layer.bianXingMoLiang),
                new SQLiteParameter("@kangLaQiangDu", layer.kangLaQiangDu),
                new SQLiteParameter("@kangYaQiangDu", layer.kangYaQiangDu),
                new SQLiteParameter("@tanXingMoLiang", layer.tanXingMoLiang),
                new SQLiteParameter("@boSonBi", layer.boSonBi),
                new SQLiteParameter("@neiMoCaJiao", layer.neiMoCaJiao),
                new SQLiteParameter("@nianJuLi", layer.nianJuLi),
                new SQLiteParameter("@q0", layer.q0),
                new SQLiteParameter("@q1", layer.q1),
                new SQLiteParameter("@q2", layer.q2),
                new SQLiteParameter("@miaoShu", layer.miaoShu)
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
            SQLiteConnection conn = null;
            string dbPath = "Data Source =" + ContainerWindow.DATABASE_PATH;
            conn = new SQLiteConnection(dbPath);
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


        public static void getDBLayers(ObservableCollection<LayerBaseParams> existedLayers, string yanXing, string wellName)
        {

            if (existedLayers == null)
                return;

            existedLayers.Clear();


            //查询well表
            SQLiteConnection conn = null;
            string dbPath = "Data Source =" + ContainerWindow.DATABASE_PATH;
            conn = new SQLiteConnection(dbPath);
            conn.Open();
            string sql = "select * from yanCeng where yanXing = '" + yanXing + "'";
            if(wellName != null)
                sql += " and wellName = '" + wellName + "'";

            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                LayerBaseParams param = new LayerBaseParams(null, new BaseLayerBaseParams());
                param.wellNamePK = (string)reader["wellName"];
                param.yanXing = (string)reader["yanXing"];
                param.leiJiShenDu = (double)reader["leiJiShenDu"];
                param.juLiMeiShenDu = (double)reader["juLiMeiShenDu"];
                param.cengHou = (double)reader["cengHou"];
                param.ziRanMiDu = (double)reader["ziRanMiDu"];
                param.bianXingMoLiang = (double)reader["bianXingMoLiang"];
                param.kangLaQiangDu = (double)reader["kangYaQiangDu"];
                param.kangYaQiangDu = (double)reader["kangYaQiangDu"];
                param.tanXingMoLiang = (double)reader["tanXingMoLiang"];
                param.boSonBi = (double)reader["boSonBi"];
                param.neiMoCaJiao = (double)reader["neiMoCaJiao"];
                param.nianJuLi = (double)reader["nianJuLi"];
                param.q0 = (double)reader["q0"];
                param.q1 = (double)reader["q1"];
                param.q2 = (double)reader["q2"];

                param.miaoShu = (string)reader["miaoShu"];
                existedLayers.Add(param);
            }
            reader.Close();
            conn.Close();
        }





        [Serializable]
        public class DataToSave
        {
            public ObservableCollection<BaseLayerBaseParams> Layers
            {
                get;
                set;
            }

            public ObservableCollection<BaseKeyLayerParams> KeyLayers
            {
                get;
                set;
            }
            
            public List<double> KeyLayerData
            {
                get;
                set;
            }

            public ObservableCollection<BaseZengYiParams> ZengYis
            {
                get;
                set;
            }
            
            public BaseManuDesignParams ManuDesignParams
            {
                get;
                set;
            }

        }
    }
}
