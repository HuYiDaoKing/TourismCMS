using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TourismCMS.Helper
{
    /// <summary>
    /// 数据库连接类和Connection对象
    /// </summary>
    public class DBHelper
    {
        //数据库连接字符串
        private readonly string connString = ConfigurationManager.ConnectionStrings["connStr"].ToString();

        //定义数据库连接对象
        private SqlConnection con;
        //定义数据命令对象
        public SqlCommand cmd;

        /// <summary>
        /// 初始化连接对象和命令对象
        /// </summary>
        public DBHelper()
        {
            con = new SqlConnection(connString);
            cmd = con.CreateCommand();
        }

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        public void ConnOpen()
        {
            con.Open();
        }

        /// <summary>
        /// 执行增,删,改SQL语句的方法
        /// </summary>
        /// <param name="sql">传入SQL语句作为参数</param>
        /// <returns>返回int类型值</returns>
        public int ExecuteNonQuery(string sql, params SqlParameter[] pamt)
        {
            con.Open();
            cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddRange(pamt);
            int result = Convert.ToInt32(cmd.ExecuteNonQuery());
            CloseDataBase();
            return result;
        }

        /// <summary>
        /// 执行查询单个值SQL语句的方法返回int型的数据
        /// </summary>
        /// <param name="sql">传入SQL语句作为参数</param>
        /// <returns>返回int类型值</returns>
        public int ExecuteScalarBySql(string sql, params SqlParameter[] pamt)
        {
            ConnOpen();
            cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddRange(pamt);
            int result = Convert.ToInt32(cmd.ExecuteScalar());
            CloseDataBase();
            return result;
        }

        /// <summary>
        /// 执行查询单个值SQL语句的方法返回string型的数据
        /// </summary>
        /// <param name="sql">传入SQL语句作为参数</param>
        /// <returns>返回string类型值</returns>
        public string NewExecuteScalar(string sql, params SqlParameter[] pamt)
        {
            cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddRange(pamt);
            string result = cmd.ExecuteScalar().ToString();
            return result;
        }

        /// <summary>
        /// 执行返回DataSet  SQL语句的方法
        /// </summary>
        /// <param name="sql">参数为SQL语句</param>
        /// <returns></returns>
        public DataSet ExecuteQuery(string sql)
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }


        /// <summary>
        /// 执行返回DataTable  SQL语句的方法
        /// </summary>
        /// <param name="sql">参数为SQL语句</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string sql, params SqlParameter[] pamt)
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            da.SelectCommand.Parameters.AddRange(pamt);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds.Tables[0];
        }

        /// <summary>
        /// 执行返回DataView  SQL语句的方法
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="filter">筛选</param>
        /// <param name="sort">排序</param>
        /// <returns></returns>
        public DataView GetDataView(string sql, string filter, string sort)
        {
            DataView dv = new DataView(ExecuteDataTable(sql));
            if (filter != "")
            {
                dv.RowFilter = filter;
            }
            if (sort != "")
            {
                dv.Sort = sort;
            }
            return dv;
        }

        /// <summary>
        /// 返回值为SqlDataReader对象的方法
        /// </summary>
        /// <param name="sql">参数为SQL语句</param>
        /// <returns></returns>
        public SqlDataReader GetDataReaderbySql(string sql, params SqlParameter[] pamt)
        {
            try
            {
                cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddRange(pamt);
                return cmd.ExecuteReader();
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// 关闭数据库的方法
        /// </summary>
        /// <returns>判断是否关闭</returns>
        public bool CloseDataBase()
        {
            con.Close();
            Dispose();
            return true;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(true);
        }

        protected virtual void Dispose(bool bDispose)
        {
            if (!bDispose)
                return;
            if (con.State != ConnectionState.Closed)
            {
                con.Close();
                con.Dispose();
                cmd = null;
                con = null;
            }
        }
    }
}