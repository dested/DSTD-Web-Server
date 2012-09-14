using System;
using System.Collections.Generic;
using System.Text;
using AnyCardGame2Classes;
using DSTDControls;

namespace Control_Namespace {
    public class Chat :Page {
        public Chat() {
            id = "Chat";
            Init += new ControlInit(Chat_Init);
            Load += new ControlLoad(Chat_Load);
            UnLoad += new ControlUnLoad(Chat_UnLoad);
        }

        private void Chat_UnLoad(Control sender)
        {
            myGameRoom g = new myGameRoom(GameID);
            ((myUser)Session["LoggedInUser"]).DeleteGameRoom(g.GameRoomID);
        }

        public override void Initialize()
        {
            ((Panel)GetControlByID("theChatBox")).Load += new ControlLoad(ChatBox_Load);
            ((Panel)GetControlByID("theUsers")).Load += new ControlLoad(Users_Load);



            base.Initialize();
        }

        void Chat_Load(Control sender) {
        }

        void Chat_Init(Control sender) { 
 

            ((TextBox)GetControlByID("theChatContent")).Enabled = false;

        }

        private int GameID = 0;

        private void ChatBox_Load(Control sender) {
            GameID = int.Parse(this.UserQuery["GameRoomID"]);

            if (((Variable)GetControlByID("theUsername")).Value == "") {

                if (Session.ContainsKey("LoggedInUser"))

                    ((Variable)GetControlByID("theUsername")).Value = ((myUser)Session["LoggedInUser"]).UserName;
                else
                    ((Variable)GetControlByID("theUsername")).Value = "Foobar" + DSTDControls.myHelper.RANDOM(0, 200);

            }
             
            if (!IsPostBack) {
                int lines = myChatLine.GetChatLinesByGameRoomID__Count(GameID);

                if (Session.ContainsKey("LineNumber"))
                    Session["LineNumber"] = lines;
                else
                    Session.Add("LineNumber",lines); 

                getMessage(GetControlByID("theChat"));
            }
        }
        private void Users_Load(Control sender) {
            GameID = int.Parse(this.UserQuery["GameRoomID"]);

            if (!IsPostBack || sender.PanelToUpdate.Split('_')[sender.PanelToUpdate.Split('_').Length-1] == "theChat")
             {
                getUsers(GetControlByID("theUsers"));
            }
        }

        public void SendMessage(Control sender)
        {
            myChatLine c = new myChatLine();
            c.GameRoomID=GameID;
            c.TimePosted = DateTime.Now;
            c.ChatLineContent = ((Variable) GetControlByID("theUsername")).Value + ": " +((TextBox) GetControlByID("theChatText")).text + "\r\n";
            c.UserID = ((myUser) Session["LoggedInUser"]).UserID;
            c.InsertData();

            getMessage(this);
            ((TextBox) GetControlByID("theChatText")).text = "";
            SelectedControl = "theChatText";

        }

        public void getMessage(Control sender)
        {
            int c = myChatLine.GetChatLinesByGameRoomID__Count(GameID);
            if (c != (int) Session["LineNumber"])
            {
               // ((TextBox) GetControlByID("theChatContent")).text = "";
                int i = 0;
                foreach (myChatLine line in myChatLine.GetChatLineByGameRoomID(GameID))
                {
                    if (i >= (int)Session["LineNumber"])
                    {
                        ((TextBox) GetControlByID("theChatContent")).text += line.ChatLineContent;
                    }
                    i++;
                }
                Session["LineNumber"] = c;
            }
        }

        public void getUsers(Control sender) {
            myGameRoom room = new myGameRoom(GameID);

            foreach (myUser s in room.Users)
            {
                sender.GetPanel.Children.Add(new Label(s.UserName));
                sender.GetPanel.Children.Add(new BR());
            }
        }
    }
}