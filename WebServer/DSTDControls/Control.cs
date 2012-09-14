using System;
using System.Collections.Generic;
using System.Xml;

namespace DSTDControls {
    public class Control {
        public static string GetIDFromid(Control p, string where) {
            Control n = p.GetControlByID(where);
            if (n != null) {
                if (n.id == where) {
                    return n.ID;
                }
            }
            return where;
        }

        public virtual void Initialize() {
            foreach (Control child in children)
            {
                child.Initialize();
            }
        }
        public static string NEWLINE = "\r\n";
        public delegate void TriggeredEvent(Control sender);

        public List<string> GrabScripts {
            get {
                List<string> str = Scripts;
                foreach (Control child in children) {
                    str.AddRange(child.GrabScripts);
                }
                return str;
            }
        }
        public List<string> Scripts = new List<string>();
        public Panel GetPanel {
            get {
                if (this is Page) {
                    return null;
                }

                if (this is Panel) {
                    return (Panel)this;
                }
                else {
                    return this.Parent.GetPanel;
                }
            }
        }
        public string Style = "";
        public Page Page {
            get {
                Control p = this;
                do {
                    if (p.Parent != null) {
                        p = p.Parent;
                    }
                } while (p.Parent != null);

                return (Page)p;
            }
        }


        public void OnEvent() {
            if (this is Button) {
                if (((Button)this).Clicked) {
                    ((Button)this).Click();
                }
            }
            if (this is Timer) {
                if (((Timer)this).Fired) {
                    ((Timer)this).Fire();
                }
            }
            if (this is TextBox) {
                if (((TextBox)this).KeyPressedEnterd) {
                    ((TextBox)this).KeyPressEnter();
                }
            }
            for (int i = children.Count-1; i >= 0; i--)
            {
                Control c = children[i];
                c.OnEvent();
            }

        }

        public string ID {
            get {
                string str = "";
                if (this.parent==null) {
                    str = id;
                }
                else
                    str = this.parent.ID + (id!=""?"_":"") + id;
                return str;
            }
        }
        public string Sender;
        public event ControlInit Init;
        public delegate void ControlInit(Control Sender);
        public event ControlLoad Load;
        public delegate void ControlLoad(Control Sender);
        public event ControlUnLoad UnLoad;
        public delegate void ControlUnLoad(Control Sender);
 

        public string GetChildrenContent() {
            if (string.IsNullOrEmpty(PanelToUpdate)) {
                string c = "";
                foreach (Control child in Children) {
                    c += child.OnRender();
                }
                return c;
            }
            else {
                string c = "";
                Panel p = getPanelByName(PanelToUpdate);

                c += p.OnRender();

                return c;
                XmlDocument doc = new XmlDocument();
                XmlNode head = doc.CreateElement("head");
                XmlNode node = doc.CreateElement("html");
                node.InnerText = c;
                head.AppendChild(node);
                node = doc.CreateElement("javascript");
                node.InnerText = GrabScripts.Flatten();
                head.AppendChild(node);

                doc.AppendChild(head);

                return doc.OuterXml;
            }

        }
        public Control GetControlByID(string id_) {
            if (id == id_ || ID == id_) {
                return this;
            }
            foreach (var control in children) {
                if (control.GetControlByID(id_) != null) {
                    return control.GetControlByID(id_);
                }
            }
            return null;
        }

        private Panel getPanelByName(string update) {
            if (id == update || ID==update) {
                return (Panel)this;
            }

            foreach (Control child in children) {
                Control c = child.getPanelByName(update);
                if (c != null) {
                    return (Panel)c;
                }
            }
            return null;
        }

        public string Value;
        public string id;
        public virtual void OnLoad() {


            if (panelToUpdate == "" || panelToUpdate == "theBody" || panelToUpdate == ID) {

                foreach (Control child in Children) {
                    child.OnLoad();
                }
                if (Load != null) {
                    Load(this);
                }
            }
            else {
                Panel p = getPanelByName(PanelToUpdate);
                //                p.OnInit();
                if (p.Load != null) {
                    p.Load(p);
                }
                foreach (Control child in p.Children) {
                    child.OnLoad();
                }
            }
        }
        public virtual void OnUnLoad() {


            if (panelToUpdate == "" || panelToUpdate == "theBody" || panelToUpdate == ID) {

                foreach (Control child in Children) {
                    child.OnUnLoad();
                }
                if (UnLoad != null) {
                    UnLoad(this);
                }
            }
            else {
                Panel p = getPanelByName(PanelToUpdate);
                //                p.OnInit();
                if (p.UnLoad != null) {
                    p.UnLoad(p);
                }
                foreach (Control child in p.Children) {
                    child.OnUnLoad();
                }
            }
        }

        public virtual void OnInit() {


            if (panelToUpdate == "" || panelToUpdate == "theBody" || panelToUpdate == ID) {

                foreach (Control child in Children) {
                    child.OnInit();
                }
                if (Init != null) {
                    Init(this);
                }
            }
            else {
                Panel p = getPanelByName(PanelToUpdate);
                //                p.OnInit();
                if (p.Init != null) {
                    p.Init(p);
                }
                foreach (Control child in p.Children) {
                    child.OnInit();
                }
            }
        }

        private string panelToUpdate = "";
        public string PanelToUpdate {
            get
            {
                if (!(this is Page))
                {
                    return this.parent.PanelToUpdate;
                }
                return panelToUpdate;
            }set
            {
                if (!(this is Page)) {
                    parent.PanelToUpdate = value;
                    return;
                }
                panelToUpdate = value;
            }
        }
        public virtual string OnRender() {
            string str = "";

            return str;
        }

        private ControlCollection children;

        public ControlCollection Children {
            get {
                return children;
            }
            set {
                children = value;
            }
        }
        private Control parent;

        public Control() {
            children = new ControlCollection(this);
        }

        public Control Parent {
            get {
                return parent;
            }
            set {
                parent = value;
            }
        }

        public string ReloadInformation(XmlDocument xml) {
            Dictionary<string, string> strs = new Dictionary<string, string>();
            string sender = "";
            if (xml.ChildNodes[1].ChildNodes.Count==0)
                return sender;
            foreach (XmlNode node in xml.ChildNodes[1].ChildNodes[0].ChildNodes) {
                if (node.Name == "Sender")
                    sender = node.Attributes["Name"].Value; 
                else {
                    if (node.Attributes["Name"].Value.Split('|').Length == 2) {
                        if (node.Attributes["Name"].Value.Split('|')[1] == "disabled")
                            strs.Add(node.Attributes["Name"].Value.Split('|')[0] + "||", "disabled");
                    }
                    else
                        strs.Add(node.Attributes["Name"].Value, System.Web.HttpUtility.UrlDecode(node.Attributes["Value"].Value));
                }
            }

            reload(strs);
            return sender;
        }

        private void reload(Dictionary<string, string> strs) {
            if (!string.IsNullOrEmpty(ID))
                if (strs.ContainsKey(ID.Split('|')[0])) {
                    if (strs.ContainsKey(ID + "||")) {
                        if (strs[ID + "||"] == "disabled") {
                            if (this is Button)
                                ((Button)this).Enabled = false;
                            if (this is TextBox)
                                ((TextBox)this).Enabled = false;
                        }
                    }
                    if (strs.ContainsKey(ID))
                        Value = strs[ID];
                }


            if (this is Page)
            {
                if (strs.ContainsKey("CurrentGUID"))
                {
                    ((Page) this).CurrentGUID = new Guid(strs["CurrentGUID"]);
                }
            }
            foreach (Control child in children) {
                child.reload(strs);

            }
        }
    }
}