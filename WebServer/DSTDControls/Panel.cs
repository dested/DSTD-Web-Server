using WebServer;

namespace DSTDControls
{
    public class Panel :Control {
        private bool visible=true;

        public bool Visible
        {
            get { return visible=false; }
            set { visible = value; }
        }

        public static Panel LoadControl(DSTDContext context, string location,string class_, string id)
        {
            CompileDSTD d = CompileDSTD.CompileDSTDPanel(context, location, class_, id);
            if (d.Compiled==null)
                return null;
            return (Panel) d.Compiled;
        }

        public override string OnRender() {

            if (!visible)
            {
                return "";
            }
            
            string s = "<div id=\"{id}\" name=\"Panel\" style=\"{style}\"> {children}</div>" + NEWLINE;

            s = s.Replace("{id}", ID).Replace("{style}", Style);

            string c = "";
            foreach (Control child in Children) {
                c += child.OnRender() + NEWLINE;
            }



            return s.Replace("{children}", c);
        }
    }
}