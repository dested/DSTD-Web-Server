namespace DSTDControls
{
    public class Table :Control {

        public override void OnInit() {
            base.OnInit();

        }
        public override void OnLoad() {
            base.OnLoad();
        }


        public override string OnRender() {
            string s = "<table> " + NEWLINE;
            foreach (TableRow row in Children) {
                s += row.OnRender() + NEWLINE;
            }
            s += "</table>" + NEWLINE;

            return s;
        }
    }
    public class TableRow :Control {

        public override void OnInit() {
            base.OnInit();

        }
        public override void OnLoad() {
            base.OnLoad();
        }

        public int ColSpan = 1;
        public int RowSpan = 1;

        public override string OnRender() {
            string s = "<tr colspan=\"{colspan}\" rowspan=\"{rowspan}\"> ".Replace("{colspan}", ColSpan.ToString()).Replace("{rowspan}", RowSpan.ToString()) + NEWLINE;
            foreach (TableCell row in Children) {
                s += row.OnRender() + NEWLINE;
            }
            s += "</tr>" + NEWLINE;

            return s;
        }
    }
    public class TableCell :Control {
        public int ColSpan = 1;
        public int RowSpan = 1;

        public override void OnInit() {
            base.OnInit();

        }
        public override void OnLoad() {
            base.OnLoad();
        }


        public override string OnRender() {
            string s = "<td colspan=\"{colspan}\" rowspan=\"{rowspan}\"> ".Replace("{colspan}", ColSpan.ToString()).Replace("{rowspan}", RowSpan.ToString()) + NEWLINE;
            foreach (Control row in Children) {
                s += row.OnRender() + NEWLINE;
            }
            s += "</td>" + NEWLINE;

            return s;
        }
    }
}