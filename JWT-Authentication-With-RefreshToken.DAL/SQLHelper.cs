using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace JWT_Authentication_With_RefreshToken.DAL
{
    internal class SQLHelper
    {
        private string _connectionString;
        private static SQLHelper sqlHelper;
        private SQLHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static SQLHelper GetSQLHelperInstance(string connectionString)
        {
            if(sqlHelper == null)
            {
                sqlHelper = new SQLHelper(connectionString);
            }
            return sqlHelper;
        }

        public async Task<int> ExecuteNonQueryAsyncHelper(string spName, SqlParameter[] spParams)
        {
            #region LocalVariables
            SqlConnection con = null;
            int flag = 0;
            #endregion
            try
            {
                using (con = new SqlConnection(this._connectionString))
                {
                    SqlCommand cmd = new SqlCommand(spName, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    foreach(SqlParameter param in spParams)
                    {
                        cmd.Parameters.Add(param);
                    }

                    con.Open();
                    flag = await cmd.ExecuteNonQueryAsync();
                    con.Close();
                }
            }
            catch(Exception ex) 
            {
                //log the exception
                throw; 
            }
            finally
            {
                if(con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
            return flag;
        }
    }
}
