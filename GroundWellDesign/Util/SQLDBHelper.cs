using GroundWellDesign.Properties;
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
        public static SQLiteConnection GetSQLiteConnection()
        {
            SQLiteConnection conn = null;
            try
            {
                string dbPath = "Data Source =" + Resources.DataBaseDirectory + Resources.DataBaseName;
                conn = new SQLiteConnection(dbPath);
                conn.Open();
                return conn;
            }
            catch(Exception e)
            {
                App.logger.Fatal(Resources.OpenDataBaseError, e);
                if (conn != null)
                    conn.Close();
                return null;
            }
        }


        public static bool CreateTable(SQLiteCommand sqlCmd, string tableName, string primarykey)
        {
            if(sqlCmd == null || tableName == null || primarykey == null)
            {
                return false;
            }

            // 首先查询表是否存在。
            object executeScalarRes = null;
            try
            {
                string sql_exist_table = "select count(*) from sqlite_master where type='table' and name='" + tableName + "'";
                sqlCmd.CommandText = sql_exist_table;
                executeScalarRes = sqlCmd.ExecuteScalar();
            }
            catch(Exception e)
            {
                App.logger.Error(Resources.QueryExistTableError, e);
                return false;
            }

            // 不存在则创建新表。
            if ((long)executeScalarRes == 0)
            {
                try
                {
                    string sql_createwell = "CREATE TABLE " + tableName + " (" + primarykey + ");";
                    sqlCmd.CommandText = sql_createwell;
                    sqlCmd.ExecuteNonQuery();
                    return true;
                }
                catch(Exception e)
                {
                    App.logger.Error(Resources.CreateTableError, e);
                    return false;
                }
            }
            else
            {
                return true;
            }
        }


        public static bool AddColumn(SQLiteCommand sqlCmd, string tableName, string colName, string colProperty)
        {
            if(sqlCmd == null || tableName == null || colName == null || colProperty == null)
            {
                return false;
            }

            // 首先查询字段是否存在。
            bool found = false;
            SQLiteDataReader reader = null;
            try
            {
                string sql_has_col = "PRAGMA table_info(" + tableName + ");";
                sqlCmd.CommandText = sql_has_col;
                reader = sqlCmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["name"].Equals(colName))
                    {
                        found = true;
                        break;
                    }
                }
            }
            catch(Exception e)
            {
                App.logger.Error(Resources.QueryExistColumeError, e);
                return false;
            }
            finally
            {
                if(reader != null)
                    reader.Close();
            }
            
            // 不存在则插入字段。
            if (!found)
            {
                try
                {
                    string sql_insertcol = "alter table " + tableName + " add column " + colName + " " + colProperty + ";";
                    sqlCmd.CommandText = sql_insertcol;
                    sqlCmd.ExecuteNonQuery();
                    return true;
                }
                catch(Exception e)
                {
                    App.logger.Error(Resources.InsertColumeError, e);
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        public static bool LayerExisted(string layerKey)
        {
            if (layerKey == null)
                return false;

            SQLiteConnection conn = GetSQLiteConnection();
            if (conn == null)
            {
                App.logger.Error(Resources.OpenDataBaseError);
                return false;
            }

            SQLiteDataReader reader = null;
            try
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "select * from yanceng where id = '" + layerKey + "'";
                reader = cmd.ExecuteReader();
                return reader.HasRows;
            }
            catch (Exception e)
            {
                App.logger.Error(Resources.OpenDataBaseError, e);
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                conn.Close();
            }
        }

        public static bool SaveLayer(LayerBaseParams layer, string uuid, bool cover, string wellName, bool wellExist)
        {
            if(layer == null || uuid == null || wellName == null)
            {
                App.logger.Error(Resources.SaveToDBInfoError);
                return false;
            }

            SQLiteConnection conn = GetSQLiteConnection();
            if(conn == null)
            {
                App.logger.Error(Resources.OpenDataBaseError);
                return false;
            }
            var trans = conn.BeginTransaction();

            // 若wellName不存在，则well表新增一项
            SQLiteCommand cmd1 = null;
            if (!wellExist)
            {
                cmd1 = new SQLiteCommand(conn);
                cmd1.Transaction = trans;
                cmd1.CommandText = "insert into well values('" + wellName + "')";
            }


            // 若需要覆盖旧记录，则先删除旧记录
            SQLiteCommand cmd2 = null;
            if (cover)
            {
                cmd2 = new SQLiteCommand(conn);
                cmd2.Transaction = trans;
                cmd2.CommandText = "delete from yanceng where id = '" + uuid + "'";
            }

            // 新增岩层记录
            SQLiteCommand cmd3 = new SQLiteCommand(conn);
            cmd3.Transaction = trans;
            cmd3.CommandText = "insert into yanceng (id, wellName, yanXing, leiJiShenDu, juLiMeiShenDu, cengHou, ziRanMiDu, " +
            "bianXingMoLiang, kangLaQiangDu, kangYaQiangDu, tanXingMoLiang, boSonBi, neiMoCaJiao, nianJuLi, f, q0, q1, q2, miaoShu) " +
            "values(@id, @wellName, @yanXing, @leiJiShenDu, @juLiMeiShenDu, @cengHou, @ziRanMiDu, @bianXingMoLiang, @kangLaQiangDu, " +
            "@kangYaQiangDu, @tanXingMoLiang, @boSonBi, @neiMoCaJiao, @nianJuLi, @f, @q0, @q1, @q2, @miaoShu)";

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
                new SQLiteParameter("@miaoShu", layer.MiaoShu == null ? "" : layer.MiaoShu)
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
                trans.Rollback();
                App.logger.Error(Resources.SaveToDBError, e);
            }
            finally
            {
                conn.Close();
            }
            
            return success;
        }


        public static List<String> getAllWellName()
        {
            List<String> wells = new List<string>();

            //查询well表
            SQLiteConnection conn = GetSQLiteConnection();
            if (conn == null)
            {
                App.logger.Error(Resources.OpenDataBaseError);
                return wells;
            }

            SQLiteDataReader reader = null;
            try
            {
                string sql = "select wellName from well";
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    wells.Add(reader.GetString(0));
                }
                return wells;
            }
            catch(Exception e)
            {
                App.logger.Error(Resources.QueryWellNamesError, e);
                return wells;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (conn != null)
                    conn.Close();
            }
        }

        /**
         * wellName为null则查询所有矿井的数据。
         **/
        public static ObservableCollection<LayerBaseParamsViewModel> getDBLayers(string yanXing, string wellName)
        {
            ObservableCollection<LayerBaseParamsViewModel> existedLayers = new ObservableCollection<LayerBaseParamsViewModel>();

            if(yanXing == null)
            {
                App.logger.Error(Resources.QueryDBLayersInfoError);
                return existedLayers;
            }

            //查询well表
            SQLiteConnection conn = GetSQLiteConnection();
            if (conn == null)
            {
                App.logger.Error(Resources.OpenDataBaseError);
                return existedLayers;
            }

            SQLiteDataReader reader = null;
            try
            {
                string sql = "select * from yanCeng where yanXing = '" + yanXing + "'";
                if (wellName != null)
                    sql += " and wellName = '" + wellName + "'";

                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                reader = cmd.ExecuteReader();

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
            }
            catch(Exception e)
            {
                App.logger.Error(Resources.QueryDBLayersError, e);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                conn.Close();
            }

            return existedLayers;
        }
    }
}
