using System;
using System.Text;

namespace DSTDControls {
    public class Timer :Control {
        public int Time;
        public override void OnInit() {
            base.OnInit();

        }
        public override void OnLoad() {
            base.OnLoad();
        }

        private string where;
        public string Where {
            get { return where; }
            set {
                where = value;
            }
        }

        public override void Initialize() {
            if (where == "PAGE")
                where = this.Page.ID;
            else if (where == "THIS")
                where = this.GetPanel.ID;
            else if (where == "BODY")
                where = "theBody";
        }

        public bool Fired = false;
        public void Fire() {
            OnFire(this.Page.GetControlByID(Where));
        }
        public event TriggeredEvent OnFire;

        public string Before = ""; 
          
        public override string OnRender() {
            string s ;//= "var abba" + ID + " = function() { setTimeout('{click}; PostPanel(document.getElementById(\\'{postfrom}\\'),\\'{where}\\'); setTimeout(abba" + ID + "," + Time + ");'," + Time + ");}; setTimeout(abba" + ID + ",1);";


            Where = GetIDFromid(this.Page, Where);

            s = "AddTimeout(document.getElementById('{postfrom}').id,setTimeout('{before}; PostPanel(document.getElementById(\\'{postfrom}\\'),\\'{click}\\');'," + Time + "));";

            s = s.Replace("{value}", Value).Replace("{id}", ID).Replace("{before}", Before).Replace("{click}", this.Page.getUserQuery()+ID + "|" + Where);


            s = s.Replace("{postfrom}", Where == "" ? ID : Where);
            if (!this.Page.Scripts.Contains(s))
                this.Page.Scripts.Add(s);

            return "<input type=\"hidden\" value=\"clicker\" id=\"{id}\" /> ".Replace("{id}", ID) + NEWLINE;
        }

    }

}
