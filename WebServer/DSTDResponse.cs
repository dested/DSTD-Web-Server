using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using DSTDControls;

namespace WebServer {
    public class DSTDContext {
        private XmlDocument getXML(string header) {
            if (!header.StartsWith("POST"))
                return null;
            if (!header.Contains("<?xml version='1.0' encoding='UTF-8'?><top>"))
                return null;

            header = header.Substring(header.IndexOf("<?xml version='1.0' encoding='UTF-8'?><top>"), header.IndexOf("</top>") - header.IndexOf("<?xml version='1.0' encoding='UTF-8'?><top>") + "</top>".Length);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(header);

            return doc;

        }
        public static string getURL(string header) {

            header = header.Substring(header.IndexOf('/'), header.Length - header.IndexOf('/'));
            header = header.Substring(0, header.IndexOf(' '));

            return header;
        }

        public Server myServer;

        public DSTDRequest Request;
        public DSTDContext(Server server, string requestHeader, Action<Page, Guid> g, Action<Guid, Dictionary<string, object>, Dictionary<string, object>> o, Page.GetApplication p) {
            OnGrabSession += g;
            myServer = server;
            OnSetSession += o;
            DSTDQuery query = new DSTDQuery(getURL(requestHeader));
            XmlDocument doc = getXML(requestHeader);
            Request = new DSTDRequest(this,query, doc,p);
            Request.Start();

        }

        public void GrabSession(Page p, Guid g) {
            OnGrabSession(p, g);
        }
        public void SetSession(Guid g, Dictionary<string, object> o, Dictionary<string, object> o2) {
            OnSetSession(g,o,o2);
        }
        public event Action<Page, Guid> OnGrabSession;
        public event Action<Guid, Dictionary<string, object>, Dictionary<string, object>> OnSetSession;
    }
    public class DSTDQuery {
        public string File = "";
        public string Query = "";
        public string Where;
        public Dictionary<string,string> UserQuery = new Dictionary<string, string>();
        public DSTDQuery(string url) {

            if (url.Split('?').Length > 1) {
                Query = url.Split('?')[1];
                if (Query.IndexOf("|") > -1) {
                    Where = Query.Split('|')[1];
                    Query = Query.Split('|')[0];
                }
                url = url.Split('?')[0];
            }


            if (url.Split('*').Length > 1) {
                string u = url.Split('*')[1];
                foreach (string s in u.Split('&')) {
                    UserQuery.Add(s.Split('=')[0], s.Split('=')[1]);
                }
                url = url.Split('*')[0];
            }

            File = url.Split('?')[0];

        }
    }


    public class DSTDRequest {
        public XmlDocument State;
        public DSTDQuery Query;
        public Page CurrentPage;
        public DSTDContext Context;
        private Page.GetApplication getApplication;

        public DSTDRequest(DSTDContext that, DSTDQuery q, XmlDocument state, Page.GetApplication p) {
            Context = that;
            State = state;
            Query = q;
            getApplication = p;
            Page h;
            if (File.Exists(Context.myServer.localDirectory + Query.File.TrimStart('/') + ".dstd")) {
                CompileDSTD compile = CompileDSTD.CompileDSTDPage(Context, Context.myServer.localDirectory + Query.File.TrimStart('/'), Query.File.TrimStart('/'));
                h = (Page)compile.Compiled;
            }
            else {
                return;
            }
            h.UserQuery = q.UserQuery;
            CurrentPage = h;
        }

        public void Start() {
            Page h=CurrentPage;

            h.Request = this;

            h.GetApp += getApplication;
            h.Initialize();
            if (State != null)
                h.IsPostBack = true;

            if (Query.Query != "") {
                if (Query.Where!=null) {
                    h.PanelToUpdate = Query.Where;
                }

            }

            h.OnInit();
            string sender = "";
            if (State != null)
                sender = h.ReloadInformation(State);
            if (h.CurrentGUID == new Guid()) {
                h.CurrentGUID = Guid.NewGuid();
            }
            Context.GrabSession(h, h.CurrentGUID);
            h.Sender = sender;
            h.OnLoad();


            if (Query.Query != "") {
                if (h.GetControlByID(Query.Query) is Button) {
                    ((Button)h.GetControlByID(Query.Query)).Clicked = true;
                }
                if (h.GetControlByID(Query.Query) is Timer) {
                    ((Timer)h.GetControlByID(Query.Query)).Fired = true;
                }
                if (h.GetControlByID(Query.Query) is TextBox) {
                    ((TextBox)h.GetControlByID(Query.Query)).KeyPressedEnterd = true;
                }
                if (Query.Query == "theBody" && h.UserQuery.ContainsKey("UNLOAD"))
                {
                    h.OnUnLoad();
                }
            }

            h.OnEvent();
        }

        public void TransferToPage(string Page)
        {
            DSTDQuery q = new DSTDQuery(Page);
            DSTDRequest r = new DSTDRequest(this.Context, q, null, getApplication);
            r.CurrentPage.FullRender = false;
            r.CurrentPage.CurrentGUID = this.CurrentPage.CurrentGUID;
            r.CurrentPage.IsPostBack = false;
            r.Start();
            r.CurrentPage.PanelToUpdate = "theBody";
            CurrentPage = r.CurrentPage;
        }

    
    }
}