namespace DSTDControls
{
    public class BR :Control {

        public override void OnInit() {
            base.OnInit();

        }
        public override void OnLoad() {
            base.OnLoad();
        }


        public override string OnRender() {
            string s = "<br /> " + NEWLINE;

            return s;
        }
    }
}