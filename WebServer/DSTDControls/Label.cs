namespace DSTDControls
{
    public class Label :Control {
        public string text { get { return Value; } set { Value = value; } }

        public override void OnInit() {
            base.OnInit();

        }
        public override void OnLoad() {
            base.OnLoad();
        }
        public Label(string text_)
        {
            text = text_;
        }

        public Label()
        {
            
        }

        public override string OnRender() {
            string s = "<span style=\"{style}\">{text}</span>" + NEWLINE;
            return s.Replace("{style}", Style).Replace("{text}", text);
        }
    }
}