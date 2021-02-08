using log4net;
using QuestionAnswer.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace QuestionAnswer.Repository.Database
{
    public class DBCommand : DBConnection
    {
        private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DBCommand() : base("ConnectionStrings:DBConnection") { }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlQueryOrSprocName"></param>
        /// <param name="sqlCommandType"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int SQLExecuteNonQuery(string sqlQueryOrSprocName, System.Data.CommandType sqlCommandType, List<SqlParameter> param)
        {
            int iCheck = -1;
            using (SqlConnection conn = new SqlConnection(DBConnectionString))
            {
                try
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlQueryOrSprocName, conn))
                    {
                        cmd.CommandType = sqlCommandType;
                        if (param.Count != 0)
                        {
                            cmd.Parameters.AddRange(param.ToArray());
                        }

                        cmd.ExecuteNonQuery();
                        iCheck = 1;
                    }
                }
                catch (Exception ex)
                {                
                    _logger.Error(ex);
                    iCheck = -1;
                }
            }
            return iCheck; 
        }

        /// <summary>
        /// Returns result set in DataTable object.
        /// </summary>
        /// <param name="sqlQueryOrSprocName">Name of the SQL query or sproc.</param>
        /// <param name="sqlCommandType">Type of the SQL command.</param>
        /// <param name="param">The parameter.</param>
        /// <returns></returns>
        public DataTable SQLExecuteReaderDataTable(string sqlQueryOrSprocName, System.Data.CommandType sqlCommandType, List<SqlParameter> param)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DBConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlQueryOrSprocName, conn))
                    {
                        cmd.CommandType = sqlCommandType;
                        if (param.Count != 0)
                        {
                            cmd.Parameters.AddRange(param.ToArray());
                        }
                        DataTable dt = new DataTable();
                        dt.Load(cmd.ExecuteReader());
                        if (dt.Rows.Count > 0)
                        {
                            return dt;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return new DataTable();
        }

        /// <summary>
        /// Returns result set in DataSet object.
        /// </summary>
        /// <param name="sqlQueryOrSprocName">Name of the SQL query or sproc.</param>
        /// <param name="sqlCommandType">Type of the SQL command.</param>
        /// <param name="param">The parameter.</param>
        /// <returns>DataSet object</returns>
        public DataSet SQLExecuteReaderDataSet(string sqlQueryOrSprocName, System.Data.CommandType sqlCommandType, List<SqlParameter> param)
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection conn = new SqlConnection(DBConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlQueryOrSprocName, conn))
                    {
                        cmd.CommandType = sqlCommandType;
                        if (param.Count != 0)
                        {
                            cmd.Parameters.AddRange(param.ToArray());
                        }
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;

                        da.Fill(ds);
                        if (ds.Tables.Count > 0)
                        {
                            return ds;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return ds;
        }

        /// <summary>
        /// Returns the first column of the first row in the result set returned by the query.
        /// </summary>
        /// <param name="sqlQueryOrSprocName">Name of the SQL query or sproc.</param>
        /// <param name="sqlCommandType">Type of the SQL command.</param>
        /// <param name="param">The parameter.</param>
        /// <returns></returns>
        public object SQLExecuteScalar(string sqlQueryOrSprocName, System.Data.CommandType sqlCommandType, List<SqlParameter> param)
        {
            using (SqlConnection conn = new SqlConnection(DBConnectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlQueryOrSprocName, conn))
                    {
                        cmd.CommandType = sqlCommandType;
                        if (param.Count != 0)
                        {
                            cmd.Parameters.AddRange(param.ToArray());
                        }
                        var scalar = cmd.ExecuteScalar();
                        if (scalar != null)
                        {
                            return scalar;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }

            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlQueryOrSprocName"></param>
        /// <param name="sqlCommandType"></param>
        /// <param name="param"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public List<T> GetList<T>(string sqlQueryOrSprocName, System.Data.CommandType sqlCommandType, List<SqlParameter> param, string[] columns = null)
        {
            List<T> returnedList = null;
            using (SqlConnection conn = new SqlConnection(DBConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sqlQueryOrSprocName, conn))
                {
                    cmd.CommandType = sqlCommandType;
                    if (param.Count != 0)
                    {
                        cmd.Parameters.AddRange(param.ToArray());
                    }
                    SqlDataReader rs = null;
                    try
                    {

                        rs = cmd.ExecuteReader();
                        returnedList = ReaderToList<T>(rs, columns);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex);
                        throw new Exception(UtilFunctions.GetDetailsException(ex));
                    }
                    finally
                    {
                        if (rs != null && rs.IsClosed == false)
                        {
                            rs.Close();
                            rs = null;
                        }
                    }
                }
            }
            return returnedList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="columns"></param>
        /// <param name="closeAfterRead"></param>
        /// <returns></returns>
        protected List<T> ReaderToList<T>(SqlDataReader reader, string[] columns = null, bool closeAfterRead = true)
        {
            List<T> retList = new List<T>();
            T obj = default(T);
            PropertyInfo p = null;
            int i = 0;
            int cnt = 0;
            string colname = "";
            try
            {
                if (!UtilFunctions.IsNull(reader))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            obj = (T)Assembly.GetAssembly(typeof(T)).CreateInstance(string.Format("{0}.{1}", typeof(T).Namespace, typeof(T).Name));
                            if (UtilFunctions.IsNull(columns) == false && columns.Length > 0)
                            {
                                foreach (string column in columns)
                                {
                                    colname = column;
                                    p = obj.GetType().GetProperty(column, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                    if (p != null)
                                    {
                                        if ((reader[column] == DBNull.Value))
                                        {
                                            if (p.GetGetMethod().ReturnParameter.ParameterType.Name == "String")
                                                p.SetValue(obj, "", null);
                                            else
                                                p.SetValue(obj, null, null);
                                        }
                                        else if (reader[column] is bool)
                                        {
                                            if (UtilFunctions.ObjectToBoolean(reader[column]) == false)
                                                p.SetValue(obj, 0, null);
                                            else
                                                p.SetValue(obj, 1, null);
                                        }
                                        else
                                            p.SetValue(obj, reader[column], null/* TODO Change to default(_) if this is not a reference type */);
                                    }
                                }
                            }
                            else
                            {
                                cnt = reader.FieldCount;
                                for (i = 0; i <= cnt - 1; i++)
                                {
                                    // WiteLogFormat(",{0}", reader.GetName(i))
                                    p = obj.GetType().GetProperty(reader.GetName(i), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                    colname = reader.GetName(i);
                                    if (p != null)
                                    {
                                        if ((reader[i] == DBNull.Value))
                                        {
                                            if (p.GetGetMethod().ReturnParameter.ParameterType.Name == "String")
                                                p.SetValue(obj, "", null);
                                            else
                                                p.SetValue(obj, null, null);
                                        }
                                        else if (reader[i] is bool)
                                        {
                                            if (UtilFunctions.ObjectToBoolean(reader[i]) == false)
                                                p.SetValue(obj, 0, null);
                                            else
                                                p.SetValue(obj, 1, null);
                                        }
                                        else
                                            p.SetValue(obj, reader[i], null/* TODO Change to default(_) if this is not a reference type */);
                                    }
                                }
                            }
                            //if (retList.Count == 3827)
                            // string test = "";
                            retList.Add(obj);
                        }
                    }
                    if (closeAfterRead == true)
                    {
                        reader.Close();
                        reader = null/* TODO Change to default(_) if this is not a reference type */;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error - ReaderToList -Table: " + typeof(T).Name + " At column: " + colname + "; " + ex.ToString());
                throw new Exception(UtilFunctions.GetDetailsException(ex));
                //if (Strings.InStr(ex.ToString(), "Thread was being aborted") == 0)
            }
            finally
            {
                p = null;
                obj = default(T);
            }
            return retList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlQueryOrSprocName"></param>
        /// <param name="sqlCommandType"></param>
        /// <param name="param"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public T GetRow<T>(string sqlQueryOrSprocName, System.Data.CommandType sqlCommandType, List<SqlParameter> param, string[] columns = null)
        {
            T retRow = default(T);
            using (SqlConnection conn = new SqlConnection(DBConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sqlQueryOrSprocName, conn))
                {
                    cmd.CommandType = sqlCommandType;
                    if (param.Count != 0)
                    {
                        cmd.Parameters.AddRange(param.ToArray());
                    }
                    SqlDataReader rs = null/* TODO Change to default(_) if this is not a reference type */;
                    try
                    {

                        rs = cmd.ExecuteReader();
                        retRow = ReaderToObject<T>(rs, columns);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex);
                        throw new Exception(UtilFunctions.GetDetailsException(ex));
                    }
                    finally
                    {
                        if (rs != null && rs.IsClosed == false)
                        {
                            rs.Close();
                            rs = null/* TODO Change to default(_) if this is not a reference type */;
                        }
                    }
                }
            }

            return retRow;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="columns"></param>
        /// <param name="closeAfterRead"></param>
        /// <returns></returns>
        protected T ReaderToObject<T>(SqlDataReader reader, string[] columns = null, bool closeAfterRead = true)
        {
            T obj = default(T);
            PropertyInfo p = null;
            int i = 0;
            int cnt = 0;
            string colname = "";
            try
            {
                if (!UtilFunctions.IsNull(reader))
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            obj = (T)Assembly.GetAssembly(typeof(T)).CreateInstance(typeof(T).Namespace + "." + typeof(T).Name);
                            if (UtilFunctions.IsNull(columns) == false && columns.Length > 0)
                            {
                                foreach (string column in columns)
                                {
                                    //column = Functions.Trim(column);
                                    colname = column;
                                    p = obj.GetType().GetProperty(column, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                    if (p != null)
                                    {
                                        if ((reader[column] == DBNull.Value))
                                        {
                                            if (p.GetGetMethod().ReturnParameter.ParameterType.Name == "String")
                                                p.SetValue(obj, "", null);
                                            else
                                                p.SetValue(obj, null, null);
                                        }
                                        else if (reader[column] is bool)
                                        {
                                            if (UtilFunctions.ObjectToBoolean(reader[column]) == false)
                                                p.SetValue(obj, 0, null);
                                            else
                                                p.SetValue(obj, 1, null);
                                        }
                                        else
                                            p.SetValue(obj, reader[column], null/* TODO Change to default(_) if this is not a reference type */);
                                    }
                                }
                            }
                            else
                            {
                                cnt = reader.FieldCount;
                                for (i = 0; i <= cnt - 1; i++)
                                {
                                    colname = reader.GetName(i);
                                    p = obj.GetType().GetProperty(reader.GetName(i), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                    if (p != null)
                                    {
                                        if ((reader[i] == DBNull.Value))
                                        {
                                            if (p.GetGetMethod().ReturnParameter.ParameterType.Name == "String")
                                                p.SetValue(obj, "", null);
                                            else
                                                p.SetValue(obj, null, null);
                                        }
                                        else if (reader[i] is bool)
                                        {
                                            if (UtilFunctions.ObjectToBoolean(reader[i]) == false)
                                                p.SetValue(obj, 0, null);
                                            else
                                                p.SetValue(obj, 1, null);
                                        }
                                        else
                                            p.SetValue(obj, reader[i], null/* TODO Change to default(_) if this is not a reference type */);
                                    }
                                }
                            }
                        }
                    }
                    if (closeAfterRead == true)
                        reader.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw new Exception(UtilFunctions.GetDetailsException(ex));
            }

            return obj;
        }
    }
}