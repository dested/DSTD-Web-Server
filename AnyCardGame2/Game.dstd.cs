using System;
using System.Collections.Generic;
using System.Text;
using AnyCardGame2Classes;
using DSTDControls;

namespace Control_Namespace {
    public class Game :Page {
        public Game() {
            id = "Game";

            Init += new ControlInit(Game_Init);
            Load += new ControlLoad(Game_Load);
        }

        void Game_Load(Control sender) {
            myUser u;
            if (Session.ContainsKey("LoggedInUser"))
                u = ((myUser)Session["LoggedInUser"]);
            else
                return;


            int roomID = 0;

            if (this.UserQuery.ContainsKey("GameRoomID"))
            {
                roomID = int.Parse(this.UserQuery["GameRoomID"]);
            }


            myGameRoom r = new myGameRoom(roomID);
            foreach (myUser user in r.Users)
            {
                this.Children.Add(new Label(user.UserName));
                this.Children.Add(new BR());
            }

        }
        void Game_Init(Control sender) {

        }
    }
}
