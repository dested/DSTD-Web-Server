using System;
using System.Collections.Generic;
using System.Text;
using AnyCardGame2;
using AnyCardGame2Classes;
using DSTDControls;

namespace Control_Namespace {
    public class UserLogin :Panel {
        public UserLogin(string id) {
//            id = "UserLogin";
            Init += new ControlInit(UserLogin_Init);
            Load += new ControlLoad(UserLogin_Load); 
        }
 
 
        void UserLogin_Init(Control sender) { 
        }
        void UserLogin_Load(Control sender) { 
        }
        void LoginUser(Control sender)
        {
            string username = ((TextBox) this.GetControlByID("theUsername")).text;
            string password = ((TextBox) this.GetControlByID("thePassword")).text;

            myUser u = new myUser();

            if (u.GetUserByUserName(username))
            {
                if (u.Password != password)
                {
                    ((Label) this.GetControlByID("theError")).text = "The password was incorrect";
                    return;
                }
            }
            else
            {
                u.UserName = username;
                u.Password = password;
                u.InsertData();
            }
            setUsername(u);

            myGameRoom g = new myGameRoom();
            if (!g.GetGameRoomByGameRoomName("Home"))
            {
                g.GameRoomName = "Home";
                g.InsertData();
            }
            g.AddUser_(u);
            this.Page.Request.TransferToPage("Chat*GameRoomID="+g.GameRoomID);
        }

        private void setUsername(myUser u)
        {
            if (this.Page.Session.ContainsKey("LoggedInUser"))
                this.Page.Session["LoggedInUser"] = u;
            else
                this.Page.Session.Add("LoggedInUser", u);

        }
    }
}
