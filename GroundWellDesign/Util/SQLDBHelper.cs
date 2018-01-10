using GroundWellDesign.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroundWellDesign.Util
{
    public class SQLDBHelper
    {
        public static void CreateTable(SQLiteCommand sqlCmd, string tableName, string primarykey)
        {
            string sql_exist_table = "select count(*) from sqlite_master where type='table' and name='" + tableName + "'";
            sqlCmd.CommandText = sql_exist_table;
            object result = sqlCmd.ExecuteScalar();
            if ((long)result == 0)  // 不存在,创建新表
            {
                string sql_createwell = "CREATE TABLE " + tableName + " (" + primarykey + ");";
                sqlCmd.CommandText = sql_createwell;
                sqlCmd.ExecuteNonQuery();
            }
        }


        public static void AddColumn(SQLiteCommand sqlCmd, string tableName, string colName, string colProperty)
        {
            string sql_has_col = "PRAGMA table_info(" + tableName + ");";
            sqlCmd.CommandText = sql_has_col;
            SQLiteDataReader reader = sqlCmd.ExecuteReader();
            bool found = false;
            while(reader.Read())
            {
                if(reader["name"].Equals(colName))
                {
                    found = true;
                    break;
                }
            }
            reader.Close();

            if (!found)  // 插入该字段
            {
                string sql_insertcol = "alter table " + tableName + " add column " + colName + " " + colProperty + ";";
                sqlCmd.CommandText = sql_insertcol;
                sqlCmd.ExecuteNonQuery();
            }
        }


        public static bool saveToSqlite(LayerBaseParams layer, string uuid, bool cover, string wellName, bool wellExist)
        {
            string dbPath = "Data Source =" + ContainerWindow.DATABASE_PATH;
            SQLiteConnection conn = new SQLiteConnection(dbPath);
            conn.Open();
            var trans = conn.BeginTransaction();

            //1well表新增一项
            SQLiteCommand cmd1 = null;
            if (!wellExist)
            {
                cmd1 = new SQLiteCommand(conn);
                cmd1.Transaction = trans;
                cmd1.CommandText = "insert into well values('" + wellName + "')";
            }


            //2删除旧的数据
            SQLiteCommand cmd2 = null;
            if (cover)
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
                if (!wellExist)
                    cmd1.ExecuteNonQuery();
                if (cover)
                    cmd2.ExecuteNonQuery();
                cmd3.ExecuteNonQuery();
                trans.Commit();
                success = true;
            }
            catch (Exception e)
            {
                layer.MiaoShu = e.Message;
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
            if (wellName != null)
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
                if (reader["miaoShu"] != DBNull.Value)
                    param.LayerParams.MiaoShu = (string)reader["miaoShu"];
                else
                    param.LayerParams.MiaoShu = "";

                existedLayers.Add(param);
            }
            reader.Close();
            conn.Close();
        }
    }
}
