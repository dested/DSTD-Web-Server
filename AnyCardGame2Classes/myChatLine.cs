using System;using System.Collections;using System.Collections.Generic;using System.Data;using System.Data.SqlClient;using System.Text;using AnyCardGame2Classes.Data_;namespace AnyCardGame2Classes {public partial class myChatLine {private int myChatLineID;
public int ChatLineID{
get { return myChatLineID; }
set { myChatLineID=value; } }
private int myUserID;
public int UserID{
get { return myUserID; }
set { myUserID=value; } }
public myUser User_{
get { return new myUser(myUserID); } }
private int myGameRoomID;
public int GameRoomID{
get { return myGameRoomID; }
set { myGameRoomID=value; } }
public myGameRoom GameRoom_{
get { return new myGameRoom(myGameRoomID); } }
private string myChatLineContent;
public string ChatLineContent{
get { return myChatLineContent; }
set { myChatLineContent=value; } }
private DateTime myTimePosted;
public DateTime TimePosted{
get { return myTimePosted; }
set { myTimePosted=value; } }
public myChatLine(){ 
InitVars(); }
public myChatLine(int myChatLineID_){ 
InitVars(); 
GetChatLineByChatLineID(myChatLineID_); }
public myChatLine(DataRow row){ 
InitVars(); 
UpdateChatLine(row); }
private void InitVars(){ 
myChatLineID = 0;
myUserID = 0;
myGameRoomID = 0;
myChatLineContent = "";
myTimePosted = new DateTime();
}
public bool InsertData(){ 
myChatLineData data=new myChatLineData();
List<SqlParameter> tempAL = new List<SqlParameter>();
SqlParameter tempP;DataSet tempDS;
tempP = new SqlParameter();
tempP.ParameterName = "@UserID";
tempP.Size = 4;
tempP.SqlDbType = SqlDbType.Int;
tempP.Value = myUserID;
tempP.Direction = ParameterDirection.Input;
tempAL.Add(tempP);
tempP = new SqlParameter();
tempP.ParameterName = "@GameRoomID";
tempP.Size = 4;
tempP.SqlDbType = SqlDbType.Int;
tempP.Value = myGameRoomID;
tempP.Direction = ParameterDirection.Input;
tempAL.Add(tempP);
tempP = new SqlParameter();
tempP.ParameterName = "@ChatLineContent";
tempP.Size = 1200;
tempP.SqlDbType = SqlDbType.VarChar;
tempP.Value = myChatLineContent;
tempP.Direction = ParameterDirection.Input;
tempAL.Add(tempP);
tempP = new SqlParameter();
tempP.ParameterName = "@TimePosted";
tempP.Size = 12;
tempP.SqlDbType = SqlDbType.DateTime;
tempP.Value = myTimePosted;
tempP.Direction = ParameterDirection.Input;
tempAL.Add(tempP);
tempDS = data.RunProcedure("csp_InsertChatLine", tempAL);
myChatLineID= int.Parse(tempDS.Tables[0].Rows[0][0].ToString());
tempDS.Dispose(); return true; }
public bool UpdateData(){ 
myChatLineData data=new myChatLineData();
List<SqlParameter> tempAL = new List<SqlParameter>();
SqlParameter tempP; 
tempP = new SqlParameter();
tempP.ParameterName = "@ChatLineID";
tempP.Size = 4;
tempP.SqlDbType = SqlDbType.Int;
tempP.Value = myChatLineID;
tempP.Direction = ParameterDirection.Input;
tempAL.Add(tempP);
tempP = new SqlParameter();
tempP.ParameterName = "@UserID";
tempP.Size = 4;
tempP.SqlDbType = SqlDbType.Int;
tempP.Value = myUserID;
tempP.Direction = ParameterDirection.Input;
tempAL.Add(tempP);
tempP = new SqlParameter();
tempP.ParameterName = "@GameRoomID";
tempP.Size = 4;
tempP.SqlDbType = SqlDbType.Int;
tempP.Value = myGameRoomID;
tempP.Direction = ParameterDirection.Input;
tempAL.Add(tempP);
tempP = new SqlParameter();
tempP.ParameterName = "@ChatLineContent";
tempP.Size = 1200;
tempP.SqlDbType = SqlDbType.VarChar;
tempP.Value = myChatLineContent;
tempP.Direction = ParameterDirection.Input;
tempAL.Add(tempP);
tempP = new SqlParameter();
tempP.ParameterName = "@TimePosted";
tempP.Size = 12;
tempP.SqlDbType = SqlDbType.DateTime;
tempP.Value = myTimePosted;
tempP.Direction = ParameterDirection.Input;
tempAL.Add(tempP);
data.RunProcedure("csp_UpdateChatLine", tempAL);
return true;  }
public void DeleteChatLine(){  new myChatLineData().DeleteChatLineByChatLineID(myChatLineID); }
public static List<myChatLine> GetAllChatLine() { 
myChatLineData tempADO = new myChatLineData();
List<myChatLine> list=new List<myChatLine>();
DataSet tempDS = tempADO.GetAllChatLine();
if (tempADO.HasData) {
foreach (DataRow row in tempDS.Tables[0].Rows)
list.Add(new myChatLine(row));
 return list; }
return new List<myChatLine>(); }
public bool GetChatLineByChatLineID(int val){ 
myChatLineData tempADO = new myChatLineData();
DataSet tempDS = tempADO.GetChatLineByChatLineID(val);
if (tempADO.HasData){
UpdateChatLine(tempDS.Tables[0].Rows[0]);
return true;}return false;}
public static List<myChatLine> GetChatLineByUser(myUser val){ 
myChatLineData tempADO = new myChatLineData();
List<myChatLine> list=new List<myChatLine>();
DataSet tempDS = tempADO.GetChatLineByUserID(val.UserID);
if (tempADO.HasData) {
foreach (DataRow row in tempDS.Tables[0].Rows)
list.Add(new myChatLine(row));
 return list; }
return new List<myChatLine>(); }
public static List<myChatLine> GetChatLineByUserID(int val){ 
myChatLineData tempADO = new myChatLineData();
List<myChatLine> list=new List<myChatLine>();
DataSet tempDS = tempADO.GetChatLineByUserID(val);
if (tempADO.HasData) {
foreach (DataRow row in tempDS.Tables[0].Rows)
list.Add(new myChatLine(row));
 return list; }
return new List<myChatLine>(); }
public static List<myChatLine> GetChatLineByGameRoom(myGameRoom val){ 
myChatLineData tempADO = new myChatLineData();
List<myChatLine> list=new List<myChatLine>();
DataSet tempDS = tempADO.GetChatLineByGameRoomID(val.GameRoomID);
if (tempADO.HasData) {
foreach (DataRow row in tempDS.Tables[0].Rows)
list.Add(new myChatLine(row));
 return list; }
return new List<myChatLine>(); }
public static List<myChatLine> GetChatLineByGameRoomID(int val){ 
myChatLineData tempADO = new myChatLineData();
List<myChatLine> list=new List<myChatLine>();
DataSet tempDS = tempADO.GetChatLineByGameRoomID(val);
if (tempADO.HasData) {
foreach (DataRow row in tempDS.Tables[0].Rows)
list.Add(new myChatLine(row));
 return list; }
return new List<myChatLine>(); }
public static List<myChatLine> GetChatLinesByGameRoomID_(int aGameRoomID ) { 
myChatLineData tempADO = new myChatLineData();
List<myChatLine> list=new List<myChatLine>();
DataSet tempDS = tempADO.GetChatLinesByGameRoomID_( aGameRoomID);
if (tempADO.HasData) {
foreach (DataRow row in tempDS.Tables[0].Rows)
list.Add(new myChatLine(row));
 return list; }
return new List<myChatLine>(); }
public static int GetChatLinesByGameRoomID__Count(int aGameRoomID ) { 
myChatLineData tempADO = new myChatLineData();
DataSet tempDS = tempADO.GetChatLinesByGameRoomID__Count( aGameRoomID);
if (tempADO.HasData) {
 return int.Parse(tempDS.Tables[0].Rows[0][0].ToString()); }
return 0; }
public static List<myChatLine> GetChatLinesByGameRoomID__Paging(int aGameRoomID,int aPageNumber,int aCount ) { 
myChatLineData tempADO = new myChatLineData();
List<myChatLine> list=new List<myChatLine>();
DataSet tempDS = tempADO.GetChatLinesByGameRoomID__Paging( aGameRoomID,aPageNumber,aCount);
if (tempADO.HasData) {
foreach (DataRow row in tempDS.Tables[0].Rows)
list.Add(new myChatLine(row));
 return list; }
return new List<myChatLine>(); }
private void UpdateChatLine(DataRow DR){
myChatLineID = int.Parse(DR["ChatLineID"].ToString());
myUserID = int.Parse(DR["UserID"].ToString());
myGameRoomID = int.Parse(DR["GameRoomID"].ToString());
myChatLineContent = DR["ChatLineContent"].ToString();
myTimePosted = DateTime.Parse(DR["TimePosted"].ToString());
}
} }
   namespace AnyCardGame2Classes.Data_ {public partial class myChatLineData: myData {public DataSet GetAllChatLine() { 
m_ado.ProcName = "csp_GetAllChatLine";
m_DS = m_ado.GetDS();
m_HasData = m_ado.HasData;
return m_DS; } 
public DataSet GetChatLineByChatLineID(int val){ 
m_ado.ProcName = "csp_GetChatLineByChatLineID";
m_ado.AddParam("@ChatLineID", SqlDbType.Int, 4, ParameterDirection.Input, val.ToString());
m_DS = m_ado.GetDS();
m_HasData = m_ado.HasData;
return m_DS; }
public DataSet GetChatLineByUserID(int val){ 
m_ado.ProcName = "csp_GetChatLineByUserID";
m_ado.AddParam("@UserID", SqlDbType.Int, 4, ParameterDirection.Input, val.ToString());
m_DS = m_ado.GetDS();
m_HasData = m_ado.HasData;
return m_DS; }
public DataSet GetChatLineByGameRoomID(int val){ 
m_ado.ProcName = "csp_GetChatLineByGameRoomID";
m_ado.AddParam("@GameRoomID", SqlDbType.Int, 4, ParameterDirection.Input, val.ToString());
m_DS = m_ado.GetDS();
m_HasData = m_ado.HasData;
return m_DS; }
public DataSet GetChatLinesByGameRoomID_(int aGameRoomID ) { 
m_ado.ProcName = "csp_GetChatLinesByGameRoomID_";
m_ado.AddParam("@GameRoomID", SqlDbType.Int, 4, ParameterDirection.Input, aGameRoomID.ToString());
m_DS = m_ado.GetDS();
m_HasData = m_ado.HasData;
return m_DS; } 
public DataSet GetChatLinesByGameRoomID__Count(int aGameRoomID ) { 
m_ado.ProcName = "csp_GetChatLinesByGameRoomID__Count";
m_ado.AddParam("@GameRoomID", SqlDbType.Int, 4, ParameterDirection.Input, aGameRoomID.ToString());
m_DS = m_ado.GetDS();
m_HasData = m_ado.HasData;
return m_DS; } 
public DataSet GetChatLinesByGameRoomID__Paging(int aGameRoomID,int aPageNumber,int aCount ) { 
m_ado.ProcName = "csp_GetChatLinesByGameRoomID__Paging";
m_ado.AddParam("@GameRoomID", SqlDbType.Int, 4, ParameterDirection.Input, aGameRoomID.ToString());
m_ado.AddParam("PageNum", SqlDbType.Int, 4, ParameterDirection.Input, aPageNumber.ToString());
m_ado.AddParam("Count", SqlDbType.Int, 4, ParameterDirection.Input, aCount.ToString());
m_DS = m_ado.GetDS();
m_HasData = m_ado.HasData;
return m_DS; } 
public void DeleteChatLineByChatLineID(int id) {
m_ado.AddParam("@ChatLineID", SqlDbType.Int, 4, ParameterDirection.Input, id.ToString());
m_ado.ProcName = "csp_DeleteChatLine";
m_DS = m_ado.GetDS(); }
} }
