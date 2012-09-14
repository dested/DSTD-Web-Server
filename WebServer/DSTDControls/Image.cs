namespace DSTDControls
{
    public class Image :Control {
        public string url;

        public override void OnInit() {
            base.OnInit();
        }
        public override void OnLoad() {

            base.OnLoad();
        }

        public override string OnRender() {
            string s = "<img src=\"{url}\" /> ";
            return s.Replace("{url}", url);
        }
    }
}