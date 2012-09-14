using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DSTDControls {



    public class TicTacToe :Panel {
        public TicTacToe(string id_) {
            id = id_;
            Init += new ControlInit(InformationToSend_Init);
            Load += new ControlLoad(InformationToSend_Load);
        }

        void InformationToSend_Load(Control sender) {
            if (!Page.IsPostBack) {
                ((Variable)sender.GetControlByID("WhosTurn")).Value = "X";
            }
            else {
                if (!string.IsNullOrEmpty(Sender)) {
                    if (Sender.Contains("clicktimer")) {
                        string[] str1 = new[] { "top", "middle", "bottom" };
                        string[] str2 = new[] { "left", "middle", "right" };

                        int i1 = myHelper.RANDOM(0, 3);
                        int i2 = myHelper.RANDOM(0, 3);

                        ((Button)sender.GetControlByID(str1[i1] + str2[i2])).label = ((Variable)sender.GetControlByID("WhosTurn")).Value;
                    }
                    else
                        ((Button)sender.GetControlByID(Sender)).label = ((Variable)sender.GetControlByID("WhosTurn")).Value;
                    if (((Variable)sender.GetControlByID("WhosTurn")).Value == "X") {
                        ((Variable)sender.GetControlByID("WhosTurn")).Value = "O";
                    }
                    else
                        ((Variable)sender.GetControlByID("WhosTurn")).Value = "X";

                }
            }
        }

        void InformationToSend_Init(Control sender) {
            Panel InformationToSend = this;
            Timer tb = new Timer();
            tb.id = "clicktimer";
            tb.Time = myHelper.RANDOM(350, 600);
            //tb.OnClick = "document.getElementById('" + this.id + "_topleft').onclick=true;";
            tb.Where = "http://localhost:9099/Home";

           // InformationToSend.Children.Add(tb);
            Color color = Color.FromArgb(myHelper.RANDOM(0, 255), myHelper.RANDOM(0, 255), myHelper.RANDOM(0, 255));
            InformationToSend.Style = "float:left;background-color:#" + color.Name.Substring(0, 6);
            Variable v = new Variable();
            v.id = "WhosTurn";
            InformationToSend.Children.Add(v);

            Table t = new Table();
            TableRow tr = new TableRow();
            TableCell td = new TableCell();


            Button b = new Button();
            b.AddOnClick(this.GetPanel.id, Fooabr);
            b.label = "_";
            b.id = "topleft";
            td.Children.Add(b);
            tr.Children.Add(td);
            td = new TableCell();

            b = new Button();
            b.AddOnClick(this.GetPanel.id, Fooabr);
            b.id = "topmiddle";
            b.label = "_";

            td.Children.Add(b);
            tr.Children.Add(td);
            td = new TableCell();
            b = new Button();
            b.AddOnClick(this.GetPanel.id, Fooabr);
            b.label = "_";

            b.id = "topright";
            td.Children.Add(b);
            tr.Children.Add(td);
            t.Children.Add(tr);

            tr = new TableRow();
            td = new TableCell();

            b = new Button();
            b.AddOnClick(this.GetPanel.id, Fooabr);
            b.label = "_";

            b.id = "middleleft";
            td.Children.Add(b);
            tr.Children.Add(td);
            td = new TableCell();
            b = new Button();
            b.id = "middlemiddle";
            b.AddOnClick(this.GetPanel.id, Fooabr);
            b.label = "_";

            td.Children.Add(b);
            tr.Children.Add(td);
            td = new TableCell();
            b = new Button();
            b.label = "_";
            b.AddOnClick(this.GetPanel.id, Fooabr);

            b.id = "middleright";
            td.Children.Add(b);
            tr.Children.Add(td);
            t.Children.Add(tr);
            tr = new TableRow();
            td = new TableCell();


            b = new Button();
            b.label = "_";
            b.AddOnClick(this.GetPanel.id, Fooabr);

            b.id = "bottomleft";
            td.Children.Add(b);
            tr.Children.Add(td);
            td = new TableCell();
            b = new Button();
            b.label = "_";
            b.AddOnClick(this.GetPanel.id, Fooabr);

            b.id = "bottommiddle";
            td.Children.Add(b);
            tr.Children.Add(td);
            td = new TableCell();
            b = new Button();
            b.AddOnClick(this.GetPanel.id, Fooabr);
            b.label = "_";

            b.id = "bottomright";
            td.Children.Add(b);
            tr.Children.Add(td);
            t.Children.Add(tr);
            InformationToSend.Children.Add(t);

        }

        private void Fooabr(Control sender) {
            ((Button)sender.GetControlByID(Sender)).label = ((Variable)sender.GetControlByID("WhosTurn")).Value;
            if (((Variable)sender.GetControlByID("WhosTurn")).Value == "X") {
                ((Variable)sender.GetControlByID("WhosTurn")).Value = "O";
            }
            else
                ((Variable)sender.GetControlByID("WhosTurn")).Value = "X";

        }
    }

    public class Home :Page {
        public Home()
        {
            Init += Page_Init;
            Load += Page_Load;
            for (int i = 0; i < 2; i++)
            {

                this.Children.Add(new TicTacToe("tictactoe"+i)); 
            }
            this.id = "Home";


        }


        public void Page_Init(Control sender) { 
        }

        public void Page_Load(Control sender) { 
           
        }
    }



    public class Where :Page {

        public override void OnLoad() {
            Panel InformationToSend = new Panel();
            InformationToSend.id = "Faggots2";
            TextBox t = new TextBox();
            t.text = "YOU ARE A FAGGOT";
            t.id = "faggot";
            InformationToSend.Children.Add(t);
            Button b = new Button();
            b.label = "DONT click me";
       //     b.OnClick = "http://localhost:9099/Home";
            b.id = "faggot";
            InformationToSend.Children.Add(b);
            this.Children.Add(InformationToSend);
        }
    }
}
