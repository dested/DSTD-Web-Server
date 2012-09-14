using System;
using System.Collections.Generic;
using System.Text;
using AnyCardGame2;
using DSTDControls;

namespace Control_Namespace {
    public class Home :Page {
        public Home() {
            id = "Home";
            Init += new ControlInit(Home_Init);
            Load += new ControlLoad(Home_Load);
        }
        public override void Initialize() {
            bugger = new Debugger();
            bugger.AddFoobar(new Debugger.GrabVariable(getUserName));
            bugger.AddFoobar(new Debugger.GrabVariable(getPassword));
     //       Children.Add(bugger);
            base.Initialize();
        }

        private GrabVariableReturn getUserName(Control sender) {
            return new GrabVariableReturn("Username", ((TextBox)this.GetControlByID("theUsername")).text);
        }
        private GrabVariableReturn getPassword(Control sender) {
            return new GrabVariableReturn("Password", ((TextBox)this.GetControlByID("thePassword")).text);
        }

        private Debugger bugger;


        void Home_Load(Control sender) {
            bugger.PushVariable(new GrabVariableReturn("Something", "Init"));

        }
        void Home_Init(Control sender) {
            bugger.PushVariable(new GrabVariableReturn("Something", "Load"));

        }
    }
}