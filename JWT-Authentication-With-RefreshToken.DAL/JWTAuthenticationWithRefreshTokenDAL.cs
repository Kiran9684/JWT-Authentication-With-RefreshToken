using JWT_Authentication_With_RefreshToken.EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace JWT_Authentication_With_RefreshToken.DAL
{
    public class JWTAuthenticationWithRefreshTokenDAL : IJWTAuthenticationWithRefreshTokenDAL
    {
        public async Task<string> AuthenticateUserDAL(string userId, string password, DBConfig dBConfig)
        {
            #region LocalVariables
            SQLHelper sqlHelper = null;
            int spExecutionStatusFlag = 0;
            string spExecutionMessage = string.Empty;
            string spResponse = string.Empty;
            #endregion
            try
            {
                SqlParameter[] Params = new SqlParameter[5];

                Params[0] = new SqlParameter("@UserId", userId);
                Params[0].Direction = ParameterDirection.Input;
                Params[0].SqlDbType = SqlDbType.VarChar;
                Params[0].Size = 50;


                Params[1] = new SqlParameter("@Password", password);
                Params[1].Direction = ParameterDirection.Input;
                Params[1].SqlDbType = SqlDbType.VarChar;
                Params[1].Size = 20;

                Params[2] = new SqlParameter("@errorNo", System.Data.SqlDbType.Int);
                Params[2].Direction = ParameterDirection.Output;

                Params[3] = new SqlParameter("@errorMessage", System.Data.SqlDbType.VarChar);
                Params[3].Direction = ParameterDirection.Output;
                Params[3].Size = -1;

                Params[4] = new SqlParameter("@spResponse", System.Data.SqlDbType.VarChar);
                Params[4].Direction = ParameterDirection.Output;
                Params[4].Size = -1;

                sqlHelper = SQLHelper.GetSQLHelperInstance(dBConfig.ConnectionString);
                await sqlHelper.ExecuteNonQueryAsyncHelper(dBConfig.storedProc, Params);

               //as  dBConfig.Params is a array which is ref type it will be holding memory address, so the address val is passed to variable accepting it's value in SQL helper class. so changes to the array in SQL helper class can be accessed here as well as both variables have same memory adress to the array.

                spExecutionStatusFlag = Convert.ToInt32(Params[2].Value);
                spExecutionMessage = Params[3].Value.ToString();
                spResponse = Params[4].Value.ToString();

            }
            catch (Exception ex)
            {
                spResponse = string.Empty;
                throw;
            }
            return spResponse;
        }

        public async Task<string> SaveRefreshTokenDAL(string refreshTokenJson, DBConfig dbConfig)
        {
            #region Local Variables
            string spResponse = string.Empty;
            int spExecutionStatusFlag = 0;
            string spExecutionMessage = string.Empty;
            SQLHelper helperInstance = null;
            #endregion
            try
            {

                SqlParameter[] Params = new SqlParameter[4];

                Params[0] = new SqlParameter("@refreshTokenJson", refreshTokenJson);
                Params[0].Direction = System.Data.ParameterDirection.Input;
                Params[0].SqlDbType = System.Data.SqlDbType.NVarChar;
                Params[0].Size = 500;

                Params[1] = new SqlParameter("@errorNo", System.Data.SqlDbType.Int);
                Params[1].Direction = System.Data.ParameterDirection.Output;

                Params[2] = new SqlParameter("@errorMessage", System.Data.SqlDbType.VarChar, -1);
                Params[2].Direction = System.Data.ParameterDirection.Output;

                Params[3] = new SqlParameter("@spResponse", System.Data.SqlDbType.VarChar, 500);
                Params[3].Direction = System.Data.ParameterDirection.Output;

                helperInstance = SQLHelper.GetSQLHelperInstance(dbConfig.ConnectionString);
                await helperInstance.ExecuteNonQueryAsyncHelper(dbConfig.storedProc,Params);

                spExecutionStatusFlag = Convert.ToInt32(Params[1].Value);
                spExecutionMessage = Params[2].Value.ToString();
                spResponse = Params[3].Value.ToString(); //this will be empty if SQL exception occurs at sp level.

            }
            catch (Exception ex)
            {
                //log the ex detail
                spResponse = string.Empty;
                throw;
            }
            return spResponse;
        }
    }
}
