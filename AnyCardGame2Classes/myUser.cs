using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using AnyCardGame2Classes.Data_;
namespace AnyCardGame2Classes
{
    public partial class myUser
    {
        private int myUserID;
        public int UserID
        {
            get { return myUserID; }
            set { myUserID = value; }
        }
        private string mySessionID;
        public string SessionID
        {
            get { return mySessionID; }
            set { mySessionID = value; }
        }
        private string myUserName;
        public string UserName
        {
            get { return myUserName; }
            set { myUserName = value; }
        }
        private string myPassword;
        public string Password
        {
            get { return myPassword; }
            set { myPassword = value; }
        }
        private List<myChatLine> myChatLines;
        public List<myChatLine> ChatLines
        {
            get { if (myChatLines == null) myChatLines = myChatLine.GetChatLineByUser(this); return myChatLines; }
            set { myChatLines = value; }
        }
        private List<myGameRoom> myGameRooms;
        public List<myGameRoom> GameRooms
        {
            get { if (myGameRooms == null) myGameRooms = myGameRoom.GetGameRoomByUser(this); return myGameRooms; }
            set { myGameRooms = value; }
        }
        public myUser()
        {
            InitVars();
        }
        public myUser(int myUserID_)
        {
            InitVars();
            GetUserByUserID(myUserID_);
        }
        public myUser(DataRow row)
        {
            InitVars();
            UpdateUser(row);
        }
        private void InitVars()
        {
            myUserID = 0;
            mySessionID = "";
            myUserName = "";
            myPassword = "";
        }
        public bool InsertData()
        {
            myUser n = new myUser();
            n.GetUserBySessionID(mySessionID);
            if (n.UserID != 0) return false;
            n = new myUser();
            n.GetUserByUserName(myUserName);
            if (n.UserID != 0) return false;
            myUserData data = new myUserData();
            List<SqlParameter> tempAL = new List<SqlParameter>();
            SqlParameter tempP; DataSet tempDS;
            tempP = new SqlParameter();
            tempP.ParameterName = "@SessionID";
            tempP.Size = 200;
            tempP.SqlDbType = SqlDbType.VarChar;
            tempP.Value = mySessionID;
            tempP.Direction = ParameterDirection.Input;
            tempAL.Add(tempP);
            tempP = new SqlParameter();
            tempP.ParameterName = "@UserName";
            tempP.Size = 200;
            tempP.SqlDbType = SqlDbType.VarChar;
            tempP.Value = myUserName;
            tempP.Direction = ParameterDirection.Input;
            tempAL.Add(tempP);
            tempP = new SqlParameter();
            tempP.ParameterName = "@Password";
            tempP.Size = 200;
            tempP.SqlDbType = SqlDbType.VarChar;
            tempP.Value = myPassword;
            tempP.Direction = ParameterDirection.Input;
            tempAL.Add(tempP);
            tempDS = data.RunProcedure("csp_InsertUser", tempAL);
            myUserID = int.Parse(tempDS.Tables[0].Rows[0][0].ToString());
            tempDS.Dispose(); return true;
        }
        public bool UpdateData()
        {
            myUser n = new myUser();
            n.GetUserBySessionID(mySessionID);
            if (n.UserID != 0 && n.UserID != myUserID) { return false; }
            n = new myUser();
            n.GetUserByUserName(myUserName);
            if (n.UserID != 0 && n.UserID != myUserID) { return false; }
            myUserData data = new myUserData();
            List<SqlParameter> tempAL = new List<SqlParameter>();
            SqlParameter tempP;
            tempP = new SqlParameter();
            tempP.ParameterName = "@UserID";
            tempP.Size = 4;
            tempP.SqlDbType = SqlDbType.Int;
            tempP.Value = myUserID;
            tempP.Direction = ParameterDirection.Input;
            tempAL.Add(tempP);
            tempP = new SqlParameter();
            tempP.ParameterName = "@SessionID";
            tempP.Size = 200;
            tempP.SqlDbType = SqlDbType.VarChar;
            tempP.Value = mySessionID;
            tempP.Direction = ParameterDirection.Input;
            tempAL.Add(tempP);
            tempP = new SqlParameter();
            tempP.ParameterName = "@UserName";
            tempP.Size = 200;
            tempP.SqlDbType = SqlDbType.VarChar;
            tempP.Value = myUserName;
            tempP.Direction = ParameterDirection.Input;
            tempAL.Add(tempP);
            tempP = new SqlParameter();
            tempP.ParameterName = "@Password";
            tempP.Size = 200;
            tempP.SqlDbType = SqlDbType.VarChar;
            tempP.Value = myPassword;
            tempP.Direction = ParameterDirection.Input;
            tempAL.Add(tempP);
            data.RunProcedure("csp_UpdateUser", tempAL);
            return true;
        }
        public void DeleteUser() { new myUserData().DeleteUserByUserID(myUserID); }
        public static List<myUser> GetAllUser()
        {
            myUserData tempADO = new myUserData();
            List<myUser> list = new List<myUser>();
            DataSet tempDS = tempADO.GetAllUser();
            if (tempADO.HasData)
            {
                foreach (DataRow row in tempDS.Tables[0].Rows)
                    list.Add(new myUser(row));
                return list;
            }
            return new List<myUser>();
        }
        public bool GetUserByUserID(int val)
        {
            myUserData tempADO = new myUserData();
            DataSet tempDS = tempADO.GetUserByUserID(val);
            if (tempADO.HasData)
            {
                UpdateUser(tempDS.Tables[0].Rows[0]);
                return true;
            } return false;
        }
        public bool GetUserBySessionID(string val)
        {
            myUserData tempADO = new myUserData();
            DataSet tempDS = tempADO.GetUserBySessionID(val);
            if (tempADO.HasData)
            {
                UpdateUser(tempDS.Tables[0].Rows[0]);
                return true;
            } return false;
        }
        public bool GetUserByUserName(string val)
        {
            myUserData tempADO = new myUserData();
            DataSet tempDS = tempADO.GetUserByUserName(val);
            if (tempADO.HasData)
            {
                UpdateUser(tempDS.Tables[0].Rows[0]);
                return true;
            } return false;
        }
        public static List<myUser> GetUserByGameRoom(myGameRoom val)
        {
            myUserData tempADO = new myUserData();
            List<myUser> list = new List<myUser>();
            DataSet tempDS = tempADO.GetUserByGameRoomID(val.GameRoomID);
            if (tempADO.HasData)
            {
                foreach (DataRow row in tempDS.Tables[0].Rows)
                    list.Add(new myUser(row));
                return list;
            }
            return new List<myUser>();
        }
        private void UpdateUser(DataRow DR)
        {
            myUserID = int.Parse(DR["UserID"].ToString());
            mySessionID = DR["SessionID"].ToString();
            myUserName = DR["UserName"].ToString();
            myPassword = DR["Password"].ToString();
        }
        public void UpdateGameRoom(int val)
        {
            myUserData tempADO = new myUserData();
            tempADO.UpdateUserByGameRoomID(val, UserID);
        }
        public void DeleteGameRoom(int val)
        {
            myUserData tempADO = new myUserData();
            tempADO.DeleteUserByGameRoomID(val, UserID);
        }
    }
}
namespace AnyCardGame2Classes.Data_
{
    public partial class myUserData : myData
    {
        public DataSet GetAllUser()
        {
            m_ado.ProcName = "csp_GetAllUser";
            m_DS = m_ado.GetDS();
            m_HasData = m_ado.HasData;
            return m_DS;
        }
        public DataSet GetUserByUserID(int val)
        {
            m_ado.ProcName = "csp_GetUserByUserID";
            m_ado.AddParam("@UserID", SqlDbType.Int, 4, ParameterDirection.Input, val.ToString());
            m_DS = m_ado.GetDS();
            m_HasData = m_ado.HasData;
            return m_DS;
        }
        public DataSet GetUserBySessionID(string val)
        {
            m_ado.ProcName = "csp_GetUserBySessionID";
            m_ado.AddParam("@SessionID", SqlDbType.VarChar, 200, ParameterDirection.Input, val.ToString());
            m_DS = m_ado.GetDS();
            m_HasData = m_ado.HasData;
            return m_DS;
        }
        public DataSet GetUserByUserName(string val)
        {
            m_ado.ProcName = "csp_GetUserByUserName";
            m_ado.AddParam("@UserName", SqlDbType.VarChar, 200, ParameterDirection.Input, val.ToString());
            m_DS = m_ado.GetDS();
            m_HasData = m_ado.HasData;
            return m_DS;
        }
        public void DeleteUserByUserID(int id)
        {
            m_ado.AddParam("@UserID", SqlDbType.Int, 4, ParameterDirection.Input, id.ToString());
            m_ado.ProcName = "csp_DeleteUser";
            m_DS = m_ado.GetDS();
        }
        public void UpdateUserByGameRoomID(int id1, int id2)
        {
            m_ado.ProcName = "csp_UpdateUserByGameRoomID";
            m_ado.AddParam("@GameRoomID", SqlDbType.Int, 4, ParameterDirection.Input, id1.ToString());
            m_ado.AddParam("@UserID", SqlDbType.Int, 4, ParameterDirection.Input, id2.ToString());
            m_DS = m_ado.GetDS();
        }
        public void DeleteUserByGameRoomID(int id1, int id2)
        {
            m_ado.ProcName = "csp_DeleteUserByGameRoomID";
            m_ado.AddParam("@GameRoomID", SqlDbType.Int, 4, ParameterDirection.Input, id1.ToString());
            m_ado.AddParam("@UserID", SqlDbType.Int, 4, ParameterDirection.Input, id2.ToString());
            m_DS = m_ado.GetDS();
        }
        public DataSet GetUserByGameRoomID(int id)
        {
            m_ado.ProcName = "csp_GetUserByGameRoomID";
            m_ado.AddParam("@GameRoomID", SqlDbType.Int, 4, ParameterDirection.Input, id.ToString());
            m_DS = m_ado.GetDS();
            m_HasData = m_ado.HasData;
            return m_DS;
        }
    }
}
