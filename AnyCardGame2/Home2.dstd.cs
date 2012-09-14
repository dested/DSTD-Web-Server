using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using DSTDControls;

namespace Control_Namespace {
    public class Home2 :Page {
        public Home2() {
            this.id = "Home2";
            Init += new ControlInit(Home2_Init);
            Load += new ControlLoad(Home2_Load);
          //  for (int i = 0; i < 1; i++)
         //   {
          //      this.Children.Add(Panel.LoadControl("aTicTac", "tictac"+i));
          //  }
        }

        public void Home2_Load(Control sender) { 

        }
        public void Home2_Init(Control sender) {
        }
    }
}