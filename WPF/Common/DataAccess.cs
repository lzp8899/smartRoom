using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
#pragma warning disable 0162

namespace Web
{
    /// <summary>
    /// 数据库访问类
    /// </summary>
    public class DataAccess
    {
        #region 公共方法

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public static string connectionString = ConfigurationManager.ConnectionStrings["CSDB"].ConnectionString;

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="strSql">SQL</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns>影响行数</returns>
        public static int ExecuteSql(string strSql, params MySqlParameter[] parameters)
        {
            NLogManager.Logger.Debug("ExecuteSql开始执行SQL语句：{0}", strSql);
            Stopwatch watch = Stopwatch.StartNew();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, strSql, parameters);
                        int rows = cmd.ExecuteNonQuery();
                        if (rows <= 0)
                        {
                            NLogManager.Logger.Warn("执行SQL语句影响0行:{0}.", strSql);
                        }
                        cmd.Parameters.Clear();

                        NLogManager.Logger.Debug("ExecuteSql完成执行SQL语句 耗时：{0} sql:{1}", watch.ElapsedMilliseconds, strSql);
                        watch.Stop();
                        watch.Reset();

                        return rows;
                    }
                    catch (Exception ex)
                    {
                        NLogManager.Logger.Error("执行SQL语句{0}出错。{1}", strSql, ex.ToString());
                        foreach (MySqlParameter parameter in parameters)
                        {
                            NLogManager.Logger.Error("参数:{0}", parameter.Value);
                        }
                        return 0;
                    }
                }
            }
        }

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="readerCallback">回数返回，回调 </param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static bool ExecuteSql(string strSql, Action<IDataReader> readerCallback = null)
        {
            NLogManager.Logger.Debug("ExecuteSqlcall 开始执行SQL语句：{0}", strSql);
            Stopwatch watch = Stopwatch.StartNew();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(strSql, connection))
                {
                    try
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 120;
                        connection.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        if (readerCallback != null)
                        {
                            readerCallback(reader);
                        }
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        NLogManager.Logger.Error("执行SQL语句{0}出错。{1}", strSql, ex.ToString());
                        return false;
                    }
                }
            }
            NLogManager.Logger.Debug("ExecuteSqlcall 完成执行SQL语句 耗时:{0} sql：{1}", watch.ElapsedMilliseconds, strSql);

            watch.Reset();

            return true;
        }

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static object ExecuteScalar<T>(string strSql)
        {
            T obj = default(T);
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(strSql, connection))
                {
                    try
                    {
                        cmd.CommandType = CommandType.Text;
                        connection.Open();
                        obj = (T)cmd.ExecuteScalar();
                        connection.Close();
                        return obj;
                    }
                    catch (Exception ex)
                    {
                        NLogManager.Logger.Error("执行SQL语句{0}出错。{1}", strSql, ex.ToString());
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="strSql">SQL</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns>数据集</returns>
        public static DataSet Query(string strSql, params MySqlParameter[] parameters)
        {
            if (!strSql.Contains("from planCamera") && !strSql.Contains("from planGroup"))
            {
                NLogManager.Logger.Debug("Query开始执行SQL语句：{0}", strSql);
            }

            Stopwatch watch = Stopwatch.StartNew();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandTimeout = 120;
                PrepareCommand(cmd, connection, null, strSql, parameters);
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (Exception ex)
                    {
                        NLogManager.Logger.Error("查询SQL语句{0}出错。{1}", strSql, ex.ToString());
                    }
                    if (!strSql.Contains("from planCamera") && !strSql.Contains("from planGroup"))
                    {
                        NLogManager.Logger.Debug("Query完成执行SQL语句 耗时:{0} sql：{1}", watch.ElapsedMilliseconds, strSql);
                    }

                    watch.Reset();

                    return ds;
                }
            }
        }

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <param name="strSql">SQL</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns>单行数据</returns>
        public static object GetSingle(string strSql, params MySqlParameter[] parameters)
        {
            NLogManager.Logger.Debug("GetSingle执行SQL语句：{0}", strSql);

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(strSql, connection))
                {
                    PrepareCommand(cmd, connection, null, strSql, parameters);
                    try
                    {
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (Exception ex)
                    {
                        NLogManager.Logger.Error("获取数据SQL语句{0}出错。{1}", strSql, ex.ToString());
                        connection.Close();
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <param name="strSql">SQL</param>
        /// <returns>单行数据</returns>
        public static object GetSingle(string strSql)
        {
            NLogManager.Logger.Debug("GetSingle2执行SQL语句：{0}", strSql);

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(strSql, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (Exception ex)
                    {
                        NLogManager.Logger.Error("获取数据SQL语句{0}出错。{1}", strSql, ex.ToString());
                        connection.Close();
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="commandInfos">命令</param>
        /// <returns>是否执行成功</returns>
        public static bool ExecuteSqlTranWithIndentity(System.Collections.Generic.List<CommandInfo> commandInfos)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlTransaction trans = conn.BeginTransaction())
                {
                    MySqlCommand cmd = new MySqlCommand();
                    try
                    {
                        int indentity = 0;
                        //循环
                        foreach (CommandInfo myDE in commandInfos)
                        {
                            string cmdText = myDE.CommandText;
                            MySqlParameter[] cmdParms = (MySqlParameter[])myDE.Parameters;
                            foreach (MySqlParameter q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.InputOutput)
                                {
                                    q.Value = indentity;
                                }
                            }
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            foreach (MySqlParameter q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.Output)
                                {
                                    indentity = Convert.ToInt32(q.Value);
                                }
                            }
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                        return true;
                    }
                    catch
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
        }

        #endregion

        #region 私有方法

        private static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, string cmdText, MySqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
            {
                cmd.Transaction = trans;
            }

            cmd.CommandType = CommandType.Text;
            if (cmdParms != null)
            {
                foreach (MySqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) && (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(parameter);
                }
            }
        }

        #endregion
    }
}

/// <summary>
/// 数据命令行对象
/// </summary>
public class CommandInfo
{
    /// <summary>
    /// 命令文本
    /// </summary>
    public string CommandText;

    /// <summary>
    /// 命令参数
    /// </summary>
    public MySqlParameter[] Parameters;

    /// <summary>
    /// 构造函数
    /// </summary>
    public CommandInfo()
    {
        CommandText = "";
        Parameters = new MySqlParameter[0];
    }
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sqlText">命令文本</param>
    /// <param name="para">命令参数</param>
    public CommandInfo(string sqlText, MySqlParameter[] para)
    {
        this.CommandText = sqlText;
        this.Parameters = para;
    }
}
