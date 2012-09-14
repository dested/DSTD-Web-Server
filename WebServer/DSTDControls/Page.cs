using System;
using System.Collections.Generic;
using System.Xml;
using WebServer;

namespace DSTDControls {
    public class Page :Control {

        public Dictionary<string, object> Session = new Dictionary<string, object>();
        private Dictionary<string, object> PrivateSession = new Dictionary<string, object>();

        public Dictionary<string, object> Application {
            get {
                return GetApp();
            }
        }

        public event GetApplication GetApp;
        public delegate Dictionary<string, object> GetApplication();

        public Guid CurrentGUID;

        public bool FullRender = true;

        public DSTDRequest Request;
         
        public bool IsPostBack = false;

        public string SelectedControl = "";
        public Dictionary<string, string> UserQuery;

        public override string OnRender() {

            if (UserQuery.ContainsKey("UNLOAD"))
                return "";

            bool doPanelUpdate = PanelToUpdate == "theBody";
            PanelToUpdate = PanelToUpdate == "theBody" ? "" : PanelToUpdate;

            if (SelectedControl != "") {
                SelectedControl = GetIDFromid(this, SelectedControl);
                Scripts.Add("document.getElementById('" + SelectedControl + "').focus();");
            }


            if (PanelToUpdate != "" && FullRender) {
                Panel p = (Panel)GetControlByID(PanelToUpdate);
                XmlDocument doc = new XmlDocument();
                XmlNode head = doc.CreateElement("head");
                XmlNode node = doc.CreateElement("html");
                node.InnerText = p.OnRender();
                head.AppendChild(node);
                node = doc.CreateElement("javascript");
                node.InnerText = GrabScripts.Flatten();
                head.AppendChild(node);
                doc.AppendChild(head);

                if (!PrivateSession.ContainsKey(p.ID))
                    PrivateSession.Add(p.ID, myHelper.ReturnHash(doc.OuterXml));
                else
                {
                    if (!myHelper.CheckAgainstHash(doc.OuterXml, ((string) PrivateSession[p.ID])))
                    {
                        doc = new XmlDocument();
                        head = doc.CreateElement("head");
                        node = doc.CreateElement("same");
                        head.AppendChild(node);
                        node = doc.CreateElement("javascript");
                        node.InnerText = GrabScripts.Flatten();
                        head.AppendChild(node);
                        doc.AppendChild(head);
                        return doc.OuterXml;
                    }
                    else
                    {
                        PrivateSession[p.ID] = myHelper.ReturnHash(doc.OuterXml);
                    }
                }
                return doc.OuterXml;
 
            }
            Scripts.Add("CurrentPage='" + this.GetType().Name + "';");

            string s = "<html><head><script src=\"/jquery-1.2.6.js\" type=\"text/javascript\"></script><script src=\"/Dictionary.js\" type=\"text/javascript\"></script><script src=\"/Server.js\" type=\"text/javascript\" ></script> <script>function Load_(){{scripts}}function UnLoad_(){PostPanel(document.body,'" + getUserQuery("UNLOAD", "UNLOAD") + "theBody|theBody',buildXML('" + CurrentGUID + "'));}</script></head><body onload=\"Load_();\" onunload=\"UnLoad_();\" id=\"theBody\">{FullContent}</body></html>";
            string s2 = "<div id=\"{id}\"><input type=\"hidden\" id=\"CurGUID\" value=\"{guid}\" />{content}</div>";

            if (!FullRender)
            {
                s = s2;
                Scripts.Add("UnLoad_ = function(){PostPanel(document.body,'" + getUserQuery("UNLOAD", "UNLOAD") + "theBody|theBody',buildXML('" + CurrentGUID + "'));}; ");
            }
            else
                s = s.Replace("{FullContent}", s2);

            s = s.Replace("{id}", ID).Replace("{guid}", CurrentGUID.ToString()).Replace("{content}", GetChildrenContent());

            PanelToUpdate = doPanelUpdate ? "theBody" : PanelToUpdate;

            if (!FullRender) {
                Scripts.Add("CurrentPage='"+this.GetType().Name+"';");
                XmlDocument doc = new XmlDocument();
                XmlNode head = doc.CreateElement("head");
                XmlNode where = doc.CreateElement("where");
                where.InnerText = "theBody";
                head.AppendChild(where);
                XmlNode node = doc.CreateElement("html");
                node.InnerText = s;
                head.AppendChild(node);
                node = doc.CreateElement("javascript");
                node.InnerText = GrabScripts.Flatten();
                head.AppendChild(node);

                doc.AppendChild(head);

                return doc.OuterXml;
            }

            return s.Replace("{scripts}", GrabScripts.Flatten());
        }

        public void SetPrivateSession(Dictionary<string, object> objects)
        {
            PrivateSession = objects;
        }

        public Dictionary<string,object> GetPrivateSession()
        {
            return PrivateSession;
        }

        public string getUserQuery() {
            if (UserQuery.Count == 0)
                return "?";

            string s = "*";
            foreach (KeyValuePair<string, string> pair in UserQuery) {
                s += pair.Key + "=" + pair.Value+"&";
            }
         s=   s.TrimEnd('&');
            return s + "?";
        }
        public string getUserQuery(string addedkey,string addedvalue) {

            UserQuery.Add(addedkey, addedvalue);
            string s = getUserQuery();
            UserQuery.Remove(addedkey);
            return s;
        }
    }
}