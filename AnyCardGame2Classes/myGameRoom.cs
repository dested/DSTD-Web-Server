using System;using System.Collections;using System.Collections.Generic;using System.Data;using System.Data.SqlClient;using System.Text;using AnyCardGame2Classes.Data_;namespace AnyCardGame2Classes {public partial class myGameRoom {private int myGameRoomID;
public int GameRoomID{
get { return myGameRoomID; }
set { myGameRoomID=value; } }
private string myGameRoomName;
public string GameRoomName{
get { return myGameRoomName; }
set { myGameRoomName=value; } }
private List<myUser> myUser_;
public List<myUser> Users{
get { return myUser_; }
set { myUser_=value; } }
public void UpdateUser_() {
myUser_ = myUser.GetUserByGameRoom(this); }
public void AddUser_(myUser val) {
val.UpdateGameRoom(myGameRoomID);
myUser_.Add(val);}
public void ClearUser_() {
foreach(myUser temp in myUser_) {
temp.DeleteGameRoom(myGameRoomID); }
myUser_.Clear(); }
public bool ContainsUser_(myUser val) {
foreach(myUser temp in myUser_) {
if (val==temp) return true; } return false;}
public bool ContainsUserID_(int val) {
foreach(myUser temp in myUser_) {
if (val==temp.UserID) return true; } return false;}
public void AddUserID_(int val) {
myUser v= new myUser(val);
v.UpdateGameRoom(myGameRoomID);
myUser_.Add(v);}
private List<myChatLine> myChatLines;
public List<myChatLine> ChatLines{
get { if(myChatLines==null) myChatLines = myChatLine.GetChatLineByGameRoom(this);  return myChatLines; }
set {myChatLines=value; }}
public myGameRoom(){ 
InitVars(); }
public myGameRoom(int myGameRoomID_){ 
InitVars(); 
GetGameRoomByGameRoomID(myGameRoomID_); }
public myGameRoom(DataRow row){ 
InitVars(); 
UpdateGameRoom(row); }
private void InitVars(){ 
myGameRoomID = 0;
myGameRoomName = "";
myUser_ = new List<myUser>();
}
public bool InsertData(){ 
myGameRoom n = new myGameRoom();
n.GetGameRoomByGameRoomName(myGameRoomName);
if (n.GameRoomID != 0) return false;
myGameRoomData data=new myGameRoomData();
List<SqlParameter> tempAL = new List<SqlParameter>();
SqlParameter tempP;DataSet tempDS;
tempP = new SqlParameter();
tempP.ParameterName = "@GameRoomName";
tempP.Size = 200;
tempP.SqlDbType = SqlDbType.VarChar;
tempP.Value = myGameRoomName;
tempP.Direction = ParameterDirection.Input;
tempAL.Add(tempP);
tempDS = data.RunProcedure("csp_InsertGameRoom", tempAL);
myGameRoomID= int.Parse(tempDS.Tables[0].Rows[0][0].ToString());
foreach(myUser temp in myUser.GetUserByGameRoom(this)) temp.DeleteGameRoom(myGameRoomID);
foreach(myUser temp in myUser_) temp.UpdateGameRoom(myGameRoomID);
tempDS.Dispose(); return true; }
public bool UpdateData(){ 
myGameRoom n = new myGameRoom();
n.GetGameRoomByGameRoomName(myGameRoomName);
if (n.GameRoomID != 0 && n.GameRoomID != myGameRoomID){ return false;}
myGameRoomData data=new myGameRoomData();
List<SqlParameter> tempAL = new List<SqlParameter>();
SqlParameter tempP; 
tempP = new SqlParameter();
tempP.ParameterName = "@GameRoomID";
tempP.Size = 4;
tempP.SqlDbType = SqlDbType.Int;
tempP.Value = myGameRoomID;
tempP.Direction = ParameterDirection.Input;
tempAL.Add(tempP);
tempP = new SqlParameter();
tempP.ParameterName = "@GameRoomName";
tempP.Size = 200;
tempP.SqlDbType = SqlDbType.VarChar;
tempP.Value = myGameRoomName;
tempP.Direction = ParameterDirection.Input;
tempAL.Add(tempP);
data.RunProcedure("csp_UpdateGameRoom", tempAL);
foreach(myUser temp in myUser.GetUserByGameRoom(this)) temp.DeleteGameRoom(myGameRoomID);
foreach(myUser temp in myUser_) temp.UpdateGameRoom(myGameRoomID);
return true;  }
public void DeleteGameRoom(){  new myGameRoomData().DeleteGameRoomByGameRoomID(myGameRoomID); }
public static List<myGameRoom> GetAllGameRoom() { 
myGameRoomData tempADO = new myGameRoomData();
List<myGameRoom> list=new List<myGameRoom>();
DataSet tempDS = tempADO.GetAllGameRoom();
if (tempADO.HasData) {
foreach (DataRow row in tempDS.Tables[0].Rows)
list.Add(new myGameRoom(row));
 return list; }
return new List<myGameRoom>(); }
public bool GetGameRoomByGameRoomID(int val){ 
myGameRoomData tempADO = new myGameRoomData();
DataSet tempDS = tempADO.GetGameRoomByGameRoomID(val);
if (tempADO.HasData){
UpdateGameRoom(tempDS.Tables[0].Rows[0]);
return true;}return false;}
public bool GetGameRoomByGameRoomName(string val){ 
myGameRoomData tempADO = new myGameRoomData();
DataSet tempDS = tempADO.GetGameRoomByGameRoomName(val);
if (tempADO.HasData){
UpdateGameRoom(tempDS.Tables[0].Rows[0]);
return true;}return false;}
public static List<myGameRoom> GetGameRoomByUser(myUser val) {
myGameRoomData tempADO = new myGameRoomData();
List<myGameRoom> list=new List<myGameRoom>();
DataSet tempDS = tempADO.GetGameRoomByUserID(val.UserID);
if (tempADO.HasData) {
foreach (DataRow row in tempDS.Tables[0].Rows)
list.Add(new myGameRoom(row));
 return list; }
return new List<myGameRoom>(); }
public static List<myGameRoom> GetGameRoomByUserID(int val){ 
myGameRoomData tempADO = new myGameRoomData();
List<myGameRoom> list=new List<myGameRoom>();
DataSet tempDS = tempADO.GetGameRoomByUserID(val );
if (tempADO.HasData) {
foreach (DataRow row in tempDS.Tables[0].Rows)
list.Add(new myGameRoom(row));
 return list; }
return new List<myGameRoom>(); }
private void UpdateGameRoom(DataRow DR){
myGameRoomID = int.Parse(DR["GameRoomID"].ToString());
myGameRoomName = DR["GameRoomName"].ToString();
myUser_ = myUser.GetUserByGameRoom(this);
}
} }
   namespace AnyCardGame2Classes.Data_ {public partial class myGameRoomData: myData {public DataSet GetAllGameRoom() { 
m_ado.ProcName = "csp_GetAllGameRoom";
m_DS = m_ado.GetDS();
m_HasData = m_ado.HasData;
return m_DS; } 
public DataSet GetGameRoomByGameRoomID(int val){ 
m_ado.ProcName = "csp_GetGameRoomByGameRoomID";
m_ado.AddParam("@GameRoomID", SqlDbType.Int, 4, ParameterDirection.Input, val.ToString());
m_DS = m_ado.GetDS();
m_HasData = m_ado.HasData;
return m_DS; }
public DataSet GetGameRoomByGameRoomName(string val){ 
m_ado.ProcName = "csp_GetGameRoomByGameRoomName";
m_ado.AddParam("@GameRoomName", SqlDbType.VarChar, 200, ParameterDirection.Input, val.ToString());
m_DS = m_ado.GetDS();
m_HasData = m_ado.HasData;
return m_DS; }
public void DeleteGameRoomByGameRoomID(int id) {
m_ado.AddParam("@GameRoomID", SqlDbType.Int, 4, ParameterDirection.Input, id.ToString());
m_ado.ProcName = "csp_DeleteGameRoom";
m_DS = m_ado.GetDS(); }
public DataSet GetGameRoomByUserID(int id) {
m_ado.ProcName = "csp_GetGameRoomByUserID";
m_ado.AddParam("@UserID", SqlDbType.Int, 4, ParameterDirection.Input, id.ToString());
m_DS = m_ado.GetDS();
m_HasData = m_ado.HasData;
return m_DS; }
} }
