using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using DSTDControls;

namespace Control_Namespace {
    public class aTicTac :Panel {
        public aTicTac(string str) {
            this.id = str;
            Init += new ControlInit(aTicTac_Init);
            Load += new ControlLoad(aTicTac_Load);

           

        }

        void aTicTac_Load(Control sender) {

        }
        void aTicTac_Init(Control sender) {
            Button[][] b = new Button[3][];
            string[] str1 = new[] { "top", "middle", "bottom" };
            string[] str2 = new[] { "left", "middle", "right" };
            int i = 0;

            foreach (string s1 in str1) {
                int a = 0;
                b[i] = new Button[3];
                foreach (string s2 in str2) {
                    b[i][a] = (Button)GetControlByID(s1 + s2);
                    b[i][a].Where = this.ID;
                    a++;
                }
                i++;
            }
            ((Timer)GetControlByID("someTimer")).Where = this.ID;
            ((Timer) GetControlByID("someTimer")).Time = myHelper.RANDOM(5,50);
        }
        public void buttonClick(Control sender) {
            ((Button)sender).label = ((Variable)GetControlByID("WhosTurn")).Value;

            Color color = Color.FromArgb(myHelper.RANDOM(0, 255), myHelper.RANDOM(0, 255), myHelper.RANDOM(0, 255));
            sender.GetPanel.Style = "float:left;background-color:#" + color.Name.Substring(0, 6) + ";";
            ((Button)sender).Enabled = false;
            handleSwitch();
            if (checkHandle()) {

                Button[][] b = new Button[3][];
                string[] str1 = new[] { "top", "middle", "bottom" };
                string[] str2 = new[] { "left", "middle", "right" };
                int i = 0;

                foreach (string s1 in str1) {
                    int a = 0;
                    b[i] = new Button[3];
                    foreach (string s2 in str2) {
                        b[i][a] = (Button)GetControlByID(s1 + s2);
                        a++;
                    }
                    i++;
                }

                for (int j = 0; j < 3; j++) {
                    for (int k = 0; k < 3; k++) {
                        b[j][k].label = "_";
                        b[j][k].Enabled = true;
                    }
                }
            }
        }

        private bool checkHandle() {
            Button[][] b = new Button[3][];
            string[] str1 = new[] { "top", "middle", "bottom" };
            string[] str2 = new[] { "left", "middle", "right" };
            int i = 0;

            foreach (string s1 in str1) {
                int a = 0;
                b[i] = new Button[3];
                foreach (string s2 in str2) {
                    b[i][a] = (Button)GetControlByID(s1 + s2);
                    a++;
                }
                i++;
            }

            bool bc = false;
            string cur = "X";
            if (b[0][0].label == cur && b[1][0].label == cur && b[2][0].label == cur)
                bc = true;
            if (b[0][1].label == cur && b[1][1].label == cur && b[2][1].label == cur)
                bc = true;
            if (b[0][2].label == cur && b[1][2].label == cur && b[2][2].label == cur)
                bc = true;

            if (b[0][0].label == cur && b[0][1].label == cur && b[0][2].label == cur)
                bc = true;
            if (b[1][0].label == cur && b[1][1].label == cur && b[1][2].label == cur)
                bc = true;
            if (b[2][0].label == cur && b[2][1].label == cur && b[2][2].label == cur)
                bc = true;

            if (b[0][0].label == cur && b[1][1].label == cur && b[2][2].label == cur)
                bc = true;
            if (b[0][2].label == cur && b[1][1].label == cur && b[2][0].label == cur)
                bc = true;

            if (bc == true) {
                GetControlByID("theLabel").Value = cur + " Has Won it!";
                return true;
            }

            cur = "O";
            if (b[0][0].label == cur && b[1][0].label == cur && b[2][0].label == cur)
                bc = true;
            if (b[0][1].label == cur && b[1][1].label == cur && b[2][1].label == cur)
                bc = true;
            if (b[0][2].label == cur && b[1][2].label == cur && b[2][2].label == cur)
                bc = true;

            if (b[0][0].label == cur && b[0][1].label == cur && b[0][2].label == cur)
                bc = true;
            if (b[1][0].label == cur && b[1][1].label == cur && b[1][2].label == cur)
                bc = true;
            if (b[2][0].label == cur && b[2][1].label == cur && b[2][2].label == cur)
                bc = true;

            if (b[0][0].label == cur && b[1][1].label == cur && b[2][2].label == cur)
                bc = true;
            if (b[0][2].label == cur && b[1][1].label == cur && b[2][0].label == cur)
                bc = true;

            if (bc == true) {
                GetControlByID("theLabel").Value = cur + " Has Won it!";
                return true;
            }


            for (int j = 0; j < 3; j++) {
                for (int k = 0; k < 3; k++) {
                    if (b[j][k].label == "_") {
                        return false;
                    }
                }
            }

            GetControlByID("theLabel").Value = "Stale mate!";



            return true;



        }

        public void RandomClick(Control sender) {
            string[] str1 = new[] { "top", "middle", "bottom" };
            string[] str2 = new[] { "left", "middle", "right" };

            int i1 = myHelper.RANDOM(0, 3);
            int i2 = myHelper.RANDOM(0, 3);

            if (!((Button)sender.GetControlByID(str1[i1] + str2[i2])).Enabled) {
                RandomClick(sender);
                return;
            }
            ((Button)sender.GetControlByID(str1[i1] + str2[i2])).Click();

        }

        void handleSwitch() {
            if (((Variable)this.GetControlByID("WhosTurn")).Value == "X") {
                ((Variable)this.GetControlByID("WhosTurn")).Value = "O";
            }
            else
                ((Variable)this.GetControlByID("WhosTurn")).Value = "X";
        }
    }
}
