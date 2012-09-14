namespace DSTDControls
{
    public class TextBox :Control {
        public string text { get { return Value; } set { Value = value; } }

        public bool Enabled = true;
        private bool multiline;
        public bool Multiline { get { return multiline; } set { multiline = value; } }

        public string type="";

        public override void OnLoad() { 
            base.OnLoad();
        }
        public override void OnInit() {
            base.OnInit();
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


        public bool KeyPressedEnterd = false;
        public void KeyPressEnter() {
            OnKeyPressEnter(this.Page.GetControlByID(Where));
        }
        public event TriggeredEvent OnKeyPressEnter;


        public int Rows=0;
        public int Cols=0;

        public override string OnRender() {

            string s = "<input type=\"{type}\" {disabled}  onkeydown=\"if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {PostPanel(this,'{click}');}}\"  style=\"{style}\" id=\"{id}\" name=\"{id}\" value=\"{text}\" /> " + NEWLINE;

            if (type=="")
                type = "text";

            Where = GetIDFromid(this.Page, Where);
            if (multiline)
            {
                s = "<textarea {disabled} style=\"{style}\" id=\"{id}\" name=\"{id}\" rows=\"{rows}\" cols=\"{cols}\" >{text}</textarea> ";
                s = s.Replace("{rows}", Rows.ToString()).Replace("{cols}", Cols.ToString());
            }

            s = s.Replace("{disabled}", (!Enabled ? "disabled=\"disabled\"" : ""));

            return s.Replace("{id}", ID).Replace("{type}", type).Replace("{text}", text).Replace("{style}", Style).Replace("{click}", this.Page.getUserQuery() + ID + "|" + Where);

        }
    }
}