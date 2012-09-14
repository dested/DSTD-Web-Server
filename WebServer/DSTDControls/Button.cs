using System;
using System.Collections.Generic;

namespace DSTDControls
{
    public class Button :Control {
        public string label { get { return Value; } set { Value = value; } }

        public bool Enabled=true;

        public override void OnInit() {
            base.OnInit();
        }
        public override void OnLoad() {
            base.OnLoad();
        }

        private string where;
        public string Where
        {
            get { return where; }
            set
            {
                    where = value;
            }
        }

        public override void Initialize() {
            updateWhere();
            base.Initialize();
        }

        private void updateWhere()
        {
            if (where == "PAGE")
                where = this.Page.ID;
            else if (where == "THIS")
                where = this.GetPanel.ID;
            else if (where == "BODY")
                where = "theBody";
        }

        public event TriggeredEvent OnClick;

        public override string OnRender() {
            updateWhere();
            string s = "<input type=\"button\" {disabled} id=\"{id}\" name=\"{id}\" style=\"{style}\" value=\"{label}\" onclick=\"PostPanel(this,'{click}')\"> </input> " + NEWLINE;

            Where = GetIDFromid(this.Page, Where);

            s = s.Replace("{disabled}", (!Enabled ? "disabled=\"disabled\"" : ""));

            return s.Replace("{id}", ID).Replace("{label}", label).Replace("{click}", this.Page.getUserQuery() + ID + "|" + Where).Replace("{style}", Style);
        }


        public void AddOnClick(string s, TriggeredEvent fooabr)
        {
            Where = s;
            OnClick+=fooabr;
        }

        public bool Clicked = false;
        public void Click()
        {
            OnClick(this);
        }
    }
 
}