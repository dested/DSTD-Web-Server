using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Xml;
using WebServer;

namespace DSTDControls {
    public class CompileDSTD  {

 

        public Control Compiled;
        private Page page;
        private Panel panel;
        private DSTDContext Context;
        private string Location;
        public static CompileDSTD CompileDSTDPage(DSTDContext context, string location, string class_)
        {
            CompileDSTD d=new CompileDSTD();

            d.Context = context;
            XmlDocument doc = new XmlDocument();
            doc.Load(location + ".dstd");
            d.Location = location.Substring(0, location.LastIndexOf("\\"));
            d.Location += d.Location.EndsWith("\\") ? "" : "\\";

            d.page = (Page)context.myServer.GetTypeFromString.Invoke(class_).GetConstructor(Type.EmptyTypes).Invoke(null);

            foreach (XmlNode n in doc.FirstChild.ChildNodes)
            {
                d.page.Children.Add(d.CompileXML(n));
            }
            d.Compiled = d.page;
            return d;
        }
        public static CompileDSTD  CompileDSTDPanel(DSTDContext context, string location, string class_, string id) {

            CompileDSTD d = new CompileDSTD();
            
            
            d.Context = context;
            XmlDocument doc = new XmlDocument();
            doc.Load(location + ".dstdp");
            d.Location = location.Substring(0, location.LastIndexOf("\\"));

            d.Location += d.Location.EndsWith("\\") ? "" : "\\";
            if (context.myServer.GetTypeFromString(class_) == null)
            {
                return d;
            }


            d.panel = (Panel)context.myServer.GetTypeFromString(class_).GetConstructor(new[] { typeof(string) }).Invoke(new object[] { id });

            Control nn = d.getControlFromNode(doc.FirstChild);

            if (nn.id!="")
              d. panel.id = nn.id;
            if (nn.Style != "")
                d.panel.Style = nn.Style;
            if (nn.Value != "")
                d.panel.Value = nn.Value;

            foreach (XmlNode n in doc.FirstChild.ChildNodes)
            {
                d.panel.Children.Add(d.CompileXML(n));
            }
            d.Compiled = d.panel;
            return d;
        }

        private Control CompileXML(XmlNode node)
        {
            Control c = getControlFromNode(node);
            foreach (XmlNode n in node.ChildNodes)
            {
                c.Children.Add(CompileXML(n));
            }
            return c;
        }

        private Control getControlFromNode(XmlNode node) {
            string id = "";
            if (node.Attributes["id"] != null)
                id = node.Attributes["id"].Value;

            string value = "";
            bool visible = true;
            if (node.Attributes["value"] != null)
                value = node.Attributes["value"].Value;
            if (node.Attributes["label"] != null)
                value = node.Attributes["label"].Value;
            if (node.Attributes["text"] != null)
                value = node.Attributes["text"].Value;
            if (node.Attributes["visible"] != null)
                visible = bool.Parse(node.Attributes["visible"].Value);

            string style = "";
            if (node.Attributes["style"] != null)
                style = node.Attributes["style"].Value;
            bool enabled = true;
            if (node.Attributes["enabled"] != null)
                enabled = bool.Parse(node.Attributes["enabled"].Value);
            string onclick = "";
            if (node.Attributes["onclick"] != null)
                onclick = node.Attributes["onclick"].Value;

         

            switch (node.Name.ToLower()) {
                case "panel":
                    Panel panel = new Panel();
                    panel.id = id;
                    panel.Value = value;
                    panel.Visible = visible;
                    panel.Style = style;
                    return panel;
                    break;
                case "page":
                    Page page = new Page();
                    page.id = id;
                    return page;
                    break;
                case "textbox":
                    TextBox text = new TextBox();
                    text.id = id;
                    text.Enabled = enabled;
                    if (node.Attributes["onkeypressenter"] != null) {
                        onclick = node.Attributes["onkeypressenter"].Value;
                        if (onclick != "") {
                            text.GetType().GetEvent("OnKeyPressEnter").AddEventHandler(text, Delegate.CreateDelegate(typeof(Control.TriggeredEvent), (Control)this.page ?? (Control)this.panel, onclick.Split('|')[0]));
                            if (onclick.Split('|').Length == 2) {
                                text.Where = onclick.Split('|')[1];
                            }
                        }
                    }



                    text.text = value;
                    text.Style = style;
                    if (node.Attributes["multiline"] != null) {
                        text.Multiline = bool.Parse(node.Attributes["multiline"].Value);
                        if (node.Attributes["rows"] != null)
                            text.Rows = int.Parse(node.Attributes["rows"].Value);
                        if (node.Attributes["cols"] != null)
                            text.Cols = int.Parse(node.Attributes["cols"].Value);
                    }
                    return text;
                    break;
                case "label":
                    Label label = new Label();
                    label.id = id;
                    label.text = value;
                    label.Style = style;
                    return label;
                    break;
                case "button":
                    Button button = new Button();
                    button.id = id;
                    button.Enabled = enabled;
                    button.label = value;
                    if (node.Attributes["enabled"] != null)
                        button.Enabled = bool.Parse(node.Attributes["enabled"].Value);


                    if (onclick != "") {
                        button.GetType().GetEvent("OnClick").AddEventHandler(button, Delegate.CreateDelegate(typeof(Control.TriggeredEvent), (Control)this.page ?? (Control)this.panel, onclick.Split('|')[0]));
                        if (onclick.Split('|').Length == 2) {
                            button.Where = onclick.Split('|')[1];
                        }
                    }
                    button.Style = style;
                    return button;
                    break;
                case "table":
                    Table table = new Table();
                    table.id = id;
                    table.Style = style;
                    return table;
                    break;
                case "tr":
                    TableRow row = new TableRow();

                    if (node.Attributes["colspan"] != null)
                        row.ColSpan = int.Parse(node.Attributes["colspan"].Value);

                    if (node.Attributes["rowspan"] != null)
                        row.RowSpan = int.Parse(node.Attributes["rowspan"].Value);

                    row.id = id;
                    row.Style = style;
                    return row;
                    break;
                case "td":
                    TableCell cell = new TableCell();
                    if (node.Attributes["colspan"] != null)
                        cell.ColSpan = int.Parse(node.Attributes["colspan"].Value);

                    if (node.Attributes["rowspan"] != null)
                        cell.RowSpan = int.Parse(node.Attributes["rowspan"].Value);
                    cell.id = id;
                    cell.Style = style;
                    return cell;
                    break;
                case "br":
                    BR br = new BR();
                    br.Style = style;
                    return br;
                    break;
                case "variable":
                    Variable variable = new Variable();
                    variable.id = id;
                    variable.Value = value;
                    variable.Style = style;
                    return variable;
                    break;
                case "timer":
                    Timer timer = new Timer();
                    timer.id = id;
                    if (node.Attributes["time"] != null)
                        timer.Time = int.Parse(node.Attributes["time"].Value);
                    if (node.Attributes["before"] != null)
                        timer.Before = (node.Attributes["before"].Value);

                    if (node.Attributes["onfire"] != null) {
                        onclick = node.Attributes["onfire"].Value;
                        timer.GetType().GetEvent("OnFire").AddEventHandler(timer, Delegate.CreateDelegate(typeof(Control.TriggeredEvent), (Control)this.page ?? (Control)this.panel, onclick.Split('|')[0]));
                        if (onclick.Split('|').Length == 2) {
                            timer.Where = onclick.Split('|')[1];
                        }
                    }
                    timer.Value = value;
                    return timer;
                    break;
                default:
                    Panel pa = Panel.LoadControl(Context, Location + node.Name, node.Name, id);
                    if (Panel.LoadControl(Context,Location+ node.Name, node.Name, id) != null) {
                        return pa;
                    }
                    throw new Exception("Control doesnt exists:" + node.Name.ToLower());
            }
        }
    }


//
//    public class CompileDSTDPanel {
// 
//        public Control Compiled;
//        private Panel p;
//        private string Location;
//        private DSTDContext Context;
//        public CompileDSTDPanel(DSTDContext context, string location,string class_,  string id) {
//            Context = context;
//            XmlDocument doc = new XmlDocument();
//            doc.Load(location + ".dstdp");
//            Location = location.Substring(0, location.LastIndexOf("\\"));
//
//            Location += Location.EndsWith("\\") ? "" : "\\";
//            if (context.myServer.GetTypeFromString(class_) == null)
//            {
//                return ;
//            }
//
//
//            p = (Panel)context.myServer.GetTypeFromString(class_).GetConstructor(new[] { typeof(string) }).Invoke(new object[] { id });
//
//            Control nn = getControlFromNode(doc.FirstChild);
//
//            if (nn.id!="")
//                p.id = nn.id;
//            if (nn.Style != "")
//                p.Style = nn.Style;
//            if (nn.Value != "")
//                p.Value = nn.Value;
//
//            foreach (XmlNode n in doc.FirstChild.ChildNodes)
//            {
//                p.Children.Add(CompileXML(n));
//            }
//            Compiled = p;
//        }
//
//        private Control CompileXML(XmlNode node) {
//            Control c = getControlFromNode(node);
//            foreach (XmlNode n in node.ChildNodes) {
//                c.Children.Add(CompileXML(n));
//            }
//            return c;
//        }
//
//
//
//        private Control getControlFromNode(XmlNode node) {
//            string id = "";
//            if (node.Attributes["id"] != null)
//                id = node.Attributes["id"].Value;
//
//            string value = "";
//            bool visible = true;
//            if (node.Attributes["value"] != null)
//                value = node.Attributes["value"].Value;
//            if (node.Attributes["label"] != null)
//                value = node.Attributes["label"].Value;
//            if (node.Attributes["text"] != null)
//                value = node.Attributes["text"].Value;
//            if (node.Attributes["visible"] != null)
//                visible= bool.Parse(node.Attributes["visible"].Value);
//
//            string style = "";
//            if (node.Attributes["style"] != null)
//                style = node.Attributes["style"].Value;
//            bool enabled = true;
//            if (node.Attributes["enabled"] != null)
//                enabled = bool.Parse(node.Attributes["enabled"].Value);
//            string onclick = "";
//            if (node.Attributes["onclick"] != null)
//                onclick = node.Attributes["onclick"].Value;
//
//            switch (node.Name.ToLower()) {
//                case "panel":
//                    Panel panel = new Panel();
//                    panel.id = id;
//                    panel.Visible = visible;
//                    panel.Value = value;
//                    panel.Style = style;
//                    return panel;
//                    break;
//                case "page":
//                    Page page = new Page();
//                    page.id = id;
//                    return page;
//                    break;
//                case "textbox":
//                    TextBox text = new TextBox();
//                    text.id = id;
//                    text.Enabled = enabled;
//                    if (node.Attributes["onkeypressenter"] != null) {
//                        onclick = node.Attributes["onkeypressenter"].Value;
//                        if (onclick != "") {
//                            text.GetType().GetEvent("OnKeyPressEnter").AddEventHandler(text, Delegate.CreateDelegate(typeof(Control.TriggeredEvent), p, onclick.Split('|')[0]));
//                            if (onclick.Split('|').Length == 2) {
//                                text.Where = onclick.Split('|')[1];
//                            }
//                        }
//                    }
//                    if (node.Attributes["type"] != null) 
//                        text.type=node.Attributes["type"].Value; 
//
//                    text.text = value;
//                    text.Style = style;
//                    if (node.Attributes["multiline"] != null) {
//                        text.Multiline = bool.Parse(node.Attributes["multiline"].Value);
//                        if (node.Attributes["rows"] != null)
//                            text.Rows = int.Parse(node.Attributes["rows"].Value);
//                        if (node.Attributes["cols"] != null)
//                            text.Cols = int.Parse(node.Attributes["cols"].Value);
//                    }
//                    return text;
//                    break;
//                case "label":
//                    Label label = new Label();
//                    label.id = id;
//                    label.text = value;
//                    label.Style = style;
//                    return label;
//                    break;
//                case "button":
//                    Button button = new Button();
//                    button.id = id;
//                    button.Enabled = enabled;
//                    button.label = value;
//                    if (node.Attributes["enabled"] != null)
//                        button.Enabled = bool.Parse(node.Attributes["enabled"].Value);
//
//
//                    if (onclick != "") {
//                        button.GetType().GetEvent("OnClick").AddEventHandler(button, Delegate.CreateDelegate(typeof(Control.TriggeredEvent), p, onclick.Split('|')[0]));
//                        if (onclick.Split('|').Length == 2) {
//                            button.Where = onclick.Split('|')[1];
//                        }
//                    }
//                    button.Style = style;
//                    return button;
//                    break;
//                case "table":
//                    Table table = new Table();
//                    table.id = id;
//                    table.Style = style;
//                    return table;
//                    break;
//                case "tr":
//                    TableRow row = new TableRow();
//
//                    if (node.Attributes["colspan"] != null)
//                        row.ColSpan = int.Parse(node.Attributes["colspan"].Value);
//
//                    if (node.Attributes["rowspan"] != null)
//                        row.RowSpan = int.Parse(node.Attributes["rowspan"].Value);
//
//                    row.id = id;
//                    row.Style = style;
//                    return row;
//                    break;
//                case "td":
//                    TableCell cell = new TableCell();
//                    if (node.Attributes["colspan"] != null)
//                        cell.ColSpan = int.Parse(node.Attributes["colspan"].Value);
//
//                    if (node.Attributes["rowspan"] != null)
//                        cell.RowSpan = int.Parse(node.Attributes["rowspan"].Value);
//                    cell.id = id;
//                    cell.Style = style;
//                    return cell;
//                    break;
//                case "br":
//                    BR br = new BR();
//                    br.Style = style;
//                    return br;
//                    break;
//                case "variable":
//                    Variable variable = new Variable();
//                    variable.id = id;
//                    variable.Value = value;
//                    variable.Style = style;
//                    return variable;
//                    break;
//                case "timer":
//                    Timer timer = new Timer();
//                    timer.id = id;
//                    if (node.Attributes["time"] != null)
//                        timer.Time = int.Parse(node.Attributes["time"].Value);
//                    if (node.Attributes["before"] != null)
//                        timer.Before = (node.Attributes["before"].Value);
//
//                    if (node.Attributes["onfire"] != null) {
//                        onclick = node.Attributes["onfire"].Value;
//                        timer.GetType().GetEvent("OnFire").AddEventHandler(timer, Delegate.CreateDelegate(typeof(Control.TriggeredEvent), p, onclick.Split('|')[0]));
//                        if (onclick.Split('|').Length == 2) {
//                            timer.Where = onclick.Split('|')[1];
//                        }
//                    }
//                    timer.Value = value;
//                    return timer;
//                    break;
//                default:
//                    Panel pa = Panel.LoadControl(Context, Location + node.Name, node.Name, id);
//                    if (Panel.LoadControl(Context, Location + node.Name, node.Name, id) != null) {
//                        return pa;
//                    }
//                    throw new Exception("Control doesnt exists:" + node.Name.ToLower());
//            }
//        }
//    }
}
