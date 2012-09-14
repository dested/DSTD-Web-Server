using System;
using System.Collections.Generic;
using System.Text;
using AnyCardGame2Classes;
using DSTDControls;

namespace Control_Namespace {
    public class GamesOpen :Panel {
        public GamesOpen(string id_) {
            id = id_;
            Init += new ControlInit(GamesOpen_Init);

            Load += new ControlLoad(GamesOpen_Load);

        }
        public override void Initialize() {
            ((Panel)GetControlByID("theGameList")).Init += new ControlInit(theGameList_Init);
            ((Button)GetControlByID("theGameButton")).Where = ID;

            base.Initialize();
        }

        private void theGameList_Init(Control sender) {

            GameID = int.Parse(this.Page.UserQuery["GameRoomID"]);

            myGameRoom g = new myGameRoom();
            g.GetGameRoomByGameRoomName("Home");
            if (g.GameRoomID == GameID)

                isHome = true;


            //if (!Page.IsPostBack)
            getGames(sender);

        }

        void GamesOpen_Load(Control sender) {
        }

        void getGames(Control sender) {
            Panel that = ((Panel)GetControlByID("theGameList"));
            that.Children.Clear();
            Timer t = new Timer();
            t.Time = 1200;
            t.OnFire += getGames;
            t.Where = "theGameList";
            t.id = "time_2";
            that.Children.Add(t);


            if (isHome) {

                Button b = new Button();
                b.label = "New";
                b.Where = ID;
                b.id = "theGameClick0";
                int i = 1;
                b.OnClick += new TriggeredEvent(b_OnClick);
                that.Children.Add(b);
                that.Children.Add(new BR());
                foreach (myGameRoom gm in myGameRoom.GetAllGameRoom()) {
                    b = new Button();
                    if (gm.GameRoomName == "Home")
                        continue;
                    b.label = gm.GameRoomName;
                    b.id = "theGameClick" + i;
                    b.Where = ID;
                    b.OnClick += new TriggeredEvent(b_OnClick);
                    that.Children.Add(b);
                    that.Children.Add(new BR());
                    i++;
                }
            }
            else {
                Button b = new Button();
                b.label = "Leave";
                b.Where = ID;
                b.id = "theGameClick0";
                int i = 1;
                b.OnClick += new TriggeredEvent(b_OnClick);
                that.Children.Add(b);
            }
        }

        private int GameID;
        private bool isHome;

        void b_OnClick(Control sender) {

            if (((Button)sender).label == "New") {
                ((Panel)GetControlByID("theNewGame")).Visible = true;
                return;
            }

            string s = ((Button)sender).label;
            myGameRoom g = moveGames(s);

            this.Page.Request.TransferToPage("Chat*GameRoomID=" + g.GameRoomID);
        }

        private myGameRoom moveGames(string s) {
            if (s == "Leave")
                s = "Home";

            myGameRoom g = new myGameRoom();
            g.GetGameRoomByGameRoomName(s);
            g.AddUser_(((myUser)Page.Session["LoggedInUser"]));
             

            myGameRoom g1 = new myGameRoom(GameID);
            ((myUser)Page.Session["LoggedInUser"]).DeleteGameRoom(g1.GameRoomID);

            if (g1.GameRoomName != "Home")
                g1.DeleteGameRoom();
            return g;
        }

        void GamesOpen_Init(Control sender) {

        }
        void NewGame(Control sender) {
            myGameRoom g = new myGameRoom();
            g.GameRoomName = ((((TextBox)GetControlByID("theGameName")).text));
            g.InsertData();

            g = moveGames(g.GameRoomName);

            this.Page.Request.TransferToPage("Chat*GameRoomID=" + g.GameRoomID);

        }
    }
}