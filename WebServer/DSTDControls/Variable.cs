namespace DSTDControls
{
    public class Variable :Control {

        public override void OnInit() {
            base.OnInit();

        }
        public override void OnLoad() {
            base.OnLoad();
        }


        public override string OnRender() {
            string s = "<input type=\"hidden\" value=\"{value}\" id=\"{id}\" /> ";
            return s.Replace("{value}", Value).Replace("{id}", ID);
        }
    }
}