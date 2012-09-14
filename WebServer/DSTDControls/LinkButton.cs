namespace DSTDControls
{
    public class LinkButton :Control {
        public string label { get { return Value; } set { Value = value; } }

        public override void OnInit() {
            base.OnInit();
        }
        public override void OnLoad() {
            base.OnLoad();
        }

        public string OnClick;

        public override string OnRender() {
            string s = "<a id=\"{id}\" name=\"{id}\" href=\"#\" onclick=\"PostPanel(this,'{click}'); return false;\">{label}</a> " + NEWLINE;
            return s.Replace("{id}", ID).Replace("{label}", label).Replace("{click}", OnClick);
        }
    }
}